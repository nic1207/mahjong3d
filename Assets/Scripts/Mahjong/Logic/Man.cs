using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家
/// </summary>
public class Man : Player 
{
    //public Man(string name) : base(name){
    //    
    //}
    //public Man(string name, EVoiceType voiceType) : base(name, voiceType){
	//
    //}

    public override bool IsAI
    {
        get{ return false; }
    }


    protected override EResponse OnHandle_TsumoHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        _action.Reset();
        _action.State = EActionState.Select_Sutehai;

        if(inTest){
            //_action.State = EActionState.Select_Sutehai;
            //return DisplayMenuList();
        }

        // 手牌をコピーする。
        Hai tsumoHai = haiToHandle;

        // check enable Tsumo
        int agariScore = MahjongAgent.getAgariScore(Tehai, tsumoHai, JiKaze);
        if( agariScore > 0 )
        {
            if( GameSettings.AllowFuriten || !isFuriten() )
            {
                _action.IsValidTsumo = true;
                _action.MenuList.Add( EActionType.Tsumo );

                if( MahjongAgent.isReach(JiKaze) )
                    return DisplayMenuList();
            }
            else{
                Utils.LogWarningFormat( "Player {0} is enable tsumo but furiten...", JiKaze.ToString() );
            }
        }

        // 九种九牌check
        //if( MahjongAgent.CheckHaiTypeOver9(Tehai, tsumoHai) ){
        //    _action.MenuList.Add( EActionType.RyuuKyoku );
        //}

        // check enable Reach
        if( CheckReachPreConditions() == true ) 
        {//聽牌
            List<int> reachHaiIndexList;
            if( MahjongAgent.tryGetReachHaiIndex(Tehai, tsumoHai, out reachHaiIndexList) )
            {
                _action.IsValidReach = true;
                _action.ReachHaiIndexList = reachHaiIndexList;
                _action.MenuList.Add( EActionType.Reach );
            }
        }

		// 制限事項。リーチ後のカンをさせない 立直後不能槓
        if( !MahjongAgent.isReach(JiKaze) ) 
        {
            if( MahjongAgent.getTotalKanCount() < GameSettings.KanCountMax )
            {
                // tsumo kans
                List<Hai> kanHais = new List<Hai>();
                if( Tehai.validAnyTsumoKan(tsumoHai, kanHais) )
                {
                    _action.setValidTsumoKan(true, kanHais);

                    _action.MenuList.Add( EActionType.Kan );
                }
            }
        }
        else
        {
            if( MahjongAgent.getTotalKanCount() < GameSettings.KanCountMax )
            {
                // if player machi hais won't change after setting AnKan, enable to to it.
                if( Tehai.validAnKan(tsumoHai) )
                {
                    List<Hai> machiHais;
                    if( MahjongAgent.tryGetMachiHais(Tehai, out machiHais) )
                    {
                        Tehai tehaiCopy = new Tehai( Tehai );
                        tehaiCopy.setAnKan( tsumoHai );
                        tehaiCopy.Sort();

                        List<Hai> newMachiHais;

                        if( MahjongAgent.tryGetMachiHais(tehaiCopy, out newMachiHais) )
                        {
                            if( machiHais.Count == newMachiHais.Count ){
                                machiHais.Sort( Tehai.Compare );
                                newMachiHais.Sort( Tehai.Compare );

                                bool enableAnkan = true;

                                for( int i = 0; i < machiHais.Count; i++ )
                                {
                                    if( machiHais[i].ID != newMachiHais[i].ID ){
                                        enableAnkan = false;
                                        break;
                                    }
                                }

                                if( enableAnkan == true )
                                {
                                    _action.setValidTsumoKan(true, new List<Hai>(){ tsumoHai });

                                    _action.MenuList.Add( EActionType.Kan );
                                    _action.MenuList.Add( EActionType.Nagashi );

                                    _action.State = EActionState.Select_Kan;
                                }
                            }
                        }
                    }
                } // end if(valid ankan)
            } // end kan count check
            
            // can Ron or Ankan, sute hai automatically.
            if( _action.MenuList.Count == 0) {
                _action.SutehaiIndex = Tehai.getJyunTehaiCount(); // sute the tsumo hai on Reach

                return DoResponse(EResponse.SuteHai);
            }
        }
		//thinkSutehai( tsumoHai );

