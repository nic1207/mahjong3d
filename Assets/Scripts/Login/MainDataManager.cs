using UnityEngine;
using System.Collections;
using System.Net;

public enum ServerStatus {
	None = 0,
	Connecting = 1,
	ConnectSuccess = 2,
	ConnectFailed = 3
}

public class MainDataManager : MonoBehaviour {

	static public MainDataManager Instance;
	public string Token = string.Empty;
	public string Account = string.Empty;
	public string Passwd = string.Empty;

	private ServerStatus _serverStatus = ServerStatus.None;
	private string _serverResult = string.Empty;

	void Awake () {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (this.gameObject);
		} else if (Instance != this) {
			Destroy (this.gameObject);
		}
	}

	private void waitServerStatusCallback(WebExceptionStatus status, string result)
	{
		if (status==WebExceptionStatus.Success){//登入成功
			_serverStatus = ServerStatus.ConnectSuccess;
			_serverResult = result;
			//Token = result;
		} else {//登入失敗
			_serverStatus = ServerStatus.ConnectFailed;
			_serverResult = result;
		}
	}

	public IEnumerator CheckServerStatus(YkiApi.RequestCallBack callback) {
		while (_serverStatus == ServerStatus.None || _serverStatus == ServerStatus.Connecting) {
			yield return new WaitForSeconds (1.0f);
			Debug.Log ("waiting for server response...");
		}
			//yield return null;
		if (callback != null) {
			if (_serverStatus == ServerStatus.ConnectSuccess)
				callback (WebExceptionStatus.Success, _serverResult);
			else if (_serverStatus == ServerStatus.ConnectFailed)
				callback (WebExceptionStatus.ConnectFailure, _serverResult);
		}
		StopCoroutine ("CheckServerStatus");
	}

	public void doLogin(string user, string pass, YkiApi.RequestCallBack callback) {
		_serverStatus = ServerStatus.Connecting;
		YkiApi.Login("0", user, pass, waitServerStatusCallback);
		StartCoroutine(CheckServerStatus(callback));
	}

	public void doAddMember(string mail, string pass, YkiApi.RequestCallBack callback) {
		_serverStatus = ServerStatus.Connecting;
		YkiApi.AddMember(mail, pass, waitServerStatusCallback);
		StartCoroutine(CheckServerStatus(callback));
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
