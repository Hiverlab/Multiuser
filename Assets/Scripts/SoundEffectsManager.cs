using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager instance;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioSource ambientSource;

    [SerializeField]
    private List<AudioClip> sfxList;

    [SerializeField]
    private AudioClip regularWaveAmbientClip;
    [SerializeField]
    private AudioClip distortedWaveAmbientClip;

    public enum SFX {
        AFFECTED_TEXT,
        NOTIFICATION_WARNING,
        POWER_START,
        MESSAGE,
        OFFICE,
        UI
    }

    private void Awake() {
        if (!instance) {
            instance = this;
        } else {
            Destroy(instance);
        }

        audioSource.volume = 0;
        ambientSource.volume = 0;
    }

    public void PlayAudioClip(SFX sfx, float duration = -1) {
        if (duration == -1) {
            audioSource.volume = 1.0f;
            audioSource.PlayOneShot(sfxList[(int)sfx]);
        } else {
            StartCoroutine(PlayAudioClipCoroutine(sfx, duration));
        }
    }

    public void PlayAmbientSound() {
        ambientSource.clip = regularWaveAmbientClip;
        ambientSource.Play();
        ambientSource.DOFade(1.0f, Utilities.animationSpeed * 2);
    }

    public void PlayDistortedAmbientSound() {
        ambientSource.clip = distortedWaveAmbientClip;
        ambientSource.Play();
        ambientSource.DOFade(0.5f, Utilities.animationSpeed * 2);
    }

    private IEnumerator PlayAudioClipCoroutine(SFX sfx, float duration) {
        // Kill all existing tweens before starting
        audioSource.DOKill();
        audioSource.PlayOneShot(sfxList[(int)sfx]);
        audioSource.DOFade(1.0f, Utilities.animationSpeed);

        yield return new WaitForSeconds(duration);

        audioSource.DOFade(0.0f, Utilities.animationSpeed).OnComplete(() => {
            audioSource.Stop();
        });
    }
}
