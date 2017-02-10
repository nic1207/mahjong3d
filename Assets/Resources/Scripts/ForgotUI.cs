using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;

public class ForgotUI : MonoBehaviour
{
    public GameObject _ForgotPage_1;
    public GameObject _ForgotPage_2;
    public static ForgotUI instance;

    public InputField ForgotMail;        // 忘記密碼頁 用戶信箱
    public GameObject ForgotMailHint;    // 忘記密碼頁 用戶信箱提示
    public GameObject ForgotTimeHint;    // 忘記密碼頁 剩餘時間提示
    public Button buttonSendCode;        // 忘記密碼頁 發送驗證碼鈕
    public Sprite[] changeForgoButtonBG;
    public InputField ForgotConfirmCode; // 忘記密碼頁 驗證碼
    public GameObject ForgotCodeHint;    // 忘記密碼頁 驗證碼錯誤提示
    public Button buttonAccept;          // 忘記密碼頁 驗證碼確定鈕

    public InputField ForgotPass_1;      // 忘記密碼頁 新密碼
    public InputField ForgotPass_2;      // 忘記密碼頁 密碼確認
    public Image ForgotPassEye_1;       // 忘記密碼頁 新密碼顯示按鈕1
    public Image ForgotPassEye_2;       // 忘記密碼頁 新密碼顯示按鈕2
    public Text _PassText;
    public Text _ConfirmPassText;
    public GameObject ForgotPassHint;    // 忘記密碼頁 錯誤提示
    public Button buttonPassAccept;      // 忘記密碼頁 密碼確定鈕
    public Sprite[] changePassButtonBG;
    public Sprite[] EyeButton;

    private Color _ColorOrange = new Color(0.90588f, 0.02745f, 0.070588f);
    //public GameObject ConnectingPanel;   // 連線中視窗

    void Start()
    {
        instance = this;
    }

    //前往重設密碼頁
    private void GoResetPassPage()
    {
        _ForgotPage_2.SetActive(true);
    }

    //[1] 發送驗證碼按鈕
    public void SendConfirm()
    {
        string forgotMail = ForgotMail.text;

        Text ForgotHintText = ForgotMailHint.GetComponent<Text>();

        //檢查帳號是否合法
        if (!CheckEmail(forgotMail))
        {
            ShowMailHint("信箱格式有誤，請重新輸入");
        }
        else
        {
            //ConnectingPanel.SetActive(true); //畫面顯示連線中

            //送出驗證碼 API getAuthCode( mail, Callback);
            //Main.Instance.doGetAuthCode(forgotMail, AuthCodeCallback);

            //成功後改變按鈕UI (之後放到CB中)
            LockSendAuthCodeBtn(); //發送驗證碼按鈕變色
            AuthCodeTime.instance.StartTimer(); //開始倒數
            ShowMailHint("驗證碼已發送，請至信箱收信");
        }
    }

    //發送驗證碼 Callback
    private void AuthCodeCallback(WebExceptionStatus status, string result)
    {
        if (status != WebExceptionStatus.Success)
        {
            //ConnectingPanel.SetActive(false); //關閉顯示連線中畫面
            Debug.Log("發送驗證碼 AuthCodeCallback Statue != WebExceptionStatus.Success ");
            Debug.Log("Statue = " + status + ", result = " + result);

            ShowMailHint("此帳戶不存在");
        }
        else
        {
            if (result == "Already Sent.")
            {
                //ConnectingPanel.SetActive(false); //關閉顯示連線中畫面
                ShowMailHint("已寄出，請稍後再試");
            }
            else
            {
                //ConnectingPanel.SetActive(false); //關閉顯示連線中畫面
                ShowMailHint("驗證碼已發送，請至信箱收信");

                Debug.Log(result);
            }
        }
    }

    //[2] 確定驗證碼
    public void AcceptAuthCode()
    {
        //ConnectingPanel.SetActive(true); //畫面顯示連線中
        //以下呼叫 API setAuthCode( mail, code, Callback);
        //Main.Instance.doSetAuthCode(forgotMail, forgotConfirmCode, ForgotCodeCallback);

        //到下一頁 (之後放到CB中)
        GoResetPassPage();
    }

