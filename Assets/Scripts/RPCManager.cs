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
        RPC_SortHighlightedNodes
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
}
