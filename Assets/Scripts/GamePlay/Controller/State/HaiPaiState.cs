using UnityEngine;
using System.Collections;


public class HaiPaiState : GameStateBase 
{

    public override void Enter() {
        base.Enter();

        DisplayKyokuInfo();

        //waitingOperation = StartCoroutine(PrepareYamaUI());
    }

    void DisplayKyokuInfo()
    {
        string kyokuStr = "";
        string honbaStr = "";

        if( logicOwner.IsLastKyoku() )
        {
            kyokuStr = ResManager.getString("info_end");

            if( logicOwner.HonBa == 0 )
                owner.Speak(ECvType.ORaSu);
        }
        else
        {
            if( logicOwner.getBaKaze() == EKaze.Nan ){
                if( logicOwner.Kyoku == 1 && logicOwner.HonBa == 0 )
                    owner.Speak(ECvType.NanBa_Start);
            }
            else if(logicOwner.getBaKaze() == EKaze.Ton){
                if( logicOwner.Kyoku == (int)EKyoku.Ton_1 && logicOwner.HonBa == 0 )
                    owner.Speak(ECvType.Kyoku_Start);
            }
			string str = "kyoku_" +logicOwner.getBaKaze().ToString().ToLower()+'_'+logicOwner.Kyoku.ToString() ;
			//Debug.Log(str);
			kyokuStr = ResManager.getString (str);
			//Debug.Log(kyokuStr);
			//string kazeStr = ResManager.getString( "kaze_" + logicOwner.getBaKaze().ToString().ToLower() );
            //kyokuStr = kazeStr + logicOwner.Kyoku.ToString() + ResManager.getString("kyoku");

            if( logicOwner.HonBa > 0 )
                honbaStr = logicOwner.HonBa.ToString() + ResManager.getString("honba");
        }
        Debug.Log( kyokuStr + " " + honbaStr + " " + ResManager.getString("start") + "!" );

        EventManager.Get().SendEvent(UIEventType.DisplayKyokuInfo, kyokuStr, honbaStr);
    }

    /*
    IEnumerator PrepareYamaUI() 
    {
        yield return new WaitForSeconds(MahjongView.KyokuInfoAnimationTime);

        OnDisplayKyokuInfoEnd();
    }
    */

    void OnDisplayKyokuInfoEnd()
    {
        StopWaitingOperation();

        EventManager.Get().SendEvent(UIEventType.Select_Wareme);
    }


    public override void OnHandleEvent(UIEventType evtID, object[] args)
    {
        switch(evtID)
        {
            case UIEventType.On_UIAnim_End:
            {
                OnDisplayKyokuInfoEnd();
            }
            break;

            case UIEventType.On_Select_Wareme_End:
            {
                OnSelectWaremeEnd();
            }
            break;
        }
    }

    void OnSelectWaremeEnd()
    {
        logicOwner.SetWaremeAndHaipai();

        EventManager.Get().SendEvent(UIEventType.SetUI_AfterHaipai);

        StartCoroutine(StartLoop());
    }

    IEnumerator StartLoop()
    {
        yield return new WaitForSeconds( MahjongView.NormalWaitTime );

        logicOwner.PrepareToStart();

        owner.ChangeState<LoopState_PickHai>();
    }
}
