using UnityEngine;
using System.Collections;


public class GameStateBase : State 
{
    public MahjongMain logicOwner;
    protected GameManager owner;

    protected Coroutine waitingOperation;


    protected virtual void Awake()
    {
        owner = GetComponent<GameManager>();
        logicOwner = owner.LogicMain;
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
