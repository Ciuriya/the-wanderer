using Player2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public List<UIElement> m_elements; // The list of UI Elements to manage
    public AudioClip m_menuOpen;       // The menu opening sound
    public AudioClip m_menuClose;      // The menu closing sound

    [HideInInspector]
    public long m_lastPause;           // The last pause time

    public float m_pauseCooldown;      // The pause button cooldown
    private bool m_volumeLoaded;       // If the volume options are loaded, the UI Manager loads before the Game Manager which causes issues

    void Start() {
        m_volumeLoaded = false;

        GameObject pauseMenu = FindElement("pause");
        GameObject settingsMenu = FindElement("settings");

        if (pauseMenu != null) {
            pauseMenu.SetActive(false);
        }

        if (settingsMenu != null) {
            settingsMenu.SetActive(false);
        }

        if (GameManager.PlayerStats) {
            if (GameManager.PlayerStats.m_jumpDisabled && FindElement("jump") != null) {
                FindElement("jump").SetActive(false);
            }

            if (GameManager.PlayerStats.m_boostDisabled && FindElement("boost") != null) {
                FindElement("boost").SetActive(false);
            }

            if (GameManager.PlayerStats.m_shootDisabled && FindElement("shoot") != null) {
                FindElement("shoot").SetActive(false);
            }

            if (GameManager.PlayerStats.m_flyDisabled && FindElement("fly") != null) {
                FindElement("fly").SetActive(false);
            }

            FindElement("height").GetComponent<Slider>().interactable = false;
        }
    }

    void Update() {
        if (!m_volumeLoaded) {
            m_volumeLoaded = true;
            MusicSlider(PlayerPrefs.GetFloat("music", 50f));
            EffectSlider(PlayerPrefs.GetFloat("effects", 50f));
        }
    }

    // Starts the game from the main menu
    public void GameStart() {
        SceneManager.LoadScene("Level1");
    }

    // Opens the settings menu
    public void OpenSettings() {
        GameObject pauseMenu = FindElement("pause");
        GameObject settingsMenu = FindElement("settings");
        GameObject mainMenu = FindElement("menu");

        if (pauseMenu != null) {
            pauseMenu.SetActive(false);
        }

        if (mainMenu != null) {
            mainMenu.SetActive(false);
        }

        settingsMenu.SetActive(true);

        GameManager.Instance.m_effectSources[0].clip = m_menuOpen;
        GameManager.Instance.m_effectSources[0].Play(0);
    }

    // Toggles the pause menu on or off
    public void TogglePause() {
        m_lastPause = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        GameObject pauseMenu = FindElement("pause");
        GameObject settingsMenu = FindElement("settings");
        GameObject mainMenu = FindElement("menu");

        if (mainMenu != null) {
            if (!mainMenu.activeSelf) {
                GameManager.Instance.m_effectSources[0].clip = m_menuClose;
                GameManager.Instance.m_effectSources[0].Play(0);
            }

            mainMenu.SetActive(true);
            settingsMenu.SetActive(false);

            return;
        }

        if (settingsMenu.activeSelf) {
            settingsMenu.SetActive(false);
            pauseMenu.SetActive(true);

            GameManager.Instance.m_effectSources[0].clip = m_menuClose;
            GameManager.Instance.m_effectSources[0].Play(0);
        } else {
            if (GameManager.Instance.m_allowPausing) {
                if (!pauseMenu.activeSelf) {
                    GameManager.Instance.m_effectSources[0].clip = m_menuOpen;
                    Time.timeScale = 0.0f;
                    FindElement("height").GetComponent<Slider>().interactable = false;
                } else {
                    GameManager.Instance.m_effectSources[0].clip = m_menuClose;
                    Time.timeScale = 1.0f;
                    GameManager.m_timeSinceUnpause = 0;
                    FindElement("height").GetComponent<Slider>().interactable = GameManager.PlayerStats.m_isFlying;
                }

                GameManager.Instance.m_effectSources[0].Play(0);

                pauseMenu.SetActive(!pauseMenu.activeSelf);
            }
        }

        if (GameManager.Instance.m_allowPausing) {
            GameManager.m_gamePaused = pauseMenu.activeSelf || settingsMenu.activeSelf;
        }
    }

    // Loads the main menu from the pause menu
    public void MainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    // Exits the game
    public void Exit() {
        Application.Quit();
    }

    // The method used by the music slider in settings to modify the music's volume in the game
    public void MusicSlider(float p_value) {
        PlayerPrefs.SetFloat("music", p_value);
        FindElement("musicLabel").GetComponent<Text>().text = ((int) p_value).ToString() + "%";
        FindElement("music").GetComponent<Slider>().value = p_value;

        foreach (AudioSource source in GameManager.Instance.m_musicSources) {
            source.volume = p_value / 100;
        }
    }

    // The method used by the effect slider in settings to modify the effects' volume in the game
    public void EffectSlider(float p_value) {
        PlayerPrefs.SetFloat("effects", p_value);
        FindElement("effectsLabel").GetComponent<Text>().text = ((int) p_value).ToString() + "%";
        FindElement("effects").GetComponent<Slider>().value = p_value;

        foreach (AudioSource source in GameManager.Instance.m_effectSources) {
            source.volume = p_value / 100;
        }
    }

    // The method used by the height slider to increment/decrement the speed value
    public void HeightSlider(float p_value) {
        if (GameManager.PlayerStats.m_isFlying && !GameManager.m_gamePaused) {
            GameManager.PlayerStats.setHeight(p_value);
        }
    }

    // The method used by the boost button to boost
    public void Boost() {
        if (!GameManager.m_gamePaused) {
            FindElement("boost").GetComponent<Button>().interactable = false;
            GameManager.PlayerStats.fillBoostTime();
            GameManager.PlayerStats.increaseHeat();
        }
    }

    // The method used by the jump button to jump
    public void Jump() {
        if (!GameManager.m_gamePaused) {
            GameManager.InputController.Jump();
        }
    }

    // The method used by the shoot button to shoot
    public void Shoot() {
        if (!GameManager.m_gamePaused) {
            FindElement("shoot").GetComponent<Button>().interactable = false;
            GameManager.PlayerStats.m_isShooting = true;
            GameManager.InputController.GetPlayer().Shoot();
        }
    }

    // The method used by the fly button to fly
    public void Fly() {
        if (!GameManager.m_gamePaused) {
            GameManager.InputController.Fly();
        }

    }

    // Finds an UI Element using its name
    public GameObject FindElement(string p_name) {
        UIElement element = m_elements.Find(e => e.m_name == p_name);

        if (element != null) {
            return element.gameObject;
        }

        return null;
    }
}
