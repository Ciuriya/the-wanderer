using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Entity {

    public AudioClip m_jumpSound; // The player's jump sound
    private long m_deathTime;     // The player's death time

    void Start() {
        m_name = "Player";
        m_canShoot = true;
        m_fireRate = 1f;
        m_autoFire = false;
    }

    void Update() {
        long currentMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        // Retry
        if (m_deathTime > 0 && currentMillis - m_deathTime > GameManager.Instance.m_gameOverLength * 1000) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (PauseCheck()) return;

        m_canShoot = !GameManager.PlayerStats.m_shootDisabled;
        if (m_canShoot) {
            GetComponent<Animator>().SetBool("HasGun", true);
        }

        m_fireRate = GameManager.PlayerStats.m_fireRate;

        if (currentMillis - m_lastShot > m_fireRate * 1000 && GameManager.PlayerStats.m_isShooting) {
            GameManager.PlayerStats.m_isShooting = false;
            GameManager.UIManager.FindElement("shoot").GetComponent<Button>().interactable = true;
        }

        if (GameManager.PlayerStats.m_heat >= GameManager.PlayerStats.m_maxHeat) {
            Die();
        }
    }

    // Kills the entity
    protected override void Die() {
        if (m_canDie) {
            m_canDie = false;
            GameManager.PlayerStats.setMaxHeat(0f);
            GameManager.m_gamePaused = true;
            m_deathTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            GetComponent<Animator>().SetBool("IsDead", true);
            GameManager.Instance.m_musicSources[0].clip = GameManager.Instance.m_gameOverSound;
            GameManager.Instance.m_musicSources[0].Play(0);
        }
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
        if (m_canShoot && currentMillis - m_lastShot > m_fireRate * 1000 && !GameManager.PlayerStats.m_shootDisabled && m_projectile) {
            m_lastShot = currentMillis;

            GameObject bullet = Instantiate(m_projectile.gameObject, new Vector2(transform.position.x, transform.position.y + 0.2f), Quaternion.identity) as GameObject;

            bullet.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("effects");
            GameManager.Instance.m_effectSources.Add(bullet.GetComponent<AudioSource>());

            bullet.GetComponent<Projectile>().Shot();
            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(m_projectile.m_speed, 0));

            GetComponent<AudioSource>().clip = m_shootSound;
            GetComponent<AudioSource>().Play(0);

            GameManager.PlayerStats.increaseHeat();
        }
    }
}