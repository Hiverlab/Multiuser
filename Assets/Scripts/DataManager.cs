using System.Collections;
using System.Linq;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DataManager : MonoBehaviour {

    public static DataManager instance;

    [SerializeField]
    private JobRolesData jobRolesData;

    [SerializeField]
    private Transform frontOffice;
    [SerializeField]
    private Transform middleOffice;
    [SerializeField]
    private Transform backOffice;

    [SerializeField]
    private GameObject jobRolePrefab;
    [SerializeField]
    private GameObject jobFamilyPrefab;
    private Dictionary<string, JobFamilyObject> jobFamilyDict;

    private List<JobRoleObject> nodesList;

    private List<JobRoleObject> jobRolesList;
    private List<JobRoleObject> jobFamiliesList;
    private List<JobRoleObject> impactLevelList;
    private List<JobRoleObject> skillsList;

    private List<JobFamilyObject> jobFamilyList;

    [SerializeField]
    private Transform jobFamilyButtonContainer;
    [SerializeField]
    private Transform jobRoleButtonContainer;
    [SerializeField]
    private Transform impactLevelButtonContainer;
    [SerializeField]
    private Transform skillsButtonContainer;

    private JobRoleObject selectedJobRole = null;

    [SerializeField]
    private Transform highlightStartPoint;
    [SerializeField]
    private Transform highlightEndPoint;

    private IEnumerable<JobRoleObject> result;
    private bool isDescending = false;

    public delegate void ButtonSelectedDelegate(GameObject button);
    public ButtonSelectedDelegate OnButtonSelected;


    public delegate void SkillPanelOpenDelegate(GameObject skillPanel);
    public SkillPanelOpenDelegate OnSkillPanelOpen;

    private void Awake() {
        if (!instance) {
            instance = this;
        } else {
            Destroy(instance);
        }

        jobRolesList = new List<JobRoleObject>();
        jobFamiliesList = new List<JobRoleObject>();
        impactLevelList = new List<JobRoleObject>();
        skillsList = new List<JobRoleObject>();

        DOTween.SetTweensCapacity(500, 50);

#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = true;
#endif
        //DontDestroyOnLoad(this);
    }

    private void SetupDropdownListeners() {
    }

    private void OnValueChanged(TMP_Dropdown dropdown) {
        Debug.Log(dropdown.name + " value changed: " + dropdown.options[dropdown.value].text);
        string value = dropdown.options[dropdown.value].text;

        switch (dropdown.name) {
            case "Job Roles Dropdown":
                SelectNodesWithJobRole(value);
                break;
            case "Job Families Dropdown":
                SelectNodesWithJobFamily(value);
                break;
            case "Impact Level Dropdown":
                SelectNodesWithImpactLevel(value);
                break;
            case "Skills Dropdown":
                SelectNodesWithSkills(value);
                break;
        }

        HighlightSelectedNodes();
    }

    public void HighlightSelectedNodes() {
        result = nodesList.ToList();

        /*
        // Check if all empty
        if (jobRolesDropdown.value == 0 && jobFamiliesDropdown.value == 0 && impactLevelDropdown.value == 0 && skillsDropdown.value == 0) {
            result = Enumerable.Empty<JobRoleObject>();
        }
        */

        if (jobRolesList.Count == 0 && jobFamiliesList.Count == 0 && impactLevelList.Count == 0 && skillsList.Count == 0) {
            result = Enumerable.Empty<JobRoleObject>();
        }

        if (jobRolesList != null && jobRolesList.Count != 0) {
            Debug.Log("Intersecting with job roles");
            result = result.Intersect(jobRolesList);
        }

        if (jobFamiliesList != null && jobFamiliesList.Count != 0) {
            Debug.Log("Intersecting with job families");
            result = result.Intersect(jobFamiliesList);
        }

        if (impactLevelList != null && impactLevelList.Count != 0) {
            Debug.Log("Intersecting with impact level");
            result = result.Intersect(impactLevelList);
        }

        if (skillsList != null && skillsList.Count != 0) {
            Debug.Log("Intersecting with skills list");
            result = result.Intersect(skillsList);
        }

        Debug.Log("Job roles: " + jobRolesList.Count + " Job families: " + jobFamiliesList.Count + " Impact level: " + impactLevelList.Count + " Skills: " + skillsList.Count);

        if (result.Count() > 0) {
            //TableManager.instance.ShowSortButtons();
        } else {
            //TableManager.instance.HideSortButtons();
        }

        for (int i = 0; i < nodesList.Count; i++) {
            if (result.Contains(nodesList[i])) {
                //Debug.Log("Highlighting: " + nodesList[i]);
                nodesList[i].Highlight();
            } else {
                nodesList[i].Unhighlight();
            }
        }
    }

    public void SortHighlightedNodesRPC() {
        PhotonNetworkManager.instance.photonView.RPC(RPCManager.instance.GetRPC(RPCManager.RPC.RPC_SortHighlightedNodes),
                    Photon.Pun.RpcTarget.All);
    }

    public void SortHighlightedNodes() {
        if (isDescending) {
            result = result.OrderByDescending(result => result.GetSize());
        } else {
            result = result.OrderBy(result => result.GetSize());
        }

        isDescending = !isDescending;

        // Check those in result and bring forward
        int size = result.Count();
        int maxNodesPerRow = 5;
        int rows = size < maxNodesPerRow ? size : size / maxNodesPerRow;

        int nodesPerRow = size < maxNodesPerRow ? size : size / rows;

        float distance = (highlightStartPoint.transform.position - highlightEndPoint.transform.position).magnitude;
        float distanceBetweenNodes = distance / nodesPerRow;

        Debug.Log("Size: " + size + " Distance between nodes: " + distanceBetweenNodes);
        List<JobRoleObject> resultArray = result.ToList();
        Vector3 offset = Vector3.zero;
        float zOffset = 0.0f;
        float xOffset = 0.0f;

        //Debug.Log("Nodes per row: " + nodesPerRow);

        Vector3 titleHeight = Vector3.zero;
        float titleY = 0.15f;
        bool isStaggerRow = false;

        for (int i = 0; i < size; i++) {

            if (i >= nodesPerRow) {
                nodesPerRow += size / rows;
                //Debug.Log("Over: " + nodesPerRow);
                zOffset += 0.3f;
                xOffset = 0.0f;

                // Add title height for new row
                titleY += 0.15f;

                isStaggerRow = !isStaggerRow;
            }

            offset = new Vector3(xOffset, offset.y, zOffset);

            Vector3 position = highlightStartPoint.position + offset;
            Vector3 staggerZ = new Vector3(0, 0, 0.05f);

            if (i % 2 == 0) {
                resultArray[i].SetJobRoleTitleHeight(titleY);
                if (isStaggerRow) {
                    position += staggerZ;
                }
            } else {
                resultArray[i].SetJobRoleTitleHeight(titleY + 0.075f);
                if (!isStaggerRow) {
                    position += staggerZ;
                }
            }

            resultArray[i].BringToFront(position);

            xOffset += distanceBetweenNodes;
        }

    }

    private void SelectNodesWithJobRole(string jobRole) {
        ClearParameterLists();

        for (int i = 0; i < nodesList.Count; i++) {
            if (nodesList[i].GetJobRoleData().role.Equals(jobRole)) {
                if (!jobRolesList.Contains(nodesList[i])) {
                    jobRolesList.Add(nodesList[i]);

                    // If not null, means selected before
                    if (selectedJobRole != null) {
                        selectedJobRole.ReturnToOriginalPosition();
                    }

                    // Select this job role
                    selectedJobRole = nodesList[i];
                    selectedJobRole.BringToFront();
                }
            } else {
                nodesList[i].ReturnToOriginalPosition();


                if (jobRolesList.Contains(nodesList[i])) {
                    jobRolesList.Remove(nodesList[i]);
                }
            }
        }
    }

    private void SelectNodesWithJobFamily(string jobFamily) {
        ClearParameterLists();

        for (int i = 0; i < nodesList.Count; i++) {
            if (nodesList[i].GetJobRoleData().family.Equals(jobFamily)) {
                if (!jobFamiliesList.Contains(nodesList[i])) {
                    jobFamiliesList.Add(nodesList[i]);
                }
            } else {
                if (jobFamiliesList.Contains(nodesList[i])) {
                    jobFamiliesList.Remove(nodesList[i]);
                }
            }
        }
    }

    private void SelectNodesWithImpactLevel(string impactLevel) {
        ClearParameterLists();

        for (int i = 0; i < nodesList.Count; i++) {
            //Debug.Log("Impact level: " + nodesList[i].GetJobRoleData().impactLevel + " Selected:" + impactLevel + " Equals: " + nodesList[i].GetJobRoleData().impactLevel.Equals(impactLevel));

            if (nodesList[i].GetJobRoleData().impactLevel.Equals(impactLevel)) {
                if (!impactLevelList.Contains(nodesList[i])) {
                    impactLevelList.Add(nodesList[i]);
                }
            } else {
                if (impactLevelList.Contains(nodesList[i])) {
                    impactLevelList.Remove(nodesList[i]);
                }
            }
        }
    }

    private void SelectNodesWithSkills(string skills) {
        ClearParameterLists();

        for (int i = 0; i < nodesList.Count; i++) {
            if (nodesList[i].GetJobRoleData().skills.Contains(skills)) {
                if (!skillsList.Contains(nodesList[i])) {
                    skillsList.Add(nodesList[i]);
                }
            } else {
                if (skillsList.Contains(nodesList[i])) {
                    skillsList.Remove(nodesList[i]);
                }
            }
        }
    }

    // Use this for initialization
    void Start() {
        PopulateDropdowns();
        SetupDropdownListeners();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.U)) {

            SelectImpact("Medium");
        }
        if (Input.GetKeyDown(KeyCode.I)) {
            SelectJobFamily("Operations");
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            SelectJobFamily("Relationship management");
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            SelectNodesWithJobRole("Wealth manager");
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            SortHighlightedNodes();
        }
    }

    private void ShowData() {
        //jobRolesData.Entities.ForEach()
    }

    private void PopulateDropdowns() {
        List<string> jobRoles = new List<string>();
        List<string> jobFamilies = new List<string>();
        List<string> impactLevels = new List<string>();
        List<string> skills = new List<string>();

        nodesList = new List<JobRoleObject>();

        // Add to list
        foreach (JobRole jobRole in jobRolesData.Entities) {
            jobRole.role = jobRole.role.Trim();
            jobRole.family = jobRole.family.Trim();
            jobRole.impactLevel = jobRole.impactLevel.Trim();
            jobRole.skills = jobRole.skills.Trim();

            AddToList(jobRoles, jobRole.role);
            AddToList(jobFamilies, jobRole.family);
            AddToList(impactLevels, jobRole.impactLevel);
            AddStringListToList(skills, jobRole.skills);

            SpawnJobRole(jobRole);
        }

        jobFamilyDict = new Dictionary<string, JobFamilyObject>();
        // Spawn job families
        for (int i = 0; i < jobFamilies.Count; i++) {
            SpawnJobFamily(jobFamilies[i]);
        }

        for (int i = 0; i < nodesList.Count; i++) {
            nodesList[i].SetLineEndAnchor(jobFamilyDict[nodesList[i].GetJobRoleData().family].transform);
        }

        // Sort alphabetically
        jobRoles.Sort();
        jobFamilies.Sort();
        skills.Sort();
        
        for (int i = 0; i < jobFamilies.Count; i++) {
            jobFamilyButtonContainer.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = jobFamilies[i];
            jobFamilyButtonContainer.GetChild(i).name = jobFamilies[i].ToLower();
        }

        for (int i = 0; i < jobRoles.Count; i++) {
            jobRoleButtonContainer.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = jobRoles[i];
            jobRoleButtonContainer.GetChild(i).name = jobRoles[i].ToLower();
        }

        for (int i = 0; i < skills.Count; i++) {
            skillsButtonContainer.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = skills[i];
            skillsButtonContainer.GetChild(i).name = skills[i].ToLower();
        }

        for (int i = 0; i < impactLevels.Count; i++) {
            impactLevelButtonContainer.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = impactLevels[i];
            impactLevelButtonContainer.GetChild(i).name = impactLevels[i].ToLower();
        }
    }

    public void SelectJobFamily(string jobFamilyName) {
        // Clear first
        ClearParameterLists();

        foreach (KeyValuePair<string, JobFamilyObject> pair in jobFamilyDict) {
            if (pair.Key.Equals(jobFamilyName)) {
                pair.Value.Highlight();
            } else {
                pair.Value.Unhighlight();
            }
        }

        // Now select
        SelectNodesWithJobFamily(jobFamilyName);

        HighlightSelectedNodes();
    }

    public void DeselectJobFamily(string jobFamilyName) {
        ClearParameterLists();
        //jobFamilyDict[jobFamilyName].Unhighlight();

        foreach (KeyValuePair<string, JobFamilyObject> pair in jobFamilyDict) {
            jobFamilyDict[pair.Key].Unhighlight();
        }

        HighlightSelectedNodes();
    }

    public void SelectJobRole(string jobRoleName) {
        SelectNodesWithJobRole(jobRoleName);
        ClearFamilyList();

        HighlightSelectedNodes();
    }

    public void ClearParameterLists() {
        jobRolesList = new List<JobRoleObject>();
        jobFamiliesList = new List<JobRoleObject>();
        skillsList = new List<JobRoleObject>();
        impactLevelList = new List<JobRoleObject>();

        if (selectedJobRole != null) {
            selectedJobRole.ReturnToOriginalPosition();
        }
    }

    public void ClearFamilyList() {
        foreach (KeyValuePair<string, JobFamilyObject> pair in jobFamilyDict) {
            jobFamilyDict[pair.Key].Unhighlight();
        }
    }

    public void DeselectJobRole(string jobRoleName) {
        ClearParameterLists();

        HighlightSelectedNodes();
    }

    public void SelectImpact(string impact) {
        SelectNodesWithImpactLevel(impact);
        ClearFamilyList();

        HighlightSelectedNodes();
    }

    public void DeselectImpact(string impact) {
        ClearParameterLists();

        HighlightSelectedNodes();
    }

    public void SelectSkill(string skill) {
        SelectNodesWithSkills(skill);
        ClearFamilyList();

        HighlightSelectedNodes();
    }

    public void DeselectSkill(string skill) {
        ClearParameterLists();

        HighlightSelectedNodes();
    }

    private void SpawnJobFamily(string jobFamilyName) {
        // For each job family, look for job roles in that family, then center in the cluster
        JobFamilyObject jobFamily = Lean.Pool.LeanPool.Spawn(jobFamilyPrefab).GetComponent<JobFamilyObject>();

        jobFamilyDict.Add(jobFamilyName, jobFamily);

        jobFamily.transform.position = GetCenterPointOfJobFamily(jobFamilyName);

        jobFamily.transform.localPosition = new Vector3(jobFamily.transform.localPosition.x, 0.5f, jobFamily.transform.localPosition.z);

        jobFamily.SetTitle(jobFamilyName);
        jobFamily.name = jobFamilyName;

        //Debug.Log("Job: " + jobFamilyName);
    }

    public List<Transform> GetJobRolesByFamilyName(string jobFamilyName) {
        List<Transform> transformList = new List<Transform>();

        for (int i = 0; i < nodesList.Count; i++) {
            if (nodesList[i].GetJobRoleData().family.Equals(jobFamilyName)) {
                transformList.Add(nodesList[i].transform);
            }
        }

        return transformList;
    }

    private Vector3 GetCenterPointOfJobFamily(string jobFamily) {
        Vector3 position = Vector3.zero;

        int familyCount = 0;
        for (int i = 0; i < nodesList.Count; i++) {
            if (nodesList[i].GetJobRoleData().family.Equals(jobFamily)) {

                position += nodesList[i].transform.position;
                familyCount++;
            }
        }

        return position / familyCount;
    }

    private void SpawnJobRole(JobRole _jobRole) {
        JobRoleObject jobRole = Lean.Pool.LeanPool.Spawn(jobRolePrefab).GetComponent<JobRoleObject>();
        jobRole.SetJobRoleData(_jobRole);
        nodesList.Add(jobRole);

        jobRole.SetTitle(_jobRole.role);
        jobRole.SetImpact(_jobRole.impactLevel);
        jobRole.SetSize(_jobRole.size);

        string[] functions = _jobRole.functions.Split('\n');

        Vector3 randomOffset = new Vector3(Random.Range(-0.3f, 0.3f), 0, Random.Range(-0.3f, 0.3f));

        // Get average point of all office positions
        Vector3 officePosition = Vector3.zero;

        for (int i = 0; i < functions.Length; i++) {
            officePosition += GetOfficePosition(functions[i]);
        }

        officePosition /= functions.Length;

        Vector3 position = new Vector3(officePosition.x + randomOffset.x, 1.1f, officePosition.z + randomOffset.z);
        //Vector3 position = new Vector3(officePosition.x + randomOffset.x, 5f, officePosition.z + randomOffset.z);
        //Vector3 position = new Vector3(officePosition.x + randomOffset.x, -0.5f, officePosition.z + randomOffset.z);
        jobRole.transform.position = position;

        //jobRole.SetOriginalPosition(position);
        jobRole.InitializeOriginalPosition();
    }

    public void ShowJobRolesOnTable() {
        for (int i = 0; i < nodesList.Count; i++) {
            nodesList[i].transform.DOMoveY(-0.55f, Utilities.animationSpeed).SetDelay(Random.Range(0.0f, 0.5f));
            nodesList[i].SetLineEndAnchor(jobFamilyDict[nodesList[i].GetJobRoleData().family].transform);
            //nodesList[i].Highlight();
        }

        /*
        foreach (KeyValuePair<string, JobFamilyObject> jobFamily in jobFamilyDict) {
            jobFamily.Value.transform.DOMoveY(-0.5f, Utilities.animationSpeed).SetDelay(Random.Range(0.0f, 0.5f));
        }
        */
    }

    private Vector3 GetOfficePosition(string office) {
        Vector3 position = Vector3.zero;

        switch (office) {
            case "Front office":
                position = frontOffice.position;
                break;
            case "Middle office":
                position = middleOffice.position;
                break;
            case "Back office":
                position = backOffice.position;
                break;
        }

        return position;
    }

    private void AddToList(List<string> list, string item) {
        item = item.Trim();

        if (list.Contains(item) || item.Length == 0) {
            return;
        }

        list.Add(item);
    }

    private void AddStringListToList(List<string> list, string item, char delimitter = '\n') {
        string[] itemList = item.Split(delimitter);

        //Debug.Log("Item list count: " + itemList.Length);
        for (int i = 0; i < itemList.Length; i++) {
            AddToList(list, itemList[i]);
        }
    }
}
