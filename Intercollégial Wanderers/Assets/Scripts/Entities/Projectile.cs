﻿using UnityEngine;
using System.Collections;
using System;

public class Projectile : Entity {
    public float m_speed;         // The speed at which the projectile travels
    public AudioClip m_hitSound;  // The sound played on projectile hit
    private Vector3 m_start;      // The shot position

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

        // If the colliding object is an entity, trigger every contained effect that transfers via touch
        if (collider.gameObject.name != gameObject.name &&
            collider.gameObject.GetComponent<Entity>() != null) {
            Effect.TriggerEffects(m_effectsGivenOff, collider.gameObject.GetComponent<Entity>(), TriggerEvent.TOUCH);
        }

        GetComponent<AudioSource>().clip = m_shootSound;
        GetComponent<AudioSource>().enabled = true;
        GetComponent<AudioSource>().Play(0);

        Die();
    }

    // Kills the entity
    protected override void Die() {
        GameManager.Instance.m_effectSources.Remove(GetComponent<AudioSource>());
        Destroy(gameObject);
    }
}