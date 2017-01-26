
// Kaze = 風 //
public enum EKaze 
{
    Ton = 0,//東
    Nan = 1,//南
    Sya = 2,//西
    Pei = 3,//北
}

public static class EKazeExtension
{
    public static EKaze Next(this EKaze kaze)
    {
        switch(kaze)
        {
            case EKaze.Ton: return EKaze.Nan;
            case EKaze.Nan: return EKaze.Sya;
            case EKaze.Sya: return EKaze.Pei;
            case EKaze.Pei: return EKaze.Ton;
        }
        return EKaze.Nan;
    }
    public static EKaze Prev(this EKaze kaze)
    {
        switch(kaze)
        {
            case EKaze.Ton: return EKaze.Pei;
            case EKaze.Nan: return EKaze.Ton;
            case EKaze.Sya: return EKaze.Nan;
            case EKaze.Pei: return EKaze.Sya;
        }
        return EKaze.Pei;
    }
}

