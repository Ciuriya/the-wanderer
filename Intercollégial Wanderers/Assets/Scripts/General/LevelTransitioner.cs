using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using Player2D;

public class LevelTransitioner : MonoBehaviour {

    public string m_nextScene;      // The scene following this level.
    public int m_transitionDelay;   // The delay before actually transitioning
    private long m_transitionStart; // The time at which the transition started

    void Update() {
        long currentMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        if (m_transitionStart > 0 && currentMillis - m_transitionStart >= m_transitionDelay * 1000) {
            SwitchScenes();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Collider2D collider = collision.collider;

        if (collider.tag == "Player") {
            GetComponent<BoxCollider2D>().enabled = false;
            Transition();
        }
    }

    // Allows for an animation to take place before transitioning over to the next level
    public void Transition() {
        m_transitionStart = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        Player player = GameManager.InputController.GetPlayer();

        player.m_canDie = false;
        player.GetComponent<PlayerController>().m_moveForce = 0f;

        // insert cool transition animation here
    }

    // Switches the scene to the next scene after the transition period
    public void SwitchScenes() {
        SceneManager.LoadScene(m_nextScene);
    }
}
