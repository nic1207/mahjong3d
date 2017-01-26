using UnityEngine;
using System.Collections;


public class GameStartState : GameStateBase 
{

    public override void Enter() {

        base.Enter();

        EventManager.Get().SendEvent( UIEventType.Init_Game );

        owner.ChangeState<GamePrepareState>();
    }

}
