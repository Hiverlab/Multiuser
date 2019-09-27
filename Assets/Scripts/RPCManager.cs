using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPCManager : MonoBehaviour
{
    public static RPCManager instance;

    public enum RPC {
        RPC_ToggleImpactPanel,
        RPC_ToggleJobFamilyPanel,
        RPC_ToggleJobRolePanel,
        RPC_ToggleSkillsPanel,
        RPC_SelectImpact,
        RPC_SelectJobFamily,
        RPC_SelectJobRole,
        RPC_SelectSkill,
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

        RPCDictionary.Add(RPC.RPC_ToggleImpactPanel, "RPC_ToggleImpactPanel");
        RPCDictionary.Add(RPC.RPC_ToggleJobFamilyPanel, "RPC_ToggleJobFamilyPanel");
        RPCDictionary.Add(RPC.RPC_ToggleJobRolePanel, "RPC_ToggleJobRolePanel");
        RPCDictionary.Add(RPC.RPC_ToggleSkillsPanel, "RPC_ToggleSkillsPanel");
        RPCDictionary.Add(RPC.RPC_SelectImpact, "RPC_SelectImpact");
        RPCDictionary.Add(RPC.RPC_SelectJobFamily, "RPC_SelectJobFamily");
        RPCDictionary.Add(RPC.RPC_SelectJobRole, "RPC_SelectJobRole");
        RPCDictionary.Add(RPC.RPC_SelectSkill, "RPC_SelectSkill");
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
    public void RPC_ToggleImpactPanel() {
        Debug.Log("[RPC] Toggle impact panel");

        TableManager.instance.ToggleImpactPanel();
    }

    [PunRPC]
    public void RPC_ToggleJobFamilyPanel() {
        Debug.Log("[RPC] Toggle job family panel");

        TableManager.instance.ToggleJobFamilyPanel();
    }

    [PunRPC]
    public void RPC_ToggleJobRolePanel() {
        Debug.Log("[RPC] Toggle job role panel");

        TableManager.instance.ToggleJobRolePanel();
    }

    [PunRPC]
    public void RPC_ToggleSkillsPanel() {
        Debug.Log("[RPC] Toggle skills panel");

        TableManager.instance.ToggleSkillsPanel();
    }

    [PunRPC]
    public void RPC_SelectImpact(string impact) {
        Debug.Log("[RPC] Select Impact: " + impact);

        DataManager.instance.SelectImpact(impact);
    }

    [PunRPC]
    public void RPC_SelectJobFamily(string jobFamily) {
        Debug.Log("[RPC] Select Job Family: " + jobFamily);

        DataManager.instance.SelectJobFamily(jobFamily);
    }

    [PunRPC]
    public void RPC_SelectJobRole(string jobRole) {
        Debug.Log("[RPC] Select Job Role: " + jobRole);

        DataManager.instance.SelectJobRole(jobRole);
    }

    [PunRPC]
    public void RPC_SelectSkill(string skill) {
        Debug.Log("[RPC] Select Skill: " + skill);

        DataManager.instance.SelectSkill(skill);
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
