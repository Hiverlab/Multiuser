using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using TMPro;

public class UI3DButton : MonoBehaviour
{
    [SerializeField]
    public string textToSet;

    public virtual void ConfirmAction()
    {

    }

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

    protected virtual void OnButtonPress()
    {
    }

    protected virtual void OnButtonRelease()
    {
    }

    #endregion

}
