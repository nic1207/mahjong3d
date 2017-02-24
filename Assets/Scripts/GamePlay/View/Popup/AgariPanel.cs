using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class AgariPanel : MonoBehaviour
{
    public Text lab_player_kaze;
	public Text lab_kyoku;
    //public TehaiUI tehai;
    //public FuuroUI fuuro;

    //public Transform omoteDoraRoot;
    //public Transform uraDoraRoot;

    public Transform yakuRoot;
    public GameObject yakuItemPrefab;
    public Vector2 yakuItemPosOffset = new Vector2( 60f, -40f);
    public float yakuDisplayTime = 0.5f;

	public Text lab_han;
	public Text lab_point;
	public Text lab_level;

    public GameObject _totalHan;     //總台數
    public GameObject _totalWin;     //當手贏得
    public GameObject _totalCoin;    //共得醬幣
    public Transform[] agariPlayers;

    public Button btn_Continue;
    public Text CountDownText;

    private const float haiOffset = 2f;
    private const int DoraHaisColumn = 5;

    private int _coinRate =  60;    // 1台多少
    private float _coinFee = 0.9f;  //手續費抽成
    private int countDownTime = 0; //CountDown second

    //private List<MahjongPai> _omoteDoraHais = new List<MahjongPai>();
    //private List<MahjongPai> _uraDoraHais = new List<MahjongPai>();
    private List<UIYakuItem> _yakuItems = new List<UIYakuItem>();


    private List<AgariUpdateInfo> agariInfoList;
    private AgariUpdateInfo currentAgari;

    #region Init
    void Start()
    {
		if(yakuItemPrefab)
        	yakuItemPrefab.SetActive(false);

        InitYakuInfo();
		if (btn_Continue) {
			//UIEventListener.Get (btn_Continue).onClick = OnClickContinue;
			//
			Text btnTag = btn_Continue.GetComponentInChildren<Text> (true);
			btnTag.text = ResManager.getString ("continue");
			btn_Continue.onClick.AddListener(OnClickContinue);
		}

        HideButtons();
    }


    void HideButtons()
    {
		if (btn_Continue) {
			btn_Continue.interactable = false;
			btn_Continue.gameObject.SetActive (false);
		}
    }

    void InitYakuInfo()
    {
		if(lab_player_kaze)
        	lab_player_kaze.text = "";
		if(lab_kyoku)
        	lab_kyoku.text = "";
		//if(lab_reachbou)
		//	lab_reachbou.text = "";

        ClearYakuItemList( _yakuItems );
		if(lab_han)
        	lab_han.text = "";
		if(lab_point)
			lab_point.text = "";
		if (lab_level) {
			lab_level.text = "";
			//lab_level.alpha = 0f;
		}
    }

    void ClearYakuItemList( List<UIYakuItem> list )
    {
        if( list == null )
            return;

        for( int i = 0; i < list.Count; i++ )
        {
            GameObject.Destroy( list[i].gameObject );
        }
        list.Clear();
    }

    //void SetTenbouInfo( bool state )
    //{
		//if(tenbouInfoRoot)
        //	tenbouInfoRoot.gameObject.SetActive( state );
    //}
    #endregion


    public void Hide()
    {
        gameObject.SetActive(false);

        ClearAgariEffect();
    }

    public void Show( List<AgariUpdateInfo> agariList )
    {

        CheckWhoGun();

        HideButtons();
        gameObject.SetActive(true);

        agariInfoList = agariList;

        StartCoroutine( Show_Internel() );
    }

    private IEnumerator Show_Internel()
    {
        for( int i = 0; i < agariInfoList.Count; i++ )
        {
            currentAgari = agariInfoList[i];

            InitYakuInfo();
            //SetTenbouInfo(false);

            Player player = currentAgari.agariPlayer;

            UpdateKyokuInfo();

            //Hai addHai = currentAgari.agariHai;
            //Hai[] hais = player.Tehai.getJyunTehai();
            //float tehaiPosOffsetX = 0; // move to left if Fuuro has too many DaiMinKan or AnKan.

            yield return StartCoroutine( ShowYakuOneByOne() );

            yield return new WaitForSeconds(3f);
        }

        ShowSkipButton();
        currentAgari = null;
		//OnClickContinue ();//......
    }


    void UpdateKyokuInfo()
    {
		if(lab_player_kaze)
        	lab_player_kaze.text = "Player: " + ResManager.getString( "kaze_" + currentAgari.agariPlayer.JiKaze.ToString().ToLower() );

        string kyokuStr = "";
        string honbaStr = "";

        if( currentAgari.isLastKyoku )
        {
            kyokuStr = ResManager.getString("info_end");
        }
        else
        {
            string kazeStr = ResManager.getString( "kaze_" + currentAgari.bakaze.ToString().ToLower() );
            kyokuStr = kazeStr + currentAgari.kyoku.ToString() + ResManager.getString("kyoku");
        }

        if( currentAgari.honba > 0 )
            honbaStr = currentAgari.honba.ToString() + ResManager.getString("honba");
		if(lab_kyoku)
        	lab_kyoku.text = kyokuStr + "  " + honbaStr;
    }

    IEnumerator ShowYakuOneByOne()
    {
        yield return new WaitForSeconds(1.0f);

        var yakuArr = currentAgari.hanteiYakus;
		if (yakuArr != null) {
			for (int i = 0; i < yakuArr.Length; i++) {
				var yaku = yakuArr [i];

				string yakuName = yaku.getYakuNameKey ();

				UIYakuItem item;

				if (yaku.isYakuman ()) {
					item = CreateYakuItem_Yakuman (yakuName, yaku.isDoubleYakuman ());
				} else {
					item = CreateYakuItem (yakuName, yaku.getHanSuu ());
				}
				if (item) {
					item.transform.parent = yakuRoot;
					item.transform.localScale = yakuItemPrefab.transform.localScale;
					item.transform.localPosition = new Vector3 (yakuItemPosOffset.x, yakuItemPosOffset.y * (i + 1), 0f);

					_yakuItems.Add (item);
				}

				AudioManager.Get ().PlaySFX (AudioConfig.GetSEPath (ESeType.Yaku));

				yield return new WaitForSeconds (yakuDisplayTime);
			}
		}

        yield return new WaitForSeconds( yakuDisplayTime * 0.5f );

        ShowTotalScrote();
    }

    void ShowTotalScrote()
    {
        int yakumanCount = 0;

        var yakuArr = currentAgari.hanteiYakus;
		if (yakuArr != null) {
			for (int i = 0; i < yakuArr.Length; i++) {
				var yaku = yakuArr [i];

				if (yaku.isDoubleYakuman ()) {
					yakumanCount += 2;
				} else if (yaku.isYakuman ()) {
					yakumanCount += 1;
				}
			}
		}


        bool isOya = currentAgari.agariPlayerIsOya;
        int point = currentAgari.agariScore;

        if( yakumanCount > 0 ){
            SetYakuman();

            //int yakumanScore = isOya? currentAgari.scoreInfo.oyaRon : currentAgari.scoreInfo.oyaRon;
        }
        else
        {
            int han = currentAgari.han;
            int fu = currentAgari.fu;

            int level = 0;

            if( han < 5 ){
                if( point >= 12000 )
                    level = 1;
                else if( !isOya && point >= 8000 )
                    level = 1;
                else
                    level = 0;
            }
            else if( han < 6 ){ //5     满贯.
                level = 1;
            }
            else if( han < 8 ){ //6-7   跳满
                level = 2;
            }
            else if( han < 11 ){ //9-10 倍满.
                level = 3;
            }
            else if( han < 13 ){ //11-12 三倍满.
                level = 4;
            }
            else{                     //13 役满.
                level = 5;
            }

            SetHan( han, fu, level );
            //SetPoint( point );
        }

        //StartCoroutine( ShowTenbouInfo() );
    }

    void SetHan( int han, int fu, int level )
    {
        if( level != 0 ){
			if (lab_level) {
				//lab_level.alpha = 1f;
				lab_level.text = ResManager.getString (GetYakuLevelNameKey (level));
			}
        }
        else{
			if (lab_level) {
				lab_level.text = "";
				//lab_level.alpha = 0f;
			}
        }

        string oyaStr = ResManager.getString( currentAgari.agariPlayerIsOya ? "parent" : "child" );
		if (lab_han) {
            //han += currentAgari.agariPlayerIsOya ? 1 : 0; //莊家

            //lab_han.text = oyaStr + "    " + string.Format ("{0}{1}    {2}{3}", 
            //	fu, ResManager.getString ("fu"),
            //	han, ResManager.getString ("han"));

            TotalScoreToggle(true);
            _totalHan.transform.GetChild(0).GetComponent<Text>().text = string.Format("{0}", han);
            CaculateHanToMoney(han);
        }

        PlayLevelVoice(level);
    }

    void SetYakuman()
    {
		if(lab_han)
        	lab_han.text = "";
		if (lab_level) {
			//lab_level.alpha = 1f;
			lab_level.text = ResManager.getString (GetYakuLevelNameKey (5));
		}

        PlayLevelVoice(5);
    }

    string GetYakuLevelNameKey(int level)
    {
        switch( level )
        {
            default:
            case 0: return "";

            case 1: return "mangan";
            case 2: return "haneman";
            case 3: return "baiman";
            case 4: return "sanbaiman";
            case 5: return "yakuman";
        }
    }

    void PlayLevelVoice(int level)
    {
        ECvType cv = ECvType.ManGan;

        switch( level )
        {
            default:
            case 0: return;

            case 1: cv = ECvType.ManGan; break;
            case 2: cv = ECvType.HaReMan; break;
            case 3: cv = ECvType.BaiMan; break;
            case 4: cv = ECvType.SanBaiMan; break;
            case 5: cv = ECvType.YakuMan; break;
        }

        string cvPath = AudioConfig.GetCVPath(currentAgari.agariPlayer.VoiceType, cv);
        AudioManager.Get().PlaySFX( cvPath );
    }

    UIYakuItem CreateYakuItem( string yakuNameKey, int han )
    {
		if (!yakuItemPrefab)
			return null;
        GameObject item = Instantiate( yakuItemPrefab ) as GameObject;
        item.SetActive( true );
        UIYakuItem comp = item.GetComponent<UIYakuItem>();
        comp.SetYaku( yakuNameKey, han );

        return comp;
    }
    UIYakuItem CreateYakuItem_Yakuman( string yakuNameKey, bool doubleYakuman )
    {
        GameObject item = Instantiate( yakuItemPrefab ) as GameObject;
        item.SetActive( true );
        UIYakuItem comp = item.GetComponent<UIYakuItem>();
        comp.SetYakuMan( yakuNameKey, doubleYakuman );

        return comp;
    }

	/*
    IEnumerator ShowTenbouInfo()
    {
        yield return new WaitForSeconds(1f);
		//if(lab_reachbou)
        //	lab_reachbou.text = "x" + currentAgari.reachBou.ToString();

        SetTenbouInfo(true);


        var tenbouInfos = currentAgari.tenbouChangeInfoList;
        EKaze nextKaze = currentAgari.manKaze;

        for( int i = 0; i < playerTenbouList.Count; i++ )
        {
            PlayerTenbouChangeInfo info = tenbouInfos.Find( ptci=> ptci.playerKaze == nextKaze );
            playerTenbouList[i].SetInfo( info.playerKaze, info.current, info.changed );
            nextKaze = nextKaze.Next();
        }

    }
    */

    void ShowSkipButton()
    {
		if (btn_Continue) {
			btn_Continue.gameObject.SetActive (true);
            // 按鈕透明度轉變
            //TweenAlpha.Begin(btn_Continue.gameObject, 0.5f, 1f).SetOnFinished(() =>  
            //{
                //btn_Continue.GetComponent<BoxCollider> ().enabled = true;
                btn_Continue.interactable = true;
            //});

            ShowCountDown(9); //按鈕倒數
        }
    }

    private void ShowCountDown(int num)
    {
        countDownTime = num;
        CountDownText.text = countDownTime.ToString();
        StopCoroutine("countDown");
        StartCoroutine("countDown");
    }

    //確定鈕的倒數計時
    private IEnumerator countDown()
    {
        while (countDownTime > 0)
        {
            yield return new WaitForSeconds(1);
            countDownTime--;
            if (countDownTime > 0 && CountDownText != null && countDownTime < 10)
                CountDownText.text = countDownTime.ToString();
            else {
                OnClickContinue();
            }
        }
    }

    //是否顯示結算總分
    private void TotalScoreToggle(bool _isShowT) {
        _totalHan.SetActive(_isShowT);
        _totalWin.SetActive(_isShowT);
        _totalCoin.SetActive(_isShowT);
    }


    void OnClickContinue()
    {
        StopCoroutine("countDown");

        Hide();

		EventManager.Instance.RpcSendEvent(UIEventType.End_Kyoku);
        RecordPreTedasi._instance.Clear(); //重置贏家
        TotalScoreToggle(false);
    }

    //轉換台數為醬幣
    private void CaculateHanToMoney(int _han) {
        int _getCoin = (int) Math.Ceiling( _han * _coinRate  * _coinFee);
        //Debug.Log("_getCoin = " + _getCoin);

        _totalWin.transform.GetChild(0).GetComponent<Text>().text = string.Format("{0}", _han * _coinRate);
        _totalCoin.transform.GetChild(0).GetComponent<Text>().text = string.Format("{0}", _getCoin);

        CaculateCoin(_getCoin);
    }

    private void CheckWhoGun() {
        int _gunPlayer = RecordPreTedasi._instance.PreTedasiPlayerIndex;
        int _ronPlayer = RecordPreTedasi._instance.RonPlayerIndex;
        int _tsumonPlayer = RecordPreTedasi._instance.TsumoPlayerIndex;
        Debug.Log("放槍的人 = " + _gunPlayer);    //列出誰放槍
        Debug.Log("胡的人 = " + _ronPlayer);      //列出誰胡
        Debug.Log("自摸的人 = " + _tsumonPlayer); //列出誰自摸

        if (_tsumonPlayer != -1)
        {
            for (int i = 0; i < agariPlayers.Length; i++)
            {
                if (i == _tsumonPlayer)
                    agariPlayers[i].GetComponent<AgariPlayer>().SetPlayerWinUI();
                else
                    agariPlayers[i].GetComponent<AgariPlayer>().SetPlayerLoseUI();
            }
        }
        else {
            for (int i = 0; i < agariPlayers.Length; i++)
            {
                if(i == _ronPlayer)
                    agariPlayers[i].GetComponent<AgariPlayer>().SetPlayerWinUI();
                else if (i == _gunPlayer)
                    agariPlayers[i].GetComponent<AgariPlayer>().SetPlayerLoseUI();
                else
                    agariPlayers[i].GetComponent<AgariPlayer>().SetPlayerDrawUI();
            }
        }
    }

    private void CaculateCoin(int _getCoin) {
        int _gunPlayer = RecordPreTedasi._instance.PreTedasiPlayerIndex;
        int _ronPlayer = RecordPreTedasi._instance.RonPlayerIndex;
        int _tsumonPlayer = RecordPreTedasi._instance.TsumoPlayerIndex;

        if (_tsumonPlayer != -1)
        {
            for (int i = 0; i < agariPlayers.Length; i++)
            {
                if (i == _tsumonPlayer)
                    agariPlayers[i].GetComponent<AgariPlayer>().AddCoin(_getCoin * 3);
                else
                    agariPlayers[i].GetComponent<AgariPlayer>().ReduceCoin(_getCoin);
            }
        }
        else
        {
            for (int i = 0; i < agariPlayers.Length; i++)
            {
                if (i == _ronPlayer)
                    agariPlayers[i].GetComponent<AgariPlayer>().AddCoin(_getCoin);
                else if (i == _gunPlayer)
                    agariPlayers[i].GetComponent<AgariPlayer>().ReduceCoin(_getCoin);
                else
                    agariPlayers[i].GetComponent<AgariPlayer>().ClearCoin();
            }
        }
    }

    private void ClearAgariEffect() {
        for (int i = 0; i < agariPlayers.Length; i++)
        {
            agariPlayers[i].GetComponent<AgariPlayer>().HideEffect();
        }
    }
}
