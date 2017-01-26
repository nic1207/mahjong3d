using UnityEngine;
using System.Collections;

// iphone3 480 x 320
// iphone4 960 x 640
public class SampleUI : MonoBehaviour
{
    public GUISkin skin;
    
    public Color titleColor = Color.white;
    GUIStyle titleStyle;
    GUIStyle statusStyle;
    GUIStyle helpStyle;

    Rect topBarRect = new Rect( 0, -4, 600, 56 );
    Rect backButtonRect = new Rect( 5, 2, 80, 46 );
    Rect titleRect = new Rect( 100, 2, 400, 46 );
    Rect helpButtonRect = new Rect( 515, 2, 80, 46 );
    Rect statusTextRect = new Rect( 30, 336, 540, 60 );
    Rect helpRect = new Rect( 50, 60, 500, 300 );

    string statusText = "";//"status text goes here";
    public string StatusText
    {
        get { return statusText; }
        set { statusText = value; }
    }

    public bool showStatusText = true;

    public string helpText = "";
    public bool showHelpButton = true;
    public bool showHelp = false;


    void Awake()
    {
        titleStyle = new GUIStyle( skin.label );
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.normal.textColor = titleColor;

        statusStyle = new GUIStyle( skin.label );
        statusStyle.alignment = TextAnchor.LowerCenter;

        helpStyle = new GUIStyle( skin.label );
        helpStyle.alignment = TextAnchor.UpperLeft;
        helpStyle.padding.left = 5;
        helpStyle.padding.right = 5;
    }

    #region Virtual Screen for automatic UI resolution scaling
    
    public static readonly float VirtualScreenWidth = 600;
    public static readonly float VirtualScreenHeight = 400;

    public static void ApplyVirtualScreen()
    {
        // resolution scaling matrix
        GUI.matrix = Matrix4x4.Scale( new Vector3( Screen.width / VirtualScreenWidth, Screen.height / VirtualScreenHeight, 1 ) );
    }

    #endregion

    protected virtual void OnGUI()
    {
        if( skin != null )
            GUI.skin = skin;

        ApplyVirtualScreen();

        GUI.Box( topBarRect, "" );

        if( GUI.Button( backButtonRect, "Back" ) )
            Application.LoadLevel( 0 );

        GUI.Label( titleRect, "FingerGestures - " + this.name, titleStyle );

        if( showStatusText )
            GUI.Label( statusTextRect, statusText, statusStyle );

        if( helpText.Length > 0 && showHelpButton && !showHelp && GUI.Button( helpButtonRect, "Help" ) )
            showHelp = true;

        if( showHelp )
        {
            GUI.Box( helpRect, "Help" );

            GUILayout.BeginArea( helpRect );
            GUILayout.BeginVertical();
            {
                GUILayout.Space( 25 );

                GUILayout.Label( helpText, helpStyle );

                GUILayout.FlexibleSpace();

                if( GUILayout.Button( "Close", GUILayout.Height( 40 ) ) )
                    showHelp = false;
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

    }
}
