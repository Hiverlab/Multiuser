using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI3DButton_DataSetter : UI3DButton
{
    [SerializeField]
    private string gps;

    [SerializeField]
    private string tabId;

    private void OnEnable()
    {
    }

    protected override void OnButtonPress()
    {
        base.OnButtonPress();
    }

    protected override void OnButtonRelease()
    {
        base.OnButtonRelease();

        //DataNodePopulator.instance.SetMapOrigin(gps, tabId);

        /*
        */

        UIWatch.instance.SetModule(this);
    }

    public override void ConfirmAction()
    {
        base.ConfirmAction();

        // If we are in mapbox scene
        if (SceneManager.GetActiveScene().name == "Scene - Main")
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            // Load main scene
            //PhotonNetwork.LoadLevel("Scene - Mapbox");

            if (PhotonNetwork.IsMasterClient)
            {
                // Load main scene
                PhotonNetwork.LoadLevel("Scene - Mapbox");
            }
        }
        else
        {
            // Otherwise just load the data
            PhotonNetworkManager.instance.photonView.RPC(RPCManager.instance.GetRPC(RPCManager.RPC.RPC_SetMapOrigin),
                Photon.Pun.RpcTarget.All, gps, tabId);
        }
    }
}
