using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
/// <summary>
/// Mahjong main. The game logic manager
/// </summary>

public class MahjongMain : Mahjong 
{
    protected List<AgariUpdateInfo> m_agariUpdateInfoList = new List<AgariUpdateInfo>();
    public List<AgariUpdateInfo> AgariUpdateInfoList
    {
        get{ return m_agariUpdateInfoList; }
        protected set{ m_agariUpdateInfoList = value; }
    }


    protected ERyuuKyokuReason m_ryuuKyokuReason = ERyuuKyokuReason.None;
    public ERyuuKyokuReason RyuuKyokuReason
    {
        get{ return m_ryuuKyokuReason; }
        set{ m_ryuuKyokuReason = value; }
    }

    protected EAgariType m_agariResult = EAgariType.None;
    public EAgariType AgariResult
    {
        get{ return m_agariResult; }
        set{ m_agariResult = value; }
    }

    public bool Reach4Flag
    {
        get; set;
    }

    protected AgariUpdateInfo GetBasicAgariInfo( AgariInfo ai = null )
    {
        AgariUpdateInfo aupdateInfo = new AgariUpdateInfo();

        if( ai != null ) AgariUpdateInfo.Copy(aupdateInfo, ai);
        
        aupdateInfo.manKaze = getManKaze();
        aupdateInfo.bakaze = getBaKaze();
        aupdateInfo.kyoku = Kyoku;
        aupdateInfo.isLastKyoku = IsLastKyoku();
        aupdateInfo.honba = HonBa;
        aupdateInfo.reachBou = ReachBou;

        return aupdateInfo;
    }


    // Step: 1
    protected override void initialize()
    {
        // 山を作成する
        m_yama = new Yama();

		m_sais = new Sai[] { new Sai(), new Sai(), new Sai() };

        // 赤ドラを設定する
        //if( GameSettings.UseRedDora ) {
        //    m_yama.setRedDora(Hai.ID_PIN_5, 2);
        //    m_yama.setRedDora(Hai.ID_WAN_5, 1);
        //    m_yama.setRedDora(Hai.ID_SOU_5, 1);
        //}

        // 捨牌を作成する
        m_suteHaiList = new List<SuteHai>();

        // プレイヤーの配列を初期化する。
        m_playerList = new List<Player>();

		GameObject go1 = new GameObject("Player_A");
		Man a  = go1.AddComponent<Man> ();
		a.VoiceType = EVoiceType.W_A;
		a.Name = "玩家A";
        a.Order = 0;
		m_playerList.Add (a);

		GameObject go2 = new GameObject ("AI_B");
		AI b  = go2.AddComponent<AI> ();
		b.VoiceType = EVoiceType.W_B;
		b.Name = "電腦B";
        b.Order = 1;
        m_playerList.Add (b);

		GameObject go3 = new GameObject ("AI_C");
		AI c  = go3.AddComponent<AI> ();
		c.VoiceType = EVoiceType.W_C;
		c.Name = "電腦C";
        c.Order = 2;
        m_playerList.Add (c);

		GameObject go4 = new GameObject ("AI_D");
		AI d  = go4.AddComponent<AI> ();
		d.VoiceType = EVoiceType.W_D;
		d.Name = "電腦D";
        d.Order = 3;
        m_playerList.Add (d);
        //m_playerList.Add( new Man("玩家A", EVoiceType.W_A) );
		//m_playerList.Add( new RPCMan("玩家B", EVoiceType.W_B) );
        //m_playerList.Add( new AI("電腦C", EVoiceType.W_C) );
        //m_playerList.Add( new AI("電腦D", EVoiceType.W_D) );

        //for( int i = 0; i < m_playerList.Count; i++ )
        //{
        //    m_playerList[i].Tenbou = GameSettings.Init_Tenbou;
        //}

        GameSettings.PlayerCount = m_playerList.Count;

        GameAgent.Instance.Initialize(this);

        // 局を初期化する
        m_kyoku = (int)EKyoku.Ton_1;
        m_honba = 0;
        m_reachBou = 0;
    }


    // Step: 2
    public Sai[] Saifuri()
    {
        m_sais[0].SaiFuri();
        m_sais[1].SaiFuri();
		m_sais[2].SaiFuri();
        return m_sais;
    }

    public bool needSelectChiiCha()
    {
        return m_chiichaIndex < 0;
    }

    // Step: 3
    public void SetOyaChiicha(int index)
    {
		m_chiichaIndex = index;

        //if(debugMode) m_oyaIndex = 0;

		m_oyaIndex = (4-m_chiichaIndex) % 4;
		//Debug.Log ("m_oyaIndex="+m_oyaIndex);
    }

    public void SetNextOya()
    {
        m_oyaIndex++;
        if(m_oyaIndex >= m_playerList.Count)
            m_oyaIndex = 0;
    }

    protected void initPlayerKaze()
    {
        EKaze kaze = EKaze.Ton; //oya is always EKaze.Ton

		for( int i = 0, j = m_oyaIndex; i < m_playerList.Count; i++,j++)
        {
            if( j >= m_playerList.Count ) j = 0;

            m_playerList[j].JiKaze = kaze;

			kaze = kaze.Prev();
        }
    }

    // Step: 4
    public void PrepareKyoku()
    {
        // 連荘を初期化する。
        m_renchan = false;

        m_isTenhou = true;
        m_isChihou = true;
        m_isRenhou = true;

        m_isTsumo = false;
        m_isRinshan = false;
        m_isChanKan = false;
        m_isLast = false;


        for( int i = 0; i < m_playerList.Count; i++ )
        {
            m_playerList[i].Init();
        }

        initPlayerKaze();


        m_suteHaiList.Clear();
        m_sutehaiIndex = -1;

        m_agariUpdateInfoList.Clear();
        m_renhouPlayers.Clear();
        m_ryuuKyokuReason = ERyuuKyokuReason.None;
        m_agariResult = EAgariType.None;
        Reach4Flag = false;

        m_reachBou = 0;

        m_tsumoHai = new Hai();
        m_suteHai = new Hai();
        m_kakanHai = new Hai();

        // 洗牌する
        //m_yama.Shuffle();
    }

    // 山に割れ目を設定する
    protected void setWareme(Sai[] sais)
    {
		int sum = sais[0].Num + sais[1].Num + sais[2].Num;
		//Debug.Log ("sum="+sum);
		int waremePlayer = 0;
		//Debug.Log ("ChiiChaIndex="+ChiiChaIndex);
		switch(ChiiChaIndex){
		case 0://東
			waremePlayer = (sum - 1) % 4;
			//Debug.Log ("東");
			break;
		case 1://北
			waremePlayer = (sum) % 4;
			//Debug.Log ("北");
			break;
		case 2://西
			waremePlayer = (sum+1) % 4;
			//Debug.Log ("西");
			break;
		case 3://南
			waremePlayer = (sum - 2) % 4;
			//Debug.Log ("南");
			break;
		}
        //int waremePlayer = (ChiiChaIndex - sum - 1) % 4;
		//int waremePlayer = (4-ChiiChaIndex + sum -1) % 4;
		//Debug.Log();
		/*
		if( waremePlayer < 0 )
            waremePlayer += 4;
		if (ChiiChaIndex > 0) {
			if (waremePlayer == 1)
				waremePlayer = 3;
			if (waremePlayer == 3)
				waremePlayer = 1;
		}
		*/
		//Debug.Log ("waremePlayer="+waremePlayer);
		m_wareme = ( (waremePlayer) % 4 ) * 34 + sum * 2;
		//Debug.Log ("m_wareme="+m_wareme);
		//startHaisIndex = 0;
		//int startHaisIndex = ((4-waremePlayer) %4) * 34)+ sum*2;
		int startHaisIndex = m_wareme-1;
		//Debug.Log ("startHaisIndex="+startHaisIndex);
        //m_wareme = startHaisIndex; // 开始拿牌位置-1.
		//Debug.Log ("m_wareme="+m_wareme);
		//Debug.Log ("startHaisIndex="+startHaisIndex);
        Yama.setTsumoHaisStartIndex(startHaisIndex);
    }

