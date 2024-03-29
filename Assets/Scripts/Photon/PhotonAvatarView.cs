﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Oculus;

public class PhotonAvatarView : MonoBehaviourPunCallbacks, IPunObservable
{
	private PhotonView photonView;
	private OvrAvatar ovrAvatar;
	private OvrAvatarRemoteDriver remoteDriver;

	public List<byte[]> packetData;

	private int localSequence;
	public string thisOculusID;

    // Start is called before the first frame update
    void Start()
    {
		photonView = GetComponent<PhotonView>();

		if (photonView.IsMine)
		{
			ovrAvatar = GetComponent<OvrAvatar>();

			ovrAvatar.oculusUserID = photonView.ViewID.ToString();
			thisOculusID = ovrAvatar.oculusUserID;

			//ovrAvatar.oculusUserID = Oculus.Platform.Users.GetLoggedInUser().ToString();
			//Debug.Log("User ID : " + ovrAvatar.oculusUserID);
			ovrAvatar.RecordPackets = true;
			ovrAvatar.PacketRecorded += OnLocalAvatarPacketRecorded;

			packetData = new List<byte[]>();
		}
		else
		{
			remoteDriver = GetComponent<OvrAvatarRemoteDriver>();
		}
    }

	// Update is called once per frame
	void Update()
	{
		

		//if (GetComponent<OvrAvatarLocalDriver>())
		//DebugAvatarView.instance.UpdateDebugText(packetData.Count);
	}

	public void OnDisable()
	{
		if (photonView.IsMine)
		{
			ovrAvatar.RecordPackets = false;
			ovrAvatar.PacketRecorded -= OnLocalAvatarPacketRecorded;
		}
	}

	private bool notReadyForSerialization
	{
		get
		{
            return (!PhotonNetwork.InRoom || (PhotonNetwork.CurrentRoom.PlayerCount < 2));/* ||
        !Oculus.Platform.Core.IsInitialized() || !ovrAvatar.Initialized);*/
        }
	}
	

	public void OnLocalAvatarPacketRecorded(object sender, OvrAvatar.PacketEventArgs args)
	{
		

		if (notReadyForSerialization)
		{
			return;
		}

		using (MemoryStream outputStream = new MemoryStream())
		{		
			BinaryWriter writer = new BinaryWriter(outputStream);

			var size = Oculus.Avatar.CAPI.ovrAvatarPacket_GetSize(args.Packet.ovrNativePacket);
			byte[] data = new byte[size];
			Oculus.Avatar.CAPI.ovrAvatarPacket_Write(args.Packet.ovrNativePacket, size, data);

			writer.Write(localSequence++);
			writer.Write(size);
			writer.Write(data);

			packetData.Add(outputStream.ToArray());

			
		}
	}

	private void DeserializeAndQueuePacketData(byte[] data)
	{
		if (notReadyForSerialization)
		{
			return;
		}

		using (MemoryStream inputStream = new MemoryStream(data))
		{
			BinaryReader reader = new BinaryReader(inputStream);
			int remoteSequence = reader.ReadInt32();

			int size = reader.ReadInt32();
			byte[] sdkData = reader.ReadBytes(size);

			System.IntPtr packet = Oculus.Avatar.CAPI.ovrAvatarPacket_Read((System.UInt32)data.Length, sdkData);
			remoteDriver.QueuePacket(remoteSequence, new OvrAvatarPacket { ovrNativePacket = packet });

            //DebugAvatarView.instance.debugText.text = "Packet Received: " + packetData.Count;
            //Debug.Log("Packet received: " + packetData.Count);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (notReadyForSerialization)
		{
			return;
		}	

		if (stream.IsWriting)
		{
			stream.SendNext(thisOculusID);

			if (packetData.Count == 0)
			{
				return;
			}

			stream.SendNext(packetData.Count);

			foreach (byte[] b in packetData)
			{
				stream.SendNext(b);
			}

			packetData.Clear();
		}

		else if (stream.IsReading)
		{
			thisOculusID = (string)stream.ReceiveNext();

			int num = (int)stream.ReceiveNext();

            if (num == 0) {
                return;
            }

			for (int counter = 0; counter < num; ++counter)
			{
				byte[] data = (byte[])stream.ReceiveNext();

				DeserializeAndQueuePacketData(data);
			}
		}
	}


}
