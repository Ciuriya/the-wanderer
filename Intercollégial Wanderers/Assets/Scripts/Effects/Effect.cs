using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class Effect : MonoBehaviour {

    protected string m_name;                   // The effect's name
    public List<TriggerEvent> m_triggerEvents; // The events that will trigger this effect
    public int m_strength;                   // The strength of the effect, a multiplier
    public float m_triggerChance;              // Percentage of triggering on the affected entity
    public float m_cooldownLength;             // Time between effect triggers in seconds
    public Entity m_holder;                    // The holder of the effect, the giver
    private long m_lastTrigger;                // The time in milliseconds at which this effect last triggered

    void Start() {
        m_holder = GetComponent<Entity>();
    }

    // Handles the effect's application process
    public void Trigger(Entity p_affected) {
        long currentMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        // If the effect can trigger again AND passes the trigger chance, we trigger
        if (currentMillis - m_lastTrigger > m_cooldownLength * 1000 && UnityEngine.Random.Range(0, 100) <= m_triggerChance) {
            m_lastTrigger = currentMillis;
            Apply(p_affected);
        }
    }

    // Applies the effect to the affected entity
    protected abstract void Apply(Entity p_affected);

    // Attempts to trigger every effect in the given effect list
    public static void TriggerEffects(List<Effect> p_effects, Entity p_affected, TriggerEvent p_event) {
        foreach (Effect effect in p_effects) {
            if (effect.m_triggerEvents.Contains(p_event)) {
                effect.Trigger(p_affected);
            }
        }
    }
}