    // 配牌する
    protected virtual void Haipai()
    {
		
        // everyone picks 4x4 hais.
        for( int i = 0, j = m_oyaIndex; i < m_playerList.Count * 16; j++ ) 
        {
            if( j >= m_playerList.Count )
                j = 0;

            // pick 4 hais once.
            Hai[] hais = m_yama.PickHaipai();
            for( int h = 0; h < hais.Length; h++ )
            {
                m_playerList[j].Tehai.addJyunTehai( hais[h] );

                i++;
            }
        }

		/*
        // then everyone picks 1 hai.
        for( int i = 0, j = m_oyaIndex; i < m_playerList.Count * 1; i++,j++ )
        {
            if( j >= m_playerList.Count )
                j = 0;

            Hai hai = m_yama.PickTsumoHai();
            m_playerList[j].Tehai.addJyunTehai( hai );
        }
		*/

        // sort hais
        for( int i = 0; i < m_playerList.Count; i++ )
        {
            m_playerList[i].Tehai.Sort();
        }


        //if(debugMode == true)
        //    StartTest();
    }

    // Step: 5
    public void SetWaremeAndHaipai() 
    {
        // 山に割れ目を設定する。
        setWareme(m_sais);

        // 配牌する。
        Haipai();
    }

    // Step: 6
    public void PrepareToStart()
    {
		//Debug.Log ("PrepareToStart("+m_oyaIndex+")");
		int i = 0;
		foreach (Player p in m_playerList) {
			if (p.JiKaze == EKaze.Ton) {
				m_activePlayer = p;
				//Debug.Log ("i="+i);
			}
			i++;
		}
		//m_activePlayer = m_playerList[m_oyaIndex]; 
        m_kazeFrom = m_activePlayer.JiKaze;
		EventManager.Instance.RpcSendEvent(UIEventType.Display_Homeba_Panel, m_oyaIndex);
    }

	//是否是最後一局
    public bool IsLastKyoku()
    {
        return m_kyoku >= GameSettings.Kyoku_Max;
    }
    protected void GoToNextKyoku()
    {
        m_kyoku++;
    }

    public void SetNextPlayer()
    {
		m_kazeFrom = m_kazeFrom.Prev();
        m_activePlayer = getPlayer( m_kazeFrom );
		//Debug.Log ("SetNextPlayer()"+m_activePlayer.Name);
		int idx = getPlayerIndex( m_kazeFrom );
		//Debug.Log ("idx="+idx);
		EventManager.Instance.RpcSendEvent(UIEventType.Display_Arrow_Panel, idx);
    }

    // ツモ牌がない場合、流局する
    public bool checkNoTsumoHai()
    {
        return m_tsumoHai == null;
    }

    /// <summary>
    /// Ends the game, and calculate the final point score.
    /// </summary>
    public void EndGame()
    {
        AgariUpdateInfoList.Clear();

        AgariUpdateInfo aupdateInfo = GetBasicAgariInfo();

        aupdateInfo.tenbouChangeInfoList = AgariScoreManager.GetPointScore(PlayerList, ref m_reachBou);

        AgariUpdateInfoList.Add( aupdateInfo );
    }

    #region Request & Response
    protected ERequest m_request = ERequest.Handle_TsumoHai;
    protected Dictionary<EKaze, EResponse> m_playerRespDict = new Dictionary<EKaze, EResponse>();

    public ERequest CurrentRequest
    {
        get{ return m_request; }
    }
    public Dictionary<EKaze, EResponse> PlayerResponseMap
    {
        get{ return m_playerRespDict; }
    }

    public void SetRequest( ERequest req )
    {
        m_request = req;
        ClearResponseCache();

        CacheActivePlayer();
    }

    public void SendRequestToActivePlayer(Hai haiToHandle)
    {
        m_activePlayer.HandleRequest(m_request, m_kazeFrom, haiToHandle, OnPlayerResponse);
    }

    protected void CacheActivePlayer()
    {
        m_kazeFrom = m_activePlayer.JiKaze;
    }
    public void ActivatePlayerBack()
    {
        ResetActivePlayer(m_kazeFrom);
    }
    public void ResetActivePlayer(EKaze kaze)
    {
        m_activePlayer = getPlayer(kaze);
    }

    protected void ClearResponseCache()
    {
        m_playerRespDict.Clear();
    }

    public void OnPlayerResponse(EKaze responserKaze, EResponse response)
    {
        if( !CheckResponseValid(m_request, response, responserKaze) )
            throw new MahjongException("Invalid response '{0}' to request '{1}' from kaze {2} to kaze {3}", 
                                       response, m_request, m_kazeFrom, responserKaze);

        switch( m_request )
        {
            case ERequest.Handle_TsumoHai:
            {
                OnResponse_Handle_TsumoHai();
            }
            break;
            case ERequest.Select_SuteHai:
            {
                OnResponse_Handle_Select_SuteHai();
            }
            break;
            case ERequest.Handle_KaKanHai:
            {
                // add to response list, if all other players do response, handle response.
                m_playerRespDict.Add(responserKaze, response);

                if( m_playerRespDict.Count >= GameSettings.PlayerCount-1 )
                    OnResponse_Handle_KaKanHai();
            }
            break;
            case ERequest.Handle_SuteHai:
            {
                // add to response list, if all other players do response, handle response.
                m_playerRespDict.Add(responserKaze, response);

                if( m_playerRespDict.Count >= GameSettings.PlayerCount-1 )
                    OnResponse_Handle_SuteHai();
            }
            break;

            default:
            {
                throw new MahjongException("Unhandled request: " + m_request.ToString());
            }
        }
    }

    public bool CheckResponseValid(ERequest request, EResponse response, EKaze responserKaze)
    {
        switch( request )
        {
            case ERequest.Handle_TsumoHai:
            {
                if( response != EResponse.Tsumo_Agari &&
                   response != EResponse.Ankan &&
                   response != EResponse.Kakan &&
                   response != EResponse.Reach && 
                   response != EResponse.SuteHai &&
                   response != EResponse.Nagashi) //only enable when ERyuuKyokuReason.HaiTypeOver9 happened
                {
                    return false;
                }
            }
            break;
            case ERequest.Select_SuteHai:
            {
                if(response != EResponse.SuteHai)
                {
                    return false;
                }
            }
            break;
            case ERequest.Handle_KaKanHai:
            {
                if(response != EResponse.Ron_Agari &&
                   response != EResponse.Nagashi )
                {
                    return false;
                }
            }
            break;
            case ERequest.Handle_SuteHai:
            {
                if(response != EResponse.Ron_Agari &&                    
                   response != EResponse.DaiMinKan &&
                   response != EResponse.Pon &&
                   response != EResponse.Chii_Left &&
                   response != EResponse.Chii_Center &&
                   response != EResponse.Chii_Right &&
                   response != EResponse.Nagashi )
                {
                    return false;
                }
                else if(response == EResponse.Chii_Left ||
                        response == EResponse.Chii_Center ||
                        response == EResponse.Chii_Right )
                {
                    if( getRelation(m_kazeFrom, responserKaze) != (int)ERelation.KaMiCha )
                        return false;
                }
            }
            break;
        }
        return true;
    }
    #endregion

