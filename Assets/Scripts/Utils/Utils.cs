using UnityEngine;
using System.Collections;

public class Utils 
{
    public static int GetRandomNum(int min, int max)
    {
        return Random.Range(min, max);
    }

    public static void Log( object msg )
    {
        Debug.Log(msg);
    }
    public static void LogFormat(string format, params object[] args)
    {
        Debug.LogFormat(format, args);
    }

    public static void LogWarning( object msg )
    {
        Debug.LogWarning(msg);
    }
    public static void LogWarningFormat(string format, params object[] args)
    {
        Debug.LogWarningFormat(format, args);
    }

    public static void LogError( object msg )
    {
        Debug.LogError(msg);
    }
    public static void LogErrorFormat(string format, params object[] args)
    {
        Debug.LogErrorFormat(format, args);
    }
}
