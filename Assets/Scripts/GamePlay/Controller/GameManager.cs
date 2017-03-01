using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameClientManager gcm;
	public GameServerManager gsm;
	public bool isClientRun = false;
	public static GameManager Instance; 

	void Awake() {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		if (isClientRun) {
			gcm.gameObject.SetActive (true);
			gsm.gameObject.SetActive (false);
		} else {
			gcm.gameObject.SetActive (false);
			gsm.gameObject.SetActive (true);
		}
	}

	public void ChangeState<T> () where T : State
	{
		if (isClientRun) {
			gcm.ChangeState<T> ();
		} else {
			gsm.ChangeState<T> ();
		}
	}

	public MahjongMain GetLogicOwner() {
		MahjongMain mm;
		if (isClientRun) {
			mm = gcm.LogicMain;
		} else {
			mm = gsm.LogicMain;
		}
		return mm;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
