using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class JobFamilyObject : MonoBehaviour {

    private string title;

    [SerializeField]
    private TextMeshProUGUI titleTextMesh;

    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private MeshRenderer meshRendender;

    private JobRole data;

    private Color defaultColor;

    private float scale = 1.0f;

    private bool isHighlighted = false;

    [SerializeField]
    private Color highColor;
    [SerializeField]
    private Color mediumColor;
    [SerializeField]
    private Color lowColor;

    private Transform lineEndAnchor;

    private float highlightHeight = 0.9f;

    // Use this for initialization
    void Start() {
        titleTextMesh.text = "";

        sprite.transform.DOLocalMoveY(0.05f, Utilities.animationSpeed * 5).SetLoops(-1, LoopType.Yoyo).SetRelative().SetDelay(Random.Range(0.0f, 0.5f));
    }

    // Update is called once per frame
    void Update() {
    }

    public void SetLineEndAnchor(Transform anchor) {
        lineEndAnchor = anchor;
    }

    public void SetJobRoleData(JobRole _data) {
        data = _data;
    }

    public JobRole GetJobRoleData() {
        return data;
    }

    public void SetImpact(string impact) {
        defaultColor = highColor;

        if (impact.Equals("Medium")) {
            scale = 0.75f;
            defaultColor = mediumColor;
        }

        if (impact.Equals("Low")) {
            scale = 0.50f;
            defaultColor = lowColor;
        }

        SetColor(defaultColor);

        //titleTextMesh.transform.DOLocalMoveY(titleTextMesh.transform.localPosition.y * scale, Utilities.animationSpeed);

        sprite.transform.localScale = Vector3.one * scale;
    }

    public void SetTitle(string _title) {
        title = _title;
        //titleTextMesh.text = title;
    }

    // Used on filter options
    public void Highlight() {
        if (isHighlighted) {
            return;
        }

        Debug.Log("Highlighting: " + name);
        //titleTextMesh.text = title;
        titleTextMesh.DOText(title, Utilities.animationSpeed);

        isHighlighted = true;

        highlightHeight *= Random.Range(0.9f, 1.1f);

        transform.DOLocalMoveY(highlightHeight, Utilities.animationSpeed).SetRelative();

        List<Transform> jobRolesList = DataManager.instance.GetJobRolesByFamilyName(name);

        for (int i = 0; i < jobRolesList.Count; i++) {
            jobRolesList[i].GetComponent<JobRoleObject>().SetFamilySelected(true);
        }
    }

    public void Unhighlight() {

        titleTextMesh.text = "";

        isHighlighted = false;

        transform.DOLocalMoveY(-0.75f, Utilities.animationSpeed);

        List<Transform> jobRolesList = DataManager.instance.GetJobRolesByFamilyName(name);

        for (int i = 0; i < jobRolesList.Count; i++) {
            jobRolesList[i].GetComponent<JobRoleObject>().SetFamilySelected(false);
        }
    }

    // Used when selecting job family, the job roles come up here
    public void Select() {
        
    }

    public void SetColor(Color color) {
        //meshRendender.material.color = color;
        meshRendender.material.DOColor(color, 0.5f);
    }
}
