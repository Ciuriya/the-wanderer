using UnityEngine;
using System.Collections;
using System;

public class DamageEffect : Effect {

    void Start() {
        m_name = "damage";
    }

    // Applies the effect to the affected entity
    protected override void Apply(Entity p_affected) {
        p_affected.Damage(m_strength);
    }
}