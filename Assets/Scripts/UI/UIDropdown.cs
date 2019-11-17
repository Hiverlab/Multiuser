using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class UIDropdown : MonoBehaviour {
    [SerializeField]
    private TMP_Dropdown dropdown;

    private void OnTriggerEnter(Collider other) {
        if (other.tag.Contains("Key"))
        {
            OnButtonPress();

            Utilities.instance.VibrateController(100, 0.25f, 0.15f, other.tag);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag.Contains("Key"))
        {
            OnButtonRelease();

            Utilities.instance.VibrateController(100, 0.25f, 0.15f, other.tag);
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
