﻿using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

namespace Player2D
{
    public class PlayerController : MonoBehaviour
    {
        private long m_lastMovementTime = 0;      // Last time the player moved

        [HideInInspector]
        public bool m_facingRight = true;         // For determining which way the player is currently facing.

        public float m_moveForce = 365f;          // Amount of force added to move the player left and right.
        public float m_maxSpeed = 5f;             // The fastest the player can travel in the x axis.
        public float m_maxFallSpeed = 10f;
        public AudioClip[] m_jumpClips;           // Array of clips for when the player jumps.
        public float m_jumpForce = 1000f;         // Amount of force added when the player jumps.

        private Transform m_groundCheck;          // A position marking where to check if the player is grounded.
        private bool m_grounded = false;          // Whether or not the player is grounded.
        private Animator m_anim;                  // Reference to the player's animator component.

        void Awake()
        {
            // Setting up references.
            m_groundCheck = transform.Find("groundChecker");
            m_anim = GetComponent<Animator>();
        }


        void Update()
        {
            // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
            m_grounded = Physics2D.Linecast(transform.position, m_groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        }

        void FixedUpdate()
        {
            if (GameManager.m_gamePaused || GameManager.m_timeSinceUnpause < 0.2f) {
                m_lastMovementTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                return;
            }

            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

            float speedBoost = (GameManager.PlayerStats.m_boostTime > 0 ? GameManager.PlayerStats.m_boostSpeedIncrement : 0);

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_anim.SetBool("IsJumping", !m_grounded);
            // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
            if (rigidbody.velocity.x < m_maxSpeed + speedBoost)
            {
                // ... add a force to the player.
                rigidbody.AddForce(Vector2.right * m_moveForce);
            }

            // If the player's horizontal velocity is greater than the maxSpeed...
            if (Mathf.Abs(rigidbody.velocity.x) > m_maxSpeed + speedBoost)
            {
                // Set the player's velocity to the maxSpeed in the X axis.
                rigidbody.velocity = new Vector2(Mathf.Sign(
                    rigidbody.velocity.x) * (m_maxSpeed + speedBoost),
                    rigidbody.velocity.y);
            }

            if (Mathf.Abs(rigidbody.velocity.y) < -m_maxFallSpeed)
            {
                // Set the player's velocity to the maxFallSpeed in the Y axis.
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, Mathf.Sign(rigidbody.velocity.y) * -m_maxFallSpeed);
            }

            if (m_grounded)
            {
                GameManager.PlayerStats.m_isJumping = false;
                GameManager.UIManager.FindElement("jump").GetComponent<Button>().interactable = true;
            }

            if (rigidbody.velocity.x > 0) {
                m_lastMovementTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            }

            long currentMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            // We hit an object
            if (currentMillis - m_lastMovementTime >= 50 && m_lastMovementTime != 0)
            {
                GameManager.PlayerStats.Damage(1);
            }
        }

        void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_facingRight = !m_facingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        public bool IsGrounded()
        {
            return m_grounded;
        }
    }
}