using UnityEngine;
/// <summary>
/// Roles: m = man, w = women
/// </summary>
public enum EVoiceType
{
    M_A = 1,
    M_B = 2,
    M_C = 3,
    M_D = 4,
    W_A = 5,
    W_B = 6,
    W_C = 7,
    W_D = 8,
}

public enum ECvType
{
	Throw = 0,//出牌
    Pon = 1,//碰
    Chii = 2,//吃
    Kan = 3,//槓
    Reach = 4,//聽
    Ron = 5,//胡
    Tsumo = 6,//自摸
    RyuuKyoku = 7,//流局
    RKK_HaiTypeOver9 = 8,
    RKK_SuteFonHai4 = 9,
    RKK_Reach4 = 10,
    RKK_KanOver4 = 11,
    RKK_Ron3 = 12,
    ManGan = 13,
    HaReMan = 14,
    BaiMan = 15,
    SanBaiMan = 16,
    YakuMan = 17,
    Double_YakuMan = 18,
    Triple_YakuMan = 19,
    ORaSu = 22, //last kyoku
    Kyoku_Start = 23,//東開始
    Survival_Start = 24,//
    NanBa_Start = 25,//南開始

}

/// <summary>
/// SE type.
/// </summary>
public enum ESeType
{
    SuteHai = 1,
    Agari = 2,
    Yaku = 3,
    Dice = 5,
}

public class AudioConfig
{
    public static string GetCVPath(EVoiceType role, ECvType cv)
    {
        string roleStr = role.ToString().ToLower();
        string num = ((int)cv).ToString("000");
		string path = string.Format("Sounds/CV/sm_cv{0}{1}", roleStr, num);
		//Debug.Log ("GetCVPath("+path+")");
        return path;
    }

	public static string GetHCVPath(EVoiceType role, ECvType cv, Hai h)
	{
		string roleStr = role.ToString().ToLower();
		string num = ((int)cv).ToString("000");
		string path = string.Format("Sounds/MJ/b/{0}", h.ID);
		//Debug.Log ("GetHCVPath("+path+")");
		return path;
	}

    public static string GetSEPath(ESeType type)
    {
        return string.Format("Sounds/SE/sm_se{0}", ((int)type).ToString("000"));
    }
}