    #region Pick Hais
    // Step: 7
    public void PickNewTsumoHai()
    {
        // ツモ牌を取得する。
        m_tsumoHai = m_yama.PickTsumoHai();
		//string name = ResManager.getMagjongName (m_tsumoHai.Kind, m_tsumoHai.Num);
		//Debug.Log ("摸牌("+name+")");
        m_isTsumo = true;

        int tsumoNokori = m_yama.getTsumoNokori();
        if( tsumoNokori <= 0 ) {
            m_isLast = true;
        }
        else {
            int chiiHouNokori = Yama.TSUMO_HAIS_MAX - (4*3+1)*GameSettings.PlayerCount - GameSettings.PlayerCount; //66
            if( tsumoNokori < chiiHouNokori ) { //66
                m_isChihou = false;
            }
        }

        //if(debugMode) m_tsumoHai = getTestPickHai();

        //Handle_TsumoHai_Internel();
    }
    protected void Handle_TsumoHai_Internel()
    {
        if( checkNoTsumoHai() )
        {
            this.RyuuKyokuReason = ERyuuKyokuReason.NoTsumoHai;

            //if( HandleNagashiMangan() ){  
            //}
            //else{
                if( HandleRyuukyokuTenpai() ){
                    
                }
                else{
                    if( IsLastKyoku() ){
                        EndGame();
                    }
                    else{
                        GoToNextKyoku();
                    }
                }
            //}
        }
        else
        {
            Ask_Handle_TsumoHai();
        }
    }

    // pick up Rinshan hai.
    public void PickRinshanHai()
    {
        m_isTsumo = true;

        m_tsumoHai = m_yama.PickRinshanHai();
        isRinshan = true;

        //if(debugMode) m_tsumoHai = getTestRinshanHai();

        //Handle_RinshanHai_Internel();
    }
    protected void Handle_RinshanHai_Internel()
    {
        if( checkKanCountOverFlow() ){
            // ERyuuKyokuReason.KanOver4
        }
        else{
            Ask_Handle_TsumoHai();
        }
    }
    #endregion

    #region Ask Handle Tsumo Hai
    public void Ask_Handle_TsumoHai()
    {
        SetRequest(ERequest.Handle_TsumoHai);
        SendRequestToActivePlayer(m_tsumoHai);
    }

    public System.Action onResponse_TsumoHai_Handler;

    public void OnResponse_Handle_TsumoHai()
    {
        if(onResponse_TsumoHai_Handler != null)
            onResponse_TsumoHai_Handler.Invoke();
        else
            Handle_TsumoHai_Response_Internel();
    }
    protected void Handle_TsumoHai_Response_Internel()
    {
        EResponse response = ActivePlayer.Action.Response;

        switch( response )
        {
            case EResponse.Tsumo_Agari:
            {
                Handle_TsumoAgari();
            }
            break;
            case EResponse.Ankan:
            {
                Handle_AnKan();
            }
            break;
            case EResponse.Kakan:
            {
                Handle_KaKan();
            }
            break;
            case EResponse.Reach:
            {
                Handle_Reach();
            }
            break;
            case EResponse.SuteHai:
            {
                Handle_SuteHai();
            }
            break;
            case EResponse.Nagashi:
            {
                //Handle_Invalid_RyuuKyoku(); handle in game state classes
            }
            break;
        }
    }
    #endregion

    #region Ask Handle KaKan Hai
    public void Ask_Handle_KaKanHai()
    {
        SetRequest(ERequest.Handle_KaKanHai);

        EKaze nextKaze = m_activePlayer.JiKaze.Next();

        // ask other 3 players.
        for(int i = 0; i < m_playerList.Count; i++)
        {
            m_activePlayer = getPlayer( nextKaze );

            if( nextKaze == m_kazeFrom )
                continue;

            SendRequestToActivePlayer(m_kakanHai);

            nextKaze = m_activePlayer.JiKaze.Next();
        }
    }

    public System.Action onResponse_KakanHai_Handler;

    public void OnResponse_Handle_KaKanHai()
    {
        if(onResponse_KakanHai_Handler != null)
            onResponse_KakanHai_Handler.Invoke();
        else
            Handle_KaKanHai_Response_Intenel();
    }
    protected void Handle_KaKanHai_Response_Intenel()
    {
        if( CheckMultiRon() == true )
        {
            Handle_KaKan_Ron();
        }
        else
        {
            Handle_KaKan_Nagashi();
        }
    }
    #endregion

    public bool CheckMultiRon()
    {
        foreach( var info in m_playerRespDict )
            if(info.Value == EResponse.Ron_Agari)
                return true;

        return false;
    }
    public List<EKaze> GetRonPlayers()
    {
        List<EKaze> ronPlayers = new List<EKaze>();

        foreach( var info in m_playerRespDict )
            if(info.Value == EResponse.Ron_Agari)
                ronPlayers.Add(info.Key);

        return ronPlayers;
    }

    #region Ask Handle Sute Hai
    public System.Action onResponse_SuteHai_Handler;

    public void Ask_Handle_SuteHai()
    {
        SetRequest(ERequest.Handle_SuteHai);

        EKaze nextKaze = m_activePlayer.JiKaze.Next();

        // ask other 3 players.
        for(int i = 0; i < m_playerList.Count; i++)
        {
            m_activePlayer = getPlayer( nextKaze );

            if( nextKaze == m_kazeFrom )
                continue;

            SendRequestToActivePlayer(m_suteHai);

            nextKaze = m_activePlayer.JiKaze.Next();
        }
    }

