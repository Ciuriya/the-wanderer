using UnityEngine;
using System.Collections;
using System;

public class Projectile : Entity {
    public float m_speed;         // The speed at which the projectile travels
    public AudioClip m_hitSound;  // The sound played on projectile hit
    private Vector3 m_start;      // The shot position
    private Vector2 m_savedSpeed; // If the game is paused, the projectile needs to save its velocity

    void Start() {
        m_name = "projectile";
        m_canShoot = false;
    }

    void Update() {
        if (PauseCheck()) return;

        if (m_start != null && Math.Abs(transform.position.x - m_start.x) > 50) {
            Die();
        }
    }

    public void Shot() {
        m_start = transform.position;
    }

    void OnCollisionEnter2D(Collision2D p_collision) {
        Collider2D collider = p_collision.collider;

        GameManager.Instance.m_effectSources[0].clip = m_hitSound;
        GameManager.Instance.m_effectSources[0].Play(0);

        Die();
    }

    // Kills the entity
    protected override void Die() {
        Destroy(gameObject);
    }
}