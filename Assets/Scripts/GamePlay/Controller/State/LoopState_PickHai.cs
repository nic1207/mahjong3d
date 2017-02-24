using UnityEngine;
using System.Collections;


public class LoopState_PickHai : GameStateBase 
{

    public override void Enter() {
        base.Enter();

        logicOwner.PickNewTsumoHai();

        if( logicOwner.checkNoTsumoHai() )
        {
            logicOwner.RyuuKyokuReason = ERyuuKyokuReason.NoTsumoHai;
            owner.ChangeState<LoopState_HandleRyuuKyoKu>();
        }
        else {
            Hai tsumoHai = logicOwner.TsumoHai;

            int lastPickIndex = logicOwner.Yama.getPreTsumoHaiIndex();
            if( lastPickIndex < 0 ){
                Debug.LogError("Error!!!");
                return;
            }

			EventManager.Instance.RpcSendEvent(UIEventType.PickTsumoHai, logicOwner.ActivePlayer, lastPickIndex, tsumoHai );

            owner.ChangeState<LoopState_AskHandleTsumoHai>();
        }
    }

}
