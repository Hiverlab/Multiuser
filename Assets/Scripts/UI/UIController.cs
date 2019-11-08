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
    private RectTransform availableParametersContainer;

    [Header("Selected Parameters References")]
    [SerializeField]
    private RectTransform selectedParametersContainer;

    #endregion

    [Header("General")]
    [SerializeField]
    private UIParameterButton uiParameterButtonPrefab;

    [SerializeField]
    private ToggleGroup parameterToggleGroup;

    private RectTransform parameterToggleGroupRectTransform;

    public delegate void ParameterSelectedDelegate(string parameter);
    public ParameterSelectedDelegate OnParameterSelected;

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

        parameterToggleGroupRectTransform = parameterToggleGroup.GetComponent<RectTransform>();
    }

    public void AddNewParameter(string parameter) {
        UIParameterButton uiParameterButton = Lean.Pool.LeanPool.Spawn(uiParameterButtonPrefab);

        Debug.Log(uiParameterButton);
        Debug.Log("Assigning parameter: " + parameter);

        uiParameterButton.Parameter = parameter;

        // Set toggle group
        uiParameterButton.SetToggleGroup(parameterToggleGroup);

        // Set parent
        uiParameterButton.transform.parent = parameterToggleGroup.transform;

        // Reset position
        uiParameterButton.transform.localPosition = Vector3.zero;

        // Force rebuild layout
        LayoutRebuilder.ForceRebuildLayoutImmediate(parameterToggleGroupRectTransform);

        //AddToAvailableParameters(uiParameterButton);
    }

    public bool IsParameterAvailable(UIParameterButton uiParameterButton) {
        return availableParametersList.Contains(uiParameterButton);
    }

    public bool IsParameterSelected(UIParameterButton uiParameterButton) {
        return selectedParametersList.Contains(uiParameterButton);
    }

    public void AddToAvailableParameters(UIParameterButton uiParameterButton) {
        // If parameter is selected
        if (IsParameterSelected(uiParameterButton)) {
            // Remove it from the list
            selectedParametersList.Remove(uiParameterButton);
        }

        // Move it to available parameters container
        uiParameterButton.transform.parent = availableParametersContainer.transform;
        uiParameterButton.transform.localPosition = Vector3.zero;
        LayoutRebuilder.ForceRebuildLayoutImmediate(availableParametersContainer);

        // Add it to available parameters list
        availableParametersList.Add(uiParameterButton);
    }

    public void AddToSelectedParameters(UIParameterButton uiParameterButton) {
        // If parameter is available
        if (IsParameterAvailable(uiParameterButton)) {
            // Remove it from the list
            availableParametersList.Remove(uiParameterButton);
        }

        // Move it to available parameters container
        uiParameterButton.transform.parent = selectedParametersContainer.transform;
        uiParameterButton.transform.localPosition = Vector3.zero;
        LayoutRebuilder.ForceRebuildLayoutImmediate(selectedParametersContainer);

        // Add it to available parameters list
        selectedParametersList.Add(uiParameterButton);
    }

    public void SelectParameter(string parameter) {
        Debug.Log("Parameter: " + parameter + " selected");

        OnParameterSelected?.Invoke(parameter);
    }
}
