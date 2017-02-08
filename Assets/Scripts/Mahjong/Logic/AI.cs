using System;
using System.Collections.Generic;


public class AI : Player 
{
    /*
	public AI(string name) : base(name){

    }
    public AI(string name, EVoiceType voiceType) : base(name, voiceType){

    }
    */

    public override bool IsAI
    {
        get{ return true; }
    }


    protected override EResponse OnHandle_TsumoHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        _action.Reset();
		//_action.SutehaiIndex =  UnityEngine.Random.Range(0, ;
		_action.SutehaiIndex = UnityEngine.Random.Range(0, Tehai.getJyunTehaiCount ()-1);
        //if(inTest){
            //_action.SutehaiIndex = Tehai.getJyunTehaiCount();
            //return DoResponse(EResponse.SuteHai);
        //}

        Hai tsumoHai = haiToHandle;
		/*
        // ツモあがりの場合は、イベント(ツモあがり)を返す。
        int agariScore = MahjongAgent.getAgariScore(Tehai, tsumoHai, JiKaze);
        if( agariScore > 0 )
        {
            if( GameSettings.AllowFuriten || !isFuriten() )
            {
                return DoResponse(EResponse.Tsumo_Agari);
            }
            else{
                Utils.LogWarningFormat( "AI {0} is enable tsumo but furiten...", JiKaze.ToString() );
            }
        }
		*/
		/*
        // 九种九牌check
        if( MahjongAgent.CheckHaiTypeOver9(Tehai, tsumoHai) ){
            return DoResponse(EResponse.Nagashi); 
        }

        // リーチの場合は、ツモ切りする
        if( MahjongAgent.isReach(JiKaze) )
        {
            _action.SutehaiIndex = Tehai.getJyunTehaiCount();

            return DoResponse(EResponse.SuteHai);
        }

        // check enable Reach
        if( CheckReachPreConditions() == true ) 
        {
            List<int> reachHaiIndexList;
            if( MahjongAgent.tryGetReachHaiIndex(Tehai, tsumoHai, out reachHaiIndexList) )
            {
                _action.IsValidReach = true;
                _action.ReachHaiIndexList = reachHaiIndexList;

                thinkReach();

                return DoResponse(EResponse.Reach);
            }
        }
		*/

        // 制限事項。リーチ後のカンをさせない
        if( !MahjongAgent.isReach(JiKaze) ) 
        {
            /*
            if( MahjongAgent.getTotalKanCount() < GameSettings.KanCountMax )
            {                
                // TODO: tsumo kans
                List<Hai> kanHais = new List<Hai>();
                if( Tehai.validAnyTsumoKan(tsumoHai, kanHais) )
                {
                    _action.setValidTsumoKan(true, kanHais);

                }
            }
            */
        }
        else
        {
			/*
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
                                    return DoResponse(EResponse.Ankan);
                                }
                            }
                        }
                    }
                }
            }
            */

            // can Ron or Ankan, sute hai automatically.
            //_action.SutehaiIndex = Tehai.getJyunTehaiCount(); // sute the tsumo hai on Reach

            //return DoResponse(EResponse.SuteHai);
        }


        //thinkSutehai( tsumoHai );

        return DoResponse(EResponse.SuteHai);
    }

    protected override EResponse OnHandle_KakanHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        _action.Reset();

        if(inTest){
            return DoResponse(EResponse.Nagashi);
        }

        Hai kanHai = haiToHandle;

        int agariScore = MahjongAgent.getAgariScore(Tehai, kanHai, JiKaze);
        if( agariScore > 0 )
        {
            if( GameSettings.AllowFuriten || !isFuriten() )
            {
                return DoResponse(EResponse.Ron_Agari);
            }
            else{
                Utils.LogWarningFormat( "AI {0} is enable ron but furiten...", JiKaze.ToString() );
            }
        }

        return DoResponse(EResponse.Nagashi);
    }

    protected override EResponse OnHandle_SuteHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        _action.Reset();
		_action.SutehaiIndex = 0;
        Hai suteHai = haiToHandle;

        // check Ron
        int agariScore = MahjongAgent.getAgariScore(Tehai, suteHai, JiKaze);
        if( agariScore > 0 ) // Ron
        {
            if( GameSettings.AllowFuriten || !isFuriten() )
            {
                return DoResponse(EResponse.Ron_Agari);
            }
            else{
                Utils.LogWarningFormat( "AI {0} is enable to ron but furiten...", JiKaze.ToString() );
            }
        }

        if( MahjongAgent.getTsumoRemain() <= 0 )
            return DoResponse(EResponse.Nagashi);

        if( MahjongAgent.isReach(JiKaze) )
            return DoResponse(EResponse.Nagashi);

        // TODO: Chii, Pon, Kan check

        // check Kan(test)
        /*
        if( MahjongAgent.getTotalKanCount() < GameSettings.KanCountMax )
        {
            if( Tehai.validDaiMinKan(suteHai) ) {
                _action.IsValidDaiMinKan = true;
                return DoResponse(EResponse.DaiMinKan);
            }
        }
        */
		//_action.Reset();

		//thinkSelectSuteHai();

		return DoResponse(EResponse.Nagashi);
        //return DoResponse(EResponse.Nagashi);
    }


    protected override EResponse OnSelect_SuteHai(EKaze fromPlayerKaze, Hai haiToHandle)
    {
        _action.Reset();

        //thinkSelectSuteHai();
		_action.SutehaiIndex = 0;
        return DoResponse(EResponse.SuteHai);
    }



    protected virtual void thinkSutehai(Hai addHai)
    {
        //_action.SutehaiIndex = Tehai.getJyunTehaiCount();
		//_action.SutehaiIndex = UnityEngine.Random.Range(0, Tehai.getJyunTehaiCount ()-1);
		_action.SutehaiIndex = 0;
        /*
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
        */
    }


    protected virtual void thinkSelectSuteHai()
    {
        thinkSutehai(null);

        if( _action.SutehaiIndex == Tehai.getJyunTehaiCount() )
        {
            //_action.SutehaiIndex = Utils.GetRandomNum(0, Tehai.getJyunTehaiCount());
			_action.SutehaiIndex = 0;
        }
    }

    protected virtual void thinkReach()
    {
        _action.ReachSelectIndex = 0;

        List<int> reachHaiIndex = _action.ReachHaiIndexList;

        Tehai tehaiCopy = new Tehai( Tehai );
        int minScore = int.MaxValue;

        for( int i = 0; i < reachHaiIndex.Count; i++ )
        {
            Hai hai = tehaiCopy.removeJyunTehaiAt( reachHaiIndex[i] ); // reachHaiIndex[i], not i

            List<Hai> machiiHais;
            if( MahjongAgent.tryGetMachiHais(tehaiCopy, out machiiHais) )
            {
                for( int m = 0; m < machiiHais.Count; m++ )
                {
                    Hai addHai = machiiHais[m];

                    FormatWorker.setCounterFormat(tehaiCopy, addHai);

                    int score = MahjongAgent.getAgariScore( tehaiCopy, addHai, JiKaze );
                    if( score <= minScore )
                    {
                        minScore = score;
                        _action.ReachSelectIndex = i;
                    }
                }
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

}
