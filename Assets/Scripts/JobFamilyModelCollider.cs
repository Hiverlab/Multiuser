using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobFamilyModelCollider : MonoBehaviour
{
    [SerializeField]
    private JobFamilyObject jobFamilyParent;

    private void OnCollisionStay(Collision other) {
        if (other.transform.tag == "JobFamilyNode") {
            // Calculate Angle Between the collision point and the player
            Vector3 dir = other.transform.position - transform.position;

            // We then get the opposite (-Vector3) and normalize it
            dir = -dir.normalized;

            Vector3 normalizedDir = new Vector3(dir.x + 1, 0, dir.z + 1);

            jobFamilyParent.transform.position += normalizedDir * 0.1f;
        }
    }
}
