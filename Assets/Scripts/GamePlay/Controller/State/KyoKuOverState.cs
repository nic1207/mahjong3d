using UnityEngine;
using System.Collections;


public class KyoKuOverState : GameStateBase 
{
    public override void Enter() {
        base.Enter();

        bool result = true;

        if( logicOwner.RyuuKyokuReason != ERyuuKyokuReason.None ){
            result = logicOwner.EndRyuuKyoku();
        }
        else  // Ron,Tsumo, or Nagashimangan.
        {  
            result = logicOwner.EndKyoku();
        }

        if( result == true )
        {
            owner.ChangeState<GameStartState>();
        }
        else
        {
            owner.ChangeState<GameOverState>();
        }
    }
}
