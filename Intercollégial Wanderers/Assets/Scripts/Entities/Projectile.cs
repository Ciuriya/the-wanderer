using UnityEngine;
using System.Collections;
using System;

public class Projectile : Entity {
    public float m_speed;        // The speed at which the projectile travels
    public AudioClip m_hitSound; // The sound played on projectile hit

    void Start() {
        m_name = "projectile";
        m_canShoot = false;
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