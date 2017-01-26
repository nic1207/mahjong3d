using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Collections;

namespace ExitGames.Demos.UI
{
	/// <summary>
	/// Simple Connection Manager.
	/// Deals with toggle UI menu and player list based on photon status.
	/// </summary>
	public class ConnectionManager : MonoBehaviour {

		public GameObject MenuUI;
		public GameObject RoomUI;
		public Text ConnectionStatusText;
		bool ConnectionInProgress;

		ClientState _clientStateCache;

		public string PlayerName
		{
			get
			{
				return PhotonNetwork.playerName;
			}
			set
			{
				PhotonNetwork.playerName = value;
			}
		}

		void Start()
		{
			PhotonNetwork.autoJoinLobby = true;
			MenuUI.SetActive(true);
			RoomUI.SetActive(false);
		}

		void Update()
		{
			if (_clientStateCache != PhotonNetwork.connectionStateDetailed)
			{
				_clientStateCache = PhotonNetwork.connectionStateDetailed;
				ConnectionStatusText.text = _clientStateCache.ToString();
			}
		}

		public virtual void OnJoinedLobby()
		{
			if (ConnectionInProgress)
			{
				PhotonNetwork.JoinRandomRoom();
				return;
			}

			MenuUI.SetActive(true);
		}
		
		public virtual void OnPhotonRandomJoinFailed()
		{
			if (!ConnectionInProgress)
			{
				return;
			}

			PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 4 }, null);
		}
		
		// the following methods are implemented to give you some context. re-implement them as needed.
		
		public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
		{
			Debug.LogError("Cause: " + cause);
		}


		public void Connect () {

			// Unity UI hack to catch TextField submition. 
			// the player name TextField OnEndEdit calls Connect(), but pressing Esc also means ending the edit, so we catch the esc key and don't proceed.
			if( Input.GetKeyDown( KeyCode.Escape ) ) 
			{
				return;
			}

			ConnectionInProgress = true;
			if (PhotonNetwork.insideLobby)
			{
				PhotonNetwork.JoinRandomRoom();
			}else{
				PhotonNetwork.ConnectUsingSettings("1.0");
			}
			MenuUI.SetActive(false);
		}
		
		public void LeaveRoom(){
			PhotonNetwork.LeaveRoom();
		}

		public virtual void OnLeftRoom()
		{
			MenuUI.SetActive(false);
			RoomUI.SetActive(false);
		}

		public void OnJoinedRoom()
		{
			ConnectionInProgress = false;
			MenuUI.SetActive(false);
			RoomUI.SetActive(true);
		}


	}
}


