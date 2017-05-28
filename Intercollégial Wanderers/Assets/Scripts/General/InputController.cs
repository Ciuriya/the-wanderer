using UnityEngine;
using System.Collections;
using Player2D;
using System;
using UnityEngine.UI;

public class InputController : MonoBehaviour {

    private Player m_player;               // The player
    private PlayerController m_controller; // The controller

    void Start() {
        if (GameManager.UIManager && GameManager.UIManager.FindElement("menu") == null) {
            m_player = GameObject.FindWithTag("Player").GetComponent<Player>();
            m_controller = m_player.gameObject.GetComponent<PlayerController>();
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && !GameManager.PlayerStats.m_jumpDisabled) {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.E) && !GameManager.PlayerStats.m_shootDisabled) {
            GameManager.UIManager.Shoot();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !GameManager.PlayerStats.m_boostDisabled) {
            GameManager.UIManager.Boost();
        }

        if (Input.GetKeyDown(KeyCode.Q) && !GameManager.PlayerStats.m_flyDisabled) {
            Fly();
        }

        if (GameManager.UIManager.FindElement("menu") == null) {
            Slider heightSlider = GameManager.UIManager.FindElement("height").GetComponent<Slider>();

            if (Input.GetKeyDown(KeyCode.W) && !GameManager.PlayerStats.m_flyDisabled) {
                float desired = heightSlider.value + 0.25f;

                if (desired > heightSlider.maxValue)
                {
                    desired = heightSlider.maxValue;
                }

                GameManager.UIManager.HeightSlider(desired);
            }

            if (Input.GetKeyDown(KeyCode.S) && !GameManager.PlayerStats.m_flyDisabled) {
                float desired = heightSlider.value - 0.25f;

                if (desired < heightSlider.minValue)
                {
                    desired = heightSlider.minValue;
                }

                GameManager.UIManager.HeightSlider(desired);
            }

        }

        long currentMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        if (Input.GetKeyDown(KeyCode.Escape) && currentMillis - GameManager.UIManager.m_lastPause > GameManager.UIManager.m_pauseCooldown * 1000) {
            GameManager.UIManager.TogglePause();
        }
    }

    void FixedUpdate() {
        if (GameManager.PlayerStats.m_isFlying) {
            float height = GameManager.PlayerStats.getHeight();
            float curHeight = m_player.transform.position.y;
            float diff = curHeight - height;

            m_player.GetComponent<Rigidbody2D>().AddForceAtPosition(new Vector2(0, -diff), new Vector2(m_player.transform.position.x, height), ForceMode2D.Force);
        }
    }

    public void Jump() {
        if (m_controller != null && !GameManager.PlayerStats.m_isJumping && m_controller.IsGrounded() &&
            !GameManager.PlayerStats.m_jumpDisabled && !GameManager.PlayerStats.m_isFlying) {
            GameManager.PlayerStats.m_isJumping = true;

            m_player.GetComponent<AudioSource>().clip = m_player.m_jumpSound;
            m_player.GetComponent<AudioSource>().Play();

            m_player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, m_controller.m_jumpForce));

            GameManager.UIManager.FindElement("jump").SetActive(false);
        }
    }

    public void Fly() {
        if (!GameManager.PlayerStats.m_isFlying && !GameManager.PlayerStats.m_flyDisabled) {
            GameManager.PlayerStats.m_isFlying = true;
            GameManager.UIManager.HeightSlider(m_player.transform.position.y + 2f);
            m_player.GetComponent<Rigidbody2D>().gravityScale = 0;
        } else if (GameManager.PlayerStats.m_isFlying && !GameManager.PlayerStats.m_flyDisabled) {
            m_player.GetComponent<Rigidbody2D>().gravityScale = 1;
            GameManager.PlayerStats.m_isFlying = false;
        }
    }

    public Player GetPlayer() {
        return m_player;
    }
}
