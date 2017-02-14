using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActivityDaily : MonoBehaviour {
    public Button _getButton;
    public Sprite[] _buttonBG;
    public Image _dailyActImg;
    public Text _dailyActText;

    public enum GetState { NEW, GOT };

    private GetState _cuurState = GetState.NEW;
    private string _dailyActItemType;
    private int _dailyActItemNum;


    public void SetInfo(string _activityName, Sprite _activitySprite, string _activityItemType, int _activityItemNum) {
        _dailyActText.text = _activityName;
        _dailyActImg.sprite = _activitySprite;
        _dailyActItemType = _activityItemType;
        _dailyActItemNum = _activityItemNum;
    }

    public void GetDailyPresent() {
        if (_cuurState == GetState.NEW) {
            //獲得對應禮物(待實作)

            _getButton.enabled = false;
            _getButton.GetComponent<Image>().sprite = _buttonBG[1];
            _getButton.GetComponentInChildren<Text>().text = "已領取";
            _getButton.GetComponentInChildren<Text>().color = Color.gray;

            _cuurState = GetState.GOT;

            Lobby_UIManager.instance.CallReceviedPanel(_dailyActItemType, _dailyActItemNum);
        }
    }

}
