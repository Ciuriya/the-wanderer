using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class Entity : MonoBehaviour {

    protected string m_name;                 // The entity's name
    protected float m_fireRate;              // The entity's fire rate
    protected bool m_canShoot;               // If the entity can shoot
    protected bool m_isDead;                 // If the entity is dead
    protected int m_life;                    // The entity's life
    public int m_maxLife;                    // The entity's max life
    public Projectile m_projectile;          // The projectile shot by the entity
    public bool m_canDie;                    // If the entity can die
    public bool m_autoFire;                  // If the entity auto-fires
    public AudioClip m_shootSound;           // Sound of the shooting of this entity
    protected long m_lastShot;               // The time of the entity's last shot
    public List<Effect> m_effectsGivenOff;   // Effects given off by this entity
    private Vector3 m_savedPosition;         // If the game is paused, the entity needs to save its location
    private Vector2 m_savedSpeed;            // If the game is paused, the entity needs to save its velocity

    void Start() {
        m_fireRate = 1f;
        m_canShoot = true;
        m_isDead = false;
        m_canDie = true;
        m_lastShot = 0;
        m_life = m_maxLife;
        m_autoFire = false;
    }

    void Update() {
        if (PauseCheck()) return;

        long currentMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        if (currentMillis - m_lastShot > m_fireRate * 1000 && m_autoFire) {
            Shoot();
        }
    }

    protected bool PauseCheck() {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        if (GameManager.m_gamePaused && m_savedSpeed == new Vector2(0, 0)) {
            m_savedPosition = transform.position;
            m_savedSpeed = rigidbody.velocity;
            rigidbody.velocity = new Vector2(0, 0);
            rigidbody.gravityScale = 0;

            return true;
        } else if (GameManager.m_gamePaused) {
            transform.position = m_savedPosition;

            return true;
        }

        if (m_savedSpeed != new Vector2(0, 0)) {
            rigidbody.velocity = m_savedSpeed;
            rigidbody.gravityScale = 1;
        }

        m_savedSpeed = new Vector2(0, 0);

        return false;
    }

    void OnCollisionEnter2D(Collision2D p_collision) {
        if (GameManager.m_gamePaused) {
            return;
        }

        Collider2D collider = p_collision.collider;

        // If the colliding object is an entity, trigger every contained effect that transfers via touch
        if (collider.gameObject.name != gameObject.name &&
            collider.gameObject.GetComponent<Entity>() != null) {
            Effect.TriggerEffects(m_effectsGivenOff, collider.gameObject.GetComponent<Entity>(), TriggerEvent.TOUCH);
        }
    }

    // Handles the entity's shooting process
    public virtual void Shoot() {
        long currentMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        // If the entity can fire again, we shoot (fireRate is in seconds)
        if (m_canShoot && currentMillis - m_lastShot > m_fireRate * 1000) {
            m_lastShot = currentMillis;

            GameObject bullet = Instantiate(m_projectile.gameObject, new Vector2(transform.position.x + 1f, transform.position.y), Quaternion.identity) as GameObject;
            bullet.GetComponent<Projectile>().Shot();
            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(m_projectile.m_speed, 0));

            AudioSource.PlayClipAtPoint(m_shootSound, transform.position);
        }
    }

    // Damages the entity
    public virtual void Damage(int p_amount) {
        if (m_life - p_amount <= 0) {
            Kill();
        }

        m_life -= p_amount;
    }

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
