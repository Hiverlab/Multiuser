using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class SkillsPanel : MonoBehaviour
{
    [SerializeField]
    private Transform skillDescriptionPanel;

    [SerializeField]
    private TextMeshProUGUI skillTitleTextMesh;
    [SerializeField]
    private TextMeshProUGUI skillDescriptionTextMesh;

    [SerializeField]
    private Transform closeButton;

    
    // Start is called before the first frame update
    void Start()
    {
        HideSkill();

        DataManager.instance.OnSkillPanelOpen += SkillPanelOpen;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SkillPanelOpen(GameObject _gameObject) {
        if (gameObject != _gameObject) {
            HideSkill();
        }
    }

    public void HideSkill() {
        skillTitleTextMesh.text = "";
        skillDescriptionTextMesh.text = "";
        skillDescriptionPanel.gameObject.SetActive(false);

        //closeButton.gameObject.SetActive(true);
    }

    public void ShowSkill(string skillTitle, string skillDescription) {
        DataManager.instance.OnSkillPanelOpen?.Invoke(gameObject);

        skillTitleTextMesh.text = "";
        skillDescriptionPanel.gameObject.SetActive(true);

        //skillTitleTextMesh.DOText(skillTitle, Utilities.animationSpeed);
        //skillDescriptionTextMesh.DOText(skillDescription, Utilities.animationSpeed);

        //closeButton.gameObject.SetActive(false);
    }
}
