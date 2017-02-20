using System.Collections.Generic;
using Math = UnityEngine.Mathf;
using UnityEngine;


/// <summary>
/// Agari(あがり) = 胡牌.
/// </summary>

public sealed class AgariScoreManager 
{
    /*
    // fields.
    static ScoreInfo[,] SCORE_LIST = new ScoreInfo[13,13]
    {
        {new ScoreInfo(    0,    0,    0,    0),new ScoreInfo(    0,    0,    0,    0),new ScoreInfo( 1500,  500, 1000,  300),new ScoreInfo( 2000,  700, 1300,  400),new ScoreInfo( 2400,  800, 1600,  400),new ScoreInfo( 2900, 1000, 2000,  500),new ScoreInfo( 3400, 1200, 2300,  600),new ScoreInfo( 3900, 1300, 2600,  700),new ScoreInfo( 4400, 1500, 2900,  800),new ScoreInfo( 4800, 1600, 3200,  800),new ScoreInfo( 5300,    0, 3600,    0),new ScoreInfo( 5800,    0, 3900,    0),new ScoreInfo( 6300,    0, 2100,    0)},
        {new ScoreInfo( 2000,  700, 1300,  400),new ScoreInfo( 2400,    0, 1600,    0),new ScoreInfo( 2900, 1000, 2000,  500),new ScoreInfo( 3900, 1300, 2600,  700),new ScoreInfo( 4800, 1600, 3200,  800),new ScoreInfo( 5800, 2000, 3900, 1000),new ScoreInfo( 6800, 2300, 4500, 1200),new ScoreInfo( 7700, 2600, 5200, 1300),new ScoreInfo( 8700, 2900, 5800, 1500),new ScoreInfo( 9600, 3200, 6400, 1600),new ScoreInfo(10600, 3600, 7100, 1800),new ScoreInfo(11600, 3900, 7700, 2000),new ScoreInfo(12000, 4000, 8000, 2000)},
        {new ScoreInfo( 3900, 1300, 2600,  700),new ScoreInfo( 4800, 1600, 3200,  800),new ScoreInfo( 5800, 2000, 3900, 1000),new ScoreInfo( 7700, 2600, 5200, 1300),new ScoreInfo( 9600, 3200, 6400, 1600),new ScoreInfo(11600, 3900, 7700, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000)},
        {new ScoreInfo( 7700, 2600, 5200, 1300),new ScoreInfo( 9600, 3200, 6400, 1600),new ScoreInfo(11600, 3900, 7700, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000)},
        {new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000),new ScoreInfo(12000, 4000, 8000, 2000)},
        {new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000)},
        {new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000),new ScoreInfo(18000, 6000,12000, 3000)},
        {new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000)},
        {new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000)},
        {new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000),new ScoreInfo(24000, 8000,16000, 4000)},
        {new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000)},
        {new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000),new ScoreInfo(36000,12000,24000, 6000)},
        {new ScoreInfo(48000,16000,32000, 8000),new ScoreInfo(48000,16000,32000, 8000),new ScoreInfo(48000,16000,32000, 8000),new ScoreInfo(48000,16000,32000, 8000),new ScoreInfo(48000,16000,32000, 8000),new ScoreInfo(48000,16000,32000, 8000),new ScoreInfo(48000,16000,32000, 8000),new ScoreInfo(48000,16000,32000, 8000),new ScoreInfo(48000,16000,32000, 8000),new ScoreInfo(48000,16000,32000, 8000),new ScoreInfo(48000,16000,32000, 8000),new ScoreInfo(48000,16000,32000, 8000),new ScoreInfo(48000,16000,32000, 8000)}
    };
    */

    // 满贯.
    static Score SCORE_MAN_GAN = new Score(12000,4000, 8000,2000,4000);

