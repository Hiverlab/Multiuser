using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField]
    private SkillsPanel skillsPanel;

    private TextMeshProUGUI skillsTitle;

    [SerializeField]
    private string skillDescription;
    
    private Collider collider;

    private bool isVisible;
    
    private Button button;

    private ColorBlock selectedColor;
    private ColorBlock normalColor;

    private void Awake() {
    }

    // Start is called before the first frame update
    void Start()
    {
        skillsTitle = GetComponentInChildren<TextMeshProUGUI>();
        
        button = GetComponent<Button>();
        collider = GetComponent<Collider>();
        isVisible = true;

        normalColor = button.colors;
        selectedColor = button.colors;
        selectedColor.normalColor = Color.white;

        DataManager.instance.OnButtonSelected += SelectButton;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) {
            SelectButton(gameObject);
        }
    }

    public void ShowSkillDescription() {
        skillsPanel.ShowSkill(skillsTitle.text, skillDescription);
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
            skillsTitle.color = Color.black;
        } else {
            if (button.colors != normalColor) {
                button.colors = normalColor;
                skillsTitle.color = Color.white;
            }
        }
    }
}
