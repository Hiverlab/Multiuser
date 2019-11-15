using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI3DButton_DataSetter : UI3DButton
{
    [SerializeField]
    private string gps;

    [SerializeField]
    private string tabId;


    protected override void OnButtonPress()
    {
        base.OnButtonPress();
    }

    protected override void OnButtonRelease()
    {
        base.OnButtonRelease();

        //DataNodePopulator.instance.SetMapOrigin(gps, tabId);

        PhotonNetworkManager.instance.photonView.RPC(RPCManager.instance.GetRPC(RPCManager.RPC.RPC_SetMapOrigin),
            Photon.Pun.RpcTarget.All, gps, tabId);
    }
}
