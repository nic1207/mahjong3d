using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent (typeof (PhotonView))]
public abstract class Player : MonoBehaviour
{
    protected string _name;
    protected EVoiceType _voiceType = EVoiceType.W_B;
    protected int _playerOrder; //順序 0玩家 1下 2對 3上 

    //public Player(string name)
    //{
    //    this._name = name;
    //}
    //public Player(string name, EVoiceType voiceType)
    //{
    //    this._name = name;
    //    this._voiceType = voiceType;
    //}

    public string Name
    {
		set{ this._name = value;}
        get{ return _name; }
    }
    public int Order
    {
        get { return _playerOrder; }
        set
        {
            this._playerOrder = value;
        }
    }
    public EVoiceType VoiceType
    {
		set{ this._voiceType = value;}
        get{ return _voiceType; }
    }

    protected PlayerAction _action = new PlayerAction();
    public PlayerAction Action
    {
        get{ return _action; }
    }


    // 手牌
    protected Tehai _tehai = new Tehai();
    public Tehai Tehai
    {
        get{ return _tehai; }
    }

    // 河
    protected Hou _hou = new Hou();
    public Hou Hou
    {
        get{ return _hou; }
    }

    // 自風
    protected EKaze _jikaze;
    public EKaze JiKaze
    {
        get{ return _jikaze; }
        set{ _jikaze = value; }
    }

    // 点棒
    protected int _tenbou;
    public int Tenbou 
    {
        get{ return _tenbou; }
        set{ _tenbou = value; }
    }

    // リーチ(can't set by self!!!)
    protected bool _reach;
    public bool IsReach
    {
        get{ return _reach; }
        set{ _reach = value; }
    }

	/*
    // ダブルリーチ
    protected bool _doubleReach;
    public bool IsDoubleReach
    {
        get{ return _doubleReach; }
        set{ _doubleReach = value; }
    }
	*/
	/*
    // 一発
    protected bool _ippatsu;
    public bool IsIppatsu
    {
        get{ return _ippatsu; }
        set{ _ippatsu = value; }
    }
	*/
    // 捨牌数
    protected int _suteHaisCount;
    public int SuteHaisCount
    {
        get{ return _suteHaisCount; }
        set{ _suteHaisCount = value; }
    }

    protected CountFormat _countFormat = new CountFormat();
    public CountFormat FormatWorker
    {
        get{ return _countFormat; }
    }

    #region Logic

    public virtual void Init() 
    {
        // 手牌を初期化します。
        _tehai.initialize();

        // 河を初期化します。
        _hou.initialize();

        // リーチを初期化します。
        _reach = false;
        //_doubleReach = false;
        //_ippatsu = false;

        _suteHaisCount = 0;
    }

    // 点棒を増やします
    public void increaseTenbou(int value)
    {
        Tenbou += value;
    }
    // 点棒を減らします
    public void reduceTenbou(int value)
    {
        Tenbou -= value;
    }

    // 听牌
    public bool isTenpai()
    {
        if( _reach == true )
            return true;

        return MahjongAgent.canTenpai( _tehai );
    }

    // 振听.
    public bool isFuriten()
    {
        List<Hai> machiHais;
        if( MahjongAgent.tryGetMachiHais(Tehai, out machiHais) )
        {
            // check hou
            SuteHai[] suteHais = Hou.getSuteHais();

            for( int i = 0; i < suteHais.Length; i++ )
            {
                SuteHai suteHaiTemp = suteHais[i];

                for( int j = 0; j < machiHais.Count; j++ )
                {
                    if( suteHaiTemp.ID == machiHais[j].ID )
                        return true;
                }
            }

            // check sutehai
            suteHais = MahjongAgent.getSuteHaiList();

            int playerSuteHaisCount = MahjongAgent.getPlayerSuteHaisCount(JiKaze);
            for( ; playerSuteHaisCount < suteHais.Length - 1; playerSuteHaisCount++ )
            {
                SuteHai suteHaiTemp = suteHais[playerSuteHaisCount];

                for( int j = 0; j < machiHais.Count; j++ )
                {
                    if( suteHaiTemp.ID == machiHais[j].ID )
                        return true;
                }
            }
        }

        return false;
    }


    public bool CheckReachPreConditions()
    {
        //return !MahjongAgent.isReach(JiKaze) && 
        //    MahjongAgent.getTsumoRemain() >= GameSettings.PlayerCount 
		//	&& Tenbou >= GameSettings.Reach_Cost;
		return !MahjongAgent.isReach(JiKaze) && 
			MahjongAgent.getTsumoRemain() >= GameSettings.PlayerCount;
    }

    #endregion


    protected GameAgent MahjongAgent
    {
        get{ return GameAgent.Instance; }
    }

    protected float ResponseDelayTime = 0.3f; // must > 0

    protected Action<EKaze, EResponse> _onResponse;

    protected ERequest _request;
    protected ERequest CurrentRequest
    {
        get{ return _request; }
    }

    protected EResponse DoResponse(EResponse response)
    {
        _action.Response = response;

        ResponseDelayTime = Math.Max(ResponseDelayTime, 0.1f);

        if( ResponseDelayTime > 0f )
			GameClientManager.Get().StartCoroutine( DoResponseDelay(ResponseDelayTime) );
        else
            DoResponseDirectly();

        return response;
    }

    protected IEnumerator DoResponseDelay(float waitTime)
    {
        yield return new UnityEngine.WaitForSeconds(waitTime);
        DoResponseDirectly();
    }
    protected void DoResponseDirectly()
    {
        if(_onResponse != null) 
            _onResponse.Invoke(JiKaze, _action.Response);
    }

    public void OnPlayerInputFinished()
    {
        //UnityEngine.Debug.Log("~~~OnPlayerInputFinished(): " + _action.Response.ToString());
        DoResponseDirectly();
    }


    public virtual void HandleRequest(ERequest request, EKaze fromPlayerKaze, Hai haiToHandle, Action<EKaze, EResponse> onResponse)
    {
        this._request = request;
        this._onResponse = onResponse;

        switch( request )
        {
            case ERequest.Handle_TsumoHai:
            {
                OnHandle_TsumoHai(fromPlayerKaze, haiToHandle);
            }
            break;
            case ERequest.Handle_KaKanHai:
            {
                OnHandle_KakanHai(fromPlayerKaze, haiToHandle);
            }
            break;
            case ERequest.Handle_SuteHai:
            {
                OnHandle_SuteHai(fromPlayerKaze, haiToHandle);
            }
            break;
            case ERequest.Select_SuteHai:
            {
                OnSelect_SuteHai(fromPlayerKaze, haiToHandle);
            }
            break;
            default:
            {
                DoResponse( EResponse.Nagashi );
            }
            break;
        }
    }

    protected abstract EResponse OnHandle_TsumoHai(EKaze fromPlayerKaze, Hai haiToHandle);
    protected abstract EResponse OnHandle_KakanHai(EKaze fromPlayerKaze, Hai haiToHandle);
    protected abstract EResponse OnHandle_SuteHai(EKaze fromPlayerKaze, Hai haiToHandle);
    protected abstract EResponse OnSelect_SuteHai(EKaze fromPlayerKaze, Hai haiToHandle);

    public abstract bool IsAI { get; }

    protected bool inTest = false;
}
