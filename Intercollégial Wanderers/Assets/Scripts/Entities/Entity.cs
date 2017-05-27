using UnityEngine;
using System.Collections;
using System;

public abstract class Entity : MonoBehaviour {

    public string m_name;    // The entity's name
    public float m_fireRate; // The entity's fire rate
    public bool m_canShoot;  // If the entity can shoot
    public bool m_isDead;    // If the entity is dead
    private long m_lastShot; // The time of the entity's last shot

    void Start() {
        m_fireRate = 1f;
        m_canShoot = true;
        m_isDead = false;
        m_lastShot = 0;
    }

    // Handles the entity's shooting process
    public void Shoot() {
        long currentMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        // If the entity can fire again, we shoot (fireRate is in seconds)
        if (m_canShoot && currentMillis - m_lastShot > m_fireRate * 1000) {
            m_lastShot = currentMillis;
            ShootProjectile();
        }
    }

    // Shoots the projectile
    protected abstract void ShootProjectile();

    // Kills the entity
    public void Kill() {
        m_isDead = true;
        Die();
    }

    // Kills the entity
    protected abstract void Die();

}
