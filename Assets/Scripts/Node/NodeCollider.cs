using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCollider : MonoBehaviour
{
    [SerializeField]
    private Node parentNode;

    private void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("PlayerTouch")) {
            parentNode.OnTouch();
        }
    }
}
