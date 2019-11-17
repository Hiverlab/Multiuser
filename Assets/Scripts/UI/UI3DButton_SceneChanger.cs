using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI3DButton_SceneChanger : UI3DButton
{
    [SerializeField]
    private string sceneToload;

    protected override void OnButtonPress()
    {
        base.OnButtonPress();
    }

    protected override void OnButtonRelease()
    {
        base.OnButtonRelease();

        UIWatch.instance.SetModule(this);
    }

    public override void ConfirmAction()
    {
        base.ConfirmAction();

        // If we are in mapbox scene
        if (SceneManager.GetActiveScene().name == "Scene - Mapbox")
        {
            // Load main scene
            PhotonNetwork.LoadLevel("Scene - Main");
        }
    }
}
