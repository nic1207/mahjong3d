
public static class GameSettings 
{
    public static int PlayerCount = 4;

    // 食断.
    public static bool UseKuitan = false;

    // 红Dora.
    //public static bool UseRedDora = false;

    // if allow furiten
    public static bool AllowFuriten = false;


    public const bool AllowRon3 = false;
    public const bool AllowReach4 = false;
    public const bool AllowSuteFonHai4 = false;

    // 局の最大値
    public const int Kyoku_Max = (int)EKyoku.Nan_4;
    public const int KanCountMax = 4;

    // 持ち点の初期値
    public const int Init_Tenbou = 25000;
    public const int Back_Tenbou = 30000; // used for calculating final pt

    public const int Reach_Cost = 0;
    public const int HonBa_Cost = 100;

}
