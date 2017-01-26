
/// <summary>
/// 捨牌を管理する
/// 捨牌: 打出去的牌
/// </summary>

public class SuteHai : Hai
{
    // 鳴きフラグ (吃碰以及明槓)
    private bool _isNaki = false;

    // リーチフラグ(立直flag)
    private bool _isReach = false;

    // 手出しフラグ(正常打出去)
    private bool _isTedashi = false;


    public bool IsNaki
    {
        get{ return _isNaki; }
        set{ _isNaki = value; }
    }

    public bool IsReach
    {
        get{ return _isReach; }
        set{ _isReach = value; }
    }

    public bool IsTedashi
    {
        get{ return _isTedashi; }
        set{ _isTedashi = value; }
    }


    public SuteHai() : base() {
    }

    public SuteHai(int id) : base(id) {
    }

    public SuteHai(Hai hai) : base(hai) {
    }

    public SuteHai(SuteHai src) : base()
    {
        copy(this, src);
    }


    public static void copy(SuteHai dest, SuteHai src)
    {
        Hai.copy(dest, src);
        dest._isNaki = src._isNaki;
        dest._isReach = src._isReach;
        dest._isTedashi = src._isTedashi;
    }

    public static void copy(SuteHai dest, Hai src)
    {
        Hai.copy(dest, src);
        dest._isNaki = false;
        dest._isReach = false;
        dest._isTedashi = false;
    }

}