    public void OnResponse_Handle_SuteHai()
    {
        if(onResponse_SuteHai_Handler != null)
            onResponse_SuteHai_Handler.Invoke();
        else
            Handle_Response_SuteHai_Internel();
    }
    protected void Handle_Response_SuteHai_Internel()
    {
        if( CheckMultiRon() == true )
        {
            Handle_SuteHai_Ron();
        }
        else
        {
            // As DaiMinKan and Pon is availabe to one player at the same time, and their priority is bigger than Chii,
            // perform DaiMinKan and Pon firstly.
            List<EKaze> validKaze = new List<EKaze>();

            foreach( var info in m_playerRespDict )
            {
                if( info.Value == EResponse.Pon || info.Value == EResponse.DaiMinKan )
                    validKaze.Add( info.Key );
            }

            if( validKaze.Count > 0 )
            {
                if( validKaze.Count == 1 )
                {
                    EKaze kaze = validKaze[0];
                    EResponse resp = m_playerRespDict[kaze];

                    ResetActivePlayer(kaze);

                    switch( resp )
                    {
                        case EResponse.Pon:
                        {
                            Handle_Pon();
                        }
                        break;
                        case EResponse.DaiMinKan:
                        {
                            Handle_DaiMinKan();
                        }
                        break;
                    }
                }
                else{
                    throw new MahjongException("More than one player perform Pon or DaiMinKan!?");
                }
            }
            else // no one Pon or DaiMinKan, perform Chii
            {
                foreach( var info in m_playerRespDict )
                {
                    if( info.Value == EResponse.Chii_Left || 
                       info.Value == EResponse.Chii_Center || 
                       info.Value == EResponse.Chii_Right )
                    {
                        validKaze.Add( info.Key );
                    }
                }

                if( validKaze.Count > 0 )
                {
                    if( validKaze.Count == 1 )
                    {
                        EKaze kaze = validKaze[0];
                        EResponse resp = m_playerRespDict[kaze];

                        ResetActivePlayer(kaze);

                        switch( resp )
                        {
                            case EResponse.Chii_Left:
                            {
                                Handle_ChiiLeft();
                            }
                            break;
                            case EResponse.Chii_Center:
                            {
                                Handle_ChiiCenter();
                            }
                            break;
                            case EResponse.Chii_Right:
                            {
                                Handle_ChiiRight();
                            }
                            break;
                        }
                    }
                    else{
                        throw new MahjongException("More than one player perform Chii!?");
                    }
                }
                else // Nagashi
                {
                    Handle_SuteHai_Nagashi();
                }
            }
        }
    }
    #endregion

    #region Ask Select Sute Hai
    public System.Action onResponse_Select_SuteHai_Handler;

    public void Ask_Select_SuteHai()
    {
        SetRequest(ERequest.Select_SuteHai);
        SendRequestToActivePlayer(null);
    }

    public void OnResponse_Handle_Select_SuteHai()
    {
        if(onResponse_Select_SuteHai_Handler != null)
            onResponse_Select_SuteHai_Handler.Invoke();
        else
            Handle_Response_Select_SuteHai_Internel();
    }
    protected void Handle_Response_Select_SuteHai_Internel()
    {
        Handle_SelectSuteHai();
    }
    #endregion


    #region Handler for All Player Responses
    // for ERequest.Handle_TsumoHai
    public void Handle_TsumoAgari()
    {
        OnTsumo();
    }

    public void Handle_AnKan()
    {
        m_isTenhou = false;
        m_isChihou = false;
        m_isRenhou = false;
        m_isTsumo = false;

        int kanSelectIndex = ActivePlayer.Action.KanSelectIndex;
        int ankanHaiID = ActivePlayer.Action.TsumoKanHaiList[kanSelectIndex].ID;

        // check if all ankan hais from tehai or tehai + tsumo hai.
        Hai[] jyunTehais = ActivePlayer.Tehai.getJyunTehai();

        int count = 0, oneIndex = 0;
        for( int i = 0; i < jyunTehais.Length; i++ )
        {
            if( jyunTehais[i].ID == ankanHaiID ){
                count++;
                oneIndex = i;
            }
        }

        Hai kanHai = null;

        if( count < Tehai.MENTSU_LENGTH_4 )
        {
            if(count != Tehai.MENTSU_LENGTH_3) Utils.LogError("Error!!!");

            kanHai = m_tsumoHai;
        }
        else{
            kanHai = ActivePlayer.Tehai.removeJyunTehaiAt(oneIndex);

            ActivePlayer.Tehai.addJyunTehai( new Hai(m_tsumoHai) );
            ActivePlayer.Tehai.Sort();
        }

        ActivePlayer.Tehai.setAnKan( kanHai );
        ActivePlayer.Tehai.Sort();

        //ActivePlayer.IsIppatsu = false;


        //PickRinshanHai();
    }

    public void Handle_KaKan()
    {
        m_isTsumo = false;
        m_isChanKan = true;

        int kanSelectIndex = ActivePlayer.Action.KanSelectIndex;
        int kakanHaiID = ActivePlayer.Action.TsumoKanHaiList[kanSelectIndex].ID;


        int kakanHaiIndex = ActivePlayer.Tehai.getHaiIndex( kakanHaiID );
        if( kakanHaiIndex < 0 ) // the tsumo hai.
        {
            m_kakanHai = m_tsumoHai;
        }
        else
        {
            m_kakanHai = m_activePlayer.Tehai.removeJyunTehaiAt( kakanHaiIndex );

            m_activePlayer.Tehai.addJyunTehai( m_tsumoHai );

            m_activePlayer.Tehai.Sort();
        }

        ActivePlayer.Tehai.setKaKan(m_kakanHai);

        //PickRinshanHai();
    }


    /// <summary>
    /// Is the kan count over 4. 
    /// Ryuukyoku rule in this game is: 
    /// once total kan count >= 4 and they are not belong to the same one player.
    /// </summary>
    public bool checkKanCountOverFlow()
    {
        int[] allPlayerKanCount = new int[4];
        int kanCount = GetTotalKanCount( allPlayerKanCount );

        if( kanCount < 4 ){
            return false;
        }
        else{
            for( int i = 0; i < allPlayerKanCount.Length; i++ )
            {
                if( allPlayerKanCount[i] >= 4 )
                    return false;
            }
        }

        return true;
    }
    public bool checkReach4()
    {
        if( PlayerList.Count < 4 )
            return false;

        for( int i = 0; i < PlayerList.Count; i++ )
            if( !PlayerList[i].IsReach )
                return false;

        return true;
    }
    public bool checkSuteFonHai4()
    {
        if( !isChiHou )
            return false;

        if( AllSuteHaiList.Count != 4 )
            return false;

        if( AllSuteHaiList.Count == 0 )
            return false;

        SuteHai hai = AllSuteHaiList[0];
        if( !hai.IsFon )
            return false;

        for( int i = 1; i < AllSuteHaiList.Count; i++ )
        {
            if( AllSuteHaiList[i].ID != hai.ID )
                return false;
        }

        return true;
    }
	/*
    public bool checkHaiTypeOver9()
    {
        return GameAgent.Instance.CheckHaiTypeOver9( ActivePlayer.Tehai, TsumoHai );
    }
    */

    // for ERyuuKyokuReason enums
    public void Handle_Invalid_RyuuKyoku()
    {
        AgariUpdateInfo aupdateInfo = GetBasicAgariInfo();

        Player player;
        for( int i = 0; i < PlayerList.Count; i++ )
        {
            player = PlayerList[i];

            // every player's tenbou change info
            PlayerTenbouChangeInfo ptci = new PlayerTenbouChangeInfo();
            ptci.playerKaze = player.JiKaze;
            ptci.isTenpai = false;
            ptci.changed = 0;
            ptci.current = player.Tenbou;
            aupdateInfo.tenbouChangeInfoList.Add( ptci );
        }

        AgariUpdateInfoList.Add( aupdateInfo );
    }

    public bool isTedashi
    {
        get; private set;
    }

