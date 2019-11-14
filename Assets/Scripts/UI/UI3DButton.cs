using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI3DButton : MonoBehaviour
{
    [SerializeField]
    private Button button;

    #region Touch interactions

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("PlayerTouch"))
        {
            OnButtonPress();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("PlayerTouch"))
        {
            OnButtonRelease();
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
