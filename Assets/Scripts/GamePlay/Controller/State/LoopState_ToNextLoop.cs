using UnityEngine;
using System.Collections;


public class LoopState_ToNextLoop : GameStateBase 
{

    public override void Enter() {
        base.Enter();

        logicOwner.SetNextPlayer();

        owner.ChangeState<LoopState_PickHai>();
    }
}
