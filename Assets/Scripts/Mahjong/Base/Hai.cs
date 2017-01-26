
/// <summary>
/// 牌
/// </summary>

public class Hai 
{
    #region Const
    // 一萬 
    public readonly static int ID_WAN_1 = 0;
    // 二萬 
    public readonly static int ID_WAN_2 = 1;
    // 三萬 
    public readonly static int ID_WAN_3 = 2;
    // 四萬 
    public readonly static int ID_WAN_4 = 3;
    // 五萬 
    public readonly static int ID_WAN_5 = 4;
    // 六萬 
    public readonly static int ID_WAN_6 = 5;
    // 七萬 
    public readonly static int ID_WAN_7 = 6;
    // 八萬 
    public readonly static int ID_WAN_8 = 7;
    // 九萬 
    public readonly static int ID_WAN_9 = 8;

    // 一筒 
    public readonly static int ID_PIN_1 = 9;
    // 二筒 
    public readonly static int ID_PIN_2 = 10;
    // 三筒 
    public readonly static int ID_PIN_3 = 11;
    // 四筒 
    public readonly static int ID_PIN_4 = 12;
    // 五筒 
    public readonly static int ID_PIN_5 = 13;
    // 六筒 
    public readonly static int ID_PIN_6 = 14;
    // 七筒 
    public readonly static int ID_PIN_7 = 15;
    // 八筒 
    public readonly static int ID_PIN_8 = 16;
    // 九筒 
    public readonly static int ID_PIN_9 = 17;

    // 一索 
    public readonly static int ID_SOU_1 = 18;
    // 二索 
    public readonly static int ID_SOU_2 = 19;
    // 三索 
    public readonly static int ID_SOU_3 = 20;
    // 四索 
    public readonly static int ID_SOU_4 = 21;
    // 五索 
    public readonly static int ID_SOU_5 = 22;
    // 六索 
    public readonly static int ID_SOU_6 = 23;
    // 七索 
    public readonly static int ID_SOU_7 = 24;
    // 八索 
    public readonly static int ID_SOU_8 = 25;
    // 九索 
    public readonly static int ID_SOU_9 = 26;

    // 東 
    public readonly static int ID_TON = 27;
    // 南 
    public readonly static int ID_NAN = 28;
    // 西 
    public readonly static int ID_SYA = 29;
    // 北 
    public readonly static int ID_PE = 30;

    // 白 
    public readonly static int ID_HAKU = 31;
    // 發 
    public readonly static int ID_HATSU = 32;
    // 中 
    public readonly static int ID_CHUN = 33;

    // IDの最小値 
    public readonly static int ID_MIN = ID_WAN_1;
    // IDの最大値 
    public readonly static int ID_MAX = ID_CHUN;

    // 1 
    public readonly static int NUM_1 = 1;
    // 2 
    public readonly static int NUM_2 = 2;
    // 3 
    public readonly static int NUM_3 = 3;
    // 4 
    public readonly static int NUM_4 = 4;
    // 5 
    public readonly static int NUM_5 = 5;
    // 6 
    public readonly static int NUM_6 = 6;
    // 7 
    public readonly static int NUM_7 = 7;
    // 8 
    public readonly static int NUM_8 = 8;
    // 9 
    public readonly static int NUM_9 = 9;

    // 一萬 
    public readonly static int NUM_WAN_1 = 1;
    // 二萬 
    public readonly static int NUM_WAN_2 = 2;
    // 三萬 
    public readonly static int NUM_WAN_3 = 3;
    // 四萬 
    public readonly static int NUM_WAN_4 = 4;
    // 五萬 
    public readonly static int NUM_WAN_5 = 5;
    // 六萬 
    public readonly static int NUM_WAN_6 = 6;
    // 七萬 
    public readonly static int NUM_WAN_7 = 7;
    // 八萬 
    public readonly static int NUM_WAN_8 = 8;
    // 九萬 
    public readonly static int NUM_WAN_9 = 9;

    // 一筒 
    public readonly static int NUM_PIN_1 = 1;
    // 二筒 
    public readonly static int NUN_PIN_2 = 2;
    // 三筒 
    public readonly static int NUM_PIN_3 = 3;
    // 四筒 
    public readonly static int NUM_PIN_4 = 4;
    // 五筒 
    public readonly static int NUM_PIN_5 = 5;
    // 六筒 
    public readonly static int NUM_PIN_6 = 6;
    // 七筒 
    public readonly static int NUM_PIN_7 = 7;
    // 八筒 
    public readonly static int NUM_PIN_8 = 8;
    // 九筒 
    public readonly static int NUM_PIN_9 = 9;

