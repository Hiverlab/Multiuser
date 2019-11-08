using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataNodeCollider : MonoBehaviour
{
    [SerializeField]
    private DataNode parentNode;

    private void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("PlayerTouch")) {
            parentNode.OnTouchEnter();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag.Equals("PlayerTouch")) {
            parentNode.OnTouchExit();
        }
    }
}
