using UnityEngine;
/// <summary>
/// 山を管理する, 所有堆起来的牌叫山牌
/// 最后的7幢牌，共14张，叫王牌
/// </summary>

public class Yama 
{
    // 山牌の配列の最大数
    public readonly static int YAMA_HAIS_MAX = 136;

    // 自摸牌の配列の最大数
    public readonly static int TSUMO_HAIS_MAX = 122;

    // 杠上开花牌の配列の最大数
    public readonly static int RINSHAN_HAIS_MAX = 4;

    // 表と裏ドラの配列の最大数
    public readonly static int DORA_HAIS_MAX = 5; //x2


    // 山牌の配列
    private Hai[] _yamaHais = new Hai[YAMA_HAIS_MAX];


    // ツモ牌の配列
    private int[] TsumoHaiIndex_InYama = new int[TSUMO_HAIS_MAX];

    // リンシャン牌の配列
    private int[] RinshanHaiIndex_InYama = new int[RINSHAN_HAIS_MAX];

    // 表ドラ牌の配列
    private int[] OmoteDoraHaiIndex_InYama = new int[DORA_HAIS_MAX];

    // 裏ドラ牌の配列
    private int[] UraDoraHaiIndex_InYama = new int[DORA_HAIS_MAX];


    // ツモ牌のインデックス(index)
    private int _tsumoHaisIndex = 0;

    // リンシャン牌の位置
    private int _rinshanHaisIndex = 0;


    public Yama()
    {
        for(int id = Hai.ID_MIN; id <= Hai.ID_MAX; id++)
        {
            for( int n = 0; n < 4; n++ )
                _yamaHais[(id * 4) + n] = new Hai(id);
        }

        setTsumoHaisStartIndex(0);
    }


    // temply implement.
    public Hai[] getYamaHais()
    {
        return _yamaHais;
    }

    // ツモ牌の残り数を取得する
    public int getTsumoNokori()
    {
        return TSUMO_HAIS_MAX - _rinshanHaisIndex - _tsumoHaisIndex;
    }

    // リンシャン(岭上开花)牌の残り数を取得する
    public int getRinshanNokori()
    {
        return RINSHAN_HAIS_MAX - _rinshanHaisIndex;
    }


    public int getTsumoHaiIndex()
    {
        return _tsumoHaisIndex;
    }

    public int getPreTsumoHaiIndex()
    {
		int idx = -1;
		if(TsumoHaiIndex_InYama.Length>_tsumoHaisIndex-2) {
			idx = TsumoHaiIndex_InYama[_tsumoHaisIndex-2];
		} else {
			Debug.LogError("TsumoHaiIndex_InYama.Length="+TsumoHaiIndex_InYama.Length+"< _tsumoHaisIndex="+_tsumoHaisIndex);
		}
		return idx;
    }

    public int getPreRinshanHaiIndex()
    {
        if(_rinshanHaisIndex == 0) return -1;

        return RinshanHaiIndex_InYama[_rinshanHaisIndex-1];
    }

    public int getLastOmoteHaiIndex()
    {
        return OmoteDoraHaiIndex_InYama[_rinshanHaisIndex];
    }
    public int getOpenedOmeteDoraCount()
    {
        return _rinshanHaisIndex + 1;
    }


    // 洗牌する
    public void Shuffle()
    {
        Hai temp;
        int j;

        for (int i = 0; i < YAMA_HAIS_MAX; i++) 
        {
            // get a random index.
            j = Utils.GetRandomNum(0, YAMA_HAIS_MAX);

            // exchange hais.
            temp = _yamaHais[i];
            _yamaHais[i] = _yamaHais[j];
            _yamaHais[j] = temp;
        }
    }

    /// <summary>
    /// 初始拿牌.
    /// </summary>
    public Hai[] PickHaipai()
    {
        Hai[] hais = new Hai[4];
        for( int i = 0; i < hais.Length; i++ ) 
        {
            Hai haiInYama = _yamaHais[ TsumoHaiIndex_InYama[_tsumoHaisIndex] ];
            hais[i] = new Hai( haiInYama );

            _tsumoHaisIndex++;
        }

        return hais;
    }


    // ツモ牌を取得する
    public Hai PickTsumoHai()
    {
        if( getTsumoNokori() <= 0)
            return null;

        Hai haiInYama = _yamaHais[ TsumoHaiIndex_InYama[_tsumoHaisIndex] ];
        Hai tsumoHai = new Hai( haiInYama );

        _tsumoHaisIndex++;

        return tsumoHai;
    }

    // リンシャン(岭上开花)牌を取得する
    public Hai PickRinshanHai()
    {
        if( getRinshanNokori() <= 0 )
            return null;

        Hai haiInYama = _yamaHais[ RinshanHaiIndex_InYama[_rinshanHaisIndex] ];
        Hai rinshanHai = new Hai( haiInYama );

        _rinshanHaisIndex++;

        return rinshanHai;
    }