    // 一索 
    public readonly static int NUM_SOU_1 = 1;
    // 二索 
    public readonly static int NUM_SOU_2 = 2;
    // 三索 
    public readonly static int NUM_SOU_3 = 3;
    // 四索 
    public readonly static int NUM_SOU_4 = 4;
    // 五索 
    public readonly static int NUM_SOU_5 = 5;
    // 六索 
    public readonly static int NUM_SOU_6 = 6;
    // 七索 
    public readonly static int NUM_SOU_7 = 7;
    // 八索 
    public readonly static int NUM_SOU_8 = 8;
    // 九索 
    public readonly static int NUM_SOU_9 = 9;

    // 東 
    public readonly static int NUM_TON = 1;
    // 南 
    public readonly static int NUM_NAN = 2;
    // 西 
    public readonly static int NUM_SHA = 3;
    // 北 
    public readonly static int NUM_PE = 4;

    // 白 
    public readonly static int NUM_HAKU = 1;
    // 發 
    public readonly static int NUM_HATSU = 2;
    // 中 
    public readonly static int NUM_CHUN = 3;


    // 萬子 
    public readonly static int KIND_WAN = 0x00000010;
    // 筒子 
    public readonly static int KIND_PIN = 0x00000020;
    // 索子 
    public readonly static int KIND_SOU = 0x00000040;
    // 風牌 
    public readonly static int KIND_FON = 0x00000100;
    // 三元牌 
    public readonly static int KIND_SANGEN = 0x00000200;

    // 数牌 
    public readonly static int KIND_SHUU = KIND_WAN | KIND_PIN | KIND_SOU;
    // 字牌 
    public readonly static int KIND_TSUU = KIND_FON | KIND_SANGEN;

    // 番号の配列 
    private readonly static int[] NUMS = 
    {
        // 萬子
        NUM_WAN_1, NUM_WAN_2, NUM_WAN_3, NUM_WAN_4, NUM_WAN_5, NUM_WAN_6, NUM_WAN_7, NUM_WAN_8, NUM_WAN_9,
        // 筒子
        NUM_PIN_1, NUN_PIN_2, NUM_PIN_3, NUM_PIN_4, NUM_PIN_5, NUM_PIN_6, NUM_PIN_7, NUM_PIN_8, NUM_PIN_9,
        // 索子
        NUM_SOU_1, NUM_SOU_2, NUM_SOU_3, NUM_SOU_4, NUM_SOU_5, NUM_SOU_6, NUM_SOU_7, NUM_SOU_8, NUM_SOU_9,
        // 風牌
        NUM_TON, NUM_NAN, NUM_SHA, NUM_PE,
        // 三元牌
        NUM_HAKU, NUM_HATSU, NUM_CHUN 
    };

    // 種類の配列 
    private readonly static int[] KINDS = 
    {
        // 萬子
        KIND_WAN, KIND_WAN, KIND_WAN, KIND_WAN, KIND_WAN, KIND_WAN, KIND_WAN, KIND_WAN, KIND_WAN,
        // 筒子
        KIND_PIN, KIND_PIN, KIND_PIN, KIND_PIN, KIND_PIN, KIND_PIN, KIND_PIN, KIND_PIN, KIND_PIN,
        // 索子
        KIND_SOU, KIND_SOU, KIND_SOU, KIND_SOU, KIND_SOU, KIND_SOU, KIND_SOU, KIND_SOU, KIND_SOU,
        // 風牌
        KIND_FON, KIND_FON, KIND_FON, KIND_FON,
        // 三元牌
        KIND_SANGEN, KIND_SANGEN, KIND_SANGEN 
    };

    // 一九牌フラグ(flag)の配列 
    private readonly static bool[] IS_ICHIKYUUS = 
    {
        // 萬子
        true, false, false, false, false, false, false, false, true,
        // 筒子
        true, false, false, false, false, false, false, false, true,
        // 索子
        true, false, false, false, false, false, false, false, true,
        // 風牌
        false, false, false, false,
        // 三元牌
        false, false, false 
    };

    // 字牌フラグ(flag)の配列 
    private readonly static bool[] IS_TSUUS = 
    {
        // 萬子
        false, false, false, false, false, false, false, false, false,
        // 筒子
        false, false, false, false, false, false, false, false, false,
        // 索子
        false, false, false, false, false, false, false, false, false,
        // 風牌
        true, true, true, true,
        // 三元牌
        true, true, true 
    };

