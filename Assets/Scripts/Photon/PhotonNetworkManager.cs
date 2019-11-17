using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class PhotonNetworkManager : MonoBehaviourPunCallbacks, ILobbyCallbacks {
	public static PhotonNetworkManager instance;

	public Transform OVRCameraRig;
    public Transform avatarSpawnPoint;

    private PhotonView myPhotonView;

	public bool isMaster;

	public string roomName;
	public List<string> playerIDList;

	public UnityEvent OnConnectSucessful;
	public UnityEvent OnDisconnect;
	public UnityEvent OnJoinRoomSuccessful;
	public UnityEvent OnCreateOrJoinRoomFailed;

	[Header("DEBUG")]
	[SerializeField]
	private ClientState networkClientState;

    [SerializeField]
    private TextMeshProUGUI networkTextmesh;

    private void DebugText(string text) {
        Debug.Log("[PHOTON NETWORK MANAGER] " + text);
    }

	private void Awake()
	{
		instance = this;
		myPhotonView = this.GetComponent<PhotonView>();
	}

	// Start is called before the first frame update
	void Start()
    {
		if (!PhotonNetwork.IsConnected)
		{
			Connect();
		}
		else
		{
			OnConnectSucessful.Invoke();
		}

		PhotonNetwork.KeepAliveInBackground = 99999999;

    }

    // Update is called once per frame
    void Update()
    {
        /*
		networkClientState = PhotonNetwork.NetworkClientState;
        //Debug.Log("Status: " + PhotonNetwork.NetworkClientState);

        networkTextmesh.text = networkClientState.ToString();

        if (PhotonNetwork.InRoom)
		{
			roomName = PhotonNetwork.CurrentRoom.Name;

			playerIDList = new List<string>();
			foreach (Player player in PhotonNetwork.PlayerList)
			{
				playerIDList.Add(player.UserId);
			}
		}
        */
    }

	public void Connect()
	{
		PhotonNetwork.ConnectUsingSettings();
	}

    public void JoinOrCreateRoom() {
        string roomName = "multiuserdataviz01";

        PhotonNetwork.AutomaticallySyncScene = true;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 8;
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public void CreateRoom(string roomName = "6666") {
        // If join room successful
        bool didJoinRoomSucceed = PhotonNetwork.JoinRoom(roomName);

        if (didJoinRoomSucceed) {
            DebugText("Join room succeeded: " + didJoinRoomSucceed);
            CreateLocalAvatar();
            return;
        }

        // If not create a room
        RoomOptions options = new RoomOptions();
        options.EmptyRoomTtl = 1;
        options.IsOpen = true;
        options.IsVisible = false;
        options.MaxPlayers = 0;
        options.PlayerTtl = 100;

        ExitGames.Client.Photon.Hashtable customPropertiesToSet = new ExitGames.Client.Photon.Hashtable();
        options.CustomRoomProperties = customPropertiesToSet;

        PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default);
    }

	public void CreateRoom()
	{
		string roomName = "";
		for (int i = 0; i < 4; i++)
		{
			roomName = roomName + Random.Range(0, 9).ToString();
		}

		RoomOptions options = new RoomOptions();
		options.EmptyRoomTtl = 1;
		options.IsOpen = true;
		options.IsVisible = false;
		options.MaxPlayers = 0;
		options.PlayerTtl = 100;

		ExitGames.Client.Photon.Hashtable customPropertiesToSet = new ExitGames.Client.Photon.Hashtable();
		options.CustomRoomProperties = customPropertiesToSet;

		PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default);
	}

    public void JoinRoom(string roomCode = "6666") {
        DebugText("Attempting to join room: " + roomCode);

        if (string.IsNullOrEmpty(roomCode)) {
            OnCreateOrJoinRoomFailed.Invoke();
            return;
        }

        PhotonNetwork.JoinRoom(roomCode);
    }

    public void JoinRoom(TMP_InputField roomCodeInputField)
	{
		if (string.IsNullOrEmpty(roomCodeInputField.text))
		{
			OnCreateOrJoinRoomFailed.Invoke();
			return;
		}

		PhotonNetwork.JoinRoom(roomCodeInputField.text);
	}

	public void QuitRoom()
	{
		PhotonNetwork.LeaveRoom();
	}

	public void CallSwitchScene(int sceneIndex)
	{
		//SceneController.instance.SwitchScene(sceneIndex);
	}

	#region OVERRIDEN PHOTON CALLBACKS

	public override void OnConnectedToMaster()
	{
		base.OnConnected();
		OnConnectSucessful.Invoke();

		PhotonNetwork.JoinLobby();
	}
    
    public override void OnJoinedLobby() {
        DebugText("Joined lobby");
        StartCoroutine(JoinLobbyCoroutine());
    }

    private IEnumerator JoinLobbyCoroutine() {
        yield return new WaitForSeconds(1.0f);
        //CreateRoom("6666");
        JoinOrCreateRoom();
    }

    public override void OnJoinedRoom()
	{
        Debug.Log("Joined room successfully");

        OnJoinRoomSuccessful.Invoke();
        
        GameObject newVoiceReceiver = PhotonNetwork.Instantiate("VoiceReceiver", Vector3.zero, Quaternion.identity);
        Debug.Log("Voice Receiver instantiated: " + newVoiceReceiver.GetComponent<PhotonView>().ViewID);

        CreateLocalAvatar();
    }

    public void CreateLocalAvatar() {
        GameObject localAvatar = Instantiate(Resources.Load("LocalAvatar")) as GameObject;
        PhotonView photonView = localAvatar.GetComponent<PhotonView>();

        if (PhotonNetwork.AllocateViewID(photonView)) {
            Debug.Log("Allocated viewid: " + photonView);

            // Set camera rig position to spawn point
            OVRCameraRig.position = avatarSpawnPoint.transform.position;
            OVRCameraRig.rotation = avatarSpawnPoint.transform.rotation;

            // Set local avatar transform to OVR camera transform
            localAvatar.transform.position = avatarSpawnPoint.transform.position;
            localAvatar.transform.rotation = avatarSpawnPoint.transform.rotation;

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
                CachingOption = EventCaching.AddToRoomCache,
                Receivers = ReceiverGroup.Others
            };

            SendOptions sendOptions = new SendOptions {
                Reliability = true
            };

            bool isEventRaised = PhotonNetwork.RaiseEvent(PhotonRaiseEventComponent.instance.InstantiateVrAvatarEventCode, photonView.ViewID, raiseEventOptions, sendOptions);
            Debug.Log("Is event raised: " + isEventRaised);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Is master");
                //PhotonNetwork.RaiseEvent(PhotonRaiseEventComponent.instance.MasterClientEventCode, photonView.ViewID, raiseEventOptions, sendOptions);
            } else
            {
                // Disable watch
                UIWatch.instance.gameObject.SetActive(false);
            }

        } else {
            Debug.LogError("Failed to allocate a ViewId.");

            Destroy(localAvatar);
        }

        Debug.Log("Created local avatar");

        if (SceneManager.GetActiveScene().name == "Scene - Mapbox") {
            //GoogleSheetsFetcher.instance.Initialize();
        }
    }

    public void CreateLocalAvatarOld ()
	{
		Debug.Log("Creating Local Avatar now...");

		

		GameObject localAvatar = Instantiate(Resources.Load("LocalAvatar"), OVRCameraRig.position, OVRCameraRig.rotation) as GameObject;
		PhotonView photonView = localAvatar.GetComponent<PhotonView>();
		//localAvatar.GetComponent<PhotonPlayerSyncComponent>().playerRoleIndex = roleIndex;


		if (PhotonNetwork.AllocateViewID(photonView))
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions
			{
				CachingOption = EventCaching.AddToRoomCache,
				Receivers = ReceiverGroup.Others
			};

			SendOptions sendOptions = new SendOptions
			{
				Reliability = true
			};

			if (!PhotonRoomSyncComponent.instance.isRole1Occupied)
			{
				localAvatar.GetComponent<PhotonPlayerSyncComponent>().SetPlayerRole(1);

				//OVRCameraRig.position = PlayAreaProperties.instance.leftSpawnPoint.position;
				//OVRCameraRig.rotation = PlayAreaProperties.instance.leftSpawnPoint.rotation;
				localAvatar.transform.position = OVRCameraRig.position;
				localAvatar.transform.rotation = OVRCameraRig.rotation;
				PhotonNetwork.LocalPlayer.NickName = photonView.ViewID.ToString();
				myPhotonView.RPC("UpdateRoleOccupation", RpcTarget.MasterClient, 1, true, PhotonNetwork.LocalPlayer.ActorNumber.ToString());

			}
			else if (!PhotonRoomSyncComponent.instance.isRole2Occupied)
			{
				localAvatar.GetComponent<PhotonPlayerSyncComponent>().SetPlayerRole(2);

				//OVRCameraRig.position = PlayAreaProperties.instance.rightSpawnPoint.position;
				//OVRCameraRig.rotation = PlayAreaProperties.instance.rightSpawnPoint.rotation;
				localAvatar.transform.position = OVRCameraRig.position;
				localAvatar.transform.rotation = OVRCameraRig.rotation;
				PhotonNetwork.LocalPlayer.NickName = photonView.ViewID.ToString();
				myPhotonView.RPC("UpdateRoleOccupation", RpcTarget.MasterClient, 2, true, PhotonNetwork.LocalPlayer.ActorNumber.ToString());

			}
			else
			{
				//SceneController.instance.SwitchScene(0);
				return;
			}

			PhotonNetwork.RaiseEvent(PhotonRaiseEventComponent.instance.InstantiateVrAvatarEventCode,
                photonView.ViewID, raiseEventOptions, sendOptions);

		}
		else
		{
			Debug.LogError("Failed to allocate a ViewId.");

			Destroy(localAvatar);
		}
	}

	public void CreateRemoteAvatarsOnStart()
	{
		foreach (Player otherPlayer in PhotonNetwork.PlayerListOthers)
		{
			if (!otherPlayer.CustomProperties.ContainsKey("isSpectator"))
			{
				GameObject remoteAvatar = Instantiate(Resources.Load("RemoteAvatar")) as GameObject;
				PhotonView photonView = remoteAvatar.GetComponent<PhotonView>();
				photonView.ViewID = int.Parse(otherPlayer.NickName);
			}
			
		}
	}

	public override void OnLeftRoom()
	{
		base.OnLeftRoom();
		//SceneController.instance.SwitchScene(0);
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		base.OnCreateRoomFailed(returnCode, message);
		OnCreateOrJoinRoomFailed.Invoke();
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		base.OnJoinRoomFailed(returnCode, message);
		OnCreateOrJoinRoomFailed.Invoke();
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		base.OnPlayerLeftRoom(otherPlayer);

		//if (RemoteAvatarStorageComponent.instance)
		//{
		//	RemoteAvatarStorageComponent.instance.RemoveRemoteAvatar(otherPlayer);
		//}
	}

	public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
	{
		Debug.Log("OnRoomPropertiesUpdate invoked!!!");
        /*
		if (OrderManagerNew.instance && !photonView.IsMine)
		{
			OrderManagerNew.instance.SyncCurrentOrderFromMaster();
		}
        */

		base.OnRoomPropertiesUpdate(propertiesThatChanged);
	}

	#endregion


	#region DEBUG
	// DEBUG ==============================================================================================================

	public void DebugJoinRoom()
	{
		RoomOptions options = new RoomOptions();
		options.EmptyRoomTtl = 1;
		options.IsOpen = true;
		options.IsVisible = true;
		options.MaxPlayers = 0;
		options.PlayerTtl = 100;

		PhotonNetwork.JoinOrCreateRoom("TEST", options, TypedLobby.Default);
	}

	// ====================================================================================================================
	#endregion
}
