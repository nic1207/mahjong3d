using System.Collections.Generic;


public class AgariUpdateInfo : AgariInfo
{
    public List<PlayerTenbouChangeInfo> tenbouChangeInfoList = new List<PlayerTenbouChangeInfo>();

    public Player agariPlayer;
    public bool agariPlayerIsOya;

    public bool isTsumo;
    public Hai agariHai;
    public Hai[] allOmoteDoraHais;
    public int openedOmoteDoraCount;
    public Hai[] allUraDoraHais;
    public int openedUraDoraCount;

    public int reachBou;
    public EKaze manKaze;
    public EKaze bakaze;
    public int kyoku;
    public int honba;
    public bool isLastKyoku;


    public AgariUpdateInfo(){
        
    }

    public AgariUpdateInfo( AgariInfo other )
    {
        Copy( this, other );
    }


    public static void Copy( AgariUpdateInfo dest, AgariInfo src )
    {
        dest.han = src.han;
        dest.fu = src.fu;

        dest.hanteiYakus = new YakuHelper.YakuHandler[src.hanteiYakus.Length];
        for( int i = 0; i < dest.hanteiYakus.Length; i++ )
            dest.hanteiYakus[i] = src.hanteiYakus[i].Clone();

        dest.scoreInfo = new Score( src.scoreInfo );
        dest.agariScore = src.agariScore;
    }

}
