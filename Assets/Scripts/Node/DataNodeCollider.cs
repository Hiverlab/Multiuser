using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataNodeCollider : MonoBehaviour
{
    [SerializeField]
    private DataNode parentNode;

    private void OnTriggerEnter(Collider other) {

        if (other.tag.Contains("Key"))
        {
            parentNode.OnTouchEnter();

            Utilities.instance.VibrateController(100, 0.25f, 0.15f, other.tag);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag.Contains("Key"))
        {
            parentNode.OnTouchExit();

            Utilities.instance.VibrateController(100, 0.25f, 0.15f, other.tag);
        }
    }
}
