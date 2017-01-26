using UnityEngine;
using System.Collections;


public class UIObject : MonoBehaviour 
{
    protected Player _ownerPlayer;
    public Player OwnerPlayer
    {
        get{ return _ownerPlayer; }
    }
    protected PlayerAction PlayerAction
    {
        get{ return _ownerPlayer.Action; }
    }

    public virtual void BindPlayer(Player p)
    {
        this._ownerPlayer = p;
    }

    protected bool isInit = false;

    public virtual void Init() {
        
    }

    public virtual void Clear() {
        
    }

    public virtual void SetParentPanelDepth( int depth ) { 
    
    }
}