    public void Handle_Reach()
    {
        //if( ActivePlayer.Tenbou < GameSettings.Reach_Cost )
        //    throw new MahjongException("Active player has not enough tenbou to reach!!!");

        m_isTenhou = false;
        m_isRinshan = false;
        m_isTsumo = false;

        // cost to set reach.
        m_activePlayer.IsReach = true;
        //m_activePlayer.IsIppatsu = true;

        //if( m_isChihou )
        //   m_activePlayer.IsDoubleReach = true;

        m_activePlayer.reduceTenbou( GameSettings.Reach_Cost );
        m_reachBou++;

        // sute hai.
        int reachSelectIndex = m_activePlayer.Action.ReachSelectIndex;
        int reachHaiIndex = m_activePlayer.Action.ReachHaiIndexList[reachSelectIndex];

        if( reachHaiIndex >= m_activePlayer.Tehai.getJyunTehaiCount() ) // ツモ切り
        {
            m_sutehaiIndex = m_activePlayer.Tehai.getJyunTehaiCount();
            m_suteHai = new Hai(m_tsumoHai);

            m_activePlayer.Hou.addHai( m_suteHai );

            isTedashi = false;
        }
        else {// 手出し
            m_sutehaiIndex = reachHaiIndex;
            m_suteHai = m_activePlayer.Tehai.removeJyunTehaiAt( m_sutehaiIndex );

            m_activePlayer.Tehai.addJyunTehai( m_tsumoHai );

            m_activePlayer.Tehai.Sort();

            m_activePlayer.Hou.addHai( m_suteHai );
            m_activePlayer.Hou.setTedashi( true );

            isTedashi = true;
        }

        // add sute hai to list
        m_suteHaiList.Add( new SuteHai( m_suteHai ) );

        m_activePlayer.SuteHaisCount = m_suteHaiList.Count;

        if(m_suteHaiList.Count >= GameSettings.PlayerCount)
            m_isRenhou = false;

        //PostUIEvent(UIEventType.Reach);
        //Ask_Handle_SuteHai();
    }

    // cancel Reach in some special situations.
    public void OnReachFail()
    {
        Player player = getPlayer(m_kazeFrom);
        player.increaseTenbou( GameSettings.Reach_Cost );
        player.IsReach = false;
        //player.IsDoubleReach = false;
        //player.IsIppatsu = false;

        m_reachBou--;
    }

    public void Handle_SuteHai()
    {
        m_isTenhou = false;
        m_isRinshan = false;
        m_isTsumo = false;

        // 捨牌のインデックスを取得する。
        m_sutehaiIndex = m_activePlayer.Action.SutehaiIndex;

        if( m_sutehaiIndex >= m_activePlayer.Tehai.getJyunTehaiCount() ) // ツモ切り
        {
            m_sutehaiIndex = m_activePlayer.Tehai.getJyunTehaiCount();
            m_suteHai = new Hai(m_tsumoHai);

            m_activePlayer.Hou.addHai( m_suteHai );

            isTedashi = false;
        }
        else {// 手出し
            m_suteHai = m_activePlayer.Tehai.removeJyunTehaiAt( m_sutehaiIndex );

            m_activePlayer.Tehai.addJyunTehai( m_tsumoHai );

            m_activePlayer.Tehai.Sort();

            m_activePlayer.Hou.addHai( m_suteHai );
            m_activePlayer.Hou.setTedashi( true );

            isTedashi = true;
        }

        m_suteHaiList.Add( new SuteHai( m_suteHai ) );

        if( !m_activePlayer.IsReach )
            m_activePlayer.SuteHaisCount = m_suteHaiList.Count;

        //m_activePlayer.IsIppatsu = false;

        if(m_suteHaiList.Count >= GameSettings.PlayerCount)
            m_isRenhou = false;

        //PostUIEvent(UIEventType.SuteHai);
        //Ask_Handle_SuteHai();
    }

    // for ERequest.Handle_KaKanHai
    public void Handle_KaKan_Ron()
    {
        m_isTsumo = false;
        OnMultiRon();
    }

    public void Handle_KaKan_Nagashi()
    {
        m_isChanKan = false;

        // if nobody chan kan, then pick up rinshan hai
        //PickRinshanHai();
    }

    // for ERequest.Handle_SuteHai
    public void Handle_SuteHai_Ron()
    {
        OnMultiRon();
    }

    public void Handle_SuteHai_Nagashi()
    {
        // go to next loop.
        //SetNextPlayer();
        //PickNewTsumoHai();
    }

    public void Handle_Pon()
    {
        m_isTenhou = false;
        m_isChihou = false;
        m_isRenhou = false;

        int relation = getRelation(m_kazeFrom, ActivePlayer.JiKaze);
        ActivePlayer.Tehai.setPon( m_suteHai, relation );
        ActivePlayer.Tehai.Sort();

        getPlayer(m_kazeFrom).Hou.setNaki(true);
    }

    public void Handle_DaiMinKan()
    {
        m_isTenhou = false;
        m_isChihou = false;
        m_isRenhou = false;

        int relation = getRelation(m_kazeFrom, ActivePlayer.JiKaze);
        ActivePlayer.Tehai.setDaiMinKan(m_suteHai, relation);
        ActivePlayer.Tehai.Sort();

        getPlayer(m_kazeFrom).Hou.setNaki(true);

        //PickRinshanHai();
    }

    public void Handle_ChiiLeft()
    {
        m_isTenhou = false;
        m_isChihou = false;
        m_isRenhou = false;

        int relation = getRelation(m_kazeFrom, ActivePlayer.JiKaze);
        ActivePlayer.Tehai.setChiiLeft(m_suteHai, relation);
        ActivePlayer.Tehai.Sort();

        getPlayer(m_kazeFrom).Hou.setNaki(true);
    }

    public void Handle_ChiiCenter()
    {
        m_isTenhou = false;
        m_isChihou = false;
        m_isRenhou = false;

        int relation = getRelation(m_kazeFrom, ActivePlayer.JiKaze);
        ActivePlayer.Tehai.setChiiCenter(m_suteHai, relation);
        ActivePlayer.Tehai.Sort();

        getPlayer(m_kazeFrom).Hou.setNaki(true);
    }

    public void Handle_ChiiRight()
    {
        m_isTenhou = false;
        m_isChihou = false;
        m_isRenhou = false;

        int relation = getRelation(m_kazeFrom, ActivePlayer.JiKaze);
        ActivePlayer.Tehai.setChiiRight(m_suteHai, relation);
        ActivePlayer.Tehai.Sort();

        getPlayer(m_kazeFrom).Hou.setNaki(true);
    }

    // for ERequest.Select_SuteHai
    public void Handle_SelectSuteHai()
    {
        m_isTenhou = false;
        m_isRinshan = false;
        m_isTsumo = false;

        // 捨牌のインデックスを取得する。
        m_sutehaiIndex = m_activePlayer.Action.SutehaiIndex;

        m_suteHai = m_activePlayer.Tehai.removeJyunTehaiAt( m_sutehaiIndex );
        if(m_suteHai == null)
            throw new MahjongException("Select sute hai won't be null, how it happend!?");

        m_activePlayer.Tehai.Sort();

        m_activePlayer.Hou.addHai( m_suteHai );
        m_activePlayer.Hou.setTedashi( true );

        isTedashi = true;

        m_suteHaiList.Add( new SuteHai( m_suteHai ) );

        if( !m_activePlayer.IsReach )
            m_activePlayer.SuteHaisCount = m_suteHaiList.Count;

        //m_activePlayer.IsIppatsu = false;

        //PostUIEvent(UIEventType.SuteHai);
        //Ask_Handle_SuteHai();
    }
    #endregion


    #region Handle Game Result

