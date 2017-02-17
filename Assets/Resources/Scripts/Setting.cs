using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Setting : MonoBehaviour {
    public GameObject setProfilePanel;
    public GameObject setSettingPanel;
    public GameObject modifySuccessPanel;
    public Button _profileBtn;
    public Button _settingBtn;
    public Sprite[] _buttonBG;

    public Text accountText;
    public InputField nicknameText;

    public enum ToggleState { PROFILE, SETTING };
    private ToggleState _cuurState = ToggleState.PROFILE;
    private Color _btnGray = new Color(0.7098f, 0.7098f, 0.713f);


    void Start() {
        InitialSetting();
    }

    //點擊上方切換鈕
    public void SettingButtonToggle(Button _btn)
    {
        ToggleState clickState = SettingToggle(_btn);
        if (_cuurState != clickState)
        {
            switch (clickState)
            {
                case ToggleState.PROFILE:
                    _cuurState = ToggleState.PROFILE;
                    break;
                case ToggleState.SETTING:
                    _cuurState = ToggleState.SETTING;
                    break;
            }
            ChangeSettingUI();
        }
    }

    public ToggleState SettingToggle(Button _btn)
    {
        ToggleState clickState = ToggleState.PROFILE;
        switch (_btn.name)
        {
            case "Button_Profile":
                clickState = ToggleState.PROFILE;
                break;
            case "Button_Setting":
                clickState = ToggleState.SETTING;
                break;
            default:
                Debug.Log("Can't find Right Button, Did you Change Button Name ?");
                break;
        }
        return clickState;
    }

    //改變按鈕樣式
    private void ChangeSettingUI()
    {
        switch (_cuurState)
        {
            case ToggleState.PROFILE:
                setProfilePanel.SetActive(true);
                setSettingPanel.SetActive(false);
                setProfilePanel.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
                _profileBtn.GetComponent<Image>().sprite = _buttonBG[1];
                _settingBtn.GetComponent<Image>().sprite = _buttonBG[0];
                _profileBtn.GetComponentInChildren<Text>().color = Color.white;
                _settingBtn.GetComponentInChildren<Text>().color = _btnGray;
                break;
            case ToggleState.SETTING:
                setProfilePanel.SetActive(false);
                setSettingPanel.SetActive(true);
                setSettingPanel.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
                _profileBtn.GetComponent<Image>().sprite = _buttonBG[0];
                _settingBtn.GetComponent<Image>().sprite = _buttonBG[1];
                _profileBtn.GetComponentInChildren<Text>().color = _btnGray;
                _settingBtn.GetComponentInChildren<Text>().color = Color.white;
                break;
        }
    }

    //初始化設定
    private void InitialSetting() {
        if (UIManager.instance != null)
        {
            //※ 之後改成API取得用戶資訊 需要有個SceneManager
            accountText.text = UIManager.instance.userAccount;
            accountText.text = UIManager.instance.userNickname;
        }
        else {
            accountText.text = "GUEST";
            nicknameText.text = "路人甲";
        } 
    }

    //改變暱稱
    public void ModifyNickname() {
        //跳出修改成功畫面 ※之後改到API callback中
        modifySuccessPanel.SetActive(true);
    }

    //離開頁面時 重置UI
    public void ResetPageUI() {
        _cuurState = ToggleState.PROFILE;
        ChangeSettingUI();
    }
}
