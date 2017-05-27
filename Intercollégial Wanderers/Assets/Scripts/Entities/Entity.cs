using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class Entity : MonoBehaviour {

    protected string m_name;                 // The entity's name
    protected float m_fireRate;              // The entity's fire rate
    protected bool m_canShoot;               // If the entity can shoot
    protected bool m_isDead;                 // If the entity is dead
    public bool m_canDie;                    // If the entity can die
    private long m_lastShot;                 // The time of the entity's last shot
    public List<Effect> m_effectsGivenOff;   // Effects given off by this entity

    void Start() {
        m_fireRate = 1f;
        m_canShoot = true;
        m_isDead = false;
        m_canDie = true;
        m_lastShot = 0;
    }

    void OnCollisionEnter(Collision p_collider) {
        // If the colliding object is an entity, trigger every contained effect that transfers via touch
        if (p_collider.gameObject.name != gameObject.name &&
            p_collider.gameObject.GetComponent<Entity>() != null) {
            Effect.TriggerEffects(m_effectsGivenOff, p_collider.gameObject.GetComponent<Entity>(), TriggerEvent.TOUCH);
        }
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
        if (m_canDie) {
            m_isDead = true;
            Die();
        }
    }

    // Kills the entity
    protected abstract void Die();
}
