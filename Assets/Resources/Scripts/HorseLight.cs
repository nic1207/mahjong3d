using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HorseLight : MonoBehaviour {
    public static HorseLight instance;
    public Text _horseLightText_1;
    public Text _horseLightText_2;
    public List<string> _rewardLists;
    public string[] _canMessages; // 當無得獎者 設定的罐頭訊息

    //跑馬燈速度
    [Range(0, 6)]
    public int RunSpeed = 2; 

    [HideInInspector]
    public bool _acceptPass = true; // 允許放行

    private float _horseLightLength_1;
    private float _horseLightLength_2;
    private bool _fixedTimeCheck = false;

    private bool _horseEmpty_1 = true;
    private bool _horseEmpty_2 = true;
    private bool _horseReadyRun_1 = false;
    private bool _horseReadyRun_2 = false;

    private int _cuurSpeed;

    void Start() {
        instance = this;

        ReadyToStart();
    }

    void FixedUpdate() {

        //偵測是否準備起跑
        if (_acceptPass) {
            if (_horseReadyRun_1)
            {
                _horseLightText_1.GetComponent<HorseRun>()._horseRun = true;
                _horseReadyRun_1 = false;
                _acceptPass = false;
            }
            else if (_horseReadyRun_2)
            {
                _horseLightText_2.GetComponent<HorseRun>()._horseRun = true;
                _horseReadyRun_2 = false;
                _acceptPass = false;
            }
            else {
                if (!_fixedTimeCheck)
                    InvokeRepeating("CheckNewRewardList", 3, 3);
            }
        }

    }

    //[0] 進入點
    public void ReadyToStart() {
        if (CheckRewardList())
        {
            CheckEmptyHorse();
        }
        else {
            //無得獎者 設定罐頭訊息
            PutCanMsgToList();
        }

    }

    //[1] 檢查中獎清單
    public bool CheckRewardList() {
        if (_rewardLists.Count == 0)
            return false;
        else if (_rewardLists[0] == "") {
            _rewardLists.RemoveAt(0);
            return false;
        }else
            return true;
    }

    //[2] 檢查目前 Text 何者為空
    private void CheckEmptyHorse() {
        if (_horseEmpty_1) {
			if(_horseLightText_1)
            	_horseLightText_1.text = _rewardLists[0];
            _horseEmpty_1 = false;
            StartCoroutine("CalculateHorseLength", 1);
            _rewardLists.RemoveAt(0);
        }
        else if (_horseEmpty_2) {
			if(_horseLightText_2)
            	_horseLightText_2.text = _rewardLists[0];
            _horseEmpty_2 = false;
            StartCoroutine("CalculateHorseLength", 2);
            _rewardLists.RemoveAt(0);
        }
    }

    //[3] 計算跑馬燈長度
    IEnumerator CalculateHorseLength(int index) {
        yield return new WaitForSeconds(0.5f);

        switch (index) {
			case 1:
			if (_horseLightText_1) {
				_horseLightLength_1 = _horseLightText_1.rectTransform.sizeDelta.x;
				_horseLightText_1.GetComponent<HorseRun> ()._horseLength = _horseLightLength_1;
			}
                break;
			case 2:
			if (_horseLightText_2) {
				_horseLightLength_2 = _horseLightText_2.rectTransform.sizeDelta.x;
				_horseLightText_2.GetComponent<HorseRun> ()._horseLength = _horseLightLength_2;
			}
                break;
            default:
                break;
        }

        ReadyRunHorse(index);
    }

    //[4] 準備發送跑馬燈
    private void ReadyRunHorse(int index) {
        switch (index)
        {
            case 1:
                if (_horseLightLength_1 == 0) {
                    StartCoroutine("CalculateHorseLength", index);
                }
                else {
                    _horseReadyRun_1 = true;
                    //Debug.Log("跑馬燈1長度 = " + _horseLightLength_1 + " 經過時間: " + Time.fixedTime + " 內容 = " + _horseLightText_1.text);
                }
                break;
            case 2:
                if (_horseLightLength_2 == 0) {
                    StartCoroutine("CalculateHorseLength", index);
                }
                else {
                    _horseReadyRun_2 = true;
                    //Debug.Log("跑馬燈2長度 = " + _horseLightLength_2 + " 經過時間: " + Time.fixedTime + " 內容 = " + _horseLightText_2.text);
                }
                break;
            default:
                break;
        }
    }

    //[6] 抵達終點時 清空該Text 且位置回到750 並設置旗標為空
    public void HorseGoal(string textName) {
        switch (textName)
        {
		case "Text_Msg1":
			if (_horseLightText_1) {
				_horseLightText_1.text = "";
				_horseLightText_1.rectTransform.anchoredPosition = new Vector2 (750, _horseLightText_1.rectTransform.anchoredPosition.y);
			}
                _horseEmpty_1 = true;
                break;
		case "Text_Msg2":
			if (_horseLightText_2) {
				_horseLightText_2.text = "";
				_horseLightText_2.rectTransform.anchoredPosition = new Vector2 (750, _horseLightText_2.rectTransform.anchoredPosition.y);
			}
                _horseEmpty_2 = true;
                break;
            default:
                break;
        }
        ReadyToStart();
    }

    //[7] 固定時間檢查有無新名單
    private void CheckNewRewardList() {
        ReadyToStart();
        _fixedTimeCheck = true;
    }

    //[8] 罐頭訊息填入播放清單
    private void PutCanMsgToList() {
        if (_canMessages.Length == 0)
        {
            _rewardLists.Add("恭喜黃大嬸詐胡 獲得9487元");
        }
        else {
            for (int i = 0; i < _canMessages.Length; i++)
            {
                _rewardLists.Add(_canMessages[i]);
            }
        }
    }

    //[9] 暫停& 繼續
    public void IsPlayHorse(bool _isPlay) {
        if (RunSpeed != 0 && !_isPlay)
        {
            _cuurSpeed = RunSpeed;
            RunSpeed = 0;
        }
        else if(RunSpeed == 0 && _isPlay)
            RunSpeed = _cuurSpeed;

        //Debug.Log("目前跑馬燈速度 = " + RunSpeed);
    }

}