    //確認驗證碼 Callback
    private void ForgotCodeCallback(WebExceptionStatus status, string result)
    {

        if (status != WebExceptionStatus.Success)
        {
            //ConnectingPanel.SetActive(false); //關閉顯示連線中畫面
            Debug.Log("確認驗證碼 ForgotCodeCallback Statue != WebExceptionStatus.Success ");
            Debug.Log("Statue = " + status + ", result = " + result);

            ForgotConfirmCode.text = ""; //清空驗證碼欄位
            LockAcceptAuthCodeBtn();
            ShowCodeHint("輸入錯誤，請重新輸入");
        }
        else
        {
            //ConnectingPanel.SetActive(false); //關閉顯示連線中畫面
            Debug.Log("確認驗證碼成功");
            Debug.Log(result);

            //到下一頁
            GoResetPassPage();
        }
    }

    //檢查信箱格式
    public bool CheckEmail(string Str)
    {
        if (Str.IndexOf("@") == -1 || Str.IndexOf("@") < 1 || Str.EndsWith("@") ||
            Str.IndexOf(".") == -1 || Str.IndexOf(".") < 1 || Str.EndsWith(".") ||
            Str.IndexOf(".@") > -1 || Str.IndexOf("@.") > -1 || Str == "")
            return false;
        else
            return true;
    }

    //依傳入字串顯示文字題示
    private void ShowMailHint(string _str)
    {
        ForgotMailHint.GetComponent<Text>().text = _str;
        ForgotMailHint.SetActive(true);
        Debug.Log(_str);
    }

    //依傳入字串顯示文字題示
    private void ShowCodeHint(string _str)
    {
        ForgotCodeHint.GetComponent<Text>().text = _str;
        ForgotCodeHint.SetActive(true);
        Debug.Log(_str);
    }

    //發送驗證碼按鈕 UI變化
    public void LockSendAuthCodeBtn()
    {
        buttonSendCode.GetComponent<Image>().sprite = changeForgoButtonBG[0];
        buttonSendCode.GetComponent<Button>().enabled = false;
        buttonSendCode.GetComponentInChildren<Text>().color = Color.gray;
    }

    //發送驗證碼按鈕 UI變化
    public void UnLockSendAuthCodeBtn()
    {
        buttonSendCode.GetComponent<Image>().sprite = changeForgoButtonBG[1];
        buttonSendCode.GetComponent<Button>().enabled = true;
        buttonSendCode.GetComponentInChildren<Text>().color = Color.white;
    }

    //檢查Email Input欄位內容
    public void CheckMailInputContent()
    {
        ForgotMailHint.SetActive(false);
        if (ForgotMail.text != "")
            UnLockSendAuthCodeBtn();
        else
            LockSendAuthCodeBtn();
    }

    //檢查Email Input欄位內容
    public void CheckCodeInputContent()
    {
        ForgotCodeHint.SetActive(false);
        if (ForgotConfirmCode.text.Length == 5) //※確認驗證碼長度後可調整Input限制
            UnLockAcceptAuthCodeBtn();
        else
            LockAcceptAuthCodeBtn();
    }

    //確認驗證碼按鈕 UI變化
    public void LockAcceptAuthCodeBtn()
    {
        buttonAccept.GetComponent<Image>().sprite = changeForgoButtonBG[0];
        buttonAccept.GetComponent<Button>().enabled = false;
        buttonAccept.GetComponentInChildren<Text>().color = Color.gray;
    }

    //確認驗證碼按鈕 UI變化
    public void UnLockAcceptAuthCodeBtn()
    {
        buttonAccept.GetComponent<Image>().sprite = changeForgoButtonBG[2];
        buttonAccept.GetComponent<Button>().enabled = true;
        buttonAccept.GetComponentInChildren<Text>().color = Color.white;
    }


    //[3] 重置密碼頁 上一頁按鈕
    public void GoBackPage()
    {
        _ForgotPage_2.SetActive(false);
    }

    //重置密碼.
    public void ResetPassword()
    {
        //檢查 1.各欄位必填 2.兩次密碼
        string resetPass1 = ForgotPass_1.text;
        string resetPass2 = ForgotPass_2.text;

        Text ForgotHintText = ForgotPassHint.GetComponent<Text>();

        //檢查密碼是否合法
        if (resetPass1 != resetPass2)
        {
            _ConfirmPassText.color = _ColorOrange;
            LockChangeBtn();
            ShowPassHint("密碼不符，請重新輸入");
        }
        else
        {
            //ConnectingPanel.SetActive(true); //畫面顯示連線中
            //以下呼叫 API setForgetPwd(mail, pass, Callback);
            //Main.Instance.doSetForgetPwd(forgotMail, resetPass1, ForgotCallback);

            //重置所有欄位 回登入頁 (之後放到CB中)
            UIManager.instance.ExitForgotPage();
        }
    }

