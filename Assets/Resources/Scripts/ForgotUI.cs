using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;

public class ForgotUI : MonoBehaviour {
    public GameObject _ForgotPage_1;
    public GameObject _ForgotPage_2;
    public GameObject _ForgotPage_3;
    public static ForgotUI instance;

    public InputField ForgotMail;        // 忘記密碼頁 用戶信箱
    public GameObject ForgotMailHint;    // 忘記密碼頁 用戶信箱提示

    public InputField ForgotConfirmCode; // 忘記密碼頁 認證碼
    public InputField ForgotUsername;    // 忘記密碼頁 用戶名稱
    public InputField ForgotPass_1;      // 忘記密碼頁 密碼
    public InputField ForgotPass_2;      // 忘記密碼頁 密碼確認
    public GameObject ForgotHint;        // 忘記密碼頁 錯誤提示

    //public GameObject ConnectingPanel;   // 連線中視窗

    void Start () {
        instance = this;
    }


    //[0] 離開忘記密碼頁前 重置欄位

    //[1] 發送驗證碼前 信箱格式判斷

    //[2] 上一頁按鈕
    public void GoBackPage(int _cuurPage) {
        switch (_cuurPage) {
            case 2:

                break;
            case 3:
                //清空目前欄位密碼 隱藏題示 按鈕重置

                break;
            default:
                break;
        }
    }

    //發送認證碼
    public void SendConfirm()
    {
        string forgotMail = ForgotMail.text;

        Text ForgotHintText = ForgotMailHint.GetComponent<Text>();
        ForgotHint.SetActive(false);

        //檢查帳號是否合法
        if (!CheckEmail(forgotMail))
        {
            ForgotHintText.text = "信箱格式有誤，請重新輸入";
            ForgotHint.SetActive(true);
            Debug.Log("信箱格式有誤，請重新輸入");
        }
        else
        {
            //ConnectingPanel.SetActive(true); //畫面顯示連線中

            //送出驗證碼 API getAuthCode( mail, TestCallback);
            //Main.Instance.doGetAuthCode(forgotMail, AuthCodeCallback);
        }
    }

    //重置密碼.
    public void ResetPassword()
    {
        //檢查 1.各欄位必填 2.信箱格式 3.兩次密碼
        string forgotMail = ForgotMail.text;
        string forgotConfirmCode = ForgotConfirmCode.text;
        string forgotName = ForgotUsername.text;
        string resetPass1 = ForgotPass_1.text;
        string resetPass2 = ForgotPass_2.text;
        Text ForgotHintText;
        ForgotHint.SetActive(false);

        ForgotHintText = ForgotHint.GetComponent<Text>();
        //檢查帳號是否合法
        if (!CheckEmail(forgotMail))
        {
            ForgotHintText.text = "信箱格式有誤，請重新輸入";
            ForgotHint.SetActive(true);
            Debug.Log("信箱格式有誤，請重新輸入");
        }
        else
        {
            //檢查密碼是否合法
            if (resetPass1 == "" || resetPass2 == "" || forgotName == "" || forgotConfirmCode == "")
            {
                ForgotHintText.text = "欄位不可空白";
                ForgotHint.SetActive(true);
                Debug.Log("欄位不可空白");
            }
            else if (resetPass1 != resetPass2)
            {
                ForgotHintText.text = "密碼兩次不符";
                ForgotHint.SetActive(true);
                Debug.Log("密碼兩次不符");
            }
            else
            {
                //ConnectingPanel.SetActive(true); //畫面顯示連線中
                //以下呼叫 API setForgetPwd( mail, pass, nickname, code, TestCallback);
                //Main.Instance.doSetForgetPwd(forgotMail, resetPass1, forgotName, forgotConfirmCode, ForgotCallback);
            }
        }
    }

    //檢查信箱格式
    public bool CheckEmail(string Str)
    {
        if (Str.IndexOf("@") == -1 || Str.IndexOf("@") < 1 || Str.EndsWith("@") ||
            Str.IndexOf(".") == -1 || Str.IndexOf(".") < 1 || Str.EndsWith(".") ||
            Str.IndexOf(".@") > -1 || Str.IndexOf("@.") > -1 || Str == "")
        {
            //Debug.Log ("帳號檢查沒過");	
            return false;
        }
        else
        {
            //Debug.Log ("帳號檢查有過");	
            return true;
        }
    }

    //發送認證碼 Callback
    private void AuthCodeCallback(WebExceptionStatus status, string result)
    {
        if (status != WebExceptionStatus.Success)
        {
            //ConnectingPanel.SetActive(false); //關閉顯示連線中畫面
            Debug.Log("發送認證碼 AuthCodeCallback Statue != WebExceptionStatus.Success ");
            Debug.Log("Statue = " + status + ", result = " + result);

            ForgotHint.GetComponent<Text>().text = "此帳戶不存在";
            ForgotHint.SetActive(true);
            Debug.Log("此帳戶不存在");

            /* if (result == "Parameter error.")
             {
                ForgotHint.GetComponent<Text>().text = "此帳戶不存在";
                ForgotHint.SetActive(true);
                Debug.Log("此帳戶不存在");
            }
            else if (result == "Already Sent.")
            {
                ForgotHint.GetComponent<Text>().text = "已寄出，請稍後再試";
                ForgotHint.SetActive(true);
                Debug.Log("已寄出，請稍後再試");
            }
            */
        }
        else
        {
            if (result == "Already Sent.")
            {
                //ConnectingPanel.SetActive(false); //關閉顯示連線中畫面
                ForgotHint.GetComponent<Text>().text = "已寄出，請稍後再試";
                ForgotHint.SetActive(true);
                Debug.Log("已寄出，請稍後再試");
            }
            else
            {
                //ConnectingPanel.SetActive(false); //關閉顯示連線中畫面
                ForgotHint.GetComponent<Text>().text = "驗證碼已寄出，請收信確認。\n(若未收到，請於五分後重試。)";
                ForgotHint.SetActive(true);
                Debug.Log("驗證碼已寄出，請收信確認。\n若未收到，請於15分後重試。");
                Debug.Log(result);
            }
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

            ForgotHint.GetComponent<Text>().text = "驗證碼或遊戲名稱錯誤";
            ForgotHint.SetActive(true);
            Debug.Log("驗證碼或遊戲名稱錯誤");

            /*
            if (result == "Parameter error.")
            {
                ForgotHint.GetComponent<Text>().text = "輸入欄位有誤";
                ForgotHint.SetActive(true);
                Debug.Log("輸入欄位有誤");
            }
            else if (result == "Auth Code Error.")
            {
                ForgotHint.GetComponent<Text>().text = "認證碼錯誤";
                ForgotHint.SetActive(true);
                Debug.Log("認證碼錯誤");
            }
            */
        }
        else
        {
            //ConnectingPanel.SetActive(false); //關閉顯示連線中畫面
            Debug.Log("重置成功");
            Debug.Log(result);

            
            //[X] 回登入頁
            //[X] 重置所有欄位
        }
    }
}
