using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ControlPanelButton : MonoBehaviour
{
    private Button button;
    private Image buttonImage;

    [SerializeField]
    private Image glowImage;

    [SerializeField]
    private bool isLooping = false;

    // Start is called before the first frame update
    void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        glowImage.DOFade(0.0f, 0.0f); 
    }

    private void Start() {
        if (isLooping) {
            GlowLoop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideButton() {
        button.interactable = false;

        buttonImage.DOFillAmount(0.0f, Utilities.animationSpeed).OnComplete(() => {
            gameObject.SetActive(false);
        });
    }

    public void ShowButton() {
        gameObject.SetActive(true);

        buttonImage.fillAmount = 1.0f;
        button.interactable = true;

        /*
        buttonImage.DOFillAmount(1.0f, Utilities.animationSpeed).OnComplete(() => {
            button.interactable = true;
        });
        */
    }

    public void Glow() {
        glowImage.DOFade(1.0f, Utilities.animationSpeed * 0.75f).OnComplete(() => {
            glowImage.DOFade(0.0f, Utilities.animationSpeed * 0.75f);
            isLooping = false;
        });
    }

    public void GlowLoop() {
        glowImage.DOFade(1.0f, Utilities.animationSpeed * 0.75f).SetLoops(-1, LoopType.Incremental).OnStepComplete(() => {
            glowImage.DOFade(0.0f, Utilities.animationSpeed * 0.75f);
        });
    }
}

