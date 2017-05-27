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

        void Update()
        {
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigidbody.AddRelativeForce(new Vector2(0, m_jumpHeight), ForceMode2D.Impulse);
            }

            rigidbody.AddRelativeForce(new Vector2(m_acceleration, 0), ForceMode2D.Force);

            if (rigidbody.velocity.x > m_moveSpeed)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.normalized.x * m_moveSpeed, rigidbody.velocity.y);
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            Collider2D collider = collision.collider;
            PlayerStats stats = GetComponent<PlayerStats>();

            // If we touch an ennemy
            if (collider.tag == "Ennemy")
            {
                stats.Damage(1);
            }

            // If we collide from the side to an object
            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 center = collider.bounds.center;

            if (contactPoint.x > center.x)
            {
                stats.Damage(1);
            }
        }
    }
}