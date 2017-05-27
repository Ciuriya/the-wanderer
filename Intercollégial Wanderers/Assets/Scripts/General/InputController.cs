using UnityEngine;
using System.Collections;
using Player2D;

public class InputController : MonoBehaviour {

    private Player m_player; // The player

    void Start() {
        m_player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GameManager.UIManager.TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }
    }

    public void Jump() {
        if (!GameManager.PlayerStats.m_isJumping) {
            PlayerController controller = m_player.gameObject.GetComponent<PlayerController>();
            Rigidbody2D rigidbody = m_player.gameObject.GetComponent<Rigidbody2D>();
            rigidbody.AddRelativeForce(new Vector2(0, controller.m_jumpHeight), ForceMode2D.Impulse);
            GameManager.PlayerStats.m_isJumping = true;
            GameManager.UIManager.FindElement("jump").SetActive(false);
        }
    }

    public Player GetPlayer() {
        return m_player;
    }
}