    // 1-4 han: 20,25,30,40,50,60,70,80,90,100,110 fu.
    static Score[] SCORE_1 = new Score[11]
    {
        //20fu               //25fu               //30fu                            //40fu                             //50fu                             //60fu                               //70fu                              //80fu                              //90fu                              //100fu                            //110fu
        new Score(0,0,0,0,0),new Score(0,0,0,0,0),new Score(1500,500,1000,300,500), new Score(2000,700,1300,400,700),  new Score(2400,800,1600,400,800),  new Score(2900,1000,2000,500,1000),  new Score(3400,1200,2300,600,1200), new Score(3900,1300,2600,700,1300), new Score(4400,1500,2900,800,1500), new Score(4800,1600,3200,800,1600),new Score(5300,1800,3600,900,1800)
    };
    static Score[] SCORE_2 = new Score[11]
    {
        //20fu               //25fu               //30fu                            //40fu                             //50fu                             //60fu                               //70fu                              //80fu                              //90fu                              //100fu                             //110fu
        new Score(0,0,0,0,0),new Score(0,0,0,0,0),new Score(2900,1000,2000,500,1000),new Score(3900,1300,2600,700,1300),new Score(4800,1600,3200,800,1600),new Score(5800,2000,3900,1000,2000),new Score(3400,1200,4500,1200,2300),new Score(7700,2600,5200,1300,2600),new Score(8700,2900,5800,1500,2900),new Score(9600,3200,6400,1600,3200),new Score(10600,3600,7100,1800,3600)
    };
    static Score[] SCORE_3 = new Score[11]
    {
        //20fu                             //25fu                             //30fu                              //40fu                              //50fu                              //60fu                               // 70fu                  //80fu                   //90fu                   //100fu                  //110fu
        new Score(3900,1300,2600,700,1300),new Score(4800,1600,3200,800,1600),new Score(5800,2000,3900,1000,2000),new Score(7700,2600,5200,1300,2600),new Score(9600,3200,6400,1600,3200),new Score(11600,3900,7700,2000,3900),new Score(SCORE_MAN_GAN),new Score(SCORE_MAN_GAN),new Score(SCORE_MAN_GAN),new Score(SCORE_MAN_GAN),new Score(SCORE_MAN_GAN)
    };
    static Score[] SCORE_4 = new Score[11]
    {
        //20fu                              //25fu                              //30fu                               //40fu                   //50fu                   //60fu                   // 70fu                  //80fu                   //90fu                   //100fu                  //110fu
        new Score(7700,2600,5200,1300,2600),new Score(9600,3200,6400,1600,3200),new Score(11600,3900,7700,2000,3900),new Score(SCORE_MAN_GAN),new Score(SCORE_MAN_GAN),new Score(SCORE_MAN_GAN),new Score(SCORE_MAN_GAN),new Score(SCORE_MAN_GAN),new Score(SCORE_MAN_GAN),new Score(SCORE_MAN_GAN),new Score(SCORE_MAN_GAN)
    };

    static Score SCORE_5     = new Score(SCORE_MAN_GAN); //(12000,4000,  8000,2000,4000);
    static Score SCORE_6_7   = new Score(18000,6000,  12000,3000,6000);
    static Score SCORE_8_10  = new Score(24000,8000,  16000,4000,8000);
    static Score SCORE_11_12 = new Score(36000,12000, 24000,6000,12000);
    static Score SCORE_13    = new Score(48000,16000, 32000,8000,16000);


    private static CountFormat formatWorker = new CountFormat();


    // 上がり点数を取得します
    public static Score GetScoreInfo(int hanSuu, int huSuu)
    {
        int iFu;
        if(huSuu <= 20){
            iFu = 0;
        } 
        else if(huSuu == 25){
            iFu = 1;
        } 
        else if(huSuu > 110){
            iFu = 10;
        } 
        else {
            iFu = (huSuu / 10) - 1;
        }

        if( hanSuu > 13 ) hanSuu = 13;

        switch( hanSuu )
        {
            case 1: return new Score( SCORE_1[iFu] );
            case 2: return new Score( SCORE_2[iFu] );
            case 3: return new Score( SCORE_3[iFu] );
            case 4: return new Score( SCORE_4[iFu] );
            case 5: return new Score( SCORE_5 );
            case 6:
            case 7: return new Score( SCORE_6_7 );
            case 8:
            case 9:
            case 10:return new Score( SCORE_8_10 );
            case 11:
            case 12:return new Score( SCORE_11_12 );
            case 13:return new Score( SCORE_13 );

            default:return new Score( SCORE_1[0] );
        }
    }

	/*
    // 流局满贯 //
    public static void SetNagashiMangan(AgariInfo agariInfo)
    {
        agariInfo.han = 5;
        agariInfo.fu = 30;
        agariInfo.scoreInfo = GetScoreInfo(5, 30);

        agariInfo.hanteiYakus = new YakuHelper.YakuHandler[]
        { 
            new YakuHelper.CheckNagashimangan(null) 
        };
    }
	*/

