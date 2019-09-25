using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRaiseEventComponent : MonoBehaviourPunCallbacks, IOnEventCallback
{
	public static PhotonRaiseEventComponent instance;

	public List<GameObject> remoteAvatarToBeLoadedList;

	public readonly byte InstantiateVrAvatarEventCode = 42;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this.gameObject);
		}

		DontDestroyOnLoad(this.gameObject);
	}

	public void OnEvent(EventData photonEvent)
	{
        //Debug.Log("Photon event raised: " + photonEvent);

		if (photonEvent.Code == InstantiateVrAvatarEventCode)// && SceneManager.GetActiveScene().buildIndex == 1)
		{
			Debug.Log("Creating remote avatar...");

			GameObject remoteAvatar = Instantiate(Resources.Load("RemoteAvatar")) as GameObject;
			PhotonView photonView = remoteAvatar.GetComponent<PhotonView>();
			photonView.ViewID = (int)photonEvent.CustomData;
		}
	}

	public override void OnEnable()
	{
		PhotonNetwork.AddCallbackTarget(this);
	}

	public override void OnDisable()
	{
		PhotonNetwork.RemoveCallbackTarget(this);
	}
}