    #region RyuuKyoku
	/*
    /// <summary>
    /// Handles the ryuukyoku man gan.
    /// Only one player who is the nearest to Oya can perform nagashimangan
    /// if some one is nagashimangan(流局满贯), return true, otherwise return false.
    /// </summary>
    /// <returns><c>true</c>, if some one is ryuukyoku man, <c>false</c> otherwise.</returns>
    public bool HandleNagashiMangan() 
    {
        // 流し満貫の確認をする, start at OyaIndex.
        for( int i = 0, j = m_oyaIndex; i < m_playerList.Count; i++, j++ ) 
        {
            if( j >= m_playerList.Count )
                j = 0;

            bool agari = true;

            SuteHai[] suteHais = m_playerList[j].Hou.getSuteHais();
            for( int k = 0; k < suteHais.Length; k++ )
            {
                // check 1,9,字./
                if( suteHais[k].IsNaki || !suteHais[k].IsYaochuu ) {
                    agari = false;
                    break;
                }
            }

            if( agari == true )
            {
                ActivePlayer = m_playerList[j];

                AgariScoreManager.SetNagashiMangan( m_agariInfo ); // visitor.

                _HandleTsumoScore();

                //PostUIEvent( UIEventType.Tsumo_Agari, m_kazeFrom, m_kazeFrom );
                //EndKyoku();
                return true;
            }

        } // end for(playerList)

        return false;
    }
	*/

    public List<int> GetTenpaiPlayerIndex()
    {
        List<int> result = new List<int>();

        for( int i = 0; i < m_playerList.Count; i++ )
        {
            if( m_playerList[i].isTenpai() )
                result.Add(i);
        }
        return result;
    }

    // テンパイの確認をする
    public bool HandleRyuukyokuTenpai() 
    {
        List<int> tenpaiPlayers = GetTenpaiPlayerIndex();

        int increasedScore = 0;
        int reducedScore = 0;

        switch( tenpaiPlayers.Count )
        {
            default:
            case 0:
            break;

            case 1:
                increasedScore = 3000;
                reducedScore = 1000;
            break;
            case 2:
                increasedScore = 1500;
                reducedScore = 1500;
            break;
            case 3:
                increasedScore = 1000;
                reducedScore = 3000;
            break;
        }


        AgariUpdateInfo aupdateInfo = GetBasicAgariInfo();

        Player player;
        for( int i = 0; i < PlayerList.Count; i++ )
        {
            player = PlayerList[i];

            // every player's tenbou change info
            PlayerTenbouChangeInfo ptci = new PlayerTenbouChangeInfo();
            ptci.playerKaze = player.JiKaze;

            if( tenpaiPlayers.Contains(i) ){
                player.increaseTenbou( increasedScore );

                ptci.isTenpai = true;
                ptci.changed = increasedScore;
            }
            else{
                player.reduceTenbou( reducedScore );

                ptci.isTenpai = false;
                ptci.changed = -reducedScore;
            }

            ptci.current = player.Tenbou;
            aupdateInfo.tenbouChangeInfoList.Add( ptci );
        }

        AgariUpdateInfoList.Add( aupdateInfo );

        //PostUIEvent( UIEventType.RyuuKyoku );
        //EndRyuuKyoku();

        return true;
    }
    #endregion


    protected List<EKaze> m_ronPlayers = new List<EKaze>();
    protected List<EKaze> m_renhouPlayers = new List<EKaze>();

    protected void OnMultiRon()
    {
        List<EKaze> ronPlayers = GetRonPlayers();
        if( ronPlayers.Count > 3 )
            throw new MahjongException("4 players Ron, how it happened?");

        if( ronPlayers.Count == 3 && !GameSettings.AllowRon3 ) // ERyuuKyokuReason.Ron3
        {
            throw new MahjongException("ERyuuKyokuReason.Ron3"); // handle in game state classes.
        }
        else
        {
            /// Note: only the first ron player can get the reach bou,
            ///       make sure the first ron player is the nearest one to m_kazeFrom player.

            int index = -1;

            if( ronPlayers.Count == 1 ){
                m_ronPlayers = ronPlayers;
            }
            else{
                m_ronPlayers.Clear();

                while( ronPlayers.Count > 0 )
                {
                    index = ronPlayers.FindIndex( kaze => kaze == m_kazeFrom.Next() );
                    if( index >= 0 ){
                        m_ronPlayers.Add( ronPlayers[index] );
                        ronPlayers.RemoveAt( index );
                    }
                    else{
                        index = ronPlayers.FindIndex( kaze => kaze == m_kazeFrom.Next().Next() );
                        if( index >= 0 ){
                            m_ronPlayers.Add( ronPlayers[index] );
                            ronPlayers.RemoveAt( index );
                        }
                        else{
                            index = ronPlayers.FindIndex( kaze => kaze == m_kazeFrom.Next().Next().Next() );
                            if( index >= 0 ){
                                m_ronPlayers.Add( ronPlayers[index] );
                                ronPlayers.RemoveAt( index );
                            }
                            else{
                                break;
                            }
                        }
                    }
                } // end while()
            }

            m_renhouPlayers.Clear();

            // handle ron one by one
            for( int i = 0; i < m_ronPlayers.Count; i++ )
            {
                EKaze kaze = m_ronPlayers[i];

                if( m_isRenhou == true && getPlayerIndex(kaze) != OyaIndex )
                {
                    m_renhouPlayers.Add( kaze );
                }

                ResetActivePlayer( kaze );

                OnRon();
            }
        }
    }

