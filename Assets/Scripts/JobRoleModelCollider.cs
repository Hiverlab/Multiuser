using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobRoleModelCollider : MonoBehaviour {

    [SerializeField]
    private JobRoleObject jobRoleParent;

    private void OnTriggerStay(Collider other) {

        if (other.tag == "Node") {
            //Debug.Log("Colliding");

            // Calculate Angle Between the collision point and the player
            Vector3 dir = other.transform.position - transform.position;

            // We then get the opposite (-Vector3) and normalize it
            dir = -dir.normalized;

            Vector3 normalizedDir = new Vector3(dir.x + 1, 0, dir.z + 1);

            jobRoleParent.transform.position += normalizedDir * 0.1f;
        }

        //Debug.Log("Colliding with: " + other);
    }

    public void ShowSkillPanel() {
        jobRoleParent.ToggleJobPanel();
    }
}
