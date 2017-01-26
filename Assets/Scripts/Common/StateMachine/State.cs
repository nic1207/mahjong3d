using UnityEngine;
using System.Collections;


public abstract class State : MonoBehaviour 
{
    public virtual void Enter ()
    {
        AddListeners();
        //Debug.LogWarningFormat( "~State {0} Enter.", GetType().Name );
    }

    public virtual void Exit ()
    {
        RemoveListeners();
    }

	public virtual void Handle ()
	{
	}

    protected virtual void OnDestroy ()
    {
        RemoveListeners();
    }

    protected virtual void AddListeners ()
    {

    }

    protected virtual void RemoveListeners ()
    {

    }
}