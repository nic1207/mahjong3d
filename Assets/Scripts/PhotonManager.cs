using UnityEngine;
using System.Collections;
using Photon;

public class PhotonManager : PunBehaviour {
	public static PhotonManager instance;
	public Transform PlayerPrefab;
	public GameObject Canvas;
	private string _gameVersion = "1";
	private GameObject _plobby;
	private GameObject _proom;

	public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
	[Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
	public byte MaxPlayersPerRoom = 4;

	void Awake()
	{
		if(instance != null)
		{
			DestroyImmediate(gameObject);
			return;
		}
		DontDestroyOnLoad(this.gameObject);
		instance = this;
		PhotonNetwork.logLevel = Loglevel;
		//PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.autoJoinLobby = true;
		if (Canvas) {
			_plobby = Canvas.transform.FindChild ("PanelLobby").gameObject;
			_proom = Canvas.transform.FindChild ("PanelRoom").gameObject;
		}
		PhotonNetwork.automaticallySyncScene = true;
	}

	// Use this for initialization
	void Start () {
		PhotonNetwork.ConnectUsingSettings(_gameVersion);
		PhotonNetwork.playerName = Random.Range(0,10).ToString();
		//Connect();
		//InvokeRepeating("UpdatePing", 2, 2);
	}

	void OnReceivedRoomListUpdate()
	{
		//Debug.Log ("!!!!");
		Debug.Log ("目前房間數:"+PhotonNetwork.GetRoomList().Length);
		foreach (RoomInfo game in PhotonNetwork.GetRoomList())
		{
			//GUILayout.Label(game.name + " " + game.playerCount + "/" + game.maxPlayers);
			Debug.Log("房名:"+game.name + "玩家" + game.playerCount + "/" + game.maxPlayers);
		}
	}

	void OnJoinedLobby()
	{
		Debug.Log ("JoinedLobby()");
		if (_plobby) {
			_plobby.SetActive (true);
		}
		if (_proom) {
			_proom.SetActive (false);
		}
		//foreach (RoomInfo game in PhotonNetwork.GetRoomList())
		//{
			//GUILayout.Label(game.name + " " + game.playerCount + "/" + game.maxPlayers);
		//	Debug.Log(game.name + " " + game.playerCount + "/" + game.maxPlayers);
		//}


		//RoomOptions options = new RoomOptions();
		//options.MaxPlayers = 4;
		//PhotonNetwork.JoinOrCreateRoom("mjroom", options, null);
		//PhotonNetwork.JoinRoom("mjroom");
	}

	void OnPhotonRandomJoinFailed()
	{
		//PhotonNetwork.CreateRoom(null);
	}

	void OnJoinedRoom()
	{
		Debug.Log("您已進入遊戲室");
		if (_plobby) {
			_plobby.SetActive (false);
		}
		if (_proom) {
			_proom.SetActive (true);
		}
		//PhotonNetwork.Instantiate("opp", transform.position, Quaternion.identity, 0 );
		//Debug.Log( PhotonNetwork.playerName);
		Debug.Log ("PhotonNetwork.playerList.Length="+PhotonNetwork.playerList.Length);
		foreach (PhotonPlayer pl in PhotonNetwork.playerList) {
			Debug.Log ("id:"+pl.ID+" name:"+pl.NickName);
		}
		Debug.Log ("roomname:"+PhotonNetwork.room.Name);
		if (PhotonNetwork.playerList.Length > 2) {
			
		}
		if(PhotonNetwork.isMasterClient)
		{
			PhotonNetwork.LoadLevel("room");
		}
	}

	void OnLevelWasLoaded(int levelNumber)
	{
		// 若不在Photon的遊戲室內, 則網路有問題..
		if (!PhotonNetwork.inRoom)
			return;
		Debug.Log("我們已進入遊戲場景了,耶~");
	}

	void UpdatePing() {
		int pingRate = PhotonNetwork.GetPing();
		Debug.Log( "Ping: " + pingRate );
	}

	public void JoinOrCreateRoom() {
		RoomOptions options = new RoomOptions();
		options.MaxPlayers = 4;
		PhotonNetwork.JoinOrCreateRoom("mjroom", options, TypedLobby.Default);
	}

	public void StartPlay() {
		Debug.Log ("StartPlay()");
		PhotonNetwork.LoadLevel ("room");
	}

	/*
	public void Connect()
	{

		// we check if we are connected or not, we join if we are , else we initiate the connection to the server.
		if (PhotonNetwork.connected)
		{
			// #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnPhotonRandomJoinFailed() and we'll create one.
			PhotonNetwork.JoinRandomRoom();
		}else{
			// #Critical, we must first and foremost connect to Photon Online Server.
			PhotonNetwork.ConnectUsingSettings(_gameVersion);
		}
	}
	*/

	/*
	// Update is called once per frame
	void Update () {
	
	}

	public void JoinGameRoom()
	{
		RoomOptions options = new RoomOptions();
		options.MaxPlayers = 4;
		PhotonNetwork.JoinOrCreateRoom("Fighting Room", options, null);
	}
	*/
	/*
	public override void OnConnectedToMaster()
	{

		Debug.Log("OnConnectedToMaster() was called by PUN");

	}

	public override void OnDisconnectedFromPhoton()
	{

		Debug.LogWarning("DemoAnimator/Launcher: OnDisconnectedFromPhoton() was called by PUN");        
	}

	public override void OnPhotonRandomJoinFailed (object[] codeAndMsg)
	{
		Debug.Log("DemoAnimator/Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");

		// #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
		PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = MaxPlayersPerRoom }, null);
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
	}
	*/

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}
}
