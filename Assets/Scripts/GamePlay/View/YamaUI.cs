using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Yama array is like:
///   ->
///  |  |
///   <-
///  so align right.
///  
///  Wareme is like:
///               Doras     Rinshan
///            -        -    -   -
/// <--top    |8|..x3..|0|  |2| |0|  <--
///            -        -    -   -
///            -        -    -   -
/// <--botom  |9|..x3..|1|  |3| |1|  <--
///            -        -    -   -
///             total x5       x2
/// </summary>

public class YamaUI : UIObject 
{
    public const int MaxYamaHaiCountInPlayer = 34; //17*2
    public readonly static Vector3 WaremeOffset = new Vector3(-6f,0f,0f);

    public Vector2 AlignRightLocalPos = new Vector2(520, 0);
    public int TopY = 0;
    public int BottomY = -30;
    public int TopDepth = 5;
    public int BottomDepth = 4;

    public const int MaxLines = 2;

    private int yama_start = -1;
    private int yama_end = -1;
	private int _index = -1;

    public int Yama_Start 
    {
        get { return yama_start; }
    }
    public int Yama_End
    {
        get { return yama_end; }
    }

    private Transform top;
    private Transform bottom;

    private Dictionary<int, MahjongPai> mahjongYama = new Dictionary<int, MahjongPai>();


    // Use this for initialization
    void Start () {
        Init();
    }

    public override void Init() {
        base.Init();

        if(isInit == false){
            top = transform.FindChild("Top");
            bottom = transform.FindChild("Bottom");

            isInit = true;
        }
    }

    public override void Clear() {
        base.Clear();

        foreach( var kv in mahjongYama ) {
            PlayerUI.CollectMahjongPai(kv.Value);
        }
        mahjongYama.Clear();
    }

    public override void SetParentPanelDepth( int depth ) {
        top.GetComponent<UIPanel>().depth = depth + TopDepth;
        bottom.GetComponent<UIPanel>().depth = depth + BottomDepth;
    }

    public void SetYamaIndex(int start, int end) {
        this.yama_start = start;
        this.yama_end = end;
    }

	public void SetPlayerIndex(int idx) {
		this._index = idx;
		switch(_index) {
		case 0://player
			transform.localPosition = new Vector3 (-0.73f, 0, 2);
			transform.localRotation = Quaternion.Euler (0, 0, 0);
			break;
		case 1://down
			transform.localPosition = new Vector3 (-9.53f, 0, -0.65f);
			transform.localRotation = Quaternion.Euler (0, -180, 0);
			break;
		case 2://opp
			transform.localPosition = new Vector3 (-8.72f, 0, -1.33f);
			transform.localRotation = Quaternion.Euler (0,-180, 0);
			break;
		case 3://up
			transform.localPosition = new Vector3 (-1.25f, 0, 2.2f);
			transform.localRotation = Quaternion.Euler (0, 0, 0);
			break;
		}
	}
    public void SetYamaHais(Dictionary<int, Hai> yamaHais, int start, int end)
    {//設定山牌
		//Debug.Log("SetYamaHais()"+yamaHais.Count+" "+start+" "+end);
        if( yamaHais == null )
            return;

        Clear();

        SetYamaIndex(start, end);

        foreach(var kv in yamaHais)
        {
            AddHai(kv.Value, kv.Key );

            if(kv.Key < start || kv.Key > end)
                Debug.LogErrorFormat("Hai's index {0} in yama is out of range [{1}, {2}]", kv.Key,start,end);
        }
    }

    protected void AddHai( Hai hai, int index )
    {
        int line = Mathf.Max( 0, (index - this.yama_start) % MaxLines );
        int indexInLine = Mathf.Max( 0, (index - this.yama_start) / MaxLines );

        Transform parent = top;
        if( line == 0 ) {
            parent = top;
        }
        else {
            parent = bottom;
        }

        // set position. align right.
        float posX = AlignRightLocalPos.x - MahjongPai.Width * indexInLine;
        Vector3 localPos = new Vector3( posX, 0, 0 );

        MahjongPai pai = PlayerUI.CreateMahjongPai( parent, localPos, hai, false );
		string name = ResManager.getMahjongTextureName (hai.Kind, hai.Num);
		//Debug.Log ("AddHai("+hai.ID+","+name+")");
        mahjongYama.Add( index, pai );

        //if(index == 0 || index == 2 || index == Yama.YAMA_HAIS_MAX-2) pai.Show();
    }


    public MahjongPai PickUp( int index )
    {
        if( mahjongYama.ContainsKey( index ) ) {
            MahjongPai pai = mahjongYama[index];
            mahjongYama.Remove(index);
            return pai;
        }
        else {
            string indexList = "";
            foreach(var maj in mahjongYama)
            {
                indexList += maj.Key.ToString() + ",";
            }
			if (mahjongYama.Count > 0) {
				//Debug.LogWarningFormat ("No such mahjong in index {0}, range is [{1}] ", index, indexList.Substring (0, indexList.Length - 1));
			} else {
				//Debug.Log ("mahjongYama.Count="+mahjongYama.Count);
			}
			return null;
        }
    }

    public void ShowHai( int index ) {
        if( mahjongYama.ContainsKey(index) ) {
            mahjongYama[index].Show();
        }
    }
    public void HideHai(int index) {
        if( mahjongYama.ContainsKey(index) ) {
            mahjongYama[index].Hide();
        }
    }

    public void SetWareme(int index)
    { 
        //if( mahjongYama.ContainsKey(index) )
        //    mahjongYama[index].SetHighlight(true);

        Debug.Log("##Player " + _ownerPlayer.JiKaze + " set wareme " + index.ToString());

        foreach(var maj in mahjongYama)
        {
            if(maj.Key >= index){
                maj.Value.transform.localPosition += new Vector3(-0.08f,0f,0f);
            }
        }
    }

    public void ShowAllYamaHais() {
        foreach(var kv in mahjongYama){
            kv.Value.Show();
        }
    }
    public void HideAllYamaHais() {
        foreach( var kv in mahjongYama ) {
            kv.Value.Hide();
        }
    }
}
