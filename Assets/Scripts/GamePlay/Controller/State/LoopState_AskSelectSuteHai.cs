using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LoopState_AskSelectSuteHai : GameStateBase 
{
    public override void Exit()
    {
        base.Exit();

        logicOwner.onResponse_Select_SuteHai_Handler = null;
    }

    public override void Enter() {
        base.Enter();

        logicOwner.onResponse_Select_SuteHai_Handler = OnHandle_ResponseSelectSuteHai;

        //logicOwner.Ask_Select_SuteHai();
        //waitingOperation = StartCoroutine( AskSelectSuteHai() );
    }

    /*
    IEnumerator AskSelectSuteHai()
    {
        // wait for naki animation time.
        yield return new WaitForSeconds( MahjongView.NakiAnimationTime + 0.1f );

        OnSuteHaiAnimEnd();
    }
    */

    public override void OnHandleEvent(UIEventType evtID, object[] args)
    {
        if( evtID == UIEventType.On_UIAnim_End )
            OnSuteHaiAnimEnd();
    }

    void OnSuteHaiAnimEnd()
    {
        StopWaitingOperation();

        logicOwner.Ask_Select_SuteHai();
    }

    void OnHandle_ResponseSelectSuteHai()
    {
        logicOwner.Handle_SelectSuteHai();

        EventManager.Get().SendEvent(UIEventType.SuteHai, logicOwner.ActivePlayer, logicOwner.SuteHaiIndex, logicOwner.SuteHai, logicOwner.isTedashi);

        owner.ChangeState<LoopState_AskHandleSuteHai>();
    }
}
