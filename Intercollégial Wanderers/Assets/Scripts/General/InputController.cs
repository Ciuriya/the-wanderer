using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GameObject.FindWithTag("UI_Manager").GetComponent<UIManager>().TogglePause();
        }
    }
}