    // some one has ron.
    protected void OnRon()
    {
        this.RyuuKyokuReason = ERyuuKyokuReason.None;

        if( m_renhouPlayers.Contains( ActivePlayer.JiKaze ) ){
            m_isRenhou = true;
        }
        else{
            m_isRenhou = false;
        }

        //AgariParam.ResetDoraHais(); // should reset params or create a new.

        //AgariParam.setOmoteDoraHais( getOpenedOmotoDoras() );
        //if( m_activePlayer.IsReach )
        //    AgariParam.setUraDoraHais( getOpenedUraDoraHais() );

        GetAgariScore(ActivePlayer.Tehai, SuteHai, ActivePlayer.JiKaze, AgariParam);

        //Utils.Log( AgariInfo.ToString() );

        _HandleRonScore();

        //PostUIEvent( UIEventType.Ron_Agari );
        //EndKyoku();
    }
    protected void _HandleRonScore()
    {
        int score = 0;

        // cache the agari update info.
        AgariUpdateInfo aupdateInfo = GetBasicAgariInfo(AgariInfo);

        aupdateInfo.agariPlayer = m_activePlayer;
        aupdateInfo.agariPlayerIsOya = getPlayerIndex( ActivePlayer.JiKaze ) == m_oyaIndex;
        aupdateInfo.isTsumo = false;
        aupdateInfo.agariHai = m_suteHai;

        //aupdateInfo.allOmoteDoraHais = Yama.getAllOmoteDoraHais();
        //aupdateInfo.allUraDoraHais = Yama.getAllUraDoraHais();
        //aupdateInfo.openedOmoteDoraCount = AgariParam.getOmoteDoraHais().Length;
        //if( m_activePlayer.IsReach )
        //    aupdateInfo.openedUraDoraCount = aupdateInfo.openedOmoteDoraCount;


        if( getPlayerIndex( m_kazeFrom ) == m_oyaIndex ) {
            score = m_agariInfo.scoreInfo.oyaAgari + (m_honba * 300);
        }
        else {
            score = m_agariInfo.scoreInfo.koAgari + (m_honba * 300);
        }

        // lost player
        getPlayer( m_kazeFrom ).reduceTenbou( score );

        PlayerTenbouChangeInfo ptci_lost = new PlayerTenbouChangeInfo();
        ptci_lost.playerKaze = m_kazeFrom;
        ptci_lost.changed = -score;
        ptci_lost.current = getPlayer( m_kazeFrom ).Tenbou; // use the reduced tenbou.
        aupdateInfo.tenbouChangeInfoList.Add( ptci_lost );

        // current win player
        m_activePlayer.increaseTenbou( score );

        PlayerTenbouChangeInfo ptci_win = new PlayerTenbouChangeInfo();
        ptci_win.playerKaze = m_activePlayer.JiKaze;
        ptci_win.changed = score;
        ptci_win.current = m_activePlayer.Tenbou; // use the added tenbou.
        aupdateInfo.tenbouChangeInfoList.Add( ptci_win );

        // other players
        Player player;
        for(int i = 0; i < m_playerList.Count; i++)
        {
            player = m_playerList[i];

            if( player.JiKaze == m_kazeFrom || player.JiKaze == ActivePlayer.JiKaze )
                continue;

            PlayerTenbouChangeInfo ptci_other = new PlayerTenbouChangeInfo();
            ptci_other.playerKaze = player.JiKaze;
            ptci_other.changed = 0;
            ptci_other.current = player.Tenbou;
            aupdateInfo.tenbouChangeInfoList.Add( ptci_other );
        }

        // update final agari score
        m_agariInfo.agariScore = score - (m_honba * 300);

        aupdateInfo.agariScore = m_agariInfo.agariScore;
        aupdateInfo.reachBou = m_reachBou;

        AgariUpdateInfoList.Add( aupdateInfo );


        /// reach bou point won't be contained to PlayerTenbouChangeInfo 
        /// 
        /// Note: only the first ron player can get the reach bou,
        ///       make sure the first ron player is the nearest one to m_kazeFrom player.

        // リーチ棒の点数を清算する
        m_activePlayer.increaseTenbou( m_reachBou * GameSettings.Reach_Cost );
        m_reachBou = 0;

        // add the reach bou point.
        ptci_win.changed += aupdateInfo.reachBou * GameSettings.Reach_Cost;
        ptci_win.current = m_activePlayer.Tenbou;
    }

    // some one has tsumo.
    protected void OnTsumo()
    {
        this.RyuuKyokuReason = ERyuuKyokuReason.None;

        //AgariParam.ResetDoraHais(); // should reset params or create a new.

        //AgariParam.setOmoteDoraHais( getOpenedOmotoDoras() );
        //if( m_activePlayer.IsReach )
        //    AgariParam.setUraDoraHais( getOpenedUraDoraHais() );

        GetAgariScore(ActivePlayer.Tehai, TsumoHai, ActivePlayer.JiKaze, AgariParam);

        //Utils.Log( AgariInfo.ToString() );

        _HandleTsumoScore();

        //PostUIEvent( UIEventType.Tsumo_Agari, m_kazeFrom, m_kazeFrom );
        //EndKyoku();
    }
    protected void _HandleTsumoScore()
    {
        this.RyuuKyokuReason = ERyuuKyokuReason.None; // for nagashimangan

        int score = 0;

        // cache the agari update info.
        AgariUpdateInfo aupdateInfo = GetBasicAgariInfo(AgariInfo);

        aupdateInfo.agariPlayer = m_activePlayer;
        aupdateInfo.isTsumo = true;
        aupdateInfo.agariHai = m_tsumoHai;

        //aupdateInfo.allOmoteDoraHais = Yama.getAllOmoteDoraHais();
        //aupdateInfo.allUraDoraHais = Yama.getAllUraDoraHais();
        //aupdateInfo.openedOmoteDoraCount = AgariParam.getOmoteDoraHais().Length;
        //if( m_activePlayer.IsReach )
        //    aupdateInfo.openedUraDoraCount = aupdateInfo.openedOmoteDoraCount;


        int playerIndex = getPlayerIndex( m_kazeFrom );
        if( playerIndex == m_oyaIndex ) 
        {
            aupdateInfo.agariPlayerIsOya = true;

            score = m_agariInfo.scoreInfo.oyaAgari + (m_honba * 300);

            Player player;
            for( int i = 0; i < GameSettings.PlayerCount-1; i++ )
            {
                playerIndex = (playerIndex + 1) % GameSettings.PlayerCount;
                player = m_playerList[playerIndex];

                int reduceTenbou = m_agariInfo.scoreInfo.oyaTsumoKoPay + (m_honba * 100);

                player.reduceTenbou( reduceTenbou );

                // every player's tenbou change info
                PlayerTenbouChangeInfo ptci = new PlayerTenbouChangeInfo();
                ptci.playerKaze = player.JiKaze;
                ptci.changed = -reduceTenbou;
                ptci.current = player.Tenbou; // use the reduced tenbou.
                aupdateInfo.tenbouChangeInfoList.Add( ptci );
            }
        }
        else
        {
            aupdateInfo.agariPlayerIsOya = false;

            score = m_agariInfo.scoreInfo.koAgari + (m_honba * 300);

            Player player;
            for( int i = 0; i < GameSettings.PlayerCount-1; i++ )
            {
                playerIndex = (playerIndex + 1) % GameSettings.PlayerCount;
                player = m_playerList[playerIndex];

                int reduceTenbou = 0;

                if( m_oyaIndex == playerIndex ) {
                    reduceTenbou = m_agariInfo.scoreInfo.koTsumoOyaPay + (m_honba * 100);
                }
                else {
                    reduceTenbou = m_agariInfo.scoreInfo.koTsumoKoPay + (m_honba * 100);
                }

                player.reduceTenbou( reduceTenbou );

                // every player's tenbou change info
                PlayerTenbouChangeInfo ptci = new PlayerTenbouChangeInfo();
                ptci.playerKaze = player.JiKaze;
                ptci.changed = -reduceTenbou;
                ptci.current = player.Tenbou; // use the reduced tenbou.
                aupdateInfo.tenbouChangeInfoList.Add( ptci );
            }
        }

        m_activePlayer.increaseTenbou( score ); //1. add Tsumo score.

        // active player's tenbou change info
        PlayerTenbouChangeInfo ptci_win = new PlayerTenbouChangeInfo();
        ptci_win.playerKaze = m_activePlayer.JiKaze;
        ptci_win.changed = score;
        ptci_win.current = m_activePlayer.Tenbou; // use the added tenbou.
        aupdateInfo.tenbouChangeInfoList.Add( ptci_win );

        // update final agari score
        m_agariInfo.agariScore = score - (m_honba * 300);

        aupdateInfo.agariScore = m_agariInfo.agariScore;
        aupdateInfo.reachBou = m_reachBou;

        AgariUpdateInfoList.Add( aupdateInfo );


        /// reach bou point won't be contained to PlayerTenbouChangeInfo 
        // リーチ棒の点数を清算する
        m_activePlayer.increaseTenbou( m_reachBou * GameSettings.Reach_Cost );
        m_reachBou = 0;

        // add the reach bou point.
        ptci_win.changed += aupdateInfo.reachBou * GameSettings.Reach_Cost;
        ptci_win.current = m_activePlayer.Tenbou;
    }


