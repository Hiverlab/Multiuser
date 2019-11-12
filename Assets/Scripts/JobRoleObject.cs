using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JobRoleObject : MonoBehaviour {

    private string title;
    private int size;

    [SerializeField]
    private Transform titleCanvas;

    [SerializeField]
    private TextMeshProUGUI titleTextMesh;
    [SerializeField]
    private Image titlePanel;

    [SerializeField]
    private TextMeshProUGUI sizeTextMesh;
    [SerializeField]
    private Image sizePanel;

    [SerializeField]
    private Transform model;

    [SerializeField]
    private MeshRenderer meshRendender;

    [SerializeField]
    private LineRenderer lineToJobFamily;
    [SerializeField]
    private LineRenderer lineToTitle;

    private JobRole data;

    private Color defaultColor;

    private float scale = 1.0f;

    private bool isHighlighted = false;
    private bool isJobFamilySelected = false;

    [SerializeField]
    private Color highColor;
    [SerializeField]
    private Color mediumColor;
    [SerializeField]
    private Color lowColor;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private Sprite defaultSprite;

    [SerializeField]
    private Sprite lowSprite;
    [SerializeField]
    private Sprite mediumSprite;
    [SerializeField]
    private Sprite highSprite;

    private Transform lineEndAnchor;

    private Vector3 originalPosition = Vector3.zero;

    [SerializeField]
    private Transform jobPanel;
    [SerializeField]
    private TextMeshProUGUI impactTextMesh;
    [SerializeField]
    private TextMeshProUGUI skillsTextMesh;

    [SerializeField]
    private Collider jobRoleCollider;

    private float originalJobRoleTitleHeight;

    private bool isShowingJobPanel = false;

    private PhotonView photonView;

    // Use this for initialization
    void Start () {
        titleTextMesh.text = "";

        //titleCanvas.DOLocalMoveY(Random.Range(-0.1f, 0.2f), 0).SetRelative();

        //spriteRenderer.transform.DOLocalMoveY(0.05f, Utilities.animationSpeed * 5).SetLoops(-1, LoopType.Yoyo).SetRelative().SetDelay(Random.Range(0.0f, 5.0f));
        //spriteRenderer.transform.DOLocalRotate(new Vector3(0, -45, 0), Utilities.animationSpeed * 5).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetDelay(Random.Range(0.0f, 5.0f));

        jobRoleCollider.transform.DOLocalMoveY(0.05f, Utilities.animationSpeed * 5).SetLoops(-1, LoopType.Yoyo).SetRelative().SetDelay(Random.Range(0.0f, 5.0f));
        jobRoleCollider.transform.DOLocalRotate(new Vector3(0, -45, 0), Utilities.animationSpeed * 5).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetDelay(Random.Range(0.0f, 5.0f));

        defaultSprite = spriteRenderer.sprite;

        lineToJobFamily.enabled = false;
        lineToTitle.enabled = false;

        jobPanel.gameObject.SetActive(false);

        sizePanel.gameObject.SetActive(false);

        photonView = GetComponent<PhotonView>();

        PhotonNetwork.AllocateViewID(photonView);
    }

    private void SetUpPhotonView() {

        if (PhotonNetwork.AllocateViewID(photonView)) {
            object[] data = new object[]
            {
            transform.position, transform.rotation, photonView.ViewID
            };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
                Receivers = ReceiverGroup.Others,
                CachingOption = EventCaching.AddToRoomCache
            };

            SendOptions sendOptions = new SendOptions {
                Reliability = true
            };

            PhotonNetwork.RaiseEvent(PhotonRaiseEventComponent.instance.InstantiateJobRoleEventCode, data, raiseEventOptions, sendOptions);
        } else {
            Debug.LogError("Failed to allocate a ViewId.");

            Destroy(transform);
        }
    }

    public void SetJobRoleTitleHeight(float y) {
        titleCanvas.DOLocalMoveY(y, Utilities.animationSpeed);
    }

    public void InitializeOriginalPosition() {
        StartCoroutine(InitializeOriginalPositionCoroutine());
    }

    private IEnumerator InitializeOriginalPositionCoroutine() {
        yield return new WaitForSeconds(2.0f);
        SetOriginalPosition(transform.position);

        //jobRoleCollider.enabled = false;
        jobRoleCollider.GetComponent<Rigidbody>().isKinematic = true;
        originalJobRoleTitleHeight = titleCanvas.localPosition.y;
    }

    // Update is called once per frame
    void Update () {
        if (isJobFamilySelected) {
            Vector3 startPoint = new Vector3(spriteRenderer.transform.position.x, spriteRenderer.transform.position.y + 0.05f, spriteRenderer.transform.position.z);
            lineToJobFamily.SetPosition(0, startPoint);

            //Vector3 endPoint = new Vector3(0, titleTextMesh.transform.localPosition.y + 0.05f, 0);
            lineToJobFamily.SetPosition(1, lineEndAnchor.localPosition);
        }

        if (isHighlighted) {
            Vector3 startPoint = new Vector3(spriteRenderer.transform.position.x, spriteRenderer.transform.position.y + 0.05f, spriteRenderer.transform.position.z);
            lineToTitle.SetPosition(0, startPoint);

            Vector3 endPoint = new Vector3(titleTextMesh.transform.position.x, titleTextMesh.transform.position.y + 0.01f, titleTextMesh.transform.position.z);
            lineToTitle.SetPosition(1, endPoint);
        }
    }

    public void SetLineEndAnchor(Transform anchor) {
        lineEndAnchor = anchor;
    }

    public void SetJobRoleData(JobRole _data) {
        data = _data;
    }

    public void SetOriginalPosition(Vector3 position) {
        originalPosition = new Vector3(position.x, 1.1f, position.z);
    }

    public JobRole GetJobRoleData() {
        return data;
    }

    public void SetSkills() {
        string result = "";

        string[] skills = data.skills.Split('\n');

        //Debug.Log("SKills count: " + skills.Length);
        
        for (int i = 0; i < skills.Length; i++) {
            result += (i + 1).ToString() + ". " + skills[i] + "\n";
        }

        skillsTextMesh.text = result;
    }

    public void SetImpact(string impact) {

        defaultColor = highColor;
        defaultSprite = highSprite;
        
        if (impact.Equals("Medium")) {
            scale = 0.75f;
            defaultColor = mediumColor;
            defaultSprite = mediumSprite;
        }

        if (impact.Equals("Low")) {
            scale = 0.50f;
            defaultColor = lowColor;
            defaultSprite = lowSprite;
        }

        SetColor(defaultColor);
        SetSprite(defaultSprite);
        
        spriteRenderer.transform.localScale = Vector3.one * scale;

        //impactTextMesh.text = impact + " Impact";

        SetSkills();
    }

    public void SetTitle(string _title) {
        title = _title;
        //titleTextMesh.text = title;
        //SetSize(0);
    }

    public void SetSize(int _size) {
        //size = Random.Range(1, 99);
        size = _size;
        sizeTextMesh.text = size.ToString();
    }

    public int GetSize() {
        return size;
    }

    public void BringToFront() {
        StartCoroutine(BringToFrontCoroutine());
    }

    private IEnumerator BringToFrontCoroutine() {
        yield return new WaitForSeconds(Utilities.animationSpeed);

        Debug.Log("Bringing to front");

        jobPanel.gameObject.SetActive(true);

        transform.DOKill();
        transform.DOMove(new Vector3(0, 1.6f, -1.5f), Utilities.animationSpeed);

        isShowingJobPanel = true;
    }

    [PunRPC]
    public void RPC_ToggleJobPanel() {
        Debug.Log("Toggle job panel");

        ToggleJobPanel();
    }

    public void ToggleJobPanelRPC() {
        photonView.RPC("RPC_ToggleJobPanel", RpcTarget.All);
    }

    public void ToggleJobPanel() {
        isShowingJobPanel = !isShowingJobPanel;

        if (isShowingJobPanel) {
            jobPanel.gameObject.SetActive(true);
        } else {
            jobPanel.gameObject.SetActive(false);
        }
    }

    public void BringToFront(Vector3 position) {
        /*
        if (transform.position != originalPosition) {
            ReturnToOriginalPosition();
            return;
        }
        */

        transform.DOMove(position, Utilities.animationSpeed);
    }

    public void ReturnToOriginalPosition() {
        jobPanel.gameObject.SetActive(false);

        SetJobRoleTitleHeight(originalJobRoleTitleHeight);

        // Don't do this if not master client
        if (!PhotonNetwork.IsMasterClient) {
            return;
        }

        if (originalPosition == Vector3.zero) {
            return;
        }

        if (transform.position == originalPosition) {
            return;
        }

        transform.DOMove(originalPosition, Utilities.animationSpeed);
    }

    public void Highlight() {
        if (isHighlighted) {
            return;
        }
        //Debug.Log("Highlighting: " + data.role);
        
        sizePanel.gameObject.SetActive(true);

        //titleTextMesh.text = title;

        //titleTextMesh.DOText(title, Utilities.animationSpeed);
        titlePanel.DOFillAmount(1.0f, Utilities.animationSpeed);

        lineToTitle.enabled = true;

        isHighlighted = true;

        // Don't do this if not master client
        if (!PhotonNetwork.IsMasterClient) {
            return;
        }

        float distanceToMove = Random.Range(0.1f, 0.25f);

        transform.DOLocalMoveY(distanceToMove, Utilities.animationSpeed).SetRelative();
    }

    public void Unhighlight() {
        //Debug.Log("Unhighlighting: " + data.role);

        //transform.DOMoveY(-0.5f, Utilities.animationSpeed);
        
        ReturnToOriginalPosition();

        sizePanel.gameObject.SetActive(false);

        titleTextMesh.DOKill();
        titleTextMesh.text = "";
        titlePanel.DOFillAmount(0.0f, Utilities.animationSpeed);

        isHighlighted = false;
        lineToTitle.enabled = false;
    }

    public void SetColor(Color color) {
        Color startColor = new Color(color.r, color.g, color.b, 0.0f);
        Color endColor = new Color(color.r, color.g, color.b, 1.0f);

        lineToTitle.startColor = startColor;
        lineToJobFamily.startColor = startColor;

        lineToTitle.endColor = endColor;
        lineToJobFamily.endColor = endColor;

        titlePanel.color = color;
    }

    public void SetSprite(Sprite sprite) {
        spriteRenderer.sprite = sprite;
    }

    public void SetFamilySelected(bool _isJobFamilySelected) {
        isJobFamilySelected = _isJobFamilySelected;

        if (isJobFamilySelected) {
            lineToJobFamily.enabled = true;
        } else {
            lineToJobFamily.enabled = false;
        }
    }
}
