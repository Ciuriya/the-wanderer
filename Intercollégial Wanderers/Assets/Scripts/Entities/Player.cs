using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Player : Entity {

    void Start() {
        m_name = "Player";
        m_canShoot = true;
        m_fireRate = 1f;
        m_autoFire = false;
    }

    void Update() {
        m_canShoot = !GameManager.PlayerStats.m_shootDisabled;
        m_fireRate = GameManager.PlayerStats.m_fireRate;

        long currentMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        if (currentMillis - m_lastShot > m_fireRate * 1000 && GameManager.PlayerStats.m_isShooting) {
            GameManager.PlayerStats.m_isShooting = false;
            GameManager.UIManager.FindElement("shoot").SetActive(true);
        }
    }

    // Kills the entity
    protected override void Die() {
        Destroy(gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    // Damages the entity
    public override void Damage(int p_amount) {
        GameManager.PlayerStats.Damage(p_amount);
    }

    // Handles the entity's shooting process
    public override void Shoot()
    {
        long currentMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        // If the entity can fire again, we shoot (fireRate is in seconds)
        if (m_canShoot && currentMillis - m_lastShot > m_fireRate * 1000) {
            m_lastShot = currentMillis;

            GameObject bullet = Instantiate(m_projectile.gameObject, new Vector2(transform.position.x + 1f, transform.position.y + 0.2f), Quaternion.identity) as GameObject;
            bullet.GetComponent<Projectile>().Shot();
            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(m_projectile.m_speed, 0));

            AudioSource.PlayClipAtPoint(m_shootSound, transform.position);
        }
    }
}