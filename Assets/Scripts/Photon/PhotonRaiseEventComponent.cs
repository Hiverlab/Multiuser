﻿using ExitGames.Client.Photon;
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
    public readonly byte InstantiateJobRoleEventCode = 43;
    public readonly byte MasterClientEventCode = 44;

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
            StartCoroutine(CreateRemoteAvatarCoroutine(photonEvent));
        }

        if (photonEvent.Code == InstantiateJobRoleEventCode) {

        }

        if (photonEvent.Code == MasterClientEventCode)
        {
            Debug.Log("Master client event");
        }
	}

    private IEnumerator CreateRemoteAvatarCoroutine(EventData photonEvent) {
        // If spectator manager is active
        if (SpectatorManager.instance.isSpectatorActive)
        {
            // Return
            yield return null;
        }

        yield return new WaitForSeconds(2.0f);

        Debug.Log("Creating remote avatar...");

        GameObject remoteAvatar = Instantiate(Resources.Load("RemoteAvatar")) as GameObject;
        Debug.Log("Instantiated remote avatar");

        PhotonView photonView = remoteAvatar.GetComponent<PhotonView>();
        Debug.Log("Got photon view");

        photonView.ViewID = (int)photonEvent.CustomData;
        Debug.Log("Set photon view id");

        // If master target not already assigned
        if (!SpectatorManager.instance.masterTarget)
        {
            Debug.Log("Assigning master target");
            SpectatorManager.instance.SetMasterTarget(remoteAvatar);
        }

    }

	public void OnEnable()
	{
		PhotonNetwork.AddCallbackTarget(this);
	}

	public void OnDisable()
	{
		PhotonNetwork.RemoveCallbackTarget(this);
	}
}
