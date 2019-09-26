using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRKeyboard.Utils;

public class TableManager : MonoBehaviour {

    public static TableManager instance;

    [Header("Panels")]
    [SerializeField]
    private Transform jobRolePanel;
    [SerializeField]
    private Transform jobFamilyPanel;
    [SerializeField]
    private Transform impactPanel;
    [SerializeField]
    private Transform skillsPanel;

    [Header("Panel Buttons")]
    [SerializeField]
    private ControlPanelButton[] controlPanelButtons;

    private bool isJobRolePanelVisible = false;
    private bool isJobFamilyPanelVisible = false;
    private bool isImpactPanelVisible = false;
    private bool isSkillsPanelVisible = false;

    private bool isSearchPanelVisible = false;
    private bool isJobFamiliesVisible = true;

    private bool isGameStarted = false;
    private bool isAnimationSkipped = false;

    private void Awake() {
        if (!instance) {
            instance = this;
        } else {
            Destroy(instance);
        }

        jobRolePanel.gameObject.SetActive(false);
        jobFamilyPanel.gameObject.SetActive(false);
        impactPanel.gameObject.SetActive(false);
        skillsPanel.gameObject.SetActive(false);
    }

    private IEnumerator ShowStartButtonCoroutine() {
        yield return new WaitForSeconds(2.0f);
    }

    public void ResetScene() {
        StartCoroutine(ResetSceneCoroutine());
    }

    private IEnumerator ResetSceneCoroutine() {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void ToggleJobRolePanel() {
        isJobRolePanelVisible = !isJobRolePanelVisible;

        if (isJobRolePanelVisible) {
            jobRolePanel.gameObject.SetActive(true);
            jobFamilyPanel.gameObject.SetActive(false);
            impactPanel.gameObject.SetActive(false);
            skillsPanel.gameObject.SetActive(false);

            isJobFamilyPanelVisible = false;
            isSkillsPanelVisible = false;
            isImpactPanelVisible = false;
        } else {
            jobRolePanel.gameObject.SetActive(false);
        }

        DataManager.instance.ClearFamilyList();
        DataManager.instance.ClearParameterLists();
        DataManager.instance.HighlightSelectedNodes();

        //KeyboardManager.instance.HideKeyboard();
    }

    public void ToggleJobFamilyPanel() {
        isJobFamilyPanelVisible = !isJobFamilyPanelVisible;

        if (isJobFamilyPanelVisible) {
            jobRolePanel.gameObject.SetActive(false);
            jobFamilyPanel.gameObject.SetActive(true);
            impactPanel.gameObject.SetActive(false);
            skillsPanel.gameObject.SetActive(false);

            isJobRolePanelVisible = false;
            isSkillsPanelVisible = false;
            isImpactPanelVisible = false;
        } else {
            jobFamilyPanel.gameObject.SetActive(false);
        }

        DataManager.instance.ClearFamilyList();
        DataManager.instance.ClearParameterLists();
        DataManager.instance.HighlightSelectedNodes();

        //KeyboardManager.instance.HideKeyboard();
    }

    public void ToggleImpactPanel() {
        isImpactPanelVisible = !isImpactPanelVisible;

        if (isImpactPanelVisible) {
            jobRolePanel.gameObject.SetActive(false);
            jobFamilyPanel.gameObject.SetActive(false);
            impactPanel.gameObject.SetActive(true);
            skillsPanel.gameObject.SetActive(false);

            isJobRolePanelVisible = false;
            isJobFamilyPanelVisible = false;
            isSkillsPanelVisible = false;
        } else {
            impactPanel.gameObject.SetActive(false);
        }

        DataManager.instance.ClearFamilyList();
        DataManager.instance.ClearParameterLists();
        DataManager.instance.HighlightSelectedNodes();

        //KeyboardManager.instance.HideKeyboard();
    }

    public void ToggleSkillsPanel() {
        isSkillsPanelVisible = !isSkillsPanelVisible;

        if (isSkillsPanelVisible) {
            jobRolePanel.gameObject.SetActive(false);
            jobFamilyPanel.gameObject.SetActive(false);
            impactPanel.gameObject.SetActive(false);
            skillsPanel.gameObject.SetActive(true);

            isJobRolePanelVisible = false;
            isJobFamilyPanelVisible = false;
            isImpactPanelVisible = false;

        } else {
            skillsPanel.gameObject.SetActive(false);
        }

        DataManager.instance.ClearFamilyList();
        DataManager.instance.ClearParameterLists();
        DataManager.instance.HighlightSelectedNodes();

        //KeyboardManager.instance.HideKeyboard();
    }
}
