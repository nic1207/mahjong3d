using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActivityMission : MonoBehaviour {
    public Image _missionActImg;
    public Text _missionActTitleText;
    public Text _missionActNumText;
    public Text _missionActTypeText;

    private string _missionActItemType;
    private int _missionActItemNum;


    public void SetInfo(string _activityName, Sprite _activitySprite, string _activityItemType, int _activityItemNum)
    {
        _missionActTitleText.text = _activityName;
        _missionActImg.sprite = _activitySprite;
        _missionActNumText.text = _activityItemNum.ToString();

        _missionActItemNum = _activityItemNum;
        _missionActItemType = _activityItemType;
    }

    public void GoMissionPage()
    {
        //進入對應活動(待實作)

        Lobby_UIManager.instance.CallReceviedPanel(_missionActItemType, _missionActItemNum);
    }

}