    // 符を計算します
    public static int CalculateHu(Tehai tehai, Hai addHai, HaiCombi combi, AgariParam param, Yaku yaku)
    {
        int countHu = 20;

        //頭の牌を取得
        Hai atamaHai = new Hai( Hai.NumKindToID(combi.atamaNumKind) );

        // ３元牌なら２符追加
        if( atamaHai.Kind == Hai.KIND_SANGEN )
            countHu += 2;

        // 場風なら２符追加
        if( (atamaHai.ID - Hai.ID_TON) == (int)param.getBakaze() )
            countHu += 2;

        // 自風なら２符追加
        if( (atamaHai.ID - Hai.ID_TON) == (int)param.getJikaze() )
            countHu += 2;

        //平和が成立する場合は、待ちによる２符追加よりも優先される
        if( yaku.checkPinfu() == false )
        {
            // 単騎待ちの場合２符追加
            if(addHai.NumKind == combi.atamaNumKind)
                countHu += 2;

            // 嵌張待ちの場合２符追加
            // 数牌の２～８かどうか判定
            if(addHai.IsYaochuu == false)
            {
                for(int i = 0; i < combi.shunCount; i++)
                {
                    if( (addHai.NumKind-1) == combi.shunNumKinds[i] )
                        countHu += 2;
                }
            }

            // 辺張待ち(3)の場合２符追加
            if( addHai.IsYaochuu == false && addHai.Num == Hai.NUM_3 )
            {
                for(int i = 0; i < combi.shunCount; i++)
                {
                    if( (addHai.NumKind-2) == combi.shunNumKinds[i] )
                        countHu += 2;
                }
            }

            // 辺張待ち(7)の場合２符追加
            if( addHai.IsYaochuu == false && addHai.Num == Hai.NUM_7 )
            {
                for(int i = 0; i < combi.shunCount; i++)
                {
                    if( addHai.NumKind == combi.shunNumKinds[i] )
                        countHu += 2;
                }
            }
        }

        // 暗刻による加点
        Hai checkHai = null;
        for(int i = 0; i < combi.kouCount; i++)
        {
            checkHai = new Hai( Hai.NumKindToID(combi.kouNumKinds[i]) );

            // 牌が字牌もしくは1,9
            if( checkHai.IsYaochuu == true ) {
                countHu += 8;
            }
            else {
                countHu += 4;
            }
        }


        Fuuro[] fuuros = tehai.getFuuros();

        for (int i = 0; i < fuuros.Length; i++) 
        {
            switch( fuuros[i].Type ) 
            {
                case EFuuroType.MinKou:
                {
                    // 牌が字牌もしくは1,9
                    if( fuuros[i].Hais[0].IsYaochuu == true) {
                        countHu += 4;
                    } 
                    else {
                        countHu += 2;
                    }
                }
                break;

                case EFuuroType.KaKan:
                case EFuuroType.DaiMinKan:
                {
                    // 牌が字牌もしくは1,9
                    if( fuuros[i].Hais[0].IsYaochuu == true) {
                        countHu += 16;
                    } 
                    else {
                        countHu += 8;
                    }
                }
                break;

                case EFuuroType.AnKan:
                {
                    // 牌が字牌もしくは1,9
                    if( fuuros[i].Hais[0].IsYaochuu == true ) {
                        countHu += 32;
                    } 
                    else {
                        countHu += 16;
                    }
                }
                break;
            }
        }

        // ツモ上がりで平和が成立していなければ２譜追加
        if( param.getYakuFlag( EYakuFlagType.TSUMO ) == true)
        {
            if(yaku.checkPinfu() == false)
                countHu += 2;
        }

        // 面前ロン上がりの場合は１０符追加
        if( param.getYakuFlag(EYakuFlagType.TSUMO) == false )
        {
            if( yaku.isNaki == false )
                countHu += 10;
        }

        // 一の位がある場合は、切り上げ
        if( countHu % 10 != 0 ) // 23 -> 30.
            countHu = countHu - (countHu % 10) + 10;

        return countHu;
    }


