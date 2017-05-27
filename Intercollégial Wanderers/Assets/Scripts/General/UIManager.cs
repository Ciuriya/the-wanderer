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
    }

    void Update() {
        if (!m_volumeLoaded) {
            m_volumeLoaded = true;
            MusicSlider(PlayerPrefs.GetFloat("music"));
            EffectSlider(PlayerPrefs.GetFloat("effects"));
        }
    }

    // Starts the game from the main menu
    public void GameStart() {
        //do shit idk
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
    }

    // Toggles the pause menu on or off
    public void TogglePause() {
        m_lastPause = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        GameObject pauseMenu = FindElement("pause");
        GameObject settingsMenu = FindElement("settings");
        GameObject mainMenu = FindElement("menu");

        if (mainMenu != null) {
            if (!mainMenu.activeSelf) {
                GameManager.Instance.m_effectSources[0].clip = m_menuOpen;
                GameManager.Instance.m_effectSources[0].Play(0);
            } else {
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
            if (!pauseMenu.activeSelf) {
                GameManager.Instance.m_effectSources[0].clip = m_menuClose;
            } else {
                GameManager.Instance.m_effectSources[0].clip = m_menuOpen;
            }

            GameManager.Instance.m_effectSources[0].Play(0);

            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }

        GameManager.m_gamePaused = pauseMenu.activeSelf || settingsMenu.activeSelf;
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
        GameManager.PlayerStats.setHeight(p_value);
    }

    // The method used by the boost button to boost
    public void Boost() {
        // insert boost code here or link it to the variable
        FindElement("boost").SetActive(false);
        GameManager.PlayerStats.m_isBoosting = true;
        // make sure to re-enable button later
    }

    // The method used by the jump button to jump
    public void Jump() {
        GameManager.InputController.Jump();
    }

    // The method used by the stop button to stop
    public void Stop() {
        // insert stop code here or link it to the variable
        FindElement("stop").SetActive(false);
        GameManager.PlayerStats.m_isStopped = true;
        // make sure to re-enable button later
    }

    // The method used by the shoot button to shoot
    public void Shoot() {
        FindElement("shoot").SetActive(false);
        GameManager.PlayerStats.m_isShooting = true;
    }

    // The method used by the fly button to fly
    public void Fly() {
        // insert flying code here or link it to the variable
        FindElement("fly").SetActive(false);
        GameManager.PlayerStats.m_isFlying = true;
        // make sure to re-enable button later
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
