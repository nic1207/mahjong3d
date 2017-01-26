using UnityEngine;
using System.Collections;


public class LoopState_AskHandleTsumoHai : GameStateBase 
{
    public override void Exit()
    {
        base.Exit();

        logicOwner.onResponse_TsumoHai_Handler = null;
    }

    public override void Enter() {
        base.Enter();

        logicOwner.onResponse_TsumoHai_Handler = OnHandle_ResponseTsumoHai;

        logicOwner.Ask_Handle_TsumoHai();
    }

	public override void Handle() {
		base.Handle();
		//Player activePlayer = logicOwner.ActivePlayer;
		//logicOwner.ActivePlayer.Action.SutehaiIndex = 0;
		logicOwner.ActivePlayer.Action.Response = EResponse.SuteHai;
		logicOwner.Handle_SuteHai();

		EventManager.Get().SendEvent(UIEventType.HideMenuList);
		logicOwner.ActivePlayer.OnPlayerInputFinished();

		owner.ChangeState<LoopState_ToNextLoop>();
		//OnHandle_ResponseTsumoHai ();
	}


    void OnHandle_ResponseTsumoHai()
    {
        Player activePlayer = logicOwner.ActivePlayer;
		//Debug.Log ("OnHandle_ResponseTsumoHai(activePlayer.Action.Response="+activePlayer.Action.Response+")");
        switch( activePlayer.Action.Response )
        {
            case EResponse.Tsumo_Agari:
            {
                logicOwner.Handle_TsumoAgari();
                logicOwner.AgariResult = EAgariType.Tsumo;

                EventManager.Get().SendEvent(UIEventType.Tsumo_Agari, activePlayer);

                owner.ChangeState<LoopState_Agari>();
            }
            break;
            case EResponse.Ankan:
            {
                logicOwner.Handle_AnKan();

                EventManager.Get().SendEvent(UIEventType.Ankan, activePlayer);

                if( logicOwner.checkKanCountOverFlow() ){
                    logicOwner.Handle_Invalid_RyuuKyoku(); // must.

                    logicOwner.RyuuKyokuReason = ERyuuKyokuReason.KanOver4;
                    owner.ChangeState<LoopState_HandleRyuuKyoKu>();
                }
                else{
                    owner.ChangeState<LoopState_PickRinshanHai>();
                }
            }
            break;
            case EResponse.Kakan:
            {
                logicOwner.Handle_KaKan();

                EventManager.Get().SendEvent(UIEventType.Kakan, activePlayer, logicOwner.KakanHai);

                if( logicOwner.checkKanCountOverFlow() ){
                    logicOwner.Handle_Invalid_RyuuKyoku(); // must.

                    logicOwner.RyuuKyokuReason = ERyuuKyokuReason.KanOver4;
                    owner.ChangeState<LoopState_HandleRyuuKyoKu>();
                }
                else{
                    owner.ChangeState<LoopState_AskHandleKakanHai>();
                }
            }
            break;
            case EResponse.Reach:
            {
                logicOwner.Handle_Reach();

                EventManager.Get().SendEvent(UIEventType.Reach, activePlayer, logicOwner.SuteHaiIndex, logicOwner.SuteHai, logicOwner.isTedashi);

                // 4 fon
                if( logicOwner.checkSuteFonHai4() && !GameSettings.AllowSuteFonHai4 ){
                    logicOwner.Handle_Invalid_RyuuKyoku(); // must.

                    logicOwner.RyuuKyokuReason = ERyuuKyokuReason.SuteFonHai4;
                    owner.ChangeState<LoopState_HandleRyuuKyoKu>();
                    return;
                }

                // need to check after sute reach hai and no one Ron.
                else if( logicOwner.checkReach4() && !GameSettings.AllowReach4 ){
                    logicOwner.Reach4Flag = true;
                }

                owner.ChangeState<LoopState_AskHandleSuteHai>();
            }
            break;
            case EResponse.SuteHai:
            {
                logicOwner.Handle_SuteHai();

                EventManager.Get().SendEvent(UIEventType.SuteHai, activePlayer, logicOwner.SuteHaiIndex, logicOwner.SuteHai, logicOwner.isTedashi);

                if( logicOwner.checkSuteFonHai4() && !GameSettings.AllowSuteFonHai4 ){
                    logicOwner.Handle_Invalid_RyuuKyoku(); // must.

                    logicOwner.RyuuKyokuReason = ERyuuKyokuReason.SuteFonHai4;
                    owner.ChangeState<LoopState_HandleRyuuKyoKu>();
                }
                else{
                    owner.ChangeState<LoopState_AskHandleSuteHai>();
                }
            }
            break;
            // This is only enable when any players select ERyuuKyokuReason.HaiTypeOver9
            case EResponse.Nagashi: 
            {
                if( logicOwner.checkHaiTypeOver9() ){
                    logicOwner.Handle_Invalid_RyuuKyoku(); // must.

                    logicOwner.RyuuKyokuReason = ERyuuKyokuReason.HaiTypeOver9;
                    owner.ChangeState<LoopState_HandleRyuuKyoKu>();
                } else {
                    //throw new MahjongException("Invalid response on ERyuuKyokuReason.HaiTypeOver9 not established");
                }
            }
            break;
        }
    }
}
