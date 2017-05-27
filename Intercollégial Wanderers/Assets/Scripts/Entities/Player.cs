using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Player : Entity {

    void Start() {
        m_name = "Player";
        m_canShoot = true;
        m_fireRate = 1f;
    }

    void Update() {
        m_canShoot = !GameManager.PlayerStats.m_shootDisabled;
        m_fireRate = GameManager.PlayerStats.m_fireRate;
    }

    // Kills the entity
    protected override void Die() {
        Destroy(gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    // Handles the entity's shooting process
    public override void Shoot()
    {
        long currentMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        // If the entity can fire again, we shoot (fireRate is in seconds)
        if (m_canShoot && currentMillis - m_lastShot > m_fireRate * 1000) {
            m_lastShot = currentMillis;

            UIManager uiManager = GameObject.FindWithTag("UI_Manager").GetComponent<UIManager>();
            uiManager.Shoot();
            GameObject bullet = Instantiate(m_projectile.gameObject, transform.position, Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 10);
            GameManager.PlayerStats.m_isShooting = false;
            uiManager.FindElement("shoot").SetActive(true);
        }
    }
}