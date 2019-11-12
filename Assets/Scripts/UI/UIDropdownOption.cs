using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDropdownOption : MonoBehaviour {
    [SerializeField]
    private Toggle toggle;

    private void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("PlayerTouch")) {
            OnButtonPress();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag.Equals("PlayerTouch")) {
            OnButtonRelease();
        }
    }

    private void OnButtonPress() {
        Debug.Log("On Button Press");

        toggle.isOn = true;
    }

    private void OnButtonRelease() {
        Debug.Log("On Button Release");
    }
}
