using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    // The list of UI Elements to manage
    public List<UIElement> m_elements;

    // The method used by the speed slider to increment/decrement the speed value
    public void SpeedSlider(float p_value) {
        GameManager.PlayerStats.setSpeed(p_value);
    }

    // The method used by the height slider to increment/decrement the speed value
    public void HeightSlider(float p_value) {
        GameManager.PlayerStats.setHeight(p_value);
    }

    // The method used by the jump button to jump
    public void Jump() {
        // insert jump code here or link it to the variable
        FindElement("jump").SetActive(false);
        GameManager.PlayerStats.m_isJumping = true;
        // make sure to re-enable button later
    }

    // The method used by the stop button to stop
    public void Stop() {
        // insert stop code here or link it to the variable
        FindElement("stop").SetActive(false);
        GameManager.PlayerStats.m_isStopped = true;
        // make sure to re-enable button later
    }

    // The method used by the shoot button to shoot
    public void Shoot() {
        // insert shooting code here or link it to the variable
        FindElement("shoot").SetActive(false);
        GameManager.PlayerStats.m_isShooting = true;
        // make sure to re-enable button later
    }

    // The method used by the fly button to fly
    public void Fly() {
        // insert flying code here or link it to the variable
        FindElement("fly").SetActive(false);
        GameManager.PlayerStats.m_isFlying = true;
        // make sure to re-enable button later
    }

    // Finds an UI Element using its name
    public GameObject FindElement(string p_name) {
        UIElement element = m_elements.Find(e => e.m_name == p_name);

        if (element != null) {
            return element.gameObject;
        }

        return null;
    }

}
