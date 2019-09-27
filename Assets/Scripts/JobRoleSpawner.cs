using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobRoleSpawner : MonoBehaviour
{
    [SerializeField]
    private JobRole jobRolePrefab;

    [SerializeField]
    private int amountToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SpawnJobRoles() {
        PhotonNetwork.Instantiate("MyPrefabName", new Vector3(0, 0, 0), Quaternion.identity, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
