using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[Serializable]
public class Actives {
    public string _activityName = "";
    public Sprite _activityImage;
    public string _activityItemType = "";
    public int _activityItemNum = 0;
}

public class Activity : MonoBehaviour {
    public GameObject actDailyPanel;
    public GameObject actMissionPanel;
    public GameObject _actDailyPrefab;
    public GameObject _actMissionPrefab;
    public Transform actDailyBirthTarget;
    public Transform actMissionBirthTarget;
    public Button _dailyBtn;
    public Button _missionBtn;
    public Sprite[] _buttonBG;
    public Actives[] _dailyActivities; //每日禮物活動數量
    public Actives[] _dailyMissions;   //每日任務活動數量

    private Color _btnGray = new Color(0.7098f, 0.7098f, 0.713f);



    public enum ToggleState { DAILY, MISSION };
    private ToggleState _cuurState = ToggleState.DAILY;

    void Start() {
        InitialCreateActivity();
    }

    //點擊上方切換鈕
    public void ActivityButtonToggle(Button _btn) {
        ToggleState clickState = ActivityToggle(_btn);
        if (_cuurState != clickState)
        {
            switch (clickState)
            {
                case ToggleState.DAILY:
                    _cuurState = ToggleState.DAILY;
                    break;
                case ToggleState.MISSION:
                    _cuurState = ToggleState.MISSION;
                    break;
            }
            ChangeActivityUI();
        }
    }

    public ToggleState ActivityToggle(Button _btn)
    {
        ToggleState clickState = ToggleState.DAILY;
        switch (_btn.name)
        {
            case "Button_Daily":
                clickState = ToggleState.DAILY;
                break;
            case "Button_Mission":
                clickState = ToggleState.MISSION;
                break;
            default:
                Debug.Log("Can't find Right Button, Did you Change Button Name ?");
                break;
        }
        return clickState;
    }

    //改變按鈕樣式
    private void ChangeActivityUI() {
        switch (_cuurState)
        {
            case ToggleState.DAILY:
                actDailyPanel.SetActive(true);
                actMissionPanel.SetActive(false);
                actDailyPanel.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
                _dailyBtn.GetComponent<Image>().sprite = _buttonBG[1];
                _missionBtn.GetComponent<Image>().sprite = _buttonBG[0];
                _dailyBtn.GetComponentInChildren<Text>().color = Color.white;
                _missionBtn.GetComponentInChildren<Text>().color = _btnGray;
                break;
            case ToggleState.MISSION:
                actDailyPanel.SetActive(false);
                actMissionPanel.SetActive(true);
                actMissionPanel.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
                _dailyBtn.GetComponent<Image>().sprite = _buttonBG[0];
                _missionBtn.GetComponent<Image>().sprite = _buttonBG[2];
                _dailyBtn.GetComponentInChildren<Text>().color = _btnGray;
                _missionBtn.GetComponentInChildren<Text>().color = Color.white;
                break;
        }
    }

    //創建活動頁項目
    private void InitialCreateActivity() {
        //每日禮物
        for (int i = 0; i < _dailyActivities.Length; i++)
        {
            GameObject go = Instantiate(_actDailyPrefab);
            ActivityDaily child = go.GetComponent<ActivityDaily>();
            go.transform.SetParent(actDailyBirthTarget);
            child.SetInfo(_dailyActivities[i]._activityName, _dailyActivities[i]._activityImage, _dailyActivities[i]._activityItemType, _dailyActivities[i]._activityItemNum);

            RectTransform rectT = go.GetComponent<RectTransform>();
            rectT.localPosition = Vector3.zero;
            rectT.localScale = Vector3.one;
        }

        //賞金任務
        for (int i = 0; i < _dailyMissions.Length; i++)
        {
            GameObject go = Instantiate(_actMissionPrefab);
            ActivityMission child = go.GetComponent<ActivityMission>();
            go.transform.SetParent(actMissionBirthTarget);
            child.SetInfo(_dailyMissions[i]._activityName, _dailyMissions[i]._activityImage, _dailyMissions[i]._activityItemType, _dailyMissions[i]._activityItemNum);

            RectTransform rectT = go.GetComponent<RectTransform>();
            rectT.localPosition = Vector3.zero;
            rectT.localScale = Vector3.one;
        }
    }

    //Animator 使用 EventTrigger
    public void HideActivityPanel() {
        gameObject.SetActive(false);
    }
}
