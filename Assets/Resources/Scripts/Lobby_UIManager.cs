using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Prototype.NetworkLobby;

public class Lobby_UIManager : MonoBehaviour {
    public static Lobby_UIManager instance;
	public LobbyManager lobbyManager;
    public Text[] _buttonTexts;
    public Text _buttonMoreText;
    public GameObject birdConnectingMask;
    public GameObject receviedMask; //獲得禮物畫面
    public GameObject buttonBGMask;
    public Animator moreBtnPanelAnimator;
    public Animator lobbyAnimator;
    public Animator activityAnimator;
    public GameObject activityPanel;    //活動頁
    public GameObject settingPanel;     //設定頁
    public GameObject particleEffects;  //粒子特效

    private Color _duckYellow = new Color(0.952f, 0.596f, 0, 1);

    void Start() {
        instance = this;

        //關閉連線視窗
        birdConnectingMask.SetActive(false);

        //particleEffects.SetActive(true); //開啟粒子特效
    }

	void OnEnable() {
		print("OnEnable()");
		particleEffects.SetActive(true);
	}

	void OnDisable() {
		print("OnDisable()");
		particleEffects.SetActive(false);
	}


    //大廳-按鈕點擊變色
    public void ChangeTextColor(Button targetBtn) {
        ResetAllBtnColor();
        targetBtn.gameObject.GetComponentInChildren<Text>().color = _duckYellow;
    }

    //點擊按鈕背景遮罩
    public void ClickBackgroundMask()
    {
        ResetAllBtnColor();
        buttonBGMask.SetActive(false);
        moreBtnPanelAnimator.SetBool("OpenMorePanel", false);
    }

    //保留更多鈕顏色
    public void RemainMoreBtnTextColor()
    {
        _buttonMoreText.color = _duckYellow;
    }

    //點擊更多按鈕
    private void ClickButtomMore()
    {
        //Debug.Log("!buttonBGMask.activeSelf = " + !buttonBGMask.activeSelf);
        //開啟背景遮罩
        buttonBGMask.SetActive(!buttonBGMask.activeSelf);
        moreBtnPanelAnimator.SetBool("OpenMorePanel", buttonBGMask.activeSelf);
    }

    //點擊 更多-排行榜
    public void ClickToolbarBtn(Button targetBtn) {
		Debug.Log ("ClickToolbarBtn()");
        if (targetBtn.name != "Button_More" && buttonBGMask.activeSelf) {
            //ClickBackgroundMask();
            StartCoroutine("DelayCloseBGMask");
        }

        ResetAllBtnColor();
        targetBtn.gameObject.GetComponentInChildren<Text>().color = _duckYellow;

        switch (targetBtn.name) {
            case "Button_Billboard":
                //點擊 排行榜

                break;
            case "Button_Activity":
                //點擊 活動
                //HorseLight.instance.IsPlayHorse(false); //暫停跑馬燈
                GoActivity();
                break;
            case "Button_Charge":
                //點擊 儲值

                break;
            case "Button_Message":
                //點擊 訊息通知

                break;
            case "Button_More":
                //點擊 更多
                //HorseLight.instance.IsPlayHorse(false); //暫停跑馬燈
                ClickButtomMore();
                break;

            case "Button_Setting":
                //點擊 更多 - 設定
                RemainMoreBtnTextColor();
                GoSetting();
                break;
            case "Button_Explain":
                //點擊 更多 - 說明
                RemainMoreBtnTextColor();
                break;
            case "Button_Question":
                //點擊 更多 - 問題
                RemainMoreBtnTextColor();
                break;
            case "Button_Invite":
                //點擊 更多 - 邀請好友
                RemainMoreBtnTextColor();
                break;
            case "Button_Record":
                //點擊 更多 - 紀錄
                RemainMoreBtnTextColor();
                break;
            default:
                //點擊  ???

                break;
        }
    }

    //重置所有按鈕顏色
    public void ResetAllBtnColor() {
        foreach (Text _text in _buttonTexts) {
            _text.color = Color.white;
        }

        //HorseLight.instance.IsPlayHorse(true); //啟動跑馬燈
    }

    IEnumerator DelayCloseBGMask() {
        yield return new WaitForSeconds(0.1f);
        ClickBackgroundMask();
    }

    //點擊開桌 準備進入遊戲畫面
    public void StartSetConnecting() {
		Debug.Log ("StartSetConnecting()");
        ResetAllBtnColor();
        particleEffects.SetActive(false); //關閉大廳粒子特效

		lobbyManager.StartMatchMaker();
		lobbyManager.matchMaker.CreateMatch(
			//matchNameInput.text,
			"oekfewodkosdkcosdf",
			(uint)lobbyManager.maxPlayers,
			true,
			"",
			lobbyManager.OnMatchCreate);

		lobbyManager.backDelegate = lobbyManager.StopHost;
		lobbyManager._isMatchmaking = true;
		lobbyManager.DisplayIsConnecting();

		lobbyManager.SetServerInfo("Matchmaker Host", lobbyManager.matchHost);
        //birdConnectingMask.SetActive(true);
    }

    //載入畫面載入完畢
    public void SetConnectingDone()
    {
        lobbyAnimator.SetBool("ConnectingDone", true);
    }

    //開啟活動頁
    private void GoActivity() {
        activityPanel.GetComponent<Activity>().ResetPageUI(); //活動頁:卷軸置頂 預設每日禮物
        particleEffects.SetActive(false); //關閉大廳粒子特效
        activityPanel.SetActive(true);
        activityAnimator.SetBool("ActivitySlideIn", true);
    }

    //離開活動頁
    public void ExitActivity()
    {
        ResetAllBtnColor();
        activityAnimator.SetBool("ActivitySlideIn", false);
        //particleEffects.SetActive(true); //開啟大廳粒子特效
        //HorseLight.instance.IsPlayHorse(true); //啟動跑馬燈
    }

    //執行獲得獎品面板
    public void CallReceviedPanel(string _itemType, int _itemNum) {
        receviedMask.GetComponent<ReceivedMask>().ShowReceived(_itemType, _itemNum);
    }

    //進入設定頁
    public void GoSetting() {
        settingPanel.GetComponent<Setting>().ResetPageUI(); //設定頁:卷軸置頂 預設個人資訊
        particleEffects.SetActive(false); //關閉大廳粒子特效
        settingPanel.SetActive(true);
        settingPanel.GetComponent<Animator>().SetBool("ActivitySlideIn", true);
    }

    //離開設定頁
    public void ExitSetting() {
        ResetAllBtnColor();
        //particleEffects.SetActive(true); //開啟大廳粒子特效
        //HorseLight.instance.IsPlayHorse(true); //啟動跑馬燈
        settingPanel.GetComponent<Animator>().SetBool("ActivitySlideIn", false);
    }
}
