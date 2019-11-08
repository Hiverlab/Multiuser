using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIParameterButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMesh;

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

    }

    private void OnButtonRelease() {

    }

    #endregion
}
