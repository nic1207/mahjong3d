using UnityEngine;
using System.Collections;


/// <summary>
/// Sai sai furi for deciding Qin jia.
/// </summary>

public class GamePrepareState : GameStateBase 
{

    public override void Enter() {
        base.Enter();

		if (logicOwner.needSelectChiiCha ()) {
			EventManager.Instance.RpcSendEvent (UIEventType.Select_ChiiCha);
		} else {
			OnSaifuriForOyaEnd ();
		}
    }


    public override void OnHandleEvent(UIEventType evtID, object[] args)
    {
        switch(evtID)
        {
            case UIEventType.On_Select_ChiiCha_End:
            {
                int index = (int)args[0];

                logicOwner.SetOyaChiicha(index);

                OnSaifuriForOyaEnd();
            }
            break;
        }
    }

    void OnSaifuriForOyaEnd()
    {
        logicOwner.PrepareKyoku();

		EventManager.Instance.RpcSendEvent(UIEventType.Init_PlayerInfoUI);
		EventManager.Instance.RpcSendEvent(UIEventType.SetYama_BeforeHaipai);

        owner.ChangeState<HaiPaiState>();
    }

}
