using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour
{
    public GUIStyle titleStyle;
    public GUIStyle buttonStyle;

    public float buttonHeight = 80;

    public Transform itemsTree;

    Transform currentMenuRoot;
    public Transform CurrentMenuRoot
    {
        get { return currentMenuRoot; }
        set { currentMenuRoot = value; }
    }

    // Use this for initialization
    void Start()
    {
        CurrentMenuRoot = itemsTree;
    }

    Rect screenRect = new Rect( 0, 0, SampleUI.VirtualScreenWidth, SampleUI.VirtualScreenHeight );
    public float menuWidth = 450;

    public float sideBorder = 30;

    void OnGUI()
    {
        SampleUI.ApplyVirtualScreen();

        GUILayout.BeginArea( screenRect );
        GUILayout.BeginHorizontal();

        GUILayout.Space( sideBorder );
            
        if( CurrentMenuRoot )
        {
            GUILayout.BeginVertical();
        
            GUILayout.Space( 15 );
            GUILayout.Label( CurrentMenuRoot.name, titleStyle );

            for( int i = 0; i < CurrentMenuRoot.childCount; ++i )
            {
                Transform item = CurrentMenuRoot.GetChild( i );

                if( GUILayout.Button( item.name, GUILayout.Height( buttonHeight ) ) )
                {
                    MenuNode menuNode = item.GetComponent<MenuNode>();
                    if( menuNode && menuNode.sceneName != null && menuNode.sceneName.Length > 0 )
                        Application.LoadLevel( menuNode.sceneName );
                    else if( item.childCount > 0 )
                        CurrentMenuRoot = item;
                }

                GUILayout.Space( 5 );
            }            

            GUILayout.FlexibleSpace();

            if( CurrentMenuRoot != itemsTree && CurrentMenuRoot.parent )
            {
                if( GUILayout.Button( "<< BACK <<", GUILayout.Height( buttonHeight ) ) )
                    CurrentMenuRoot = CurrentMenuRoot.parent;

                GUILayout.Space( 15 );
            }

            GUILayout.EndVertical();
        }

        GUILayout.Space( sideBorder );
        GUILayout.EndHorizontal();        
        GUILayout.EndArea();
    }
}