        // always display menu on pick tsumo hai.
        return DisplayMenuList();
		//return DoResponse(EResponse.SuteHai);
    }

    protected override EResponse OnHandle_KakanHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        _action.Reset();

        if(inTest){
            //return DoResponse(EResponse.Nagashi);
        }

        Hai kanHai = haiToHandle;

        int agariScore = MahjongAgent.getAgariScore(Tehai, kanHai, JiKaze);
        if( agariScore > 0 ) 
        {
            if( GameSettings.AllowFuriten || !isFuriten() )
            {
                _action.IsValidRon = true;
                _action.MenuList.Add( EActionType.Ron );

                if( MahjongAgent.isReach(JiKaze) ){
                    return DisplayMenuList();
                }
                else{
                    _action.MenuList.Add( EActionType.Nagashi );
                    return DisplayMenuList();
                }
            }
            else{
                Utils.LogWarningFormat( "Player {0} is enable ron but furiten...", JiKaze.ToString() );
            }
        }

        return DoResponse(EResponse.Nagashi);
    }

    protected override EResponse OnHandle_SuteHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
		string pp = ResManager.getMagjongName (haiToHandle.Kind, haiToHandle.Num);
		//Debug.Log ("OnHandle_SuteHai("+fromPlayerKaze.ToString()+" "+pp+")");
        _action.Reset();

        //if(inTest){
            //_action.MenuList.Add(EActionType.Nagashi);
            //return DisplayMenuList();
        //}

        Hai suteHai = haiToHandle;

        // check Ron
        int agariScore = MahjongAgent.getAgariScore(Tehai, suteHai, JiKaze);
		//Debug.Log ("agariScore=" + agariScore);
        if( agariScore > 0 ) // Ron
        {
            if( GameSettings.AllowFuriten || !isFuriten() )
            {
                _action.IsValidRon = true;
                _action.MenuList.Add( EActionType.Ron );

                if( MahjongAgent.isReach(JiKaze) ){
					//_action.MenuList.Add( EActionType.Ron );
                    //return DisplayMenuList();
					return DoResponse(EResponse.Ron_Agari);
                }
                else{
					_action.MenuList.Add( EActionType.Ron );
                }
            }
            else{
                Utils.LogWarningFormat( "Player {0} is enable to ron but furiten...", JiKaze.ToString() );
            }
        }

        if( MahjongAgent.getTsumoRemain() <= 0 )
            return DoResponse(EResponse.Nagashi);

        if( MahjongAgent.isReach(JiKaze) )
            return DoResponse(EResponse.Nagashi);
        

        // check menu Kan
        if( MahjongAgent.getTotalKanCount() < GameSettings.KanCountMax )
        {
            if( Tehai.validDaiMinKan(suteHai) ) {
                _action.IsValidDaiMinKan = true;
                _action.MenuList.Add( EActionType.Kan );
				//return DoResponse(EResponse.DaiMinKan);
            }
        }

        // check menu Pon
        if( Tehai.validPon(suteHai) ){
            _action.IsValidPon = true;
            _action.MenuList.Add( EActionType.Pon );
			//return DoResponse(EResponse.Pon);
        }

        // check menu Chii
        int relation = Mahjong.getRelation(fromPlayerKaze, JiKaze);
        if( relation == (int)ERelation.KaMiCha ) 
        {
            List<Hai> sarashiHaiRight = new List<Hai>();
            if( Tehai.validChiiRight(suteHai, sarashiHaiRight) )
            {
                _action.setValidChiiRight(true, sarashiHaiRight);

                if( !_action.MenuList.Contains(EActionType.Chii) )
                    _action.MenuList.Add(EActionType.Chii);
				//return DoResponse(EResponse.Chii_Right);
            }

            List<Hai> sarashiHaiCenter = new List<Hai>();
            if( Tehai.validChiiCenter(suteHai, sarashiHaiCenter) )
            {
                _action.setValidChiiCenter(true, sarashiHaiCenter);

                if( !_action.MenuList.Contains(EActionType.Chii) )
                    _action.MenuList.Add(EActionType.Chii);
				//return DoResponse(EResponse.Chii_Center);
            }

            List<Hai> sarashiHaiLeft = new List<Hai>();
            if( Tehai.validChiiLeft(suteHai, sarashiHaiLeft) )
            {
                _action.setValidChiiLeft(true, sarashiHaiLeft);

                if( !_action.MenuList.Contains(EActionType.Chii) )
                    _action.MenuList.Add(EActionType.Chii);
				//return DoResponse(EResponse.Chii_Left);
            }
        }

        if( _action.MenuList.Count > 0 ){
            _action.MenuList.Add( EActionType.Nagashi );
            return DisplayMenuList();
        }

		return DoResponse (EResponse.Nagashi);
    }


    protected override EResponse OnSelect_SuteHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
		
        _action.Reset();
        _action.State = EActionState.Select_Sutehai;

        return DisplayMenuList();
        
		//_action.Reset();

		//thinkSelectSuteHai();

		//return DoResponse(EResponse.SuteHai);
    }

	protected virtual void thinkSelectSuteHai()
	{
		thinkSutehai(null);
		_action.SutehaiIndex = Tehai.getJyunTehaiCount ();
		//if( _action.SutehaiIndex == Tehai.getJyunTehaiCount() )
		//{
		//	_action.SutehaiIndex = 
		//	_action.SutehaiIndex = Utils.GetRandomNum(0, Tehai.getJyunTehaiCount());
		//}
	}

	protected virtual void thinkSutehai(Hai addHai)
	{
		_action.SutehaiIndex = Tehai.getJyunTehaiCount();

		Tehai tehaiCopy = new Tehai( Tehai );

		FormatWorker.setCounterFormat(tehaiCopy, null);
		int maxScore = getCountFormatScore(FormatWorker);

		for( int i = 0; i < tehaiCopy.getJyunTehaiCount(); i++ )
		{
			Hai hai = tehaiCopy.removeJyunTehaiAt(i);

			FormatWorker.setCounterFormat(tehaiCopy, addHai);

			int score = getCountFormatScore( FormatWorker );
			if( score > maxScore )
			{
				maxScore = score;
				_action.SutehaiIndex = i;
			}

			tehaiCopy.insertJyunTehai(i, hai);
		}
	}

	protected readonly static int HYOUKA_SHUU = 1;
	protected virtual int getCountFormatScore(CountFormat countFormat)
	{
		int score = 0;
		HaiCounterInfo[] countArr = countFormat.getCounterArray();

		for( int i = 0; i < countArr.Length; i++ ) 
		{
			if( (countArr[i].numKind & Hai.KIND_SHUU) != 0)
				score += countArr[i].count * HYOUKA_SHUU;

			if( countArr[i].count == 2 )
				score += 4;

			if( countArr[i].count >= 3 )
				score += 8;

			if( (countArr[i].numKind & Hai.KIND_SHUU) > 0 )
			{
				if( (i + 1) < countArr.Length && (countArr[i].numKind + 1) == countArr[i + 1].numKind )
					score += 4;

				if( (i + 2) < countArr.Length && (countArr[i].numKind + 2) == countArr[i + 2].numKind )
					score += 4;
			}
		}

		return score;
	}


    protected EResponse DisplayMenuList()
    {
        MahjongAgent.PostUiEvent(UIEventType.DisplayMenuList);

        return EResponse.Nagashi;
    }

}