using UnityEngine;
using System.Collections;
using Player2D;
using System;
using UnityEngine.UI;

public class InputController : MonoBehaviour {

    private Player m_player;                // The player
    private PlayerController m_controller;  // The controller
    private Vector2 m_lastFlyForce;         // The last fly force applied
    private bool m_flyHeatIncreased;        // If the fly heating kicked in at least once
    private float m_timeBeforeFlyHeat;      // Length of time before the next heat increase
    private float m_timeBeforeHeatDecrease; // Length of time before the next heat decrease

    void Start() {
        if (GameManager.UIManager && GameManager.UIManager.FindElement("menu") == null) {
            m_player = GameObject.FindWithTag("Player").GetComponent<Player>();
            m_controller = m_player.gameObject.GetComponent<PlayerController>();
        }

        m_timeBeforeHeatDecrease = 0;
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

        if (GameManager.UIManager.FindElement("menu") == null && !GameManager.PlayerStats.m_flyDisabled && GameManager.PlayerStats.m_isFlying) {
            Slider heightSlider = GameManager.UIManager.FindElement("height").GetComponent<Slider>();

            float desiredHeight = heightSlider.value;

            if (Input.GetKey(KeyCode.W)) {
                desiredHeight += 0.1f;

                if (desiredHeight > heightSlider.maxValue) {
                    desiredHeight = heightSlider.maxValue;
                }
            }

            if (Input.GetKey(KeyCode.S)) {
                desiredHeight -= 0.1f;

                if (desiredHeight < heightSlider.minValue) {
                    desiredHeight = heightSlider.minValue;
                }
            }

            GameManager.UIManager.HeightSlider(desiredHeight);
        }

        long currentMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        if (Input.GetKeyDown(KeyCode.Escape) && currentMillis - GameManager.UIManager.m_lastPause > GameManager.UIManager.m_pauseCooldown * 1000) {
            GameManager.UIManager.TogglePause();
        }
    }

    void FixedUpdate() {
        if (!GameManager.PlayerStats.m_isFlying && !GameManager.PlayerStats.m_isShooting && GameManager.PlayerStats.m_boostTime <= 0) {
            m_timeBeforeHeatDecrease -= Time.deltaTime * 1000;

            if (m_timeBeforeHeatDecrease <= 0) {
                GameManager.PlayerStats.decreaseHeat();
                m_timeBeforeHeatDecrease = 1000;
            }
        }

        if (GameManager.PlayerStats.m_isFlying && !GameManager.m_gamePaused) {
            if (!m_controller.IsGrounded()) {
                m_timeBeforeFlyHeat -= Time.deltaTime * 1000;

                if (m_timeBeforeFlyHeat <= 0) {
                    GameManager.PlayerStats.increaseHeat();
                    m_timeBeforeFlyHeat = 350;
                    m_flyHeatIncreased = true;
                }
            } else if (m_flyHeatIncreased || m_timeBeforeFlyHeat < 175) {
                Fly();
                return;
            }


            float height = GameManager.PlayerStats.getHeight();
            float curHeight = m_player.transform.position.y;
            float diff = curHeight - height;

            m_lastFlyForce = new Vector2(0, m_lastFlyForce.y - diff);
            m_player.GetComponent<Rigidbody2D>().AddForceAtPosition(new Vector2(0, -diff), new Vector2(m_player.transform.position.x, height), ForceMode2D.Force);
        }
    }

    public void Jump() {
        if (m_controller != null && !GameManager.PlayerStats.m_isJumping && m_controller.IsGrounded() &&
            !GameManager.PlayerStats.m_jumpDisabled && !GameManager.PlayerStats.m_isFlying && !GameManager.m_gamePaused) {
            GameManager.PlayerStats.m_isJumping = true;

            m_player.GetComponent<AudioSource>().clip = m_player.m_jumpSound;
            m_player.GetComponent<AudioSource>().Play();

            Vector2 velocity = m_player.GetComponent<Rigidbody2D>().velocity;
            velocity.y = 0;
            m_player.GetComponent<Rigidbody2D>().velocity = velocity;
            m_player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, m_controller.m_jumpForce));

            GameManager.UIManager.FindElement("jump").GetComponent<Button>().interactable = false;
        }
    }

    public void Fly() {
        if (!GameManager.PlayerStats.m_isFlying && !GameManager.PlayerStats.m_flyDisabled && !GameManager.m_gamePaused) {
            m_timeBeforeFlyHeat = 350;
            m_flyHeatIncreased = false;
            GameManager.PlayerStats.m_isFlying = true;
            GameManager.UIManager.FindElement("height").GetComponent<Slider>().interactable = true;

            GameManager.UIManager.HeightSlider(m_player.transform.position.y + 1f);

            m_player.GetComponent<Rigidbody2D>().gravityScale = 0;
            GameManager.PlayerStats.increaseHeat();
        } else if (GameManager.PlayerStats.m_isFlying && !GameManager.PlayerStats.m_flyDisabled && !GameManager.m_gamePaused) {
            m_player.GetComponent<Rigidbody2D>().gravityScale = 1;
            GameManager.PlayerStats.m_isFlying = false;
            GameManager.UIManager.FindElement("height").GetComponent<Slider>().interactable = false;
        }
    }

    public Player GetPlayer() {
        return m_player;
    }
}
