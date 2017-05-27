using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;   // Reference to the player's game object
    private Vector3 offset;     // Offset distance between the player and this camera
    
    void Start() {
        offset = transform.position - player.transform.position;
    }
    
    void LateUpdate() {
        if (player != null) {
            transform.position = player.transform.position + offset;
        }
    }
}
