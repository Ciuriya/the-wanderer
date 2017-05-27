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
    public List<AudioSource> m_musicSources;  // All musical audio sources currently used in the game
    public List<AudioSource> m_effectSources; // All effect audio sources currently used in the game

    void Start() {
        m_gamePaused = false;
        m_manager = gameObject.GetComponent<GameManager>();
        m_playerStats = gameObject.AddComponent<PlayerStats>();
        m_uiManager = GameObject.FindWithTag("UI_Manager").GetComponent<UIManager>();

        // This is the main menu, so we want to reset everything for future use
        if (UIManager.FindElement("menu") != null) {
            PlayerStats.ResetStats();
        } else {
            m_inputController = gameObject.AddComponent<InputController>();
        }
	}
}
