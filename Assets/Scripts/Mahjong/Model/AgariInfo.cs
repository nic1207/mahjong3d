using System.Collections;
using System.Collections.Generic;
using YakuHelper;


public class AgariInfo 
{
    public int han;
    public int fu;
    public Score scoreInfo;
    public YakuHandler[] hanteiYakus;

    public int agariScore;


    public string[] getYakuNames()
    {
        if( hanteiYakus == null )
            return null;
        
        List<string> yakuNames = new List<string>();

        int hanSuu = 0;
        string current = "";

        for(int i = 0; i < hanteiYakus.Length; i++)
        {
            string yakuName = ResManager.getString( hanteiYakus[i].getYakuNameKey() );

            if( hanteiYakus[i].isYakuman() ) {
                if( hanteiYakus[i].isDoubleYakuman() )
                    current = yakuName + "  " + ResManager.getString("double") + ResManager.getString("yakuman");
                else
                    current = yakuName + "  " + ResManager.getString("yakuman");
            }
            else{
                hanSuu = hanteiYakus[i].getHanSuu();

                current = yakuName + "  " + hanSuu.ToString() + ResManager.getString("han");
            }

            yakuNames.Add(current);
        }

        return yakuNames.ToArray();
    }


    public override string ToString()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.Append( scoreInfo.ToString() );
        sb.Append( "\n" );

        sb.Append( "Yaku Names: \n" );

        string[] yakuNames = getYakuNames();
        for(int i = 0; i < yakuNames.Length; i++)
            sb.Append(  yakuNames[i] + "\n" );

        sb.Append( "Han: " + han.ToString() );
        sb.Append( "\n" );
        sb.Append( "Fu: " + fu.ToString() );
        sb.Append( "\n" );

        sb.Append( "AgariScore: " + agariScore.ToString() );
        sb.Append( "\n" );

        return sb.ToString();
    }
}
