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

    public enum SFX {
        UI,
        STANDBY,
        SELECT
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
