using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    public int m_maxLife;               // The player's maximum life
    private int m_life;                 // The player's current life
    public float m_maxHeat;             // The player's maximum heat
    private float m_heatRate;           // The player's overheat/sec rate
    private float m_heat;               // The player's current heat
    public float m_initialHitCooldown;  // The player's initial hit cooldown time after being hit (in seconds)
    private float m_hitCooldown;        // The player's current cooldown time (in seconds)
    public float m_fireRate;            // The player's current firing rate/sec
    private float m_height;             // The player's current flying height
    public bool m_isStopped;            // If the player is currently stopped
    public bool m_isShooting;           // If the player is currently shooting
    public bool m_isJumping;            // If the player is currently jumping
    public bool m_isFlying;             // If the player is currently flying
    public bool m_isBoosting;           // If the player is currently boosting
    public bool m_boostDisabled;        // If the player's boosting is disabled
    public bool m_shootDisabled;        // If the player's shooting is disabled
    public bool m_heightDisabled;       // If the player's height meter is disabled
    public bool m_flyDisabled;          // If the player's flying is disabled
    public bool m_stopDisabled;         // If the player's stopping is disabled
    public bool m_jumpDisabled;         // If the player's jumping is disabled
    public bool m_updateSliders;        // Used by sliders to know whether or not sliders need to be updated
    private UIManager m_uiManager;      // A local instance of the UI Manager to update sliders

    void Start() {
        // values are saved in PlayerPrefs to allow for easy transfer between levels
        m_maxLife = PlayerPrefs.GetInt("maxlife", 1);
        m_life = PlayerPrefs.GetInt("life", 1);
        m_maxHeat = PlayerPrefs.GetFloat("maxHeat", 1f);
        m_heatRate = PlayerPrefs.GetFloat("heatRate", 0.1f);
        m_heat = PlayerPrefs.GetFloat("heat", 0f);
        m_fireRate = PlayerPrefs.GetFloat("fireRate", 1f);
        m_height = PlayerPrefs.GetFloat("height", 1f);
        m_isStopped = false;
        m_isShooting = false;
        m_isJumping = false;
        m_isFlying = false;
        m_isBoosting = false;
        m_boostDisabled = PlayerPrefs.GetInt("boostDisabled", 0) == 1;
        m_shootDisabled = PlayerPrefs.GetInt("shootDisabled", 0) == 1;
        m_heightDisabled = PlayerPrefs.GetInt("heightDisabled", 0) == 1;
        m_flyDisabled = PlayerPrefs.GetInt("flyDisabled", 0) == 1;
        m_stopDisabled = PlayerPrefs.GetInt("stopDisabled", 0) == 1;
        m_jumpDisabled = PlayerPrefs.GetInt("jumpDisabled", 0) == 1;
        m_uiManager = GameObject.FindWithTag("UI_Manager").GetComponent<UIManager>();
        m_updateSliders = true;

        if (m_uiManager.FindElement("cooling") != null) {
            Slider coolingSlider = m_uiManager.FindElement("cooling").GetComponent<Slider>();
            coolingSlider.value = m_heat;
            coolingSlider.maxValue = m_maxHeat;

            Slider heightSlider = m_uiManager.FindElement("height").GetComponent<Slider>();
            heightSlider.value = m_height;
            heightSlider.gameObject.SetActive(!m_heightDisabled);

            m_uiManager.FindElement("boost").SetActive(!m_boostDisabled);
            m_uiManager.FindElement("shoot").SetActive(!m_shootDisabled);
            m_uiManager.FindElement("fly").SetActive(!m_flyDisabled);
            m_uiManager.FindElement("stop").SetActive(!m_stopDisabled);
            m_uiManager.FindElement("jump").SetActive(!m_jumpDisabled);
        }
    }

    private void Update()
    {
        // Update the hit cooldown
        if (m_hitCooldown > 0f)
        {
            m_hitCooldown -= Time.deltaTime;
        }
    }

    public void ResetStats() {
        setMaxLife(1);
        setLife(1);
        setMaxHeat(1f);
        setHeatRate(0.1f);
        setHeat(0f);
        setFireRate(1f);
        setHeight(1f);
        m_isStopped = false;
        m_isShooting = false;
        m_isJumping = false;
        m_isFlying = false;
        m_isBoosting = false;
        setBoostDisabled(false);
        setHeightDisabled(false);
        setShootDisabled(false);
        setFlyDisabled(false);
        setStopDisabled(false);
        setJumpDisabled(false);
    }

    public int getCurrentLife() {
        return m_life;
    }

    protected void setMaxLife(int p_maxlife) {
        m_maxLife = p_maxlife;
        PlayerPrefs.SetInt("maxlife", p_maxlife);
    }

    protected void setLife(int p_life) {
        int life = p_life;

        if (p_life > m_maxLife) {
            life = m_maxLife;
        }

        m_life = life;
        PlayerPrefs.SetInt("life", life);
    }

    public void Damage(int p_amount) {
        if (m_hitCooldown <= 0 && p_amount > m_life) {
            p_amount = m_life;
            m_hitCooldown = m_initialHitCooldown;
        }

        m_life -= p_amount;

        if (m_life <= 0) {
            GameObject.FindWithTag("Player").GetComponent<Player>().Kill();
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

    protected void setHeat(float p_heat) {
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

    public float getHeight()
    {
        return m_height;
    }

    public void setHeight(float p_height) {
        m_height = p_height;
        PlayerPrefs.SetFloat("height", p_height);

        if (m_updateSliders) {
            m_uiManager.FindElement("height").GetComponent<Slider>().value = p_height;
        }
    }

    public void setBoostDisabled(bool p_disabled) {
        m_boostDisabled = p_disabled;
        PlayerPrefs.SetInt("boostDisabled", p_disabled ? 1 : 0);

        if (m_updateSliders) {
            m_uiManager.FindElement("boost").SetActive(!p_disabled);
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
