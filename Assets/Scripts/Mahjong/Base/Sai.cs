
/// <summary>
/// サイコロ(色子)を管理する
/// </summary>

public class Sai 
{
    // 番号
    private int _num = 1;

    public int Num
    {
        get{ return _num; }
    }

    /// <summary>
    /// サイコロを振って番号を取得する。
    /// 摇色子，结果1-6.
    /// </summary>
    public int SaiFuri()
    {
        _num = Utils.GetRandomNum(1, 7);
		//_num = 1;
        return _num;
    }
}