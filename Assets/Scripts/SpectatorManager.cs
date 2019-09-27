using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpectatorManager : MonoBehaviour {
    [SerializeField]
    private bool isSpectatorActive = false;

    [SerializeField]
    private GameObject ovrCameraRig;

    [SerializeField]
    private Transform spectatorCamera;

    [SerializeField]
    private List<Transform> cameraPositionList;

    [SerializeField]
    private float delayBetweenViews = 5.0f;

    private int cameraPositionIndex;

    // Start is called before the first frame update
    void Start() {
        if (Application.platform == RuntimePlatform.Android) {
            isSpectatorActive = false;
        }

        if (isSpectatorActive) {
            Initialize();
        }
    }

    private void Initialize() {
        ovrCameraRig.SetActive(false);

        cameraPositionIndex = 0;

        StartCoroutine(SwitchCameraViewCoroutine());
    }

    private IEnumerator SwitchCameraViewCoroutine() {
        spectatorCamera.DOMove(cameraPositionList[cameraPositionIndex].position, 1.0f);
        spectatorCamera.DORotate(cameraPositionList[cameraPositionIndex].eulerAngles, 1.0f);

        yield return new WaitForSeconds(delayBetweenViews);

        cameraPositionIndex = (cameraPositionIndex + 1) % cameraPositionList.Count;

        StartCoroutine(SwitchCameraViewCoroutine());
    }
}
