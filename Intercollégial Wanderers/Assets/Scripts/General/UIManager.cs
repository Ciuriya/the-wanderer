using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    // The list of UI Elements to manage
    public List<UIElement> m_elements;

    void Start() {
        GameObject pauseMenu = FindElement("pause");
        GameObject settingsMenu = FindElement("settings");

        if (pauseMenu != null) {
            pauseMenu.SetActive(false);
        }

        if (settingsMenu != null) {
            settingsMenu.SetActive(false);
        }

        MusicSlider(PlayerPrefs.GetFloat("music"));
        EffectSlider(PlayerPrefs.GetFloat("effects"));
    }


    public void GameStart() {
        //do shit idk
    }

    public void OpenSettings() {
        FindElement("pause").SetActive(false);
        FindElement("settings").SetActive(true);
    }

    public void TogglePause() {
        GameObject pauseMenu = FindElement("pause");
        GameObject settingsMenu = FindElement("settings");

        if (settingsMenu.activeSelf) {
            settingsMenu.SetActive(!settingsMenu.activeSelf);
        } else {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }

        GameManager.m_gamePaused = pauseMenu.activeSelf || settingsMenu.activeSelf;
    }

    public void MainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

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
        // insert jump code here or link it to the variable
        FindElement("jump").SetActive(false);
        GameManager.PlayerStats.m_isJumping = true;
        // make sure to re-enable button later
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
        // insert shooting code here or link it to the variable
        FindElement("shoot").SetActive(false);
        GameManager.PlayerStats.m_isShooting = true;
        // make sure to re-enable button later
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
