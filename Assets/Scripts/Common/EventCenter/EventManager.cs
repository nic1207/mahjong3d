using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


public class EventManager: NetworkBehaviour
{
	public List<IObserver> _observerList = new List<IObserver>();

    //private EventManager() 
    //{
    //    _observerList = new List<IObserver>();
    //}

    public static EventManager Instance = null;
    /*
	public static EventManager Get()
    {
		if (instance == null) {
			GameObject go = new GameObject ("EventManager");
			instance = go.AddComponent<EventManager> ();
			//instance = new EventManager ();
		}
        return instance;
    }
    */
	void Awake() {
		Instance = this;
		//DontDestroyOnLoad ();
	}


    public static void CleanUp()
    {
		Instance._observerList.Clear();
		//Instance = null;
    }

	/*[Client]*/
	/*[ClientRpc]*/
	public void RpcAddObserver(IObserver observer) 
    {
		Debug.Log ("addObserver("+observer+")");
        if(observer == null)
            return;
        
        if(!_observerList.Contains(observer))
            _observerList.Add(observer);
    }

	/*[Client]*/
	/*[ClientRpc]*/
	public void RpcRemoveObserver(IObserver observer) 
    {
		Debug.Log ("removeObserver("+observer+")");
        if(observer == null)
            return;
        
        if(_observerList.Contains(observer))
            _observerList.Remove(observer);
    }

	/*[ClientRpc]*/
	/*[Client]*/
	/*[ClientRpc]*/
    // send ui event.
    public void RpcSendEvent(UIEventType eventType, params object[] args) 
    {
		Debug.Log ("SendEvent(_observerList.Count="+_observerList.Count+")");
        for( int i = 0; i < _observerList.Count; i++ ) 
        {
            IObserver observer = (IObserver)_observerList[i];
            if( observer != null )
                observer.OnHandleEvent(eventType, args);
        }
    }
}
