using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Player2D
{
    [RequireComponent(typeof(PlayerStats), typeof(BoxCollider2D), typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public float m_acceleration;    // Acceleration of the player
        public float m_moveSpeed;       // Max movement speed
        public float m_jumpHeight;      // Speed of the jump
        private bool m_startedMoving;   // If the player started moving

        void Update()
        {
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

            if (rigidbody.velocity.y == 0) {
                GameManager.PlayerStats.m_isJumping = false;
                GameManager.UIManager.FindElement("jump").SetActive(true);
            }

            rigidbody.AddRelativeForce(new Vector2(m_acceleration, 0), ForceMode2D.Force);

            if (rigidbody.velocity.x > m_moveSpeed) {
                rigidbody.velocity = new Vector2(rigidbody.velocity.normalized.x * m_moveSpeed, rigidbody.velocity.y);
            }

            // We hit an object
            if (rigidbody.velocity.x == 0 && m_startedMoving && !GameManager.PlayerStats.m_isStopped) {
                GameManager.PlayerStats.Damage(1);
            } else if (!m_startedMoving) {
                m_startedMoving = true;
            }
        }
    }
}