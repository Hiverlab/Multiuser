using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour {
    public static Utilities instance;

    private void Awake() {
        if (!instance) {
            instance = this;
        } else {
            Destroy(instance);
        }

        //DontDestroyOnLoad(this);
    }

    public static float animationSpeed = 1.0f;

    public static void Log(string className, string debugMessage) {
        Debug.Log("<b>" + className + "<b> " + debugMessage);
    }

    public static float GetRandomAnimationSpeed(float speed = -1) {
        // Override if speed is -1
        if (speed == -1) {
            speed = animationSpeed;
        }

        // Return speed at with a different range at 0.05%
        return speed * Random.Range(0.95f, 1.05f);
    }

    public void VibrateController(float frequency, float amplitude, float duration, OVRInput.Controller controller) {
        //SoundEffectsManager.instance.PlayAudioClip(SoundEffectsManager.SFX.UI);
        StartCoroutine(VibrateControllerCoroutine(frequency, amplitude, duration, controller));
    }

    private static IEnumerator VibrateControllerCoroutine(float frequency, float amplitude, float duration, OVRInput.Controller controller) {
        OVRInput.SetControllerVibration(frequency, amplitude, controller);

        yield return new WaitForSeconds(duration);

        OVRInput.SetControllerVibration(0, 0, controller);
    }
}
