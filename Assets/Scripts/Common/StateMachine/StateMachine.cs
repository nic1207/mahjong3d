using UnityEngine;
using System.Collections;


public class StateMachine : MonoBehaviour 
{
	public virtual State CurrentState
	{
		get { return _currentState; }
		set { Transition (value); }
	}

	protected State _currentState;
	protected bool _inTransition;

	public virtual T GetState<T> () where T : State
	{
		T target = GetComponent<T>();
		if (target == null)
			target = gameObject.AddComponent<T>();
		return target;
	}
	
	public virtual void ChangeState<T> () where T : State
	{
		CurrentState = GetState<T>();
	}

	protected virtual void Transition (State value)
	{
        if (_currentState == value || _inTransition)
        {
            if(_inTransition) Debug.LogWarning("~StateMachine is in transition: " + _currentState.GetType().Name);
			return;
        }

		_inTransition = true;
		
		if (_currentState != null)
			_currentState.Exit();
		
		_currentState = value;
		
        _inTransition = false;

		if (_currentState != null)
			_currentState.Enter();
	}
}