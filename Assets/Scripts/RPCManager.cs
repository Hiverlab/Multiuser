using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPCManager : MonoBehaviour
{
    public static RPCManager instance;

    public enum RPC {
        RPC_SelectImpact,
        RPC_SortHighlightedNodes,
        RPC_AssistantStandby,
        RPC_AssistantSuccess,
        RPC_AssistantFailure
    }

    public Dictionary<RPC, string> RPCDictionary;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }

        InitializeRPCDictionary();
    }

    private void InitializeRPCDictionary() {
        RPCDictionary = new Dictionary<RPC, string>();

        RPCDictionary.Add(RPC.RPC_SelectImpact, "RPC_SelectImpact");
        RPCDictionary.Add(RPC.RPC_SortHighlightedNodes, "RPC_SortHighlightedNodes");
        RPCDictionary.Add(RPC.RPC_AssistantStandby, "RPC_AssistantStandby");
        RPCDictionary.Add(RPC.RPC_AssistantSuccess, "RPC_AssistantSuccess");
        RPCDictionary.Add(RPC.RPC_AssistantFailure, "RPC_AssistantFailure");
    }

    public string GetRPC(RPC rpc) {
        try {
            return RPCDictionary[rpc];
        } catch (Exception e) {
            Debug.LogError(e);

            return string.Empty;
        }
    }

    [PunRPC]
    public void RPC_SelectImpact(string impact) {
        Debug.Log("[RPC] Select Impact: " + impact);

        DataManager.instance.SelectImpact(impact);
    }

    [PunRPC]
    public void RPC_SortHighlightedNodes() {
        Debug.Log("[RPC] Sort Highlighted Nodes");

        DataManager.instance.SortHighlightedNodes();
    }

    [PunRPC]
    public void RPC_AssistantStandby() {
        Debug.Log("[RPC] Assistant standby");

        Assistant.instance.TriggerStandby();
    }

    [PunRPC]
    public void RPC_AssistantSuccess() {
        Debug.Log("[RPC] Assistant success");

        Assistant.instance.TriggerSuccess();
    }

    [PunRPC]
    public void RPC_AssistantFailure() {
        Debug.Log("[RPC] Assistant failure");

        Assistant.instance.TriggerFailure();
    }
}
