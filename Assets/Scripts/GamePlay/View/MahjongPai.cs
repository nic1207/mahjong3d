using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MahjongPaiComparer : IComparer<MahjongPai> 
{
    public int Compare(MahjongPai x, MahjongPai y)
    {
        return x.ID - y.ID;
    }
}

[RequireComponent(typeof(BoxCollider))]
public class MahjongPai : UIObject 
{
	private bool _isPlayer = false;
    protected Hai _data;
    public int ID
    {
        get { return _data == null? -1 : _data.ID; }
    }
	public bool isPlayer
	{
		get { return _isPlayer;}
		set { _isPlayer = value;}

	}
    public bool isRed
    {
        get; private set;
    }
    public bool isTedashi
    {
        get; private set;
    }
    public bool isNaki
    {
        get; private set;
    }
    public bool isReach
    {
        get; private set;
    }

    public readonly static float Width  = .64f;
    public readonly static float Height = .84f;

    public const int LandHaiPosOffsetY = 0; // 当麻将横着放时，往下移15像素. /


	public Color[] normalColor;
	public Color[] savecolors = new Color[3];
    public Color redDoraColor = Color.red;
    public Color nakiColor = new Color(0.7f, 0.7f, 0.7f);
    public Color disableColor = new Color(0.6f, 0.6f, 0.6f);


    protected BoxCollider boxCollider;
	protected MeshRenderer background;
	protected MeshRenderer majSprite;


    protected EFrontBack curFrontBack = EFrontBack.Front;

    public bool IsShownOut 
    {
        get { return curFrontBack == EFrontBack.Front; }
    }


    public void DisableInput(bool updateColor = false)
    {
        boxCollider.enabled = false;

        if(updateColor) SetEnableStateColor(false);
    }
    public void EnableInput(bool updateColor = false)
    {
        boxCollider.enabled = true;

        if(updateColor) SetEnableStateColor(true);
    }

    public void SetEnableStateColor(bool state)
    {
        if(state){
			int i = 0;
			foreach(Material ma in  background.materials)
			{
				//background.material.color = normalColor;
				//if(savecolors!=null && savecolors.Length>0 && savecolors[i]!=null)
				//if(i==2)
				ma.color = savecolors[i];
				i++;
			}
        }
        else{
			//background.material.color = disableColor;
			int i = 0;
			foreach(Material ma in  background.materials)
			{
				savecolors[i] = ma.color;
				//background.material.color = normalColor;
				//if(i==2)
				ma.color = disableColor;
				i++;
			}
        }
    }

	void OnTap( TapGesture gesture )
	{
		//if (!_isPlayer)
		//	return;
		// this object was clicked - do something
		//Debug.Log("gesture.Selection="+gesture.Selection);
		//Debug.Log(gesture.StartSelection);
		if (gesture.Selection == this.gameObject) {
			//Debug.Log ("OnTap(" + this.ID + ")");
			OnClick ();
		}
	}

	//void OnFingerHover(FingerHoverEvent e)
	//{
	//	Debug.Log ("OnFingerHover()"+e);
	//}

    public override void Init() 
    {
        base.Init();

        if( isInit == false )
        {
			background = GetComponent<MeshRenderer>();
			int i = 0;
			foreach (Material ma in  background.materials) {
				savecolors [i] = ma.color;
				i++;
			}
			majSprite = transform.FindChild("face").GetComponent<MeshRenderer>();
            boxCollider = GetComponent<BoxCollider>();
			TapRecognizer tapRecognizer = GetComponent<TapRecognizer> ();
			if (tapRecognizer) {
				tapRecognizer.OnGesture += OnTap;
			}
            isInit = true;
        }

        SetOrientation(EOrientation.Portrait);
        SetRedDora(false);
        Hide();

        DisableInput();
    }

