using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PhotonRoomSyncComponent : MonoBehaviour, IPunObservable
{
	public static PhotonRoomSyncComponent instance;

	public bool isSync;
	public bool avatarIsLoaded;

	[Space]
	public string sessionStartDateTime;
	public string sessionCountry;
	public string sessionSchool;

	public string roomName;
	public bool isRole1Occupied;
	public bool isRole2Occupied;
	public string role1PlayerID;
	public string role2PlayerID;
	public string role1Name;
	public string role2Name;

	public int gameStateEnumIndex;

	void Awake()
	{
		instance = this;
	}

    // Start is called before the first frame update
    void Start()
    {
		PhotonNetwork.SendRate = 100;

		if (PlayerPrefs.HasKey("country"))
		{
			sessionCountry = PlayerPrefs.GetString("country");
		}
		if (PlayerPrefs.HasKey("school"))
		{
			sessionSchool = PlayerPrefs.GetString("school");
		}
	}

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom)// && SceneManager.GetActiveScene().buildIndex == 1)
		{

			if ((isSync || GetComponent<PhotonView>().IsMine) && !avatarIsLoaded)
			{
				if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("isSpectator"))
				{
					PhotonNetworkManager.instance.CreateLocalAvatar();
				}
				else
				{
					Debug.Log("Load Remote Avatars in Spectator Mode...");
					PhotonNetworkManager.instance.Invoke("CreateRemoteAvatarsOnStart", 0.1f);
					
				}

				isSync = true;
				avatarIsLoaded = true;
			}
		}

		//Debug.Log("(" + SceneManager.GetActiveScene().name +
		//		  ", isRole1Occupied: " + isRole1Occupied +
		//		  ", isRole2Occupied: " + isRole2Occupied +
		//		  ", role1PlayerID: " + role1PlayerID +
		//		  ", role2PlayerID: " + role2PlayerID);
    }

	

	void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting == true)
		{
			stream.SendNext(sessionStartDateTime);
			stream.SendNext(sessionCountry);
			stream.SendNext(sessionSchool);
			stream.SendNext(isRole1Occupied);
			stream.SendNext(isRole2Occupied);
			stream.SendNext(role1PlayerID);
			stream.SendNext(role2PlayerID);
			stream.SendNext(role1Name);
			stream.SendNext(role2Name);

            /*
			if (AccentureGameManager.instance)
			{
				gameStateEnumIndex = (int)AccentureGameManager.instance.currentGameState;
			}
			else
			{
				gameStateEnumIndex = -1;
			}
			*/

			stream.SendNext(gameStateEnumIndex);
		}
		else
		{
			sessionStartDateTime = (string)stream.ReceiveNext();
			sessionCountry = (string)stream.ReceiveNext();
			sessionSchool = (string)stream.ReceiveNext();
			isRole1Occupied = (bool)stream.ReceiveNext();
			isRole2Occupied = (bool)stream.ReceiveNext();
			role1PlayerID = (string)stream.ReceiveNext();
			role2PlayerID = (string)stream.ReceiveNext();
			role1Name = (string)stream.ReceiveNext();
			role2Name = (string)stream.ReceiveNext();

			gameStateEnumIndex = (int)stream.ReceiveNext();

			isSync = true;
		}
	}
}
