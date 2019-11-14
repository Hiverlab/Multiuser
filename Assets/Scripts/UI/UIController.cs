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

    public delegate void TestParameterSelectedDelegate(string parameter);
    public TestParameterSelectedDelegate OnTestParameterSelected;

    private string currentParameter;

    public delegate void DimensionTypeSelectedDelegate(DataNode.DimensionType dimensionType);
    public DimensionTypeSelectedDelegate OnDimensionTypeSelected;

    public delegate void ParameterSelectedDelegate(string parameter, DataNode.DimensionType dimensionType);
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

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            OnDimensionTypeSelected(DataNode.DimensionType.ColorScale);
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            OnDimensionTypeSelected(DataNode.DimensionType.Scale);
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            OnDimensionTypeSelected(DataNode.DimensionType.None);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DataNodePopulator.instance.SetMapOrigin("1.28015, 103.845854");
            GoogleSheetsFetcher.instance.Initialize("2132687534");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DataNodePopulator.instance.SetMapOrigin("1.298752, 103.786885");
            GoogleSheetsFetcher.instance.Initialize("734724819");
        }
    }

    public void AddNewParameter(string parameter) {
        UIParameterButton uiParameterButton = Lean.Pool.LeanPool.Spawn(uiParameterButtonPrefab);

        Debug.Log(uiParameterButton);
        Debug.Log("Assigning parameter: " + parameter);

        // Set parameter
        uiParameterButton.Parameter = parameter;

        // Populate dropdown options
        uiParameterButton.PopulateDropdownOptions();

        // Set toggle group
        uiParameterButton.SetToggleGroup(parameterToggleGroup);

        // Set parent
        uiParameterButton.transform.parent = parameterToggleGroup.transform;

        // Reset position
        uiParameterButton.transform.localPosition = Vector3.zero;

        // Reset rotation
        uiParameterButton.transform.localEulerAngles = Vector3.zero;

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

        currentParameter = parameter;

        OnTestParameterSelected?.Invoke(parameter);
    }

    public void SelectDimensionType(DataNode.DimensionType dimensionType) {
        Debug.Log("Dimension Type: " + dimensionType + " selected");

        OnDimensionTypeSelected?.Invoke(dimensionType);
    }
}
