using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class RegisterUI : MonoBehaviour {
    static public RegisterUI Instance;

    public InputField ClubRegisterNickname; // Club註冊頁 用戶暱稱
    public InputField ClubRegisterAccount;  // Club註冊頁 用戶帳號
    public InputField ClubRegisterPass_1;   // Club註冊頁 密碼
    public InputField ClubRegisterPass_2;   // Club註冊頁 密碼確認
    public InputField ClubRegisterMail;     // Club註冊頁 用戶信箱
    public InputField ClubRegisterPhone;    // Club註冊頁 用戶手機
    public Toggle ClubAgreeRule;            // Club註冊頁 同意條款
    public GameObject ClubRegisterHint;     // Club註冊頁 錯誤提示

    public string[] _canNickName; //罐頭暱稱
    private string _defaultNickName = "路人甲"; //預設暱稱

    void Awake()
    {
        Instance = this;
    }

    //註冊頁-確定鈕
    public void ClubRegisterJoin()
    {
        string registerNickname = ClubRegisterNickname.text;
        string registerAccount = ClubRegisterAccount.text.ToLower();
        string registerPass1 = ClubRegisterPass_1.text;
        string registerPass2 = ClubRegisterPass_2.text;
        string registerMail = ClubRegisterMail.text;
        string registerPhone = ClubRegisterPhone.text;
        bool registerAgree = ClubAgreeRule.isOn;
        ClubRegisterHint.SetActive(false);

        //檢查欄位是否合法
        if (registerAccount == "") {
            ClubRegisterHint.GetComponent<Text>().text = "帳號欄位不可空白";
            ClubRegisterHint.SetActive(true);
            Debug.Log("帳號欄位不可空白");
        }
        else if (!CheckName(registerAccount))
        {
            ClubRegisterHint.GetComponent<Text>().text = "帳號格式錯誤";
            ClubRegisterHint.SetActive(true);
            Debug.Log("帳號格式錯誤");
        }
        else if (!CheckEmail(registerMail))
        {
            ClubRegisterHint.GetComponent<Text>().text = "信箱格式錯誤";
            ClubRegisterHint.SetActive(true);
            Debug.Log("信箱格式錯誤");
        }
        else
        {
            //檢查密碼是否合法
            if (registerPass1 == "" || registerPass2 == "")
            {
                ClubRegisterHint.GetComponent<Text>().text = "密碼欄位不可空白";
                ClubRegisterHint.SetActive(true);
                Debug.Log("密碼欄位不可空白");
            }
            else if (registerPass1 != registerPass2)
            {
                ClubRegisterHint.GetComponent<Text>().text = "密碼兩次不符";
                ClubRegisterHint.SetActive(true);
                Debug.Log("密碼兩次不符");
            }
            else if (!registerAgree) {
                ClubRegisterHint.GetComponent<Text>().text = "同意鈕不勾選不給註冊";
                ClubRegisterHint.SetActive(true);
                Debug.Log("同意鈕不勾選不給註冊");
            }
            else
            {
                //把暱稱傳給UIManager管理
                UIManager.instance.userAccount = CheckNickName();
                //Debug.Log("暱稱為 " + CheckNickName());

                //以下呼叫 API(registerMail, registerPass1)
                //ykiAPI = this.GetComponent<YkiApi>();
                //ykiAPI.AddMember(registerMail, registerPass1, CheckNickName(), RegisterCallback);
                //ConnectingPanel.SetActive(true); //畫面顯示連線中
                //Main.Instance.doAddMember(registerMail, registerPass1, CheckNickName(), RegisterCallback);

                UIManager.instance.ExitRegisterPage(); //離開註冊頁面

                ResetAllInput();
            }
        }
    }

    //檢查帳號格式 (6-14 小寫英文+數字)
    public bool CheckName(string str)
    {

        if (str.Length < 1)
        {
            return false;
        }
        else
        {
            //檢查第一個字元是否為數字
            bool isN = IsNumeric(str.Substring(0, 1));

            //帳號長度 6-14 首位不得位數字
            if (str == "" || str.Length < 6 || isN)
                return false;
            else
                return true;
        }
    }

    //定義一個函數,作用:判斷strNumber是否為數字,是數字返回True,不是數字返回False
    public bool IsNumeric(string strNumber)
    {
        Regex NumberPattern = new Regex("[^0-9.-]");
        return !NumberPattern.IsMatch(strNumber);
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

    //註冊 Callback
    private void RegisterCallback(WebExceptionStatus status, string result)
    {
        //ConnectingPanel.SetActive(false); //關閉顯示連線中畫面
        if (status != WebExceptionStatus.Success)
        {
            ClubRegisterHint.GetComponent<Text>().text = "此帳號已存在";
            ClubRegisterHint.SetActive(true);
            Debug.Log("註冊失敗! " + result);
        }
        else
        {
            Debug.Log("註冊成功! " + result);

            UIManager.instance.ExitRegisterPage(); //離開註冊頁面

            ResetAllInput();
        }
    }

    public void ResetAllInput() {
        ClubRegisterNickname.text = "";
        ClubRegisterAccount.text = "";
        ClubRegisterPass_1.text = "";
        ClubRegisterPass_2.text = "";
        ClubRegisterMail.text = "";
        ClubRegisterPhone.text = "";
        ClubAgreeRule.isOn = false;
        ClubRegisterHint.SetActive(false);
    }

    //判斷有無填暱稱欄位
    private string CheckNickName() {
        if (ClubRegisterNickname.text != "")
        {
            return ClubRegisterNickname.text;
        }
        else if (_canNickName.Length == 0)
        {
            return _defaultNickName;
        }
        else {
            int _randIndex = Random.Range(0, _canNickName.Length);
            return _canNickName[_randIndex]; 
        }
    }
}
