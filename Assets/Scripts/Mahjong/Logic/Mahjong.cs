using System.Collections.Generic;

/// <summary>
/// 麻将を管理するクラスです
/// </summary>

public abstract class Mahjong
{
    #region Fields.
    // 山
    protected Yama m_yama;
    public Yama Yama
    {
        get{ return m_yama; }
        protected set{ m_yama = value; }
    }

    // 本場
    protected int m_honba;
    public int HonBa
    {
        get{ return m_honba; }
        protected set{ m_honba = value; }
    }

    // 局
    protected int m_kyoku;
    public int Kyoku
    {
        get{
            int ton4 = (int)EKyoku.Ton_4;
            return m_kyoku > ton4? (m_kyoku-ton4) : m_kyoku; 
        }
        protected set{ m_kyoku = value; }
    }

    // リーチ棒の数
    protected int m_reachBou;
    public int ReachBou
    {
        get{ return m_reachBou; }
        protected set{ m_reachBou = value; }
    }

    // 連荘
    protected bool m_renchan;
    public bool isRenChan
    {
        get{ return m_renchan; }
        protected set{ m_renchan = value; }
    }

    // プレイヤーの配列
    protected List<Player> m_playerList;
    public List<Player> PlayerList
    {
        get{ return m_playerList; }
        protected set{ m_playerList = value; }
    }

    // サイコロの配列
    protected Sai[] m_sais;
    public Sai[] Sais
    {
        get{ return m_sais; }
        protected set{ m_sais = value; }
    }

    // 起家のプレイヤーインデックス
    protected int m_chiichaIndex = -1;
    public int ChiiChaIndex
    {
        get{ return m_chiichaIndex; }
        protected set{ m_chiichaIndex = value; }
    }

    // 割れ目
    protected int m_wareme = -1;
    public int Wareme
    {
        get{ return m_wareme; }
        protected set{ m_wareme = value; }
    }

    // 親のプレイヤーインデックス
    protected int m_oyaIndex = -1;
    public int OyaIndex
    {
        get{ return m_oyaIndex; }
        protected set{ m_oyaIndex = value; }
    }

    // 捨牌
    protected List<SuteHai> m_suteHaiList;
    public List<SuteHai> AllSuteHaiList
    {
        get{ return m_suteHaiList; }
        protected set{ m_suteHaiList = value; }
    }

    protected int m_sutehaiIndex = -1;
    public int SuteHaiIndex
    {
        get{ return m_sutehaiIndex; }
        set{ m_sutehaiIndex = value; }
    }


    // イベントを発行した風
    protected EKaze m_kazeFrom;
    public EKaze FromKaze
    {
        get{ return m_kazeFrom; }
        set{ m_kazeFrom = value; }
    }

    // current player
    protected Player m_activePlayer;
    public Player ActivePlayer
    {
        get{ return m_activePlayer; }
        protected set{ m_activePlayer = value; }
    }

    // 摸入牌
    protected Hai m_tsumoHai;
    public Hai TsumoHai
    {
        get{ return m_tsumoHai; }
        set{ m_tsumoHai = value; }
    }

    // 打出牌
    protected Hai m_suteHai;
    public Hai SuteHai
    {
        get{ return m_suteHai; }
        set{ m_suteHai = value; }
    }

    // kakan hai
    protected Hai m_kakanHai;
    public Hai KakanHai
    {
        get{ return m_kakanHai; }
        set{ m_kakanHai = value; }
    }


    protected AgariParam m_agariParam = new AgariParam();
    public AgariParam AgariParam
    {
        get{ return m_agariParam; }
        protected set{ m_agariParam = value; }
    }

    protected HaiCombi[] m_combis = new HaiCombi[10]
    {
        new HaiCombi(),new HaiCombi(),new HaiCombi(),
        new HaiCombi(),new HaiCombi(),new HaiCombi(),
        new HaiCombi(),new HaiCombi(),new HaiCombi(),new HaiCombi(),
    };
    public HaiCombi[] Combis
    {
        get{ return m_combis; }
        protected set{ m_combis = value; }
    }

    protected CountFormat m_countFormat = new CountFormat();
    public CountFormat CountFormater
    {
        get{ return m_countFormat; }
        protected set{ m_countFormat = value; }
    }

    protected AgariInfo m_agariInfo = new AgariInfo();
    public AgariInfo AgariInfo
    {
        get{ return m_agariInfo; }
        protected set{ m_agariInfo = value; }
    }


    protected bool m_isTenhou = false;  //天和
    public bool isTenHou
    {
        get{ return m_isTenhou; }
        set{ m_isTenhou = value; }
    }

    protected bool m_isChihou = false; //地和
    public bool isChiHou
    {
        get{ return m_isChihou; }
        set{ m_isChihou = value; }
    }

    protected bool m_isRenhou = false; //人和
    public bool isRenHou
    {
        get{ return m_isRenhou; }
        set{ m_isRenhou = value; }
    }

    protected bool m_isTsumo = false;
    public bool isTsumo
    {
        get{ return m_isTsumo; }
        set{ m_isTsumo = value; }
    }

    protected bool m_isRinshan = false;
    public bool isRinshan
    {
        get{ return m_isRinshan; }
        set{ m_isRinshan = value; }
    }

    protected bool m_isChanKan = false;
    public bool isChanKan
    {
        get{ return m_isChanKan; }
        set{ m_isChanKan = value; }
    }

