using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EventManager 
{
    private List<IObserver> _observerList = null;

    private EventManager() 
    {
        _observerList = new List<IObserver>();
    }

    private static EventManager instance = null;
    public static EventManager Get()
    {
        if(instance == null)
            instance = new EventManager();
        return instance;
    }


    public static void CleanUp()
    {
        instance._observerList.Clear();
        instance = null;
    }


    public void addObserver(IObserver observer) 
    {
		Debug.Log ("addObserver("+observer+")");
        if(observer == null)
            return;
        
        if(!_observerList.Contains(observer))
            _observerList.Add(observer);
    }
    public void removeObserver(IObserver observer) 
    {
        if(observer == null)
            return;
        
        if(_observerList.Contains(observer))
            _observerList.Remove(observer);
    }


    // send ui event.
    public void SendEvent(UIEventType eventType, params object[] args) 
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
