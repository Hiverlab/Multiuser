using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public CustomEvents.UnityEventSingleFloat OnButtonClick;

    private bool isHovering = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (other.tag != "Hand") {
            return;
        }

        isHovering = true;

        Utilities.Log(name, "Enter");
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag != "Hand") {
            return;
        }

        isHovering = false;

        Utilities.Log(name, "Exit");
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag != "Hand") {
            return;
        }

        isHovering = false;

        Utilities.Log(name, "Staying");
    }

    public void ButtonClick() {
        OnButtonClick.Invoke(true);
    }

}
