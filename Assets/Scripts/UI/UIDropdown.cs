using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class UIDropdown : MonoBehaviour {
    [SerializeField]
    private TMP_Dropdown dropdown;

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

        dropdown.Show();
    }

    private void OnButtonRelease() {
        Debug.Log("On Button Release");
    }
}