    public bool EndKyoku()
    {
        // 親を更新する
        if( (isTsumo && getPlayerIndex(m_kazeFrom) == m_oyaIndex) ||
           (!isTsumo && getPlayerIndex(m_ronPlayers[0]) == m_oyaIndex) )
        {
            m_renchan = true;
            m_honba++;
        }
        else{
            if( IsLastKyoku() ){
                return false;
            }
            else{
                GoToNextKyoku();

                SetNextOya();
                m_honba = 0;
            }
        }

        return true;
    }

    public bool EndRyuuKyoku()
    {
        // 親を更新する
        if( getPlayer(m_oyaIndex).isTenpai() )
        {
            m_renchan = true;
            m_honba++;
        }
        else{
            if( IsLastKyoku() ){
                return false;
            }
            else{
                GoToNextKyoku();

                SetNextOya();
                m_honba = 0;
            }
        }

        return true;
    }
    #endregion



    #region Test Method

    protected void StartTest()
    {
        int[] haiIds = getTestHaiIds();

        // remove all the hais of Oya player.
        for( int i = 0; i < m_playerList.Count; i++ )
        {
            //if( i == m_oyaIndex )
            {
                while( m_playerList[i].Tehai.getJyunTehaiCount() > 0 )
                    m_playerList[i].Tehai.removeJyunTehaiAt(0);

                //int[][] arr = getTestHaiArr();
                //for( int a = 0; a < arr[i].Length; a++ )
                //    haiIds[a] = arr[i][a];

                // add the test hais.
                for( int j = 0; j < haiIds.Length - 1; j++ )
                    m_playerList[i].Tehai.addJyunTehai( new Hai(haiIds[j]) );

                m_playerList[i].Tehai.Sort();
            }
        }
    }


    //protected bool debugMode = false;

    protected Hai getTestRinshanHai()
    {
        //return new Hai(18);
        return m_tsumoHai;
    }
    protected Hai getTestPickHai()
    {
        if( getTsumoRemainCount() <= 0 )
            return m_tsumoHai;

        //if(m_suteHaiList.Count == 3) return new Hai( 32 );

        return new Hai( Utils.GetRandomNum(0,12) );

        //return Utils.GetRandomNum(0,3) < 2? new Hai(27) : m_tsumoHai;
    }

    protected int[][] getTestHaiArr()
    {
        int[][] haiArr = new int[4][]
        {
            //new int[14]{0, 1, 2, 10, 11, 12, 13, 14, 15, 31, 31, 33, 33, 33}, //普通牌.
            //new int[14]{0, 1, 2, 10, 11, 12, 13, 14, 15, 31, 31, 33, 33, 33}, //普通牌.
            //new int[14]{0, 1, 2, 10, 11, 12, 13, 14, 15, 31, 31, 33, 33, 33}, //普通牌.
            //new int[14]{0, 1, 2, 9, 10, 11, 18, 19, 20, 33, 33, 33, 27, 27},  //三色同顺 混全.

            new int[14]{0, 0, 0, 4, 4, 4, 8, 8, 8, 1, 5, 8, 27, 27},       //三色同刻 三暗刻.
            new int[14]{1, 1, 1, 5, 5, 5, 9, 9, 9, 1, 5, 8, 27, 27},       //三色同刻 三暗刻.
            new int[14]{2, 2, 2, 6, 6, 6, 10, 10, 10, 1, 5, 8, 27, 27},       //三色同刻 三暗刻.
            new int[14]{3, 3, 3, 7, 7, 7, 11, 11, 11, 1, 5, 8, 27, 27},       //三色同刻 三暗刻.
        };
        return haiArr;
    }

    protected int[] getTestHaiIds() 
    {
        //int[] haiIds = {0,0,0, 0,0,0, 0,0,0, 0,0,0, 0,0}; //测试副露.
        //int[] haiIds = {0, 0, 0, 0, 1, 1, 1, 2, 3, 4, 5, 8, 8, 8};               //暗槓，加槓，大明槓
        //int[] haiIds = {0, 1, 2, 10, 11, 12, 13, 14, 15, 31, 31, 33, 33, 33};    //普通牌.
        //int[] haiIds = {0, 1, 2, 3, 4, 5, 6, 7, 8, 8, 8, 27, 27, 30};            //一气通贯.
        //int[] haiIds = {0, 1, 2, 9, 10, 11, 18, 19, 20, 33, 33, 33, 27, 27};     //三色同顺 混全.
        //int[] haiIds = {0, 0, 0, 9, 9, 9, 18, 18, 18, 1, 2, 3, 27, 27};          //三色同刻 三暗刻.
        //int[] haiIds = {0, 0, 0, 0, 8, 8, 8, 8, 9, 9, 9, 9, 18, 18};             //三槓.
        //int[] haiIds = {31, 31, 31, 32, 32, 32, 33, 33, 27, 27, 27, 6, 7, 8};    //小三元.
        //int[] haiIds = {0, 0, 1, 2, 3, 4, 4, 6, 27, 27, 27, 31, 31, 31};         //混一色.
        //int[] haiIds = {0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 8, 8};               //清一色 纯全 二杯口(一色四连顺).
        //int[] haiIds = {0, 0, 1, 1, 2, 2, 6, 6, 7, 7, 8, 8, 8, 8};               //清一色 纯全 二杯口.
        //int[] haiIds = {1, 1, 3, 3, 5, 5, 7, 7, 30, 30, 31, 31, 32, 32};         //七对子.

        int[] haiIds = {10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 15, 16, 16}; //连七对(大车轮).
        //int[] haiIds = {0, 0, 8, 8, 29, 29, 30, 30, 31, 31, 32, 32, 33, 33};     //混老头 七对子.
        //int[] haiIds = {27, 27, 28, 28, 29, 29, 30, 30, 31, 31, 32, 32, 33, 33}; //字一色 七对子.
        //int[] haiIds = {19, 19, 20, 20, 21, 21, 23, 23, 23, 23, 25, 25, 25, 25}; //绿一色.
        //int[] haiIds = {0, 0, 0, 8, 8, 8, 9, 9, 9, 17, 17, 17, 18, 18};          //清老头.
        //int[] haiIds = {29, 29, 30, 30, 30, 31, 31, 31, 32, 32, 32, 33, 33, 33}; //字一色 大三元
        //int[] haiIds = {27, 27, 27, 28, 28, 28, 29, 29, 29, 30, 30, 31, 31, 31}; //字一色 小四喜.
        //int[] haiIds = {27, 27, 27, 28, 28, 28, 29, 29, 29, 30, 30, 30, 31, 31}; //字一色 大四喜.
        //int[] haiIds = {0, 0, 0, 9, 9, 9, 18, 18, 18, 27, 27, 28, 28, 28};       //四暗刻.
        //int[] haiIds = {0, 8, 9, 17, 18, 26, 27, 28, 29, 30, 31, 33, 33, 33};    //国士无双.
        //int[] haiIds = {0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 8, 8, 8, 28};              //九连宝灯.

        //int[] haiIds = {0, 0, 0, 2, 2, 2, 3, 3, 3, 4, 4, 4, 10, 10};             //四暗刻单骑.
        //int[] haiIds = {0, 8, 9, 17, 18, 26, 27, 28, 29, 30, 31, 32, 33, 33};    //国士无双十三面.
        //int[] haiIds = {0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 8, 8, 8};               //纯正九连宝灯.

        return haiIds;
    }
    #endregion

}
