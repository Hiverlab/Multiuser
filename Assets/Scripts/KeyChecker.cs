using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRKeyboard.Utils;

public class KeyChecker : MonoBehaviour {
    [SerializeField]
    private OVRInput.Controller controller;

    private bool canCollide = true;

    // Start is called before the first frame update
    void Start() {
        //Debug.Log("KeyChecker started");
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        /*
        if (!canCollide) {
            return;
        }

        Debug.Log(name + " Collided with: " + other);

        // Invoke button click if collided with panel button
        if (other.tag == "PanelButton") {

            ParameterButton parameterButton = other.GetComponent<ParameterButton>();

            Debug.Log("Parameter button: " + parameterButton);

            // If is parameter button
            if (parameterButton != null) {
                // And not visible
                if (!parameterButton.GetIsVisible()) {
                    return;
                }

                DataManager.instance.OnButtonSelected?.Invoke(other.gameObject);
            }

            other.GetComponent<Button>().onClick.Invoke();

            StartCoroutine(ResetCanCollideCoroutine());

            Utilities.instance.VibrateController(100, 0.25f, 0.15f, controller);
        }

        // Invoke button click if collided with panel button
        if (other.tag == "SkillButton") {
            SkillButton skillButton = other.GetComponent<SkillButton>();

            // If is parameter button
            if (skillButton != null) {
                // And not visible
                if (!skillButton.GetIsVisible()) {
                    return;
                }

                DataManager.instance.OnButtonSelected?.Invoke(other.gameObject);
            }

            other.GetComponent<Button>().onClick.Invoke();

            StartCoroutine(ResetCanCollideCoroutine());

            Utilities.instance.VibrateController(100, 0.25f, 0.15f, controller);
        }

        if (other.name == "JobRoleCollider") {
            other.GetComponent<JobRoleModelCollider>().ShowSkillPanel();
            
            StartCoroutine(ResetCanCollideCoroutine());

            Utilities.instance.VibrateController(100, 0.25f, 0.15f, controller);
        }

        // If collided with input field
        if (other.tag == "InputField") {
            Debug.Log("Keyboard: " + KeyboardManager.instance);
            Debug.Log("Other: " + other.GetComponent<TMP_InputField>());

            KeyboardManager.instance.LinkKeyboardToInputField(other.GetComponent<TMP_InputField>());

            StartCoroutine(ResetCanCollideCoroutine());

            Utilities.instance.VibrateController(100, 0.25f, 0.15f, controller);
        }

        // Hide keyboard if not interacting with it
        if (other.tag != "InputField" && other.tag != "VRGazeInteractable" && other.tag != "PanelButton" && other.tag != "ScrollRectCollider") {
            //KeyboardManager.instance.HideKeyboard();
        }

        if (other.GetComponent<Key>()) {
            other.GetComponent<Key>().ClickKey();

            StartCoroutine(ResetCanCollideCoroutine());

            Utilities.instance.VibrateController(100, 0.25f, 0.15f, controller);
        }

        if (other.name.Equals("Back") || other.name.Equals("Clear")) {
            StartCoroutine(ResetCanCollideCoroutine());

            other.GetComponent<Button>().onClick.Invoke();

            Utilities.instance.VibrateController(100, 0.25f, 0.15f, controller);
        }
        */
    }

    private IEnumerator ResetCanCollideCoroutine() {
        canCollide = false;
        yield return new WaitForSeconds(0.5f);
        canCollide = true;
    }
}
