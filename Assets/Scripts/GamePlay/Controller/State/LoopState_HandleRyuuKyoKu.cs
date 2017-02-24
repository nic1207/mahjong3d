using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LoopState_HandleRyuuKyoKu : GameStateBase 
{
    public override void Enter()
    {
        base.Enter();

		Debug.Log("## 流局 ##");

        if( logicOwner.RyuuKyokuReason == ERyuuKyokuReason.NoTsumoHai )
        {
			/*
            if( logicOwner.HandleNagashiMangan() )
            {
                logicOwner.AgariResult = EAgariType.NagashiMangan;

                EventManager.Get().SendEvent(UIEventType.Tsumo_Agari, logicOwner.ActivePlayer);

                owner.ChangeState<LoopState_Agari>();
            }
            else
            {
            */
            logicOwner.HandleRyuukyokuTenpai();
			EventManager.Instance.RpcSendEvent(UIEventType.RyuuKyoku, ERyuuKyokuReason.NoTsumoHai, logicOwner.AgariUpdateInfoList);
            //}
        }
        else
        {
			EventManager.Instance.RpcSendEvent(UIEventType.RyuuKyoku, logicOwner.RyuuKyokuReason, logicOwner.AgariUpdateInfoList);
        }
    }


    public override void OnHandleEvent(UIEventType evtID, object[] args)
    {
        if( evtID == UIEventType.End_RyuuKyoku )
        {
            owner.ChangeState<KyoKuOverState>();
        }
    }

}
