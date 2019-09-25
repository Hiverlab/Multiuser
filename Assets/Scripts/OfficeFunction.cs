using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class OfficeFunction : MonoBehaviour {
    [SerializeField]
    private Transform modelParent;

    [SerializeField]
    private Transform officeBefore;
    [SerializeField]
    private Transform officeAfter;

    [SerializeField]
    private TextMeshPro titleTextMesh;

    [SerializeField]
    private Material[] buildingMaterials;

    private Color[] originalColor;

    private Vector3 originalPosition;

    private Vector3 originalParentPosition;

    [SerializeField]
    private Color dangerLight;

    // Start is called before the first frame update
    void Start() {
        officeBefore.gameObject.SetActive(true);
        officeAfter.gameObject.SetActive(false);

        originalPosition = officeAfter.transform.localPosition;

        originalParentPosition = transform.localPosition;

        officeAfter.transform.localPosition = new Vector3(officeAfter.transform.localPosition.x, -2, officeAfter.transform.localPosition.z);

        StartIdleAnimation();

        InitializeBuildingMaterials();
    }

    private void InitializeBuildingMaterials() {
        originalColor = new Color[buildingMaterials.Length];

        for (int i = 0; i < buildingMaterials.Length; i++) {
            //originalColor[i] = buildingMaterials[i].GetColor("_BaseColor");
            //Debug.Log("Original color: " + buildingMaterials[i].GetColor("_BaseColor"));
        }
    }

    public void SetBuildingMaterialsToRed() {
        originalColor = new Color[buildingMaterials.Length];

        for (int i = 0; i < buildingMaterials.Length; i++) {
            //buildingMaterials[i].SetColor("_EmissionColor", dangerLight);

            //DOTween.To(() => buildingMaterials[i].GetColor("_EmissionColor"), x => buildingMaterials[i].SetColor("_EmissionColor", x), Color.red, Utilities.animationSpeed);
        }
    }

    public void ResetBuildingMaterials() {
        originalColor = new Color[buildingMaterials.Length];

        for (int i = 0; i < buildingMaterials.Length; i++) {
            //buildingMaterials[i].SetColor("_EmissionColor", buildingMaterials[i].GetColor("_BaseColor"));
        }
    }

    private void StartIdleAnimation() {
        officeBefore.DOLocalRotate(new Vector3(officeBefore.localRotation.x, officeBefore.localRotation.y, -45.0f), Utilities.animationSpeed * 6, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        officeAfter.DOLocalRotate(new Vector3(officeAfter.localRotation.x, officeAfter.localRotation.y, -45.0f), Utilities.animationSpeed * 6, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.G)) {
            ShowOfficeAfter();
        }
    }

    public void ShowOfficeAfter() {
        StartCoroutine(ShowOfficeAfterCoroutine());
    }

    private IEnumerator ShowOfficeAfterCoroutine() {
        titleTextMesh.DOFade(0.0f, Utilities.animationSpeed);

        officeBefore.DOLocalMoveY(-4, Utilities.animationSpeed * 5).OnComplete(()=> {
            officeBefore.gameObject.SetActive(false);
        });

        yield return new WaitForSeconds(Utilities.animationSpeed * 5);

        officeAfter.gameObject.SetActive(true);

        officeAfter.DOLocalMoveY(originalPosition.y, Utilities.animationSpeed);
    }

    public void ShowOfficeAfterImmediate() {
        officeAfter.DOKill();

        officeBefore.gameObject.SetActive(false);
        officeAfter.gameObject.SetActive(true);

        transform.localPosition = originalParentPosition;
        officeAfter.localPosition = originalPosition;

        StartIdleAnimation();
    }
}
