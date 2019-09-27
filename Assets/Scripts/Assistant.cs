using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Assistant : MonoBehaviour
{
    public static Assistant instance;

    [SerializeField]
    private Light assistantLight;

    [SerializeField]
    private Color standbyLightColor;
    [SerializeField]
    private Color successLightColor;
    [SerializeField]
    private Color failureLightColor;

    [SerializeField]
    private float activeIntensity;
    private float inactiveIntensity;

    private float animationSpeed = 1.0f;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }

        Initialize();
    }

    private void Initialize() {
        inactiveIntensity = 0;

        assistantLight.intensity = 0;
    }

    public void TriggerInactive() {
        assistantLight.DOColor(standbyLightColor, animationSpeed);
        assistantLight.DOIntensity(inactiveIntensity, animationSpeed);
    }

    public void TriggerStandby() {
        assistantLight.DOColor(standbyLightColor, animationSpeed);
        assistantLight.DOIntensity(activeIntensity, animationSpeed);

        SoundEffectsManager.instance.PlayAudioClip(SoundEffectsManager.SFX.STANDBY);
    }

    public void TriggerSuccess() {
        assistantLight.DOColor(successLightColor, animationSpeed);
        assistantLight.DOIntensity(activeIntensity, animationSpeed);

        StartCoroutine(TriggerInactiveCoroutine());

        SoundEffectsManager.instance.PlayAudioClip(SoundEffectsManager.SFX.SELECT);
    }

    public void TriggerFailure() {
        assistantLight.DOColor(failureLightColor, animationSpeed);
        assistantLight.DOIntensity(activeIntensity, animationSpeed);

        StartCoroutine(TriggerInactiveCoroutine());
    }

    private IEnumerator TriggerInactiveCoroutine() {
        yield return new WaitForSeconds(2.0f);
        TriggerInactive();
    }
}
