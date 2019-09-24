using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonPlayerSyncComponent : MonoBehaviour, IPunObservable
{
	public enum PlayerRole
	{
		NotApplicable = 0,
		Role1_Manager = 1,
		Role2_Supplier = 2
	}

	private PlayerRole playerRole;
	public int playerRoleIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

	// Update is called once per frame
	void Update()
	{

		if ((int) playerRole != playerRoleIndex)
		{
			playerRole = (PlayerRole)playerRoleIndex;
		}

		if (GetComponent<OvrAvatarLocalDriver>())
		{
			this.transform.position = PhotonNetworkManager.instance.OVRCameraRig.position;
			this.transform.rotation = PhotonNetworkManager.instance.OVRCameraRig.rotation;
		}
	
    }

	//public void SetPlayerRole(PlayerRole newPlayerRole)
	//{
	//	playerRole = newPlayerRole;
	//}

	public void SetPlayerRole(int newPlayerRoleIndex)
	{
		playerRoleIndex = newPlayerRoleIndex;
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting == true)
		{
			stream.SendNext(playerRoleIndex);
		}
		else
		{
			playerRoleIndex = (int)stream.ReceiveNext();
		}
	}
}
