using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    public float m_maxHealth;     // The player's maximum health
    public float m_health;        // The player's current health
    public float m_maxSpeed;      // The player's maximum speed
    public float m_speed;         // The player's current speed
    public float m_maxHeat;       // The player's maximum heat
    public float m_heatRate;      // The player's overheat/sec rate
    public float m_heat;          // The player's current heat
    public float m_fireRate;      // The player's current firing rate/sec
    public float m_height;        // The player's current flying height
    public bool m_isStopped;      // If the player is currently stopped
    public bool m_isShooting;     // If the player is currently shooting
    public bool m_isJumping;      // If the player is currently jumping
    public bool m_isFlying;       // If the player is currently flying
    public bool m_speedDisabled;  // If the player's speed meter is disabled
    public bool m_shootDisabled;  // If the player's shooting is disabled
    public bool m_heightDisabled; // If the player's height meter is disabled
    public bool m_flyDisabled;    // If the player's flying is disabled
    public bool m_stopDisabled;   // If the player's stopping is disabled
    public bool m_jumpDisabled;   // If the player's jumping is disabled
    public bool m_updateSliders;  // Used by sliders to know whether or not sliders need to be updated
    private UIManager m_uiManager;

    void Start() {
        // values are saved in PlayerPrefs to allow for easy transfer between levels
        m_maxHealth = PlayerPrefs.GetFloat("maxHealth", 100f);
        m_health = PlayerPrefs.GetFloat("health", 100f);
        m_maxSpeed = PlayerPrefs.GetFloat("maxSpeed", 1f);
        m_speed = PlayerPrefs.GetFloat("speed", 1f);
        m_maxHeat = PlayerPrefs.GetFloat("maxHeat", 1f);
        m_heatRate = PlayerPrefs.GetFloat("heatRate", 0.1f);
        m_heat = PlayerPrefs.GetFloat("heat", 0f);
        m_fireRate = PlayerPrefs.GetFloat("fireRate", 1f);
        m_height = PlayerPrefs.GetFloat("height", 1f);
        m_isStopped = false;
        m_isShooting = false;
        m_isJumping = false;
        m_isFlying = false;
        m_speedDisabled = PlayerPrefs.GetInt("speedDisabled", 0) == 1;
        m_shootDisabled = PlayerPrefs.GetInt("shootDisabled", 0) == 1;
        m_heightDisabled = PlayerPrefs.GetInt("heightDisabled", 0) == 1;
        m_flyDisabled = PlayerPrefs.GetInt("flyDisabled", 0) == 1;
        m_stopDisabled = PlayerPrefs.GetInt("stopDisabled", 0) == 1;
        m_jumpDisabled = PlayerPrefs.GetInt("jumpDisabled", 0) == 1;
        m_uiManager = GameObject.FindWithTag("UI_Manager").GetComponent<UIManager>();
        m_updateSliders = true;

        Slider speedSlider = m_uiManager.FindElement("speed").GetComponent<Slider>();
        speedSlider.value = m_speed;
        speedSlider.maxValue = m_maxSpeed;
        speedSlider.gameObject.SetActive(!m_speedDisabled);

        Slider coolingSlider = m_uiManager.FindElement("cooling").GetComponent<Slider>();
        coolingSlider.value = m_heat;
        coolingSlider.maxValue = m_maxHeat;

        Slider heightSlider = m_uiManager.FindElement("height").GetComponent<Slider>();
        heightSlider.value = m_height;
        heightSlider.gameObject.SetActive(!m_heightDisabled);

        m_uiManager.FindElement("shoot").SetActive(!m_shootDisabled);
        m_uiManager.FindElement("fly").SetActive(!m_flyDisabled);
        m_uiManager.FindElement("stop").SetActive(!m_stopDisabled);
        m_uiManager.FindElement("jump").SetActive(!m_jumpDisabled);
    }

    public void setMaxHealth(float p_maxHealth) {
        m_maxHealth = p_maxHealth;
        PlayerPrefs.SetFloat("maxHealth", p_maxHealth);
    }

    public void setHealth(float p_health) {
        float health = p_health;

        if (p_health > m_maxHealth){
            health = m_maxHealth;
        }

        m_health = health;
        PlayerPrefs.SetFloat("health", health);
    }

    public void setMaxSpeed(float p_maxSpeed) {
        m_maxSpeed = p_maxSpeed;
        PlayerPrefs.SetFloat("maxSpeed", p_maxSpeed);

        if(m_updateSliders) {
            m_uiManager.FindElement("speed").GetComponent<Slider>().maxValue = p_maxSpeed;
        }
    }

    public void setSpeed(float p_speed) {
        float speed = p_speed;

        if (p_speed > m_maxSpeed) {
            speed = m_maxSpeed;
        }

        m_speed = speed;
        PlayerPrefs.SetFloat("speed", speed);

        if (m_updateSliders) {
            m_uiManager.FindElement("speed").GetComponent<Slider>().value = speed;
        }
    }

    public void setMaxHeat(float p_maxHeat) {
        m_maxHeat = p_maxHeat;
        PlayerPrefs.SetFloat("maxHeat", p_maxHeat);

        if (m_updateSliders) {
            m_uiManager.FindElement("cooling").GetComponent<Slider>().maxValue = p_maxHeat;
        }
    }

    public void setHeatRate(float p_heatRate) {
        m_heatRate = p_heatRate;
        PlayerPrefs.SetFloat("heatRate", p_heatRate);
    }

    public void setHeat(float p_heat) {
        float heat = p_heat;

        if (p_heat > m_maxHeat) {
            heat = m_maxHeat;
        }

        m_heat = heat;
        PlayerPrefs.SetFloat("heat", heat);

        if (m_updateSliders) {
            m_uiManager.FindElement("cooling").GetComponent<Slider>().value = heat;
        }
    }

    public void setFireRate(float p_fireRate) {
        m_fireRate = p_fireRate;
        PlayerPrefs.SetFloat("fireRate", p_fireRate);
    }

    public void setHeight(float p_height) {
        m_height = p_height;
        PlayerPrefs.SetFloat("height", p_height);

        if (m_updateSliders) {
            m_uiManager.FindElement("height").GetComponent<Slider>().value = p_height;
        }
    }

    public void setSpeedDisabled(bool p_disabled) {
        m_speedDisabled = p_disabled;
        PlayerPrefs.SetInt("speedDisabled", p_disabled ? 1 : 0);

        if (m_updateSliders) {
            m_uiManager.FindElement("speed").SetActive(!p_disabled);
        }
    }

    public void setShootDisabled(bool p_disabled) {
        m_shootDisabled = p_disabled;
        PlayerPrefs.SetInt("shootDisabled", p_disabled ? 1 : 0);

        if (m_updateSliders) {
            m_uiManager.FindElement("shoot").SetActive(!p_disabled);
        }
    }

    public void setHeightDisabled(bool p_disabled) {
        m_heightDisabled = p_disabled;
        PlayerPrefs.SetInt("heightDisabled", p_disabled ? 1 : 0);

        if (m_updateSliders) {
            m_uiManager.FindElement("height").SetActive(!p_disabled);
        }
    }

    public void setFlyDisabled(bool p_disabled) {
        m_flyDisabled = p_disabled;
        PlayerPrefs.SetInt("flyDisabled", p_disabled ? 1 : 0);

        if (m_updateSliders) {
            m_uiManager.FindElement("fly").SetActive(!p_disabled);
        }
    }

    public void setStopDisabled(bool p_disabled) {
        m_stopDisabled = p_disabled;
        PlayerPrefs.SetInt("stopDisabled", p_disabled ? 1 : 0);

        if (m_updateSliders) {
            m_uiManager.FindElement("stop").SetActive(!p_disabled);
        }
    }

    public void setJumpDisabled(bool p_disabled) {
        m_jumpDisabled = p_disabled;
        PlayerPrefs.SetInt("jumpDisabled", p_disabled ? 1 : 0);

        if (m_updateSliders) {
            m_uiManager.FindElement("jump").SetActive(!p_disabled);
        }
    }
}
