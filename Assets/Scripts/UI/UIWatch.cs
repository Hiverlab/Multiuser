using DG.Tweening;
using Doozy.Engine.UI;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIWatch : MonoBehaviour
{
    public static UIWatch instance;

    [SerializeField]
    private Transform uiAnchorPoint;

    [SerializeField]
    private List<UIView> watchFaceUIViewList;

    [SerializeField]
    private List<UIView> watchFaceSideUIViewList;

    private bool isActive;

    private bool isAnimationComplete;

    private UI3DButton currentButton;

    [SerializeField]
    private TextMeshProUGUI targetTextMesh;

    [SerializeField]
    private UI3DButton_ModuleConfirm moduleConfirmButton;

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
        isAnimationComplete = true;

        HideUIViews();
        HideUISideViews();
    }

    private void ShowUIViews()
    {
        for (int i = 0; i < watchFaceUIViewList.Count; i++)
        {
            watchFaceUIViewList[i].Show();
        }
    }

    private void HideUIViews()
    {
        for (int i = 0; i < watchFaceUIViewList.Count; i++)
        {
            watchFaceUIViewList[i].Hide();
        }

        targetTextMesh.text = "Select a module";
        moduleConfirmButton.gameObject.SetActive(false);
    }
    private void ShowUISideViews()
    {
        for (int i = 0; i < watchFaceSideUIViewList.Count; i++)
        {
            watchFaceSideUIViewList[i].Show();
        }
    }

    private void HideUISideViews()
    {
        for (int i = 0; i < watchFaceSideUIViewList.Count; i++)
        {
            watchFaceSideUIViewList[i].Hide();
        }

        targetTextMesh.text = "Select a module";
        moduleConfirmButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Loading main scene");
            PhotonNetwork.LoadLevel("Scene - Main");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Loading mapbox scene");
            PhotonNetwork.LoadLevel("Scene - Mapbox");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            OnButtonPress();
        }
        */
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //PhotonNetworkManager.instance.photonView.RPC(RPCManager.instance.GetRPC(RPCManager.RPC.RPC_SetMapOrigin),
            //    Photon.Pun.RpcTarget.All, "1.28015, 103.845854", "2132687534");
            PhotonNetworkManager.instance.photonView.RPC(RPCManager.instance.GetRPC(RPCManager.RPC.RPC_SetMapOrigin),
                Photon.Pun.RpcTarget.All, "1.280431, 103.848632", "2132687534");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PhotonNetworkManager.instance.photonView.RPC(RPCManager.instance.GetRPC(RPCManager.RPC.RPC_SetMapOrigin),
                Photon.Pun.RpcTarget.All, "1.297994, 103.788665", "734724819");
        }
    }

    public void SetModule(UI3DButton _currentButton)
    {
        currentButton = _currentButton;

        targetTextMesh.text = "";

        targetTextMesh.DOText(_currentButton.textToSet, Utilities.animationSpeed);

        moduleConfirmButton.gameObject.SetActive(true);
    }

    public void ConfirmModule()
    {
        // Confirm action
        currentButton.ConfirmAction();
    }


    #region Touch interactions

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Key"))
        {
            OnButtonPress();

            Utilities.instance.VibrateController(100, 0.25f, 0.15f, other.tag);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Key"))
        {
            OnButtonRelease();

            Utilities.instance.VibrateController(100, 0.25f, 0.15f, other.tag);
        }
    }

    private void OnButtonPress()
    {
        // If animation is not complete, return
        if (!isAnimationComplete)
        {
            return;
        }

        isAnimationComplete = false;

        StartCoroutine(ResetAnimationTimer());

        isActive = !isActive;

        if (isActive)
        {
            ShowUIViews();
            ShowUISideViews();
        }
        else
        {
            HideUIViews();
            HideUISideViews();
        }
    }

    private void OnButtonRelease()
    {

    }

    #endregion

    private IEnumerator ResetAnimationTimer()
    {
        yield return new WaitForSeconds(2.0f);

        isAnimationComplete = true;
    }
}
