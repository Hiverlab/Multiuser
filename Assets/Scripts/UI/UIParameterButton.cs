using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIParameterButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMesh;

    [SerializeField]
    private Toggle toggle;

    [SerializeField]
    private UIToggle uiToggle;

    private string parameter;

    public string Parameter {
        get {
            return parameter;
        }
        set {
            parameter = value;

            SetText(parameter);
        }
    }

    public void SetText(string text) {
        textMesh.text = text;
    }

    public void SetToggleGroup(ToggleGroup toggleGroup) {
        if (!toggle) {
            return;
        }

        toggle.group = toggleGroup;
    }

    #region Touch interactions

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

        uiToggle.ExecuteOnButtonSelected();

        UIController.instance.SelectParameter(Parameter);
    }

    private void OnButtonRelease() {
        Debug.Log("On Button Release");

        /*
        // If this parameter is available
        if (UIController.instance.IsParameterAvailable(this)) {
            // Add to selected paramaters
            UIController.instance.AddToSelectedParameters(this);

            // Make sure to return so we don't turn it back to available
            return;
        }

        // If this parameter is selected
        if (UIController.instance.IsParameterSelected(this)) {
            // Add to available paramaters
            UIController.instance.AddToAvailableParameters(this);

            // Make sure to return so we don't turn it back to selected
            return;
        }
        */
    }

    #endregion
}