    //忘記密碼 Callback
    private void ForgotCallback(WebExceptionStatus status, string result)
    {
        if (status != WebExceptionStatus.Success)
        {
            //ConnectingPanel.SetActive(false); //關閉顯示連線中畫面
            Debug.Log("忘記密碼 重置 ForgotCallback Statue != WebExceptionStatus.Success ");
            Debug.Log("Statue = " + status + ", result = " + result);

            ShowPassHint("不明原因錯誤，請稍後再試");
        }
        else
        {
            //ConnectingPanel.SetActive(false); //關閉顯示連線中畫面
            Debug.Log("重置成功");
            Debug.Log(result);


            //重置所有欄位 回登入頁
            UIManager.instance.ExitForgotPage();
        }
    }

    // 密碼眼睛鈕
    public void PassButtonEyeToggle(int _index) {
        switch (_index) {
            case 1:
                if (ForgotPassEye_1.sprite == EyeButton[0])
                {
                    ShowHideToogle(_index, true);
                    ForgotPassEye_1.sprite = EyeButton[1];
                }
                else {
                    ShowHideToogle(_index, false);
                    ForgotPassEye_1.sprite = EyeButton[0];
                }
                break;
            case 2:
                if (ForgotPassEye_2.sprite == EyeButton[0])
                {
                    ShowHideToogle(_index, true);
                    ForgotPassEye_2.sprite = EyeButton[1];
                }
                else
                {
                    ShowHideToogle(_index, false);
                    ForgotPassEye_2.sprite = EyeButton[0];
                }
                break;
            default:
                break;
        }
    }

    private void ShowHideToogle(int _index, bool _isShow) {
        switch (_index) {
            case 1:
                if (_isShow) {
                    //ForgotPass_1.contentType = InputField.ContentType.Custom;
                    ForgotPass_1.inputType = InputField.InputType.Standard;
                    _PassText.text = ForgotPass_1.text;
                }
                else {
                    //ForgotPass_1.contentType = InputField.ContentType.Custom;
                    ForgotPass_1.inputType = InputField.InputType.Password;
                    _PassText.text = "";
                    for (int i = 0; i < ForgotPass_1.ToString().Length; i++)
                    {
                        _PassText.text += "*";
                    }
                }
                break;
            case 2:
                if (_isShow)
                {
                    //ForgotPass_2.contentType = InputField.ContentType.Custom;
                    ForgotPass_2.inputType = InputField.InputType.Standard;
                    _ConfirmPassText.text = ForgotPass_1.text;
                }
                else
                {
                    //ForgotPass_2.contentType = InputField.ContentType.Custom;
                    ForgotPass_2.inputType = InputField.InputType.Password;
                    _ConfirmPassText.text = "";
                    for (int i = 0; i < ForgotPass_2.ToString().Length; i++)
                    {
                        _ConfirmPassText.text += "*";
                    }
                }
                break;
            default:
                break;
        }
    }

    //檢查 Pass、ConfirmPass Input欄位內容
    public void CheckPassInputContent()
    {
        ForgotPassHint.SetActive(false);
        _ConfirmPassText.color = Color.black;
        if (ForgotPass_1.text.Length >= 6 && ForgotPass_2.text.Length >= 6)
            UnLockChangeBtn();
        else
            LockChangeBtn();
    }

    //確認變更密碼按鈕 UI變化
    private void UnLockChangeBtn() {
        buttonPassAccept.GetComponent<Image>().sprite = changePassButtonBG[1];
        buttonPassAccept.GetComponent<Button>().enabled = true;
        buttonPassAccept.GetComponentInChildren<Text>().color = Color.white;
    }

    //確認變更密碼按鈕 UI變化
    private void LockChangeBtn()
    {
        buttonPassAccept.GetComponent<Image>().sprite = changePassButtonBG[0];
        buttonPassAccept.GetComponent<Button>().enabled = true;
        buttonPassAccept.GetComponentInChildren<Text>().color = Color.gray;
    }

    //依傳入字串顯示文字題示
    private void ShowPassHint(string _str)
    {
        ForgotPassHint.GetComponent<Text>().text = _str;
        ForgotPassHint.SetActive(true);
        //Debug.Log(_str);
    }

    //[0] 離開忘記密碼頁前 重置欄位
    public void ResetAllInput()
    {
        ForgotMail.text = "";
        ForgotConfirmCode.text = "";
        ForgotPass_1.text = "";
        ForgotPass_2.text = "";
        ShowHideToogle(1, false);
        ForgotPassEye_1.sprite = EyeButton[0];
        ShowHideToogle(2, false);
        ForgotPassEye_2.sprite = EyeButton[0];
        _ForgotPage_2.SetActive(false);
    }
}