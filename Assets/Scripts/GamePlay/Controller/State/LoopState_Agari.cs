using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LoopState_Agari : GameStateBase
{
    public override void Exit()
    {
        base.Exit();
    }

    public override void Enter()
    {
        base.Enter();

        // TODO: send agari event here instead of before this state entering
        /*
        if( logicOwner.AgariResult == EAgariType.Tsumo )
        {
            EventManager.Get().SendEvent(UIEventType.Tsumo_Agari, logicOwner.ActivePlayer);
        }
        else if( logicOwner.AgariResult == EAgariType.NagashiMangan )
        {
            EventManager.Get().SendEvent(UIEventType.Tsumo_Agari, logicOwner.ActivePlayer);
        }
        else if( logicOwner.AgariResult == EAgariType.Ron )
        {
            EventManager.Get().SendEvent(UIEventType.Ron_Agari, logicOwner.GetRonPlayers(), logicOwner.FromKaze, logicOwner.SuteHai);
        }
        else if( logicOwner.AgariResult == EAgariType.ChanKanRon )
        {
            EventManager.Get().SendEvent(UIEventType.Ron_Agari, logicOwner.GetRonPlayers(), logicOwner.FromKaze, logicOwner.KakanHai);
        }
        */

        //waitingOperation = StartCoroutine(HandleAgariRon());
    }

    /*
    IEnumerator HandleAgariRon() {
        yield return new WaitForSeconds( MahjongView.AgariAnimationTime );

        OnAgariAnimEnd();
    }
    */

    void OnAgariAnimEnd()
    {
        StopWaitingOperation();

		EventManager.Instance.RpcSendEvent(UIEventType.Display_Agari_Panel, logicOwner.AgariUpdateInfoList);
    }


    public override void OnHandleEvent(UIEventType evtID, object[] args)
    {
		Debug.Log ("LoopState_Agari.OnHandleEvent("+evtID+")");
        if( evtID == UIEventType.On_UIAnim_End )
        {
            OnAgariAnimEnd();
        }
        else if( evtID == UIEventType.End_Kyoku )
        {
            owner.ChangeState<KyoKuOverState>();
        }
    }
}
