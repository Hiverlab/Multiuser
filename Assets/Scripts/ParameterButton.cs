using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParameterButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMesh;

    private string buttonName;

    private Collider collider;

    private bool isVisible;

    private Button button;

    private ColorBlock selectedColor;
    private ColorBlock normalColor;

    public enum PARAMETER_TYPE {
        JOB_ROLE,
        JOB_FAMILY,
        IMPACT,
        SKILL
    }

    [SerializeField]
    private PARAMETER_TYPE parameterType;

    private void Awake() {
    }

    // Start is called before the first frame update
    void Start() {
        button = GetComponent<Button>();
        collider = GetComponent<Collider>();
        isVisible = false;

        normalColor = button.colors;
        selectedColor = button.colors;
        selectedColor.normalColor = Color.white;

        DataManager.instance.OnButtonSelected += SelectButton;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.H)) {
            SelectButton(gameObject);
        }
    }

    public void OnClick() {
        buttonName = textMesh.text;

        Debug.Log("Clicking: " + buttonName + " Type: " + parameterType);

        switch (parameterType) {
            case PARAMETER_TYPE.JOB_ROLE:
                DataManager.instance.SelectJobRole(buttonName);
                break;
            case PARAMETER_TYPE.JOB_FAMILY:
                DataManager.instance.SelectJobFamily(buttonName);
                break;
            case PARAMETER_TYPE.IMPACT:
                DataManager.instance.SelectImpact(buttonName);
                break;
            case PARAMETER_TYPE.SKILL:
                DataManager.instance.SelectSkill(buttonName);
                break;
            default:
                break;
        }
    }

    public void OnHide() {
        buttonName = textMesh.text;

        switch (parameterType) {
            case PARAMETER_TYPE.JOB_ROLE:
                DataManager.instance.DeselectJobRole(buttonName);
                break;
            case PARAMETER_TYPE.JOB_FAMILY:
                DataManager.instance.DeselectJobFamily(buttonName);
                break;
            case PARAMETER_TYPE.IMPACT:
                DataManager.instance.DeseectImpact(buttonName);
                break;
            case PARAMETER_TYPE.SKILL:
                DataManager.instance.DeselectSkill(buttonName);
                break;
            default:
                break;
        }
    }
    
    private void OnTriggerStay(Collider other) {
        if (isVisible) {
            return;
        }

        if (other.tag == "ScrollRectCollider") {

            Debug.Log(transform.name + " collided with: " + other);
            isVisible = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "ScrollRectCollider") {
            isVisible = false;
        }
    }

    public bool GetIsVisible() {
        return isVisible;
    }

    private void SelectButton(GameObject _button) {
        //Debug.Log("Selecting button: " + _button + " true: " + (gameObject == _button));

        if (gameObject == _button) {
            button.colors = selectedColor;
            textMesh.color = Color.black;
        } else {
            if (button.colors != normalColor) {
                button.colors = normalColor;
                textMesh.color = Color.white;
            }
        }
    }
}
