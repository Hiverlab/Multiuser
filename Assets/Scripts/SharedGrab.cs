using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedGrab : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col) {
        if (col.CompareTag("OVR_Grab")) {
            if (!this.GetComponent<PhotonView>().IsMine) {
                this.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
            }
        }
    }
}
