
/// <summary>
/// Agari setting.
/// Agari(あがり) = 胡牌
/// </summary>

public class AgariParam 
{
    // 自風の設定
    private EKaze _jiKaze = EKaze.Ton;

    // 場風の設定
    private EKaze _baKaze = EKaze.Ton;

    // 表ドラ
    private Hai[] _omoteDoraHais = null;

    // 裏ドラ
    private Hai[] _uraDoraHais = null;


    // 役成立フラグの配列
    private bool[] _yakuFlag = null;


    public AgariParam()
    {
        _yakuFlag = new bool[(int)EYakuFlagType.Count];

        for(int i = 0; i < _yakuFlag.Length; i++)
            _yakuFlag[i] = false;
    }

    public void ResetYakuFlags()
    {
        for(int i = 0; i < _yakuFlag.Length; i++)
            _yakuFlag[i] = false;
    }
    public void ResetDoraHais()
    {
        _omoteDoraHais = null;
        _uraDoraHais = null;
    }


    public void setYakuFlag(EYakuFlagType yakuNum, bool flg) {
        _yakuFlag[(int)yakuNum] = flg;
    }

    public bool getYakuFlag(EYakuFlagType yakuFlag) {
        return _yakuFlag[(int)yakuFlag];
    }


    public void setJikaze(EKaze jikaze) {
        _jiKaze = jikaze;
    }
    public EKaze getJikaze() {
        return _jiKaze;
    }

    public void setBakaze(EKaze bakaze) {
        _baKaze = bakaze;
    }
    public EKaze getBakaze() {
        return _baKaze;
    }

    // 表ドラ
    public void setOmoteDoraHais(Hai[] omoteDoraHais) {
        _omoteDoraHais = omoteDoraHais;
    }
    public Hai[] getOmoteDoraHais() {
        return _omoteDoraHais;
    }

    // 裏ドラ
    public void setUraDoraHais(Hai[] uraDoraHais) {
        _uraDoraHais = uraDoraHais;
    }
    public Hai[] getUraDoraHais() {
        return _uraDoraHais;
    }
}
