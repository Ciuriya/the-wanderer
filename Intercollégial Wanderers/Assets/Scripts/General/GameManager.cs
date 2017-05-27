using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    // The instance of the game's manager
    private static GameManager m_manager;
    public static GameManager Instance {
        get {
            return m_manager;
        }
    }

    // The instance of the player's stats
    private static PlayerStats m_playerStats;
    public static PlayerStats PlayerStats {
        get {
            return m_playerStats;
        }
    }

    // The instance of the player's input controller
    private static InputController m_inputController;
    public static InputController InputController {
        get {
            return m_inputController;
        }
    }

    public static bool m_gamePaused; // If game is currently paused

    void Start() {
        m_gamePaused = false;
        m_manager = gameObject.GetComponent<GameManager>();
        m_playerStats = gameObject.AddComponent<PlayerStats>();
        m_inputController = gameObject.AddComponent<InputController>();
	}
}
