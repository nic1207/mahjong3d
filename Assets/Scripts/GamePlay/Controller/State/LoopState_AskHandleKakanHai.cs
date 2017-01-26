using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LoopState_AskHandleKakanHai : GameStateBase
{
    public override void Exit()
    {
        base.Exit();

        logicOwner.onResponse_KakanHai_Handler = null;
    }

    public override void Enter() {
        base.Enter();

        logicOwner.onResponse_KakanHai_Handler = OnHandle_ResponseKakanHai;

        //waitingOperation = StartCoroutine(AskHandleKakanHai());
    }
    /*
    IEnumerator AskHandleKakanHai()
    {
        yield return new WaitForSeconds( MahjongView.NakiAnimationTime + 0.1f );

        OnKakanAnimEnd();
    }
    */

    public override void OnHandleEvent(UIEventType evtID, object[] args)
    {
        if( evtID == UIEventType.On_UIAnim_End )
            OnKakanAnimEnd();
    }

    void OnKakanAnimEnd()
    {
        StopWaitingOperation();

        logicOwner.Ask_Handle_KaKanHai();
    }


    void OnHandle_ResponseKakanHai()
    {
        List<EKaze> ronPlayers = logicOwner.GetRonPlayers();
        if( ronPlayers.Count > 0 )
        {
            if( ronPlayers.Count >= 3 && !GameSettings.AllowRon3 ){
                logicOwner.Handle_Invalid_RyuuKyoku(); // must.

                logicOwner.RyuuKyokuReason = ERyuuKyokuReason.Ron3;
                owner.ChangeState<LoopState_HandleRyuuKyoKu>();
            }
            else{
                logicOwner.Handle_KaKan_Ron();
                logicOwner.AgariResult = EAgariType.ChanKanRon;

                // show ron ui.
                EventManager.Get().SendEvent(UIEventType.Ron_Agari, ronPlayers, logicOwner.FromKaze, logicOwner.KakanHai);

                owner.ChangeState<LoopState_Agari>();
            }
        }
        else
        {
            logicOwner.Handle_KaKan_Nagashi();

            owner.ChangeState<LoopState_PickRinshanHai>();
        }
    }
}
