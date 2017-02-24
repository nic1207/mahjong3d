using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


public class EventManager: MonoBehaviour
{
	public List<IObserver> _observerList = new List<IObserver>();

	private static EventManager s_Instance = null;
    
	public static EventManager Instance
	{
		get {
			if (s_Instance == null) {
				s_Instance = GameObject.FindObjectOfType (typeof(EventManager)) as EventManager;
			}
			//if (s_Instance == null) {
			//	GameObject go = new GameObject ("EventManager");
			//	s_Instance = go.AddComponent<EventManager> () as EventManager;
			//}

			return s_Instance;
		}
    }

    public static void CleanUp()
    {
		Instance._observerList.Clear();
		//Instance = null;
    }

	/*[Client]*/
	/*[Client]*/
	public void AddObserver(IObserver observer) 
    {
		Debug.Log ("EventManager.addObserver("+observer+")");
        if(observer == null)
            return;
        
        if(!_observerList.Contains(observer))
            _observerList.Add(observer);
    }

	/*[Client]*/
	/*[Client]*/
	public void RemoveObserver(IObserver observer) 
    {
		Debug.Log ("EventManager.removeObserver("+observer+")");
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
