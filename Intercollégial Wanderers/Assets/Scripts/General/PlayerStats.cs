using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public float m_maxHealth; // The player's maximum health
    public float m_health;    // The player's current health
    public float m_maxSpeed;  // The player's maximum speed
    public float m_speed;     // The player's current speed
    public float m_maxHeat;   // The player's maximum heat
    public float m_heatRate;  // The player's overheat/sec rate
    public float m_heat;      // The player's current heat
    public float m_fireRate;  // The player's current firing rate/sec
    public float m_height;    // The player's current flying height
    public bool m_isStopped;  // If the player is currently stopped
    public bool m_isShooting; // If the player is currently shooting
    public bool m_isJumping;  // If the player is currently jumping
    public bool m_isFlying;   // If the player is currently flying

    void Start() {
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
    }

    public void setSpeed(float p_speed) {
        float speed = p_speed;

        if (p_speed > m_maxSpeed) {
            speed = m_maxSpeed;
        }

        m_speed = speed;
        PlayerPrefs.SetFloat("speed", speed);
    }

    public void setMaxHeat(float p_maxHeat) {
        m_maxHeat = p_maxHeat;
        PlayerPrefs.SetFloat("maxHeat", p_maxHeat);
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
    }

    public void setFireRate(float p_fireRate) {
        m_fireRate = p_fireRate;
        PlayerPrefs.SetFloat("fireRate", p_fireRate);
    }

    public void setHeight(float p_height) {
        m_height = p_height;
        PlayerPrefs.SetFloat("height", p_height);
    }
}
