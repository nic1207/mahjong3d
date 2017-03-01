using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameStateBase : State 
{
    public MahjongMain logicOwner;
	//public StateMachine xxx;
	public GameManager owner;

    protected Coroutine waitingOperation;


    protected virtual void Awake()
    {
		owner = GameManager.Instance;
		logicOwner = owner.GetLogicOwner();
    }

    public virtual void OnHandleEvent(UIEventType evtID, object[] args) 
    {
        
    }

    public override void Exit()
    {
        base.Exit();

        StopWaitingOperation();
    }

    public void StopWaitingOperation()
    {
        if( waitingOperation != null ){
            StopCoroutine(waitingOperation);
            waitingOperation = null;
        }
    }
}
