using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI3DButton_ModuleConfirm : UI3DButton
{
    [SerializeField]
    private UIWatch uiWatch;

    protected override void OnButtonPress()
    {
        base.OnButtonPress();
    }

    protected override void OnButtonRelease()
    {
        base.OnButtonRelease();

        uiWatch.ConfirmModule();
    }
}
