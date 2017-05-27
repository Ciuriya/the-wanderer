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

    void Start() {
        m_manager = gameObject.GetComponent<GameManager>();
        m_playerStats = gameObject.AddComponent<PlayerStats>();
	}
}
