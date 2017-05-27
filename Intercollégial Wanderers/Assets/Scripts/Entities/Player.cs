using UnityEngine;
using System.Collections;
using System;

public class Player : Entity {

    void Start() {
        m_name = "Player";
        m_canShoot = !GameManager.PlayerStats.m_shootDisabled;
        m_fireRate = GameManager.PlayerStats.m_fireRate;
    }

    void Update() {
        m_canShoot = !GameManager.PlayerStats.m_shootDisabled;
        m_fireRate = GameManager.PlayerStats.m_fireRate;
    }

    // Kills the entity
    protected override void Die() {
        Destroy(gameObject);
        // throw back to main menu or something
    }

    // Shoots the projectile
    protected override void ShootProjectile() {
        throw new NotImplementedException();
    }
}