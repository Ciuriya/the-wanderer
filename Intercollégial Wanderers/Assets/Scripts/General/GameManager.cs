using Player2D;
using System;
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

    private static UIManager m_uiManager;
    public static UIManager UIManager {
        get {
            return m_uiManager;
        }
    }
    
    public static bool m_gamePaused;          // If game is currently paused
    public AudioClip m_gameOverSound;         // Sound played when the game is over
    public AudioClip m_victorySound;          // Sound played when the game is won
    public int m_gameOverLength;              // Length of the game over fail time, to replace with a real menu if possible
    public List<AudioSource> m_musicSources;  // All musical audio sources currently used in the game
    public List<AudioSource> m_effectSources; // All effect audio sources currently used in the game
    public bool m_jumpDisabled;               // If jump is disabled for this stage
    public bool m_boostDisabled;              // If boost is disabled for this stage
    public bool m_shootDisabled;              // If shooting is disabled for this stage
    public bool m_flyDisabled;                // If flying is disabled for this stage

    void Start() {
        m_gamePaused = false;
        m_manager = gameObject.GetComponent<GameManager>();
        m_playerStats = gameObject.AddComponent<PlayerStats>();
        m_inputController = gameObject.AddComponent<InputController>();
        m_uiManager = GameObject.FindWithTag("UI_Manager").GetComponent<UIManager>();

        PlayerStats.ResetStats();

        m_playerStats.setJumpDisabled(m_jumpDisabled);
        m_playerStats.setBoostDisabled(m_boostDisabled);
        m_playerStats.setShootDisabled(m_shootDisabled);
        m_playerStats.setFlyDisabled(m_flyDisabled);
        m_playerStats.setHeightDisabled(m_flyDisabled);
    }
}
