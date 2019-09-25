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

    [SerializeField]
    private Transform jobFamily1;
    [SerializeField]
    private Transform jobFamily2;
    [SerializeField]
    private Transform jobFamily3;

    private OfficeFunction frontOfficeFunction;
    private OfficeFunction middleOfficeFunction;
    private OfficeFunction backOfficeFunction;

    [SerializeField]
    private TextMeshProUGUI tableTextMeshPro;

    [SerializeField]
    private Light tableLight;
    [SerializeField]
    private Color normalLight;
    [SerializeField]
    private Color dangerLight;

    [Header("Idle objects")]
    [SerializeField]
    private Transform tableCanvas;
    [SerializeField]
    private Transform water;
    [SerializeField]
    private Image idlePanel;
    [SerializeField]
    private TextMeshProUGUI idleText;
    [SerializeField]
    private LineRenderer idleLineRenderer;
    [SerializeField]
    private Sprite messagePanelSprite;
    [SerializeField]
    private TextMeshPro startText;

    [Header("Panels")]
    [SerializeField]
    private Transform jobRolePanel;
    [SerializeField]
    private Transform jobFamilyPanel;
    [SerializeField]
    private Transform impactPanel;
    [SerializeField]
    private Transform skillsPanel;

    [SerializeField]
    private Transform dataAnalyticsPanel;
    [SerializeField]
    private Transform cyberRiskPanel;
    [SerializeField]
    private Transform ecosystemPanel;
    [SerializeField]
    private Transform interpersonalPanel;

    [SerializeField]
    private Transform legendPanel;
    [SerializeField]
    private Transform restartPanel;
    [SerializeField]
    private Transform skillsTaxonomyPanel;

    [Header("Panel Buttons")]
    [SerializeField]
    private ControlPanelButton[] controlPanelButtons;
    [SerializeField]
    private GameObject[] sortButtons;

    [Header("Animated objects")]
    [SerializeField]
    private AnimatedSprite hiverlabLogo;
    [SerializeField]
    private AnimatedSprite animatedIntroPanel;

    [SerializeField]
    private AnimatedSprite animatedFrontOffice;
    [SerializeField]
    private AnimatedSprite animatedMiddleOffice;
    [SerializeField]
    private AnimatedSprite animatedBackOffice;

    [SerializeField]
    private AnimatedSprite animatedFrontOfficeCorrupted;
    [SerializeField]
    private AnimatedSprite animatedMiddleOfficeCorrupted;
    [SerializeField]
    private AnimatedSprite animatedBackOfficeCorrupted;

    [SerializeField]
    private AnimatedSprite animatedWarningText;
    [SerializeField]
    private SpriteRenderer warningGrid;

    [SerializeField]
    private AnimatedSprite animatedAnalytics;
    [SerializeField]
    private AnimatedSprite animatedRobotics;
    [SerializeField]
    private AnimatedSprite animatedAI;

    private bool isJobRolePanelVisible = false;
    private bool isJobFamilyPanelVisible = false;
    private bool isImpactPanelVisible = false;
    private bool isSkillsPanelVisible = false;

    private bool isDataAnalyticsPanelVisible = false;
    private bool isCyberRiskPanelVisible = false;
    private bool isEcosystemPanelVisible = false;
    private bool isInterpersonalPanelVisible = false;

    private bool isLegendPanelVisible = false;
    private bool isRestartPanelVisible = false;

    private bool isSkillsTaxonomyPanelVisible = false;

    private bool isSearchPanelVisible = false;
    private bool isJobFamiliesVisible = true;

    private bool isGameStarted = false;
    private bool isAnimationSkipped = false;

    private List<string> openingLines;
    private int linesIndex = 0;

    [SerializeField]
    private PanelGazeChecker panelGazeChecker;

    [SerializeField]
    private Transform panelButtons;

    [SerializeField]
    private OVRScreenFade ovrScreenFade;

    private void Awake() {
        if (!instance) {
            instance = this;
        } else {
            Destroy(instance);
        }

        //DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start() {
        PopulateOpeningLines();

        //HideTextPanel(true);
        HideJobFamilies();

        frontOfficeFunction = jobFamily1.GetComponent<OfficeFunction>();
        middleOfficeFunction = jobFamily2.GetComponent<OfficeFunction>();
        backOfficeFunction = jobFamily3.GetComponent<OfficeFunction>();

        //QualitySettings.antiAliasing = 4;
    }

    public void ShowStartButton() {

        controlPanelButtons[0].ShowButton();
        
        //StartCoroutine(ShowStartButtonCoroutine());
    }

    private IEnumerator ShowStartButtonCoroutine() {
        yield return new WaitForSeconds(2.0f);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            //ShowJobFamilies();
            StartOpeningSequence();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            ResetScene();
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            SkipAnimation();
        }
    }

    public void ResetScene() {
        StartCoroutine(ResetSceneCoroutine());
    }

    private IEnumerator ResetSceneCoroutine() {
        ovrScreenFade.FadeOut();
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void PopulateOpeningLines() {
        openingLines = new List<string>();
        openingLines.Add("At OCBC, people are the anchor of everything we do. We are committed to investing in our 30,000 employees globally.");
        openingLines.Add("The technology-driven evolution of the labour market carries significant implications for businesses and workers. Job roles will change, and so will the skillsets needed for them.");
        openingLines.Add("How can you prepare yourself for the change? Explore the data to find out more!");
    }

    private void HideJobFamilies() {
        if (!isJobFamiliesVisible) {
            return;
        }
        isJobFamiliesVisible = false;

        jobFamily1.DOLocalMoveY(-1.0f, 0).SetEase(Ease.InOutBack).SetRelative();
        jobFamily2.DOLocalMoveY(-1.0f, 0).SetEase(Ease.InOutBack).SetRelative();
        jobFamily3.DOLocalMoveY(-1.0f, 0).SetEase(Ease.InOutBack).SetRelative();

        jobFamily1.DOScale(0.1f, 0).SetEase(Ease.InOutBack);
        jobFamily2.DOScale(0.1f, 0).SetEase(Ease.InOutBack);
        jobFamily3.DOScale(0.1f, 0).SetEase(Ease.InOutBack);

        hiverlabLogo.gameObject.SetActive(false);

        animatedFrontOfficeCorrupted.gameObject.SetActive(false);
        animatedMiddleOfficeCorrupted.gameObject.SetActive(false);
        animatedBackOfficeCorrupted.gameObject.SetActive(false);


        warningGrid.transform.DOScale(0, 0);
        warningGrid.gameObject.SetActive(false);
        animatedWarningText.gameObject.SetActive(false);

        animatedAnalytics.gameObject.SetActive(false);
        animatedRobotics.gameObject.SetActive(false);
        animatedAI.gameObject.SetActive(false);

        idleLineRenderer.transform.DOLocalMoveY(-1.5f, 0.0f);
        idlePanel.fillAmount = 0.0f;

        panelButtons.transform.localPosition = new Vector3(0, 1.05f, 0.55f);
        panelButtons.transform.localEulerAngles = new Vector3(-40, 0, 0);

        startText.DOFade(0, 0).OnComplete(() => {
            startText.DOFade(1.0f, Utilities.animationSpeed).SetDelay(1.0f);
            startText.transform.DOLocalMoveY(0.05f, Utilities.animationSpeed * 2).SetRelative().SetLoops(-1, LoopType.Yoyo);
        });
    }

    private void ShowJobFamilies() {
        if (isJobFamiliesVisible) {
            HideJobFamilies();
            return;
        }
        isJobFamiliesVisible = true;

        jobFamily1.DOLocalMoveY(1.0f, Utilities.GetRandomAnimationSpeed() * 2).SetEase(Ease.InOutBack).SetRelative();
        jobFamily2.DOLocalMoveY(1.0f, Utilities.GetRandomAnimationSpeed() * 2).SetEase(Ease.InOutBack).SetRelative();
        jobFamily3.DOLocalMoveY(1.0f, Utilities.GetRandomAnimationSpeed() * 2).SetEase(Ease.InOutBack).SetRelative();

        jobFamily1.DOScale(1, Utilities.GetRandomAnimationSpeed() * 2).SetEase(Ease.InOutBack);
        jobFamily2.DOScale(1, Utilities.GetRandomAnimationSpeed() * 2).SetEase(Ease.InOutBack);
        jobFamily3.DOScale(1, Utilities.GetRandomAnimationSpeed() * 2).SetEase(Ease.InOutBack);
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

        KeyboardManager.instance.HideKeyboard();
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

        KeyboardManager.instance.HideKeyboard();
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

        KeyboardManager.instance.HideKeyboard();
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

        KeyboardManager.instance.HideKeyboard();
    }

    public void ToggleDataAnalyticsPanel() {
        isDataAnalyticsPanelVisible = !isDataAnalyticsPanelVisible;

        if (isDataAnalyticsPanelVisible) {
            dataAnalyticsPanel.gameObject.SetActive(true);
            cyberRiskPanel.gameObject.SetActive(false);
            ecosystemPanel.gameObject.SetActive(false);
            interpersonalPanel.gameObject.SetActive(false);

            isCyberRiskPanelVisible = false;
            isEcosystemPanelVisible = false;
            isInterpersonalPanelVisible = false;
        } else {
            dataAnalyticsPanel.gameObject.SetActive(false);
        }

        KeyboardManager.instance.HideKeyboard();
    }

    public void ToggleCyberRiskPanel() {
        isCyberRiskPanelVisible = !isCyberRiskPanelVisible;

        if (isCyberRiskPanelVisible) {
            dataAnalyticsPanel.gameObject.SetActive(false);
            cyberRiskPanel.gameObject.SetActive(true);
            ecosystemPanel.gameObject.SetActive(false);
            interpersonalPanel.gameObject.SetActive(false);

            isDataAnalyticsPanelVisible = false;
            isEcosystemPanelVisible = false;
            isInterpersonalPanelVisible = false;
        } else {
            cyberRiskPanel.gameObject.SetActive(false);
        }

        KeyboardManager.instance.HideKeyboard();
    }

    public void ToggleEcosystemPanel() {
        isEcosystemPanelVisible = !isEcosystemPanelVisible;

        if (isEcosystemPanelVisible) {
            dataAnalyticsPanel.gameObject.SetActive(false);
            cyberRiskPanel.gameObject.SetActive(false);
            ecosystemPanel.gameObject.SetActive(true);
            interpersonalPanel.gameObject.SetActive(false);

            isDataAnalyticsPanelVisible = false;
            isCyberRiskPanelVisible = false;
            isInterpersonalPanelVisible = false;
        } else {
            ecosystemPanel.gameObject.SetActive(false);
        }

        KeyboardManager.instance.HideKeyboard();
    }

    public void ToggleInterpersonalPanel() {
        isInterpersonalPanelVisible = !isInterpersonalPanelVisible;

        if (isInterpersonalPanelVisible) {
            dataAnalyticsPanel.gameObject.SetActive(false);
            cyberRiskPanel.gameObject.SetActive(false);
            ecosystemPanel.gameObject.SetActive(false);
            interpersonalPanel.gameObject.SetActive(true);

            isDataAnalyticsPanelVisible = false;
            isCyberRiskPanelVisible = false;
            isEcosystemPanelVisible = false;
        } else {
            interpersonalPanel.gameObject.SetActive(false);
        }

        KeyboardManager.instance.HideKeyboard();
    }

    public void ToggleLegendPanel() {
        isLegendPanelVisible = !isLegendPanelVisible;

        if (isLegendPanelVisible) {
            legendPanel.gameObject.SetActive(true);
        } else {
            legendPanel.gameObject.SetActive(false);
        }

        KeyboardManager.instance.HideKeyboard();
    }

    public void ToggleRestartPanel() {
        isRestartPanelVisible = !isRestartPanelVisible;

        if (isRestartPanelVisible) {
            restartPanel.gameObject.SetActive(true);
        } else {
            restartPanel.gameObject.SetActive(false);
        }

        KeyboardManager.instance.HideKeyboard();
    }

    public void ToggleSkillsTaxonomyPanel() {
        isSkillsTaxonomyPanelVisible = !isSkillsTaxonomyPanelVisible;

        if (isSkillsTaxonomyPanelVisible) {
            skillsTaxonomyPanel.gameObject.SetActive(true);
        } else {
            skillsTaxonomyPanel.gameObject.SetActive(false);
        }

        KeyboardManager.instance.HideKeyboard();
    }

    public void ShowSortButtons() {
        sortButtons[1].SetActive(true);
        sortButtons[2].SetActive(true);
        sortButtons[3].SetActive(true);
    }

    public void HideSortButtons() {
        sortButtons[1].SetActive(false);
        sortButtons[2].SetActive(false);
        sortButtons[3].SetActive(false);
    }

    public void StartOpeningSequence() {
        if (isGameStarted) {
            return;
        }

        isGameStarted = true;

        controlPanelButtons[0].DOKill();
        controlPanelButtons[0].HideButton();

        SoundEffectsManager.instance.PlayAudioClip(SoundEffectsManager.SFX.POWER_START);

        StartCoroutine(StartOpeningSequenceCoroutine());
    }

    private IEnumerator StartOpeningSequenceCoroutine() {

        startText.DOFade(0, Utilities.animationSpeed).OnComplete(() => {
            startText.gameObject.SetActive(false);
        });

        yield return new WaitForSeconds(Utilities.animationSpeed * 2);
        
        controlPanelButtons[controlPanelButtons.Length - 1].ShowButton();

        //panelGazeChecker.PlayPanelAnimation();
        //yield return new WaitForSeconds(1.0f);

        hiverlabLogo.gameObject.SetActive(true);
        hiverlabLogo.Play();

        yield return new WaitForSeconds(3.0f);

        hiverlabLogo.Reverse();

        yield return new WaitForSeconds(2.0f);

        hiverlabLogo.gameObject.SetActive(false);

        // Show line
        idleLineRenderer.transform.DOLocalMoveY(0.25f, Utilities.animationSpeed);
        yield return new WaitForSeconds(Utilities.animationSpeed);
        idlePanel.DOFillAmount(1.0f, Utilities.animationSpeed);
        yield return new WaitForSeconds(Utilities.animationSpeed);

        animatedIntroPanel.Play();

        yield return new WaitForSeconds(5.0f);

        // Hide idle table objects and line
        idleText.DOFade(0, Utilities.animationSpeed * 0.5f);
        idlePanel.DOFillAmount(0, Utilities.animationSpeed);
        yield return new WaitForSeconds(Utilities.animationSpeed);
        idleLineRenderer.transform.DOLocalMoveY(-1.5f, Utilities.animationSpeed);

        yield return new WaitForSeconds(Utilities.animationSpeed * 0.25f);

        // Move canvas forward
        // tableCanvas.DOLocalMoveZ(-1.5f, 0);

        // Hide text
        idleText.text = "";

        // Spread water
        //water.DOScale(1.15f, Utilities.animationSpeed).SetEase(Ease.OutBack);
        water.DOScale(1.1f, Utilities.animationSpeed).SetEase(Ease.InBack);

        SoundEffectsManager.instance.PlayAmbientSound();

        yield return new WaitForSeconds(Utilities.animationSpeed * 0.25f);

        water.DOLocalMoveY(-1.0f, Utilities.animationSpeed);
        //water.DOLocalMoveY(-1.215f, Utilities.animationSpeed);

        yield return new WaitForSeconds(2.0f);
        //ShowJobFamilies();

        jobFamily1.DOLocalMoveY(1.0f, Utilities.GetRandomAnimationSpeed() * 1).SetEase(Ease.Linear).SetRelative();
        jobFamily1.DOScale(1, Utilities.GetRandomAnimationSpeed() * 1).SetEase(Ease.Linear);
        yield return new WaitForSeconds(Utilities.animationSpeed);
        animatedFrontOffice.Play();
        SoundEffectsManager.instance.PlayAudioClip(SoundEffectsManager.SFX.OFFICE, 2.0f);

        yield return new WaitForSeconds(Utilities.animationSpeed);

        jobFamily2.DOLocalMoveY(1.0f, Utilities.GetRandomAnimationSpeed() * 1).SetEase(Ease.Linear).SetRelative();
        jobFamily2.DOScale(1, Utilities.GetRandomAnimationSpeed() * 1).SetEase(Ease.Linear);
        yield return new WaitForSeconds(Utilities.animationSpeed);
        animatedMiddleOffice.Play();
        SoundEffectsManager.instance.PlayAudioClip(SoundEffectsManager.SFX.OFFICE, 2.0f);

        yield return new WaitForSeconds(Utilities.animationSpeed);

        jobFamily3.DOLocalMoveY(1.0f, Utilities.GetRandomAnimationSpeed() * 1).SetEase(Ease.Linear).SetRelative();
        jobFamily3.DOScale(1, Utilities.GetRandomAnimationSpeed() * 1).SetEase(Ease.Linear);
        yield return new WaitForSeconds(Utilities.animationSpeed);
        animatedBackOffice.Play();
        SoundEffectsManager.instance.PlayAudioClip(SoundEffectsManager.SFX.OFFICE, 2.0f);

        yield return new WaitForSeconds(2.0f);

        // Show line
        idleLineRenderer.transform.DOLocalMoveY(0.25f, Utilities.animationSpeed);

        yield return new WaitForSeconds(Utilities.animationSpeed);

        idlePanel.sprite = messagePanelSprite;

        idlePanel.DOFillAmount(1, Utilities.animationSpeed);

        idleText.DOFade(1, Utilities.animationSpeed);

        yield return new WaitForSeconds(1.0f);

        tableTextMeshPro.DOText(openingLines[0], 2.0f);
        SoundEffectsManager.instance.PlayAudioClip(SoundEffectsManager.SFX.MESSAGE, 1.0f);

        yield return new WaitForSeconds(8.0f);
        
        // Hide line
        idleText.DOFade(0, Utilities.animationSpeed * 0.5f);
        idlePanel.DOFillAmount(0, Utilities.animationSpeed);
        yield return new WaitForSeconds(Utilities.animationSpeed);
        idleLineRenderer.transform.DOLocalMoveY(-1.5f, Utilities.animationSpeed);
        yield return new WaitForSeconds(Utilities.animationSpeed);

        // Show warning text
        warningGrid.gameObject.SetActive(true);
        warningGrid.transform.DOScale(0.35f, Utilities.animationSpeed);

        SoundEffectsManager.instance.PlayAudioClip(SoundEffectsManager.SFX.NOTIFICATION_WARNING, 4.0f);

        yield return new WaitForSeconds(Utilities.animationSpeed);
        animatedWarningText.gameObject.SetActive(true);
        animatedWarningText.Play();

        // Fade out water
        //water.GetComponent<Renderer>().material.DOFade(0.0f, Utilities.animationSpeed);

        //DOTween.To(() => water.GetComponent<Renderer>().material.GetFloat("_Alpha"), x => water.GetComponent<Renderer>().material.SetFloat("_Alpha", x), 0.0f, Utilities.animationSpeed);

        yield return new WaitForSeconds(5.0f);

        SoundEffectsManager.instance.PlayDistortedAmbientSound();

        animatedFrontOfficeCorrupted.gameObject.SetActive(true);
        animatedMiddleOfficeCorrupted.gameObject.SetActive(true);
        animatedBackOfficeCorrupted.gameObject.SetActive(true);

        animatedFrontOffice.gameObject.SetActive(false);
        animatedMiddleOffice.gameObject.SetActive(false);
        animatedBackOffice.gameObject.SetActive(false);

        animatedFrontOfficeCorrupted.Play();
        animatedMiddleOfficeCorrupted.Play();
        animatedBackOfficeCorrupted.Play();

        SoundEffectsManager.instance.PlayAudioClip(SoundEffectsManager.SFX.AFFECTED_TEXT, 6.0f);

        tableLight.DOColor(dangerLight, Utilities.animationSpeed * 2.0f);

        water.GetComponent<Renderer>().material.SetFloat("Vector1_39047021", 10);

        water.GetComponent<Renderer>().material.SetColor("_EmissionColor", dangerLight);

        // Fade water back in
        //DOTween.To(() => water.GetComponent<Renderer>().material.GetFloat("_Alpha"), x => water.GetComponent<Renderer>().material.SetFloat("_Alpha", x), 1.0f, Utilities.animationSpeed);

        //wavesParticleSytem.Play();
        yield return StartCoroutine(ShakeJobFamiliesCoroutine());

        // Fade out water
        DOTween.To(() => water.GetComponent<Renderer>().material.GetFloat("_Alpha"), x => water.GetComponent<Renderer>().material.SetFloat("_Alpha", x), 0.0f, Utilities.animationSpeed);

        SoundEffectsManager.instance.PlayAmbientSound();

        yield return new WaitForSeconds(Utilities.animationSpeed * 2.0f);

        water.GetComponent<Renderer>().material.SetFloat("Vector1_39047021", 1);

        tableLight.DOColor(normalLight, Utilities.animationSpeed * 2.0f);

        water.GetComponent<Renderer>().material.SetColor("_EmissionColor", normalLight);

        // Fade water back in
        DOTween.To(() => water.GetComponent<Renderer>().material.GetFloat("_Alpha"), x => water.GetComponent<Renderer>().material.SetFloat("_Alpha", x), 1.0f, Utilities.animationSpeed);

        /*
        animatedFrontOfficeCorrupted.gameObject.SetActive(false);
        animatedMiddleOfficeCorrupted.gameObject.SetActive(false);
        animatedBackOfficeCorrupted.gameObject.SetActive(false);
        */

        yield return new WaitForSeconds(Utilities.animationSpeed);

        animatedFrontOffice.gameObject.SetActive(true);
        animatedMiddleOffice.gameObject.SetActive(true);
        animatedBackOffice.gameObject.SetActive(true);

        animatedFrontOffice.Play();
        animatedMiddleOffice.Play();
        animatedBackOffice.Play();

        yield return new WaitForSeconds(2.0f);

        DataManager.instance.ShowJobRolesOnTable();
        
        yield return new WaitForSeconds(2.0f);

        // Show line
        idleLineRenderer.transform.DOLocalMoveY(0.25f, Utilities.animationSpeed);
        yield return new WaitForSeconds(Utilities.animationSpeed);
        idlePanel.DOFillAmount(1.0f, Utilities.animationSpeed);
        yield return new WaitForSeconds(Utilities.animationSpeed);

        tableTextMeshPro.text = "";
        tableTextMeshPro.DOFade(1.0f, Utilities.animationSpeed);
        tableTextMeshPro.DOText(openingLines[2], 2.0f);
        SoundEffectsManager.instance.PlayAudioClip(SoundEffectsManager.SFX.MESSAGE, 1.0f);
        
        yield return new WaitForSeconds(5.0f);

        controlPanelButtons[1].ShowButton();
        yield return new WaitForSeconds(Utilities.animationSpeed * 0.25f);
        controlPanelButtons[2].ShowButton();
        yield return new WaitForSeconds(Utilities.animationSpeed * 0.25f);
        controlPanelButtons[3].ShowButton();
        yield return new WaitForSeconds(Utilities.animationSpeed * 0.25f);
        controlPanelButtons[4].ShowButton();
        yield return new WaitForSeconds(Utilities.animationSpeed * 0.25f);

        yield return new WaitForSeconds(1.0f);

        // Hide skip button
        controlPanelButtons[controlPanelButtons.Length - 1].DOKill();
        controlPanelButtons[controlPanelButtons.Length - 1].HideButton();

        panelButtons.transform.DOLocalMove(Vector3.zero, Utilities.animationSpeed * 2);
        panelButtons.transform.DOLocalRotate(Vector3.zero, Utilities.animationSpeed * 2);
        panelGazeChecker.PlayPanelAnimation();

        idleText.DOFade(0, Utilities.animationSpeed * 0.5f);
        idlePanel.DOFillAmount(0, Utilities.animationSpeed);

        yield return new WaitForSeconds(Utilities.animationSpeed);

        idleLineRenderer.transform.DOLocalMoveY(-1.5f, Utilities.animationSpeed);

        controlPanelButtons[5].ShowButton();
        controlPanelButtons[6].ShowButton();
        controlPanelButtons[7].ShowButton();
    }

    public void SkipAnimation() {
        if (isAnimationSkipped) {
            return;
        }

        isAnimationSkipped = true;

        StopAllCoroutines();

        DOTween.KillAll();

        hiverlabLogo.gameObject.SetActive(false);
        tableCanvas.gameObject.SetActive(false);

        controlPanelButtons[0].DOKill();
        controlPanelButtons[0].HideButton();

        controlPanelButtons[controlPanelButtons.Length - 1].DOKill();
        controlPanelButtons[controlPanelButtons.Length - 1].HideButton();

        startText.gameObject.SetActive(false);
        hiverlabLogo.gameObject.SetActive(false);

        water.GetComponent<Renderer>().material.SetFloat("Vector1_39047021", 1);
        water.GetComponent<Renderer>().material.SetColor("_EmissionColor", normalLight);

        water.DOKill();
        DOTween.To(() => water.GetComponent<Renderer>().material.GetFloat("_Alpha"), x => water.GetComponent<Renderer>().material.SetFloat("_Alpha", x), 1.0f, Utilities.animationSpeed);
        water.DOLocalMoveY(-1.0f, Utilities.animationSpeed);
        water.DOScale(1.1f, Utilities.animationSpeed).SetEase(Ease.InBack);

        jobFamily1.DOScale(1, Utilities.animationSpeed).SetEase(Ease.InOutBack);
        jobFamily2.DOScale(1, Utilities.animationSpeed).SetEase(Ease.InOutBack);
        jobFamily3.DOScale(1, Utilities.animationSpeed).SetEase(Ease.InOutBack);

        frontOfficeFunction.ResetBuildingMaterials();

        frontOfficeFunction.ShowOfficeAfterImmediate();
        middleOfficeFunction.ShowOfficeAfterImmediate();
        backOfficeFunction.ShowOfficeAfterImmediate();
        
        animatedFrontOffice.Play();
        animatedMiddleOffice.Play();
        animatedBackOffice.Play();

        animatedAnalytics.gameObject.SetActive(false);
        animatedRobotics.gameObject.SetActive(false);
        animatedAI.gameObject.SetActive(false);

        animatedFrontOfficeCorrupted.gameObject.SetActive(false);
        animatedMiddleOfficeCorrupted.gameObject.SetActive(false);
        animatedBackOfficeCorrupted.gameObject.SetActive(false);

        DataManager.instance.ShowJobRolesOnTable();

        panelButtons.transform.localPosition = Vector3.zero;
        panelButtons.transform.localEulerAngles = Vector3.zero;

        panelGazeChecker.PlayPanelAnimation();
        
        controlPanelButtons[1].ShowButton();
        controlPanelButtons[2].ShowButton();
        controlPanelButtons[3].ShowButton();
        controlPanelButtons[4].ShowButton();
        controlPanelButtons[5].ShowButton();
        controlPanelButtons[6].ShowButton();
        controlPanelButtons[7].ShowButton();
    }

    private IEnumerator PlayAllOpeningLines() {
        float delay = 2.0f;

        for (int i = 0; i < openingLines.Count; i++) {
            tableTextMeshPro.DOText(openingLines[i], Utilities.animationSpeed);
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator ShakeJobFamiliesCoroutine() {
        jobFamily1.DOShakePosition(8.0f, 0.1f, 5);
        jobFamily2.DOShakePosition(8.0f, 0.1f, 5);
        jobFamily3.DOShakePosition(8.0f, 0.1f, 5);

        frontOfficeFunction.SetBuildingMaterialsToRed();

        yield return new WaitForSeconds(2.0f);

        warningGrid.transform.DOScale(0.0f, Utilities.animationSpeed * 2);
        animatedWarningText.GetComponent<SpriteRenderer>().DOFade(0.0f, Utilities.animationSpeed * 2);

        animatedRobotics.Play();
        yield return new WaitForSeconds(Utilities.animationSpeed * 2);

        animatedAnalytics.Play();
        yield return new WaitForSeconds(Utilities.animationSpeed * 2);

        animatedAI.Play();
        yield return new WaitForSeconds(Utilities.animationSpeed * 2);
        
        // Show line
        idleLineRenderer.transform.DOLocalMoveY(0.25f, Utilities.animationSpeed);
        yield return new WaitForSeconds(Utilities.animationSpeed);
        idlePanel.DOFillAmount(1.0f, Utilities.animationSpeed);
        yield return new WaitForSeconds(Utilities.animationSpeed);
        tableTextMeshPro.text = "";
        tableTextMeshPro.DOFade(1.0f, Utilities.animationSpeed);
        tableTextMeshPro.DOText(openingLines[1], 2.0f);

        yield return new WaitForSeconds(12.0f);
        
        // Hide line
        idleText.DOFade(0, Utilities.animationSpeed * 0.5f);
        idlePanel.DOFillAmount(0, Utilities.animationSpeed);
        yield return new WaitForSeconds(Utilities.animationSpeed);
        idleLineRenderer.transform.DOLocalMoveY(-1.5f, Utilities.animationSpeed);

        animatedFrontOfficeCorrupted.Reverse();
        animatedMiddleOfficeCorrupted.Reverse();
        animatedBackOfficeCorrupted.Reverse();

        yield return new WaitForSeconds(2.0f);

        frontOfficeFunction.ShowOfficeAfter();
        middleOfficeFunction.ShowOfficeAfter();
        backOfficeFunction.ShowOfficeAfter();

        yield return new WaitForSeconds(1.5f);

        frontOfficeFunction.ResetBuildingMaterials();
    }

    public void UpdateFilterParameters() {

    }
}
