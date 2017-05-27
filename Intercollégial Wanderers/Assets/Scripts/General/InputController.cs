using UnityEngine;
using System.Collections;
using Player2D;
using System;

public class InputController : MonoBehaviour {

    private Player m_player;               // The player
    private PlayerController m_controller; // The controller

    void Start() {
        m_player = GameObject.FindWithTag("Player").GetComponent<Player>();
        m_controller = m_player.gameObject.GetComponent<PlayerController>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }

        long currentMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        if (Input.GetKeyDown(KeyCode.Escape) && currentMillis - GameManager.UIManager.m_lastPause > GameManager.UIManager.m_pauseCooldown * 1000) {
            GameManager.UIManager.TogglePause();
        }
    }

    public void Jump() {
        if (!GameManager.PlayerStats.m_isJumping && m_controller.IsGrounded()) {
            GameManager.PlayerStats.m_isJumping = true;
            m_player.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, m_controller.m_jumpForce));
            GameManager.UIManager.FindElement("jump").SetActive(false);
        }
    }

    public Player GetPlayer() {
        return m_player;
    }
}