    public override void Clear()
    {
        base.Clear();

        SetRedDora(false);
        SetTedashi(false);
        SetNaki(false);
        SetReach(false);
        setShining(false);

        Hide();

        DisableInput();
        SetEnableStateColor(true);

        SetOnClick(null);
        SetInfo(null);
    }

    public void SetInfo(Hai hai)
    {
        this._data = hai;
    }

	public Hai GetInfo()
	{
		return this._data;
	}

    public void UpdateImage() {
		//Debug.Log ("UpdateImage()"+ID);
		Texture t = ResManager.getMahjongTexture(_data.Kind, _data.Num);
		//atlas.GetSprite(spriteName);
		majSprite.material.mainTexture = t;
        //SetRedDora(_data.IsRed);
    }

    public void SetRedDora(bool isRed)
    {
        this.isRed = isRed;

        if( isRed ) {
            //majSprite.color = redDoraColor;
        }
        else {
            //majSprite.color = normalColor;
        }
    }

    public void SetTedashi(bool state)
    {
        this.isTedashi = state;


    }

    public void SetNaki(bool state)
    {
        this.isNaki = state;

        if(isNaki){
            //background.color = nakiColor;
        }
        else{
            //background.color = normalColor;
        }
    }

    public void SetReach(bool state)
    {
        this.isReach = state;

        if(isReach == true){
            //SetOrientation(EOrientation.Landscape_Left); //喊聽牌將扔出去的牌側翻 
            //transform.localPosition += new Vector3((Height-Width)*0.5f, MahjongPai.LandHaiPosOffsetY, 0);  //喊聽牌將扔出去的牌位移
        }
        else
        {
            SetOrientation(EOrientation.Portrait);
        }
    }

    // shining on menu list shown out.
    public void setShining(bool isShining)
    {
        if( isShining ) {
            TweenAlpha tweener = TweenAlpha.Begin( background.gameObject, 1f, 0.5f );
            tweener.style = UITweener.Style.PingPong;
            tweener.method = UITweener.Method.EaseInOut;
        }
        else {
            TweenAlpha tweener = GetComponent<TweenAlpha>();
            if( tweener != null )
                TweenAlpha.Begin( background.gameObject, 0f, 1f );
        }
    }

    public void Show() {
        SetFrontBack(EFrontBack.Front);
    }
    public void Hide() {
        SetFrontBack(EFrontBack.Back);
    }

    protected void SetFrontBack(EFrontBack fb)
    {
        curFrontBack = fb;

		if( fb == EFrontBack.Front ) {
            majSprite.gameObject.SetActive(true);
			gameObject.transform.localRotation = Quaternion.Euler (0, 0, 180);
        } else {
            //majSprite.gameObject.SetActive(false);
			majSprite.gameObject.SetActive(true);
        }
        // mark as changed. don't know why UIPanel won't update.
        //gameObject.SetActive(false);
        //gameObject.SetActive(true);
    }

    public void SetOrientation(EOrientation orien) {
        if( orien == EOrientation.Landscape_Left ){
            transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if( orien == EOrientation.Landscape_Right ) {
            transform.localEulerAngles = new Vector3(0, 0, -90);
        }
        else if( orien == EOrientation.Portrait_Down ) {
            transform.localEulerAngles = new Vector3(0, 0, -180);
        }
        else // if( orien == EOrientation.Portrait ) 
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }


    public static MahjongPai current = null;

    protected System.Action _onClick;
    protected void OnClick()
    {
        if(current == null && enabled)
        {
            current = this;
            if( _onClick != null ) _onClick();
            current = null;
        }
    }

    public void SetOnClick(System.Action onClick)
    {
		if (onClick != null) {
			_onClick = onClick;
			TapRecognizer tr = gameObject.GetComponent<TapRecognizer> ();
			tr.enabled = true;
		}
    }
    public void ClearOnClick()
    {
        _onClick = null;
		TapRecognizer tr = gameObject.GetComponent<TapRecognizer> ();
		tr.enabled = false;
    }
}
