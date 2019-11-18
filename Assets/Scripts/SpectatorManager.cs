using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpectatorManager : MonoBehaviour
{
    public static SpectatorManager instance;

    [SerializeField]
    public bool isSpectatorActive = false;

    [SerializeField]
    private GameObject ovrCameraRig;

    [SerializeField]
    private Transform spectatorCamera;

    [SerializeField]
    private List<Transform> cameraPositionList;

    [SerializeField]
    private float delayBetweenViews = 5.0f;

    private bool isViewingMaster = false;
    public GameObject masterTarget;

    private int cameraPositionIndex;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        isSpectatorActive = true;
#else
        isSpectatorActive = false;
#endif

        if (isSpectatorActive)
        {
            Initialize();
        }
    }

    private void Initialize()
    {
        //ovrCameraRig.SetActive(false);

        cameraPositionIndex = 0;

        StartCoroutine(SwitchCameraViewCoroutine());
    }

    private IEnumerator SwitchCameraViewCoroutine()
    {
        spectatorCamera.DOMove(cameraPositionList[cameraPositionIndex].position, 1.0f);
        spectatorCamera.DORotate(cameraPositionList[cameraPositionIndex].eulerAngles, 1.0f);

        yield return new WaitForSeconds(delayBetweenViews);

        cameraPositionIndex = (cameraPositionIndex + 1) % cameraPositionList.Count;

        StartCoroutine(SwitchCameraViewCoroutine());
    }

    public void SetMasterTarget(GameObject _masterTarget)
    {
        //masterTarget = GameObject.Find("head_JNT");
        Debug.Log("Master target: " + masterTarget);
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFirstPerson();

        }

        if (isViewingMaster && masterTarget)
        {
            spectatorCamera.transform.position = masterTarget.transform.position;
            spectatorCamera.transform.rotation = masterTarget.transform.rotation;
        }
        */
    }

    private void ToggleFirstPerson()
    {
        isViewingMaster = !isViewingMaster;

        if (isViewingMaster)
        {
            masterTarget = GameObject.Find("head_JNT");
            StopCoroutine(SwitchCameraViewCoroutine());
            spectatorCamera.transform.position = masterTarget.transform.position;
            spectatorCamera.transform.rotation = masterTarget.transform.rotation;
        }
        else
        {
        }
    }
}
