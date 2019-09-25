using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ScrollablePanelManager : MonoBehaviour
{
    private bool isMoving = false;

    [SerializeField]
    private Transform scrollablePanel;

    private float amountToMove;
    private float numberOfButtons;

    private ScrollRect scrollRect;

    private List<Transform> buttonList;

    // Start is called before the first frame update
    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();

        scrollRect.horizontalNormalizedPosition = 0.0f;
        scrollRect.verticalNormalizedPosition = 1.0f;
        
        numberOfButtons = scrollablePanel.childCount;
        amountToMove = 1.0f / numberOfButtons * 4;

        Debug.Log("Number of buttons: " + numberOfButtons + " amount to move: " + amountToMove);

        buttonList = new List<Transform>();
        for (int i = 0; i < scrollablePanel.childCount; i++) {
            buttonList.Add(scrollablePanel.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.J)) {
            MoveLeft();
        }

        if (Input.GetKey(KeyCode.K)) {
            MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.V)) {
            UpdateSearchResults("ales");
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            UpdateSearchResults("");
        }
    }

    public void MoveLeft() {
        // Return if is moving
        if (isMoving || scrollRect.verticalNormalizedPosition >= 0.99f) {
            return;
        }
        Debug.Log("Moving left");
        
        isMoving = true;

        scrollRect.DOVerticalNormalizedPos(amountToMove, Utilities.animationSpeed).SetRelative().OnComplete(() => {
            isMoving = false;
        });
    }

    public void MoveRight() {
        // Return if is moving
        if (isMoving || scrollRect.verticalNormalizedPosition <= 0.01f) {
            return;
        }
        Debug.Log("Moving right");

        isMoving = true;
        
        scrollRect.DOVerticalNormalizedPos(-amountToMove, Utilities.animationSpeed).SetRelative().OnComplete(() => {
            isMoving = false;
        });
    }

    public void UpdateSearchResults(string searchTerm) {
        Debug.Log("Updating search results with term: " + searchTerm);

        // Look through children and check if name matches term
        for (int i = 0; i < buttonList.Count; i++) {

            if (buttonList[i].name.Contains(searchTerm) || searchTerm.Length == 0) {
                buttonList[i].gameObject.SetActive(true);
            } else {
                buttonList[i].gameObject.SetActive(false);
            }
        }
    }
}
