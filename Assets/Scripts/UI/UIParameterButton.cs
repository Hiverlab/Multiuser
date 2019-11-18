using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using Doozy.Engine.UI;
using TMPro;
using Photon.Pun;

public class UIParameterButton : MonoBehaviour
{
    [Header("Toggle References")]
    [SerializeField]
    private TextMeshProUGUI textMesh;

    [SerializeField]
    private Toggle toggle;

    [SerializeField]
    private UIToggle uiToggle;

    [Header("Style Panel References")]
    [SerializeField]
    private CanvasGroup stylePanelCanvasGroup;

    [SerializeField]
    private TMP_Dropdown optionsDropdown;

    private List<string> optionsList;

    private bool canPress = true;

    #region Properties

    private string parameter;

    public string Parameter
    {
        get
        {
            return parameter;
        }
        set
        {
            parameter = value;

            SetText(parameter);
        }
    }

    private DataNode.DimensionType dimensionType;

    public DataNode.DimensionType DimensionType
    {
        get
        {
            return dimensionType;
        }
        set
        {
            dimensionType = value;
        }
    }

    #endregion

    #region Setters

    public void SetText(string text)
    {
        textMesh.text = text;

        transform.name = text;
    }

    public void SetToggleGroup(ToggleGroup toggleGroup)
    {
        if (!toggle)
        {
            return;
        }

        toggle.group = toggleGroup;

        toggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(toggle);
        });
    }

    public void Initialize()
    {
        UIController.instance.OnPanelSelected += OnPanelSelected;
    }

    private void OnPanelSelected(string uiPanelName)
    {
        // If not this parameter button
        if (!uiPanelName.Equals(transform.name))
        {
            return;
        }

        // If cannot press
        if (!canPress)
        {
            return;
        }

        Debug.Log("On Panel Selected");

        // Set can press to false
        canPress = false;

        uiToggle.IsOn = !uiToggle.IsOn;

        SetParameterAndDimension();

        StartCoroutine(ResetCanPressCoroutine());
    }

    private IEnumerator ResetCanPressCoroutine()
    {
        yield return new WaitForSeconds(Utilities.animationSpeed);
        canPress = true;

        Debug.Log("Can press");
    }

    public void PopulateDropdownOptions()
    {
        // Clear options dropdown
        optionsDropdown.ClearOptions();

        // Create new options list
        optionsList = new List<string>();

        // Populate options list based on dimension type
        string[] dimensionTypeNames = System.Enum.GetNames(typeof(DataNode.DimensionType));
        for (int i = 0; i < dimensionTypeNames.Length; i++)
        {
            //Debug.Log(dimensionTypeNames[i]);

            optionsList.Add(dimensionTypeNames[i]);
        }

        // Add options list to dropdown
        optionsDropdown.AddOptions(optionsList);

        HideDimensionTypePanel();
    }

    #endregion

    #region General

    public DataNode.DimensionType GetDimensionTypeFromString(string dimensionTypeText)
    {
        return (DataNode.DimensionType)System.Enum.Parse(typeof(DataNode.DimensionType), dimensionTypeText);
    }

    public void SetParameterAndDimension()
    {
        //UIController.instance.OnParameterSelected(Parameter, DimensionType);

        PhotonNetworkManager.instance.photonView.RPC(RPCManager.instance.GetRPC(RPCManager.RPC.RPC_SetParameterAndDimension),
            Photon.Pun.RpcTarget.All, Parameter, DimensionType);
    }

    #endregion

    #region Toggles 

    public void OnValueChanged(bool value)
    {
        if (value)
        {
            OnToggleOn();
        }
        else
        {
            OnToggleOff();
        }
    }

    // When toggle is on
    private void OnToggleOn()
    {
        ShowDimensionTypePanel();
    }

    private void OnToggleOff()
    {
        HideDimensionTypePanel();
    }

    private void ShowDimensionTypePanel()
    {
        stylePanelCanvasGroup.DOFade(1.0f, Utilities.animationSpeed).OnComplete(() =>
        {
            stylePanelCanvasGroup.interactable = true;
            stylePanelCanvasGroup.blocksRaycasts = true;
        });
    }

    private void HideDimensionTypePanel()
    {
        stylePanelCanvasGroup.interactable = true;
        stylePanelCanvasGroup.blocksRaycasts = true;
        stylePanelCanvasGroup.DOFade(0.0f, Utilities.animationSpeed);
    }

    #endregion

    #region Dropdowns

    public void OnDropdownValueChanged()
    {
        // Get value of dropdown
        DimensionType = GetDimensionTypeFromString(optionsDropdown.options[optionsDropdown.value].text);

        // Set parameter type and dimension
        SetParameterAndDimension();
    }

    #endregion

    #region Touch interactions

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Key"))
        {
            OnButtonPress();

            Utilities.instance.VibrateController(100, 0.25f, 0.15f, other.tag);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Key"))
        {
            OnButtonRelease();

            Utilities.instance.VibrateController(100, 0.25f, 0.15f, other.tag);
        }
    }

    private void ToggleValueChanged(Toggle change)
    {
        // Hide dropdown options
        optionsDropdown.Hide();

        if (change.isOn)
        {
            transform.DOLocalMoveZ(-0.015f, Utilities.animationSpeed);
        }
        else
        {
            transform.DOLocalMoveZ(0.0f, Utilities.animationSpeed).OnComplete(() =>
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
            });
        }
    }

    private void OnButtonPress()
    {
        /*
        if (!canPress)
        {
            return;
        }

        Debug.Log("On Button Press");

        canPress = false;

        //uiToggle.SelectToggle();

        uiToggle.IsOn = !uiToggle.IsOn;

        //uiToggle.ExecuteOnButtonSelected();

        //UIController.instance.SelectParameter(Parameter);

        // If dimension type is not none
        //if (DimensionType != DataNode.DimensionType.None) {
            SetParameterAndDimension();
        //}
        */

        // Send RPC to all saying that this button is selected
        //PhotonNetworkManager.instance.photonView.RPC(RPCManager.instance.GetRPC(RPCManager.RPC.RPC_ParameterPanelSelected),
        //    Photon.Pun.RpcTarget.All, transform.name);

        OnPanelSelected(transform.name);
    }

    private void OnButtonRelease()
    {
        if (canPress)
        {
            return;
        }

        Debug.Log("On Button Release");

        //canPress = true;

        /*
        // If this parameter is available
        if (UIController.instance.IsParameterAvailable(this)) {
            // Add to selected paramaters
            UIController.instance.AddToSelectedParameters(this);

            // Make sure to return so we don't turn it back to available
            return;
        }

        // If this parameter is selected
        if (UIController.instance.IsParameterSelected(this)) {
            // Add to available paramaters
            UIController.instance.AddToAvailableParameters(this);

            // Make sure to return so we don't turn it back to selected
            return;
        }
        */
    }

    /*
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(optionsDropdown.value);
            stream.SendNext(Parameter);
            stream.SendNext(DimensionType);
        }

        else if (stream.IsReading)
        {
            optionsDropdown.value = (int)stream.ReceiveNext();
            Parameter = (string)stream.ReceiveNext();
            DimensionType = (DataNode.DimensionType)stream.ReceiveNext();


            Debug.Log("Receiving options value: " + optionsDropdown.value);
        }
    }
    */

    #endregion
}
