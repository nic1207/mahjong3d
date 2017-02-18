using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TehaiUI : UIObject 
{
    public Vector2 AlignLeftLocalPos = new Vector2(-380, 0);
    public float HaiPosOffsetX = .65f;
    public float NewHaiPosOffsetX = 0f;
	private int _index = -1;
    //public float TehaiDepth = .6f;

    private List<MahjongPai> tehaiList = new List<MahjongPai>();

    //private UIPanel uiPanel;


    void Awake(){
        
    }

    void Start () {
        
    }

	public void SetPlayerUIndex(int idx) {
		_index = idx;
		switch(_index) {
		case 0://player
			transform.localPosition = new Vector3 (-10.3f, -0.7f, -0.34f);
			transform.localRotation = Quaternion.Euler (-90, 0, 0);
			break;
		case 1://down
			transform.localPosition = new Vector3 (-10.35f, -0.7f, 0.26f);
			transform.localRotation = Quaternion.Euler (-90, 0, 0);
			break;
		case 2://opp
			transform.localPosition = new Vector3 (-9.6f, -0.7f, 0.1f);
			transform.localRotation = Quaternion.Euler (-90, 0, 0);
			break;
		case 3://up
			transform.localPosition = new Vector3 (-0.4f, -0.6f, 0.85f);
			transform.localRotation = Quaternion.Euler (-90, -180, 0);
			break;
		}
	}

    public override void Clear() {
        base.Clear();

        for( int i = 0; i < tehaiList.Count; i++ ) {
            PlayerUI.CollectMahjongPai( tehaiList[i] );
        }
        tehaiList.Clear();
    }

    //public override void SetParentPanelDepth( int depth ) {
    //    uiPanel.depth = depth + TehaiDepth;
    //}


    public void SortTehai(Hai[] hais, float delay, bool setLastNew)
    {
        StartCoroutine(Sort(hais, delay, setLastNew));
    }
    protected IEnumerator Sort(Hai[] hais, float delay, bool setLastNew)
    {
        yield return new WaitForSeconds( delay );

        SetTehai( hais );
    }

    public void SetTehai(Hai[] hais, bool setLastNew = false) 
    {
        Clear();

        for( int i = 0; i < hais.Length; i++ ) 
        {
            AddPai( hais[i], setLastNew, !OwnerPlayer.IsAI );
        }
        //uiPanel.Update();
    }

    public void AddPai( Hai hai, bool newPicked = false, bool isShow = false )
    {
        MahjongPai pai = PlayerUI.CreateMahjongPai( transform, Vector3.zero, hai, isShow );
        AddPai(pai, newPicked, isShow);
    }

    public void AddPai( MahjongPai pai, bool newPicked = false, bool isShow = false )
    {
        int index = tehaiList.Count;
        float posX = AlignLeftLocalPos.x + MahjongPai.Width * index + HaiPosOffsetX * index;

        pai.transform.parent = transform;
        pai.transform.localPosition = new Vector3( posX, AlignLeftLocalPos.y, 0 );

        if( isShow ) {
            pai.Show();
        } else {
            pai.Hide();
        }

		if (OwnerPlayer.IsAI == false) {
			//pai.gameObject.layer = LayerMask.NameToLayer("PlayerTehai");
			Utils.SetLayerRecursively (pai.gameObject, LayerMask.NameToLayer ("PlayerTehai"));
			pai.SetOnClick (OnClickMahjong);
			pai.isPlayer = true;
		} else {
			Utils.SetLayerRecursively (pai.gameObject, LayerMask.NameToLayer ("Default"));
		}

        pai.gameObject.name = pai.ID.ToString();
        tehaiList.Add( pai );

        if( newPicked ) 
            pai.transform.localPosition += new Vector3( NewHaiPosOffsetX, 0, 0 );
    }

	//出牌
    public MahjongPai SuteHai( int index )
    {
        if( index >= 0 && index < tehaiList.Count )
        {
            MahjongPai pai = tehaiList[index];
            tehaiList.RemoveAt(index);

            pai.transform.parent = null;

            return pai;
        }
        return null;
    }


    public void SetAllHaisVisiable(bool visiable) 
    {
        for( int i = 0; i < tehaiList.Count; i++ ) 
        {
            if( visiable ) {
                tehaiList[i].Show();
            } else {
                tehaiList[i].Hide();
            }
        }
        //uiPanel.Update();
    }


    protected readonly static Vector3 SelectStatePosOffset = new Vector3(0f, 20f, 0f);

    protected List<MahjongPai> chiiPaiSelectList = new List<MahjongPai>();



    void OnClickMahjong()
    {
		//Debug.Log ("OnClickMahjong()");
        int index = tehaiList.IndexOf( MahjongPai.current );
		//int index = 0;
		//Debug.Log("OnClickMahjong(" + index.ToString()+")");
        //index = OwnerPlayer.Tehai.getJyunTehaiCount() - 1; // Test: the last one.

        switch(PlayerAction.State)
        {
            case EActionState.Select_Agari:
            case EActionState.Select_Sutehai:
            {//丟牌
                PlayerAction.Response = EResponse.SuteHai;
                PlayerAction.SutehaiIndex = index;

                EventManager.Get().SendEvent(UIEventType.HideMenuList);
                OwnerPlayer.OnPlayerInputFinished();
            }
            break;

            case EActionState.Select_Reach:
            {//聽牌
                PlayerAction.Response = EResponse.Reach;
                PlayerAction.ReachSelectIndex = PlayerAction.ReachHaiIndexList.FindIndex(i => i == index);

                SetEnableStateColor(true);
                EventManager.Get().SendEvent(UIEventType.HideMenuList);
                OwnerPlayer.OnPlayerInputFinished();
            }
            break;

            case EActionState.Select_Kan:
            {//乾
                Hai kanHai = new Hai( MahjongPai.current.ID );
				//Hai kanHai = new Hai( 0);

                if( OwnerPlayer.Tehai.validKaKan( kanHai ) )
                {
                    PlayerAction.Response = EResponse.Kakan;
                }
                else
                {
                    PlayerAction.Response = EResponse.Ankan;
                }

                PlayerAction.KanSelectIndex = PlayerAction.TsumoKanHaiList.FindIndex(h=> h.ID == kanHai.ID);

                SetEnableStateColor(true);
                EventManager.Get().SendEvent(UIEventType.HideMenuList);
                OwnerPlayer.OnPlayerInputFinished();
            }
            break;

            case EActionState.Select_Chii:
            {//吃
                MahjongPai curSelect = MahjongPai.current;
				//MahjongPai curSelect = new MahjongPai();

                if( chiiPaiSelectList.Contains(curSelect) )
                {
                    chiiPaiSelectList.Remove( curSelect );
                    curSelect.transform.localPosition -= SelectStatePosOffset;

                    // check to enable select other chii type pai.
                    List<int> enableIndexList = new List<int>();
                    Hai[] jyunTehais = OwnerPlayer.Tehai.getJyunTehai();

                    for(int i = 0; i < PlayerAction.AllSarashiHais.Count; i++)
                    {
                        for( int j = 0; j < jyunTehais.Length; j++){
                            if( jyunTehais[j].ID == PlayerAction.AllSarashiHais[i].ID )
                                enableIndexList.Add( j );
                        }
                    }

                    EnableInput( enableIndexList );
                }
                else
                {
                    chiiPaiSelectList.Add( curSelect );
					if(curSelect)
                    	curSelect.transform.localPosition += SelectStatePosOffset;

                    if( chiiPaiSelectList.Count >= 2 ) // confirm Chii.
                    {
                        chiiPaiSelectList.Sort( MahjongPaiCompare );

                        if( PlayerAction.SarashiHaiRight.Count >= 2 )
                        {
                            PlayerAction.SarashiHaiRight.Sort( Tehai.Compare );
                            if( chiiPaiSelectList[0].ID == PlayerAction.SarashiHaiRight[0].ID &&
                               chiiPaiSelectList[1].ID == PlayerAction.SarashiHaiRight[1].ID)
                            {
                                PlayerAction.Response = EResponse.Chii_Right;
                                PlayerAction.ChiiSelectType = PlayerAction.Chii_Select_Right;
                                Debug.Log("Chii type is Chii_Right");
                            }
                        }

                        if( PlayerAction.SarashiHaiCenter.Count >= 2 )
                        {
                            PlayerAction.SarashiHaiCenter.Sort( Tehai.Compare );
                            if( chiiPaiSelectList[0].ID == PlayerAction.SarashiHaiCenter[0].ID &&
                               chiiPaiSelectList[1].ID == PlayerAction.SarashiHaiCenter[1].ID)
                            {
                                PlayerAction.Response = EResponse.Chii_Center;
                                PlayerAction.ChiiSelectType = PlayerAction.Chii_Select_Center;
                                Debug.Log("Chii type is Chii_Center");
                            }
                        }

                        if( PlayerAction.SarashiHaiLeft.Count >= 2 )
                        {
                            PlayerAction.SarashiHaiLeft.Sort( Tehai.Compare );
                            if( chiiPaiSelectList[0].ID == PlayerAction.SarashiHaiLeft[0].ID &&
                               chiiPaiSelectList[1].ID == PlayerAction.SarashiHaiLeft[1].ID)
                            {
                                PlayerAction.Response = EResponse.Chii_Left;
                                PlayerAction.ChiiSelectType = PlayerAction.Chii_Select_Left;
                                Debug.Log("Chii type is Chii_Left");
                            }
                        }

                        EventManager.Get().SendEvent(UIEventType.HideMenuList);
                        OwnerPlayer.OnPlayerInputFinished();

                        chiiPaiSelectList.Clear();
                    }
                    else // check to disable select other chii type pai.
                    {
                        List<int> enableIndexList = new List<int>();
                        Hai[] jyunTehais = OwnerPlayer.Tehai.getJyunTehai();

                        int curSelectID = chiiPaiSelectList[0].ID;
                        enableIndexList.Add( index );

                        if( PlayerAction.SarashiHaiRight.Exists(h => h.ID == curSelectID) )
                        {
                            Hai otherHai = PlayerAction.SarashiHaiRight.Find(h => h.ID != curSelectID);

                            for( int i = 0; i < jyunTehais.Length; i++ ){
                                if( jyunTehais[i].ID == otherHai.ID && !enableIndexList.Contains(i) )
                                    enableIndexList.Add( i );
                            }
                        }

                        if( PlayerAction.SarashiHaiCenter.Exists(h => h.ID == curSelectID) )
                        {
                            Hai otherHai = PlayerAction.SarashiHaiCenter.Find(h => h.ID != curSelectID);

                            for( int i = 0; i < jyunTehais.Length; i++ ){
                                if( jyunTehais[i].ID == otherHai.ID && !enableIndexList.Contains(i) )
                                    enableIndexList.Add( i );
                            }
                        }

                        if( PlayerAction.SarashiHaiLeft.Exists(h => h.ID == curSelectID) )
                        {
                            Hai otherHai = PlayerAction.SarashiHaiLeft.Find(h => h.ID != curSelectID);

                            for( int i = 0; i < jyunTehais.Length; i++ ){
                                if( jyunTehais[i].ID == otherHai.ID && !enableIndexList.Contains(i) )
                                    enableIndexList.Add( i );
                            }
                        }

                        EnableInput( enableIndexList );
                    }
                }
            }
            break;
        }
    }

    public static int MahjongPaiCompare(MahjongPai x, MahjongPai y)
    {
        return x.ID - y.ID;
    }

    public void DisableInput(bool updateColor = false)
    {
        for(int i = 0; i < tehaiList.Count; i++)
            tehaiList[i].DisableInput(updateColor);
    }
    public void EnableInput(bool updateColor = false)
    {
        for(int i = 0; i < tehaiList.Count; i++)
            tehaiList[i].EnableInput(updateColor);
    }


    public void DisableInput(List<int> indexList)
    {
        if(indexList == null)
            return;

        EnableInput(true);

        int index = 0;
        for( int i = 0; i < indexList.Count; i++ )
        {
            index = indexList[i];

            if( index >= 0 && index < indexList.Count )
                tehaiList[index].DisableInput(true);
        }
    }
    public void EnableInput(List<int> indexList)
    {
        if(indexList == null)
            return;

        DisableInput(true);

        int index = 0;
        for( int i = 0; i < indexList.Count; i++ )
        {
            index = indexList[i];

            if( index >= 0 && index < tehaiList.Count )
                tehaiList[index].EnableInput(true);
        }
    }

    public void SetEnableStateColor(bool state)
    {
        for(int i = 0; i < tehaiList.Count; i++)
            tehaiList[i].SetEnableStateColor( state );
    }
}
