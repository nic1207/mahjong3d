using UnityEngine;
using System.Collections;


/// <summary>
/// Sai sai furi for deciding Qin jia.
/// </summary>

public class GamePrepareState : GameStateBase 
{

    public override void Enter() {
        base.Enter();

        if( logicOwner.needSelectChiiCha() )
            EventManager.Get().SendEvent(UIEventType.Select_ChiiCha);
        else
            OnSaifuriForOyaEnd();
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

        EventManager.Get().SendEvent(UIEventType.Init_PlayerInfoUI);
        EventManager.Get().SendEvent(UIEventType.SetYama_BeforeHaipai);

        owner.ChangeState<HaiPaiState>();
    }

}
