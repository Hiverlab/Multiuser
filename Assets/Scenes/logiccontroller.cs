using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logiccontroller : MonoBehaviour, dialogFocus
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void runWithExpectedOutput(DialogueContent content)
    {
        Debug.Log("??????????"+content.ToString("G"));

        string result = content.ToString("G");

        if (result == "high") {
            PhotonNetworkManager.instance.photonView.RPC(RPCManager.instance.GetRPC(RPCManager.RPC.RPC_SelectImpact),
                    Photon.Pun.RpcTarget.All,
                    "High");

            // Assistant
            PhotonNetworkManager.instance.photonView.RPC(RPCManager.instance.GetRPC(RPCManager.RPC.RPC_AssistantSuccess),
                        Photon.Pun.RpcTarget.All);
        }

        else if (result == "medium") {
            PhotonNetworkManager.instance.photonView.RPC(RPCManager.instance.GetRPC(RPCManager.RPC.RPC_SelectImpact),
                    Photon.Pun.RpcTarget.All,
                    "Medium");

            // Assistant
            PhotonNetworkManager.instance.photonView.RPC(RPCManager.instance.GetRPC(RPCManager.RPC.RPC_AssistantSuccess),
                        Photon.Pun.RpcTarget.All);
        }

        else if (result == "low") {
            PhotonNetworkManager.instance.photonView.RPC(RPCManager.instance.GetRPC(RPCManager.RPC.RPC_SelectImpact),
                    Photon.Pun.RpcTarget.All,
                    "Low");

            // Assistant
            PhotonNetworkManager.instance.photonView.RPC(RPCManager.instance.GetRPC(RPCManager.RPC.RPC_AssistantSuccess),
                        Photon.Pun.RpcTarget.All);
        }

        else {
            // Assistant
            PhotonNetworkManager.instance.photonView.RPC(RPCManager.instance.GetRPC(RPCManager.RPC.RPC_AssistantFailure),
                        Photon.Pun.RpcTarget.All);
        }
    }
}
