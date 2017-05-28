using UnityEngine;
using System.Collections;
using System;

public class Drone : Entity {

    void Start() {
        m_autoFire = true;
        m_canDie = true;
        m_fireRate = 1f;
        m_maxLife = 1;
        m_name = "drone";
    }

    protected override void Die() {
        Destroy(gameObject);
    }
}