    // 表ドラの配列を取得する
    public Hai[] getOpenedOmoteDoraHais()
    {
        int omoteDoraHaisCount = _rinshanHaisIndex + 1;
        Hai[] omoteDoraHais = new Hai[omoteDoraHaisCount];

        for( int i = 0; i < omoteDoraHais.Length; i++ ){
            Hai haiInYama = _yamaHais[ OmoteDoraHaiIndex_InYama[i] ];
            omoteDoraHais[i] = new Hai( haiInYama );
        }
        return omoteDoraHais;
    }
    public Hai[] getAllOmoteDoraHais()
    {
        int length = OmoteDoraHaiIndex_InYama.Length;
        Hai[] allHais = new Hai[length];

        for( int i = 0; i < length; i++ )
        {
            Hai haiInYama = _yamaHais[ OmoteDoraHaiIndex_InYama[i] ];
            allHais[i] = new Hai( haiInYama );
        }

        return allHais;
    }


    // 裏ドラの配列を取得する
    public Hai[] getOpenedUraDoraHais()
    {
        int uraDoraHaisCount = _rinshanHaisIndex + 1;
        Hai[] uraDoraHais = new Hai[uraDoraHaisCount];

        for( int i = 0; i < uraDoraHais.Length; i++ ){
            Hai haiInYama = _yamaHais[ UraDoraHaiIndex_InYama[i] ];
            uraDoraHais[i] = new Hai( haiInYama );
        }

        return uraDoraHais;
    }
    public Hai[] getAllUraDoraHais()
    {
        int length = UraDoraHaiIndex_InYama.Length;
        Hai[] allHais = new Hai[length];

        for( int i = 0; i < length; i++ )
        {
            Hai haiInYama = _yamaHais[ UraDoraHaiIndex_InYama[i] ];
            allHais[i] = new Hai( haiInYama );
        }

        return allHais;
    }


    /**
     * ツモ牌の開始位置を設定する。
     *
     *  the correct array is like:
     *  
     *  Tsumo Start <--| Wareme |<-- Rinshan 2x2 <-- Doras 2x5 |<-- Tsumo End <--.
     */

    public bool setTsumoHaisStartIndex(int tsumoHaiStartIndex)
    {
        if( tsumoHaiStartIndex >= YAMA_HAIS_MAX )
            return false;

        int yamaHaisIndex = tsumoHaiStartIndex;


        // tsumo hais. 122.
        for(int i = 0; i < TSUMO_HAIS_MAX; i++)
        {
            TsumoHaiIndex_InYama[i] = yamaHaisIndex;

            yamaHaisIndex++;

            if( yamaHaisIndex >= YAMA_HAIS_MAX )
                yamaHaisIndex = 0;
        }
        _tsumoHaisIndex = 0;


        // dora hais. 1+4=5. 
        for( int i = DORA_HAIS_MAX - 1; i >= 0; i-- ) // reverse.
        {
            // 表dora.
            OmoteDoraHaiIndex_InYama[i] = yamaHaisIndex;

            yamaHaisIndex++;
            if( yamaHaisIndex >= YAMA_HAIS_MAX )
                yamaHaisIndex = 0;

            // 里dora.
            UraDoraHaiIndex_InYama[i] = yamaHaisIndex;

            yamaHaisIndex++;
            if( yamaHaisIndex >= YAMA_HAIS_MAX )
                yamaHaisIndex = 0;
        }

        // rinshan hais. 4.
        for( int i = 0; i < RINSHAN_HAIS_MAX; i++ )
        {
            RinshanHaiIndex_InYama[i] = yamaHaisIndex;

            yamaHaisIndex++;
            if( yamaHaisIndex >= YAMA_HAIS_MAX )
                yamaHaisIndex = 0;
        }
        _rinshanHaisIndex = 0;


        /// reverse rinshan hai index.
        ///   2 0  ->  0 2
        ///   3 1      1 3
        int tempIndex = RinshanHaiIndex_InYama[0];
        RinshanHaiIndex_InYama[0] = RinshanHaiIndex_InYama[2];
        RinshanHaiIndex_InYama[2] = tempIndex;

        tempIndex = RinshanHaiIndex_InYama[1];
        RinshanHaiIndex_InYama[1] = RinshanHaiIndex_InYama[3];
        RinshanHaiIndex_InYama[3] = tempIndex;

        return true;
    }


    // 赤ドラ牌
    public void setRedDora(int id, int num)
    {
        if(num <= 0) return;

        for(int i = 0; i < _yamaHais.Length; i++) 
        {
            if( _yamaHais[i].ID == id )
            {
                _yamaHais[i].IsRed = true;

                num--;
                if(num <= 0) break;
            }
        } 
    }

    public void resetRedDoras()
    {
        for(int i = 0; i < _yamaHais.Length; i++) 
            _yamaHais[i].IsRed = false;
    }

}