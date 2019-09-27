using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceManager : MonoBehaviour {
    public static VoiceManager instance;

    private Recorder thisRecorder;

    [SerializeField]
    private bool recorderIsSet;

    [SerializeField]
    private SpeechSandboxStreaming speechSandboxStreaming;

    [SerializeField]
    private bool isVoiceActive = false;

    private void Awake() {
        if (!instance) {
            instance = this;
        } else {
            Destroy(instance);
        }
    }

    public bool GetIsVoiceActive() {
        return isVoiceActive;
    }

    // Start is called before the first frame update
    void Start() {
        thisRecorder = this.GetComponent<Recorder>();
        thisRecorder.AutoStart = true;

        //InvokeRepeating("ResetRecorder", 0, 5);
    }

    private void Update() {
        // If pressed A or X
        if (OVRInput.GetDown(OVRInput.Button.One) || Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Toggle voice recorder");

            isVoiceActive = true;

            StartCoroutine(StartRecorderCoroutine());

            StopWatsonVoice();
        } else if (OVRInput.GetUp(OVRInput.Button.One) || Input.GetKeyUp(KeyCode.Space)) {
            Debug.Log("Toggle watson");

            isVoiceActive = false;
            
            thisRecorder.TransmitEnabled = false;
            thisRecorder.IsRecording = false;
            SetWatsonVoice();
        }
    }

    private IEnumerator StartRecorderCoroutine() {

        thisRecorder.TransmitEnabled = true;

        thisRecorder.IsRecording = false;
        
        yield return new WaitForSeconds(0.5f);

        thisRecorder.IsRecording = true;
    }

    private void SetWatsonVoice() {
        speechSandboxStreaming.Initialize();
    }

    private void StopWatsonVoice() {
        speechSandboxStreaming.StopService();
    }

    IEnumerator StartRecorder() {
        thisRecorder.IsRecording = true;
        yield return new WaitForSecondsRealtime(1f);
        thisRecorder.TransmitEnabled = true;
        thisRecorder.IsRecording = false;
        yield return new WaitForSecondsRealtime(1f);
        thisRecorder.IsRecording = true;


    }

    public void ResetRecorder() {
        StartCoroutine(DoResetRecorder());
    }
    

    IEnumerator DoResetRecorder() {

        Debug.Log("Resetting recorder");

        thisRecorder.TransmitEnabled = true;

        if (thisRecorder.LevelMeter.CurrentPeakAmp > 0.1f) {
            yield break;
        }

        thisRecorder.IsRecording = false;
        yield return null;
        thisRecorder.IsRecording = true;
    }
}
