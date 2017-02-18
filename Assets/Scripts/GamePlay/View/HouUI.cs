using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 河管理器
/// </summary>

public class HouUI : UIObject 
{
    public Vector2 AlignLeftLocalPos = new Vector2(-150, 0);
    public float HaiPosOffsetX = 2;

	public float Line_PosY_1 = 0;
	public float Line_PosY_2 = -85;
	public float Line_PosY_3 = -170;

    public const int Max_Lines = 3;
    public const int MaxCoutPerLine = 6;

    private List<Transform> lineParents;
    private List<MahjongPai> _allHais = new List<MahjongPai>(Hou.SUTE_HAIS_LENGTH_MAX);

    private int _curLine = 0;
    private float _curLineRightAligPosX = 0f;
	private int _index = -1;


    // Use this for initialization
    void Start () {
        Init();
    }

    public override void Init() {
        base.Init();

        if(isInit == false){
            lineParents = new List<Transform>(Max_Lines);
            for( int i = 0; i < Max_Lines; i++ ) {
                Transform line = transform.FindChild("Line_" + (i+1));
                if(line != null){
                    lineParents.Add(line);
                }
            }

            isInit = true;
        }

        _curLineRightAligPosX = AlignLeftLocalPos.x;
    }

	//根據索引值決定位置
	public void SetPlayerIndex(int idx) {
		_index = idx;
		switch(_index) {
		case 0://player
			transform.localPosition = new Vector3 (-5, -0.5f, 5);
			//transform.localRotation = Quaternion.Euler (-90, 0, 0);
			break;
		case 1://down
			transform.localPosition = new Vector3 (-5, -0.5f, -4);
			//transform.localRotation = Quaternion.Euler (-90, 0, 0);
			break;
		case 2://opp
			transform.localPosition = new Vector3 (-5, -0.5f, -5);
			//transform.localRotation = Quaternion.Euler (-90, 0, 0);
			break;
		case 3://up
			transform.localPosition = new Vector3 (-5, -0.5f, 4);
			//transform.localRotation = Quaternion.Euler (90, 0, 0);
			break;
		}
	}

    public override void Clear() {
        base.Clear();

        // clear all hai.
        for( int i = 0; i < _allHais.Count; i++ ) {
            PlayerUI.CollectMahjongPai(_allHais[i]);
        }
        _allHais.Clear();
    }

    //public override void SetParentPanelDepth( int depth ) {
    //    for( int i = 0; i < lineParents.Count; i++ ) {
    //        UIPanel panel = lineParents[i].GetComponent<UIPanel>();
	//
    //        panel.depth = depth + (lineParents.Count - i);
    //    }
    //}

    public void AddHai(MahjongPai pai) 
    {
        int inLine = _allHais.Count / MaxCoutPerLine;  //inLine=0,1,2. >2 has a small chance.
        int indexInLine = _allHais.Count % MaxCoutPerLine;

        int EndingLine = Max_Lines - 1;

        if( inLine != _curLine )
        {
            if( inLine <= EndingLine ){
                _curLine = inLine;
                _curLineRightAligPosX = AlignLeftLocalPos.x;
            }
            else if(inLine > EndingLine){
                _curLine = EndingLine;

                indexInLine += MaxCoutPerLine;
                _curLineRightAligPosX += MahjongPai.Width + HaiPosOffsetX;
            }
        }
        else{
            if(indexInLine > 0)
                _curLineRightAligPosX += MahjongPai.Width + HaiPosOffsetX;
        }
		//pai.gameObject.layer = LayerMask.NameToLayer ("Hou");
		Utils.SetLayerRecursively (pai.gameObject, LayerMask.NameToLayer ("Hou"));
        pai.transform.parent = lineParents[_curLine];
        pai.transform.localPosition = new Vector3(_curLineRightAligPosX, 0, 0);

        pai.DisableInput();
        pai.SetEnableStateColor(true);
        _allHais.Add(pai);

        pai.Show();
        //lineParents[_curLine].GetComponent<UIPanel>().Update();
    }

    public bool setTedashi(bool isTedashi)
    {
        if( _allHais.Count <= 0 )
            return false;

        // set last hai tedashi.
        MahjongPai lastHai = _allHais[_allHais.Count - 1];
        lastHai.SetTedashi(isTedashi);

        return true;
    }

    public bool SetReach(bool isReach)
    {
        if( _allHais.Count <= 0 )
            return false;

        // set last hai reach.
        MahjongPai lastHai = _allHais[_allHais.Count - 1];
        lastHai.SetReach(isReach);

        _curLineRightAligPosX += (MahjongPai.Height - MahjongPai.Width);

        return true;
    }

    public bool setNaki(bool isNaki)
    {
        if( _allHais.Count <= 0 )
            return false;

        // set last hai naki.
        MahjongPai lastHai = _allHais[_allHais.Count - 1];
        lastHai.SetNaki(isNaki);

        setShining(false);

        return true;
    }

    public bool setShining(bool isShining)
    {
        if( _allHais.Count <= 0 )
            return false;

        // set last hai shining.
        MahjongPai lastHai = _allHais[_allHais.Count - 1];
        lastHai.setShining(isShining);

        return true;
    }

}
