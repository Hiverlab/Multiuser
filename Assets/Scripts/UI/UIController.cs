using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    #region Properties
    
    // Available parameters list
    private List<UIParameterButton> availableParametersList;

    // Selected parameters list
    private List<UIParameterButton> selectedParametersList;

    #endregion

    #region Parameter Container References

    [Header("Available Parameters References")]
    [SerializeField]
    private GridLayoutGroup availableParametersContainer;

    [Header("Selected Parameters References")]
    [SerializeField]
    private GridLayoutGroup selectedParametersContainer;

    #endregion

    [Header("General")]
    [SerializeField]
    private UIParameterButton uiParameterButtonPrefab;

    private void Awake() {
        if (!instance) {
            instance = this;
        } else {
            Destroy(instance);
        }

        Initialize();
    }

    private void Initialize() {
        availableParametersList = new List<UIParameterButton>();
        selectedParametersList = new List<UIParameterButton>();
    }

    public void AddNewParameter(string parameter) {
        UIParameterButton uiParameterButton = Lean.Pool.LeanPool.Spawn(uiParameterButtonPrefab);

        Debug.Log(uiParameterButton);
        Debug.Log("Assigning parameter: " + parameter);

        uiParameterButton.Parameter = parameter;

        AddToAvailableParameters(uiParameterButton);
    }

    public void AddToAvailableParameters(UIParameterButton uiParameterButton) {
        // If parameter is selected
        if (selectedParametersList.Contains(uiParameterButton)) {
            // Remove it from the list
            selectedParametersList.Remove(uiParameterButton);
        }

        // Move it to available parameters container
        uiParameterButton.transform.parent = availableParametersContainer;

        // Add it to available parameters list
        availableParametersList.Add(uiParameterButton);
    }

    public void AddToSelectedParameters(UIParameterButton uiParameterButton) {
        // If parameter is available
        if (availableParametersList.Contains(uiParameterButton)) {
            // Remove it from the list
            availableParametersList.Remove(uiParameterButton);
        }

        // Move it to available parameters container
        uiParameterButton.transform.parent = selectedParametersContainer;

        // Add it to available parameters list
        selectedParametersList.Add(uiParameterButton);
    }
}
