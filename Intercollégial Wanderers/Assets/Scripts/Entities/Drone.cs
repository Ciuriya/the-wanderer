using UnityEngine;
using System.Collections;
using System;

public class Drone : Entity {
    Animator m_anim;

    void Start() {
        m_autoFire = true;
        m_canDie = true;
        m_fireRate = 1f;
        m_maxLife = 1;
        m_name = "drone";
        m_anim = GetComponent<Animator>();
    }

    protected override void Die() {
        m_anim.SetBool("IsDead", true);
        Destroy(gameObject, 0.5f);
    }
}