    protected bool m_isLast = false;
    public bool isLast
    {
        get{ return m_isLast; }
        set{ m_isLast = value; }
    }

    #endregion Fields.


    // -----------------------virtual methods start---------------------------
    #region virtual methods.

    // get player from index.
    public Player getPlayer( int index )
    { 
        if(index >= 0 && index < m_playerList.Count)
            return m_playerList[index];
        return null;
    }

    // get player from kaze.
    public Player getPlayer( EKaze kaze )
    { 
        return m_playerList.Find( (p) => p.JiKaze == kaze );
    }

    // get player index from kaze.
    public int getPlayerIndex( EKaze kaze )
    { 
        return m_playerList.FindIndex( (p) => p.JiKaze == kaze );
    }

    // 表ドラ、槓ドラの配列を取得する
    //public Hai[] getOpenedOmotoDoras()
    //{
    //    return Yama.getOpenedOmoteDoraHais();
    //}

    // 里ドラ、槓ドラの配列を取得する
    //public Hai[] getOpenedUraDoraHais()
    //{
    //    return Yama.getOpenedUraDoraHais();
    //}

    // ツモの残り数を取得する
    public int getTsumoRemainCount()
    {
        return Yama.getTsumoNokori();
    }

    // 場風を取得する
    public EKaze getBaKaze() 
    {
        return m_kyoku <= (int)EKyoku.Ton_4 ? EKaze.Ton : EKaze.Nan;
    }

    public EKaze getManKaze()
    {
        return m_playerList[0].JiKaze;
    }


    public int GetTotalKanCount( int[] eachPlayerKanCount )
    {
        int totalKan = 0;

        if(eachPlayerKanCount == null)
            eachPlayerKanCount = new int[4];

        for( int i = 0; i < PlayerList.Count; i++ )
        {
            Fuuro[] fuuros = PlayerList[i].Tehai.getFuuros();
            int kanCount = 0;

            for( int j = 0; j < fuuros.Length; j++ )
            {
                switch( fuuros[j].Type )
                {
                    case EFuuroType.AnKan:
                    case EFuuroType.KaKan:
                    case EFuuroType.DaiMinKan:
                    kanCount++;
                    break;
                }
            }

            eachPlayerKanCount[i] = kanCount;

            totalKan += kanCount;
        }

        return totalKan;
    }

    public int GetAgariScore(Tehai tehai, Hai addHai, EKaze jikaze, AgariParam param = null)
    {
        if(param == null) {
            param = AgariParam;
            //param.ResetDoraHais();
        }

        param.setBakaze( getBaKaze() );
        param.setJikaze( jikaze );

        param.ResetYakuFlags(); // should reset params or create a new.

        if( m_activePlayer.IsReach ) {
            //if( m_activePlayer.IsDoubleReach ) {
            //    param.setYakuFlag(EYakuFlagType.DOUBLE_REACH, true);
            //}
            //else {
                param.setYakuFlag(EYakuFlagType.REACH, true);
            //}
        }

        if( m_isTsumo ) {
            param.setYakuFlag(EYakuFlagType.TSUMO, true);

            if( m_isTenhou ) {
                param.setYakuFlag(EYakuFlagType.TENHOU, true);
            }
            else if( m_isChihou ) {
                param.setYakuFlag(EYakuFlagType.TIHOU, true);
            }
        }
        else if( m_isRenhou ){
            if( getPlayerIndex( jikaze ) != OyaIndex ) // only ko can Renhou.
                param.setYakuFlag(EYakuFlagType.RENHOU, true);
        }

        if( m_isTsumo && m_isRinshan ) {
            param.setYakuFlag(EYakuFlagType.RINSYAN, true);
        }

        if( m_isChanKan ){
            param.setYakuFlag(EYakuFlagType.CHANKAN, true);
        }

        if( m_isLast ) {
            if( m_isTsumo ) {
                param.setYakuFlag(EYakuFlagType.HAITEI, true);
            }
            else {
                param.setYakuFlag(EYakuFlagType.HOUTEI, true);
            }
        }

        //if( m_activePlayer.IsIppatsu ) {
        //    param.setYakuFlag(EYakuFlagType.IPPATU, true);
        //}

        if( GameSettings.UseKuitan ) {
            param.setYakuFlag(EYakuFlagType.KUITAN, true);
        }

        return AgariScoreManager.GetAgariScore(tehai, addHai, param, ref m_combis, ref m_agariInfo);
    }

    #endregion virtual methods.


    // -----------------------static methods start---------------------------
    public static int getRelation(EKaze otherKaze, EKaze selfKaze)
    {
        int other = (int)otherKaze;
        int self = (int)selfKaze;

        ERelation relation;

        if( self == other ) {
            relation = ERelation.JiBun; //自家
        }
        else if( (self + 1) % 4 == other ) {
            relation = ERelation.ShiMoCha; //下家.
			//relation = ERelation.KaMiCha; //上家.
        }
        else if( (self + 2) % 4 == other ) {
            relation = ERelation.ToiMen;  //对家
        }
        else //if( (self + 3) % 4 == other )
        {
            relation = ERelation.KaMiCha; //上家.
			//relation = ERelation.ShiMoCha; //下家.
        }

        return (int)relation;
    }


    public Mahjong()
    {
        initialize();
    }

    // abstract methods.
    protected abstract void initialize();

}