    // ネクスト牌のIDの配列 
    private readonly static int[] NEXT_HAI_IDS = 
    {
        // 萬子
        ID_WAN_2, ID_WAN_3, ID_WAN_4, ID_WAN_5, ID_WAN_6, ID_WAN_7, ID_WAN_8, ID_WAN_9, ID_WAN_1,
        // 筒子
        ID_PIN_2, ID_PIN_3, ID_PIN_4, ID_PIN_5, ID_PIN_6, ID_PIN_7, ID_PIN_8, ID_PIN_9, ID_PIN_1,
        // 索子
        ID_SOU_2, ID_SOU_3, ID_SOU_4, ID_SOU_5, ID_SOU_6, ID_SOU_7, ID_SOU_8, ID_SOU_9, ID_SOU_1,
        // 風牌
        ID_NAN, ID_SYA, ID_PE, ID_TON,
        // 三元牌
        ID_HATSU, ID_CHUN, ID_HAKU 
    };
    #endregion


    private int _id = -1;
    private bool _isRed = false;

    public Hai() {
        _id = -1;
        _isRed = false;
    }

    public Hai(int id) {
        this._id = id;
        this._isRed = false;
    }

    public Hai(int id, bool isRed) {
        this._id = id;
        this._isRed = isRed;
    }

    public Hai(Hai hai) {
        copy(this, hai);
    }


    /// <summary>
    /// Copy the specified hai src to dest.
    /// 牌をコピーする
    /// </summary>

    public static void copy( Hai dest, Hai src )
    {
        dest._id = src._id;
        dest._isRed = src._isRed;
    }

    /// <summary>
    /// Gets the ID.
    /// </summary>

    public int ID
    {
        get{ return _id; }
    }

    // 赤ドラ
    /// <summary>
    /// Gets or sets the Hai is 赤ドラ.
    /// </summary>

    public bool IsRed
    {
        get{ return _isRed; }
        set{ _isRed = value; }
    }

    /// <summary>
    /// 番号を取得する.
    /// </summary>

    public int Num
    {
        get{ return NUMS[_id]; }
    }

    /// <summary>
    /// 種類を取得する.
    /// </summary>

    public int Kind
    {
        get{ return KINDS[_id]; }
    }

    /// <summary>
    /// NK(番号と種類のOR)を取得する.
    /// </summary>

    public int NumKind
    {
        get{ return NUMS[_id] | KINDS[_id]; }
    }

    /// <summary>
    /// 一九牌フラグを取得する.
    /// </summary>

    public bool IsIchikyuu
    {
        get{ return IS_ICHIKYUUS[_id]; }
    }

    /// <summary>
    /// 字牌フラグ(flag)を取得する.
    /// </summary>

    public bool IsTsuu
    {
        get{ return IS_TSUUS[_id]; }
    }

    public bool IsFon
    {
        get{ return _id >= ID_TON && _id <= ID_PE; }
    }
    public bool IsSanGen
    {
        get{ return _id >= ID_HAKU && _id <= ID_CHUN; }
    }

    /// <summary>
    /// 一九、字牌 フラグを取得する
    /// </summary>

    public bool IsYaochuu
    {
        get{ return IS_ICHIKYUUS[_id] | IS_TSUUS[_id]; }
    }

    /// <summary>
    /// Gets the next hai identifier.
    /// </summary>

    public int NextHaiID
    {
        get{ return NEXT_HAI_IDS[_id]; }
    }


    /// <summary>
    /// 字牌フラグを取得する.
    /// </summary>

    public static bool CheckIsTsuu(int numKind)
    {
        return (numKind & KIND_TSUU) != 0;
    }

    /// <summary>
    /// NK(番号と種類のOR)をIDに変換する
    /// </summary>

    public static int NumKindToID(int numKind)
    {
        int id;
        if (numKind >= KIND_SANGEN) {
            id = numKind - KIND_SANGEN + ID_HAKU - 1;
        } 
        else if (numKind >= KIND_FON) {
            id = numKind - KIND_FON + ID_TON - 1;
        } 
        else if (numKind >= KIND_SOU) {
            id = numKind - KIND_SOU + ID_SOU_1 - 1;
        } 
        else if (numKind >= KIND_PIN) {
            id = numKind - KIND_PIN + ID_PIN_1 - 1;
        } 
        else {
            id = numKind - KIND_WAN + ID_WAN_1 - 1;
        }
        if(id < ID_MIN || id > ID_MAX) 
            Utils.LogError("Invalid hai numKind " + numKind.ToString());
        return id;
    }

    /// <summary>
    /// check if the target hai is valid.
    /// </summary>

    public static bool IsValidHai(Hai hai)
    {
        return (hai != null) && IsValidHaiID(hai.ID);
    }

    /// <summary>
    /// check if the target hai ID is valid.
    /// </summary>

    public static bool IsValidHaiID(int id)
    {
        return id >= ID_MIN && id <= ID_MAX;
    }
}