    public static int GetAgariScore(Tehai tehai, Hai addHai, AgariParam param, ref HaiCombi[] combis, ref AgariInfo agariInfo)
    {
        formatWorker.setCounterFormat(tehai, addHai);

        // あがりの組み合わせを取得します
        int combisCount = formatWorker.calculateCombisCount( combis );
		//Debug.Log ("GetAgariScore() combisCount="+combisCount);
        combis = formatWorker.getCombis();
		/*
        /// 1. check Chiitoitsu(七对子)
        if( formatWorker.isChiitoitsu() )
        {
            Yaku yaku = Yaku.NewYaku_Chiitoitsu(tehai, addHai, param);

            agariInfo.han = yaku.getHan();
            agariInfo.fu = 25;
            agariInfo.hanteiYakus = yaku.getHanteiYakus();

            agariInfo.scoreInfo = GetScoreInfo(agariInfo.han, agariInfo.fu);

            return agariInfo.scoreInfo.koAgari;
        }
		*/
		/*
        /// 2. check Kokushi(国士无双)
        if( formatWorker.isKokushi() )
        {
            Yaku yaku = Yaku.NewYaku_Kokushi(tehai, addHai, param);

            if( yaku.isKokushi )
            {
                agariInfo.han = 13;
                agariInfo.fu = 20;
                agariInfo.hanteiYakus = yaku.getHanteiYakus();

                agariInfo.scoreInfo = GetScoreInfo(agariInfo.han, agariInfo.fu);

                return agariInfo.scoreInfo.koAgari;
            }

            return 0;
        }
		*/
		//return combisCount;

        /// 3. check common combi yaku.
        if( combisCount <= 0 )
            return 0;

		int[] hanSuuArr = new int[combisCount]; // 役 計算台數
        //int[] huSuuArr  = new int[combisCount]; // 符
        //int[] scoreArr = new int[combisCount]; // 点数（子のロン上がり）

        int maxAgariScore = 0; // 最大の点数

        for(int i = 0; i < combisCount; i++)
        {
            Yaku yaku = Yaku.NewYaku_Common(tehai, addHai, combis[i], param);
            hanSuuArr[i] = yaku.calculateHanSuu();//計算台數
			//maxAgariScore += hanSuuArr[i];
            //huSuuArr[i] = CalculateHu(tehai, addHai, combis[i], param, yaku);

            //Score scoreInfo = GetScoreInfo(hanSuuArr[i], huSuuArr[i]);

            //scoreArr[i] = scoreInfo.koAgari;

            if( hanSuuArr[i] > maxAgariScore )
            {
                agariInfo.han = hanSuuArr[i];
                //agariInfo.fu = huSuuArr[i];
                agariInfo.hanteiYakus = yaku.getHanteiYakus();

                //agariInfo.scoreInfo = scoreInfo;

                maxAgariScore = hanSuuArr[i];
            }
            
            
        }
		//agariInfo.han = maxAgariScore;
		//agariInfo.fu = 0;
		//agariInfo.scoreInfo = GetScoreInfo(5, 30);

		//agariInfo.hanteiYakus = yaku.getHanteiYakus();
        // 最大値を探す
        //maxAgariScore = 0;

        //for(int i = 0; i < combisCount; i++) 
        //{
        //    if( scoreArr[i] > maxAgariScore )
        //        maxAgariScore = scoreArr[i];
        //}
        //return maxAgariScore;
		return combisCount;
    }


    /// <summary>
    /// Gets the final point score.
    /// 
    /// Note: if the reach bou is not 0, the topest player will get it.
    ///       if more than 1 player has the same topest score, let the one who is the nearest EKaze.Ton be topest.
    /// </summary>

    public static List<PlayerTenbouChangeInfo> GetPointScore( List<Player> playerList, ref int reachBou )
    {
        List<Player> playerList_Sort = new List<Player>( playerList );
        playerList_Sort.Sort( SortPlayer );

        // topest player
        int topestPlayerIndex = 0;

        //马: 20-10 (20,10,-10,-20)
        int ShunMa = 10;
        int BackTenbou = GameSettings.Back_Tenbou;
        float Base = 1000f;
        int TopBonus = Math.FloorToInt((BackTenbou - GameSettings.Init_Tenbou) / Base * playerList.Count); //20pt


        if( reachBou > 0 ){
            playerList_Sort[topestPlayerIndex].increaseTenbou( reachBou * GameSettings.Reach_Cost );
            reachBou = 0;
        }

        // calculate points
        List<PlayerTenbouChangeInfo> resultPointList = new List<PlayerTenbouChangeInfo>();
        int totalPoint = 0;
        int bonus = 0;
        Player player;

        for( int i = 0; i < playerList_Sort.Count; i++ )
        {
            player = playerList_Sort[i];

            bonus = (i == topestPlayerIndex)? TopBonus : 0;

            PlayerTenbouChangeInfo ptci = new PlayerTenbouChangeInfo();
            ptci.playerKaze = player.JiKaze;
            ptci.current = player.Tenbou;
            ptci.changed = Math.FloorToInt( (player.Tenbou - BackTenbou)/Base + ((2-i) * ShunMa) + bonus );

            resultPointList.Add( ptci );

            totalPoint += ptci.changed;
        }

        // adjust
        if( totalPoint != 0 )
            resultPointList[topestPlayerIndex].changed -= totalPoint;

        return resultPointList;
    }

    static int SortPlayer(Player x, Player y)
    {
        if( x.Tenbou == y.Tenbou )
        {
            return (int)x.JiKaze - (int)y.JiKaze;  // As EKaze.Ton == 0, this can be so simple
        }
        else{
            return -(x.Tenbou - y.Tenbou);
        }
    }

}
