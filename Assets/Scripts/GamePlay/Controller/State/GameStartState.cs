using UnityEngine;
using System.Collections;


public class GameStartState : GameStateBase 
{

    public override void Enter() {

        base.Enter();

		EventManager.Instance.RpcSendEvent( UIEventType.Init_Game );

        owner.ChangeState<GamePrepareState>();
    }

}
