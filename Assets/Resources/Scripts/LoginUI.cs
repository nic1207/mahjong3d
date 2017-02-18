using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Text.RegularExpressions;

public class LoginUI : MonoBehaviour {
    public static LoginUI Instance;

	public InputField ClubLoginAccount;// Club登入頁 用戶帳號
	public InputField ClubLoginPass;   // Club登入頁 用戶密碼
	public GameObject ClubLoginHint_Account;   // Club登入頁 錯誤提示
    public GameObject ClubLoginHint_Password;   // Club登入頁 錯誤提示

    void Awake () {
		Instance = this;
    }

    void Start() {
        // 若前次成功登入時 有勾選記憶check-box 則這次自動填入
        //if (PlayerPrefs.GetInt("USERKEEP") == 1)
        //{
		ClubLoginAccount.text = PlayerPrefs.GetString ("USERNAME");
		ClubLoginPass.text = PlayerPrefs.GetString ("USERPASS");
        //    YkiApi.Login("0", PlayerPrefs.GetString("USERNAME"), PlayerPrefs.GetString("USERPASS"), LoginCallback);
        //    Debug.Log("讀取 PlayerPrefs");
        //}
    }

	//登入按鈕
	public void ClubSigninClick() {
		string userName = ClubLoginAccount.text;
		string userPass = ClubLoginPass.text;
        ClubLoginHint_Account.SetActive(false);
        ClubLoginHint_Password.SetActive(false);



        if (userPass == "")
        {
            ClubLoginHint_Password.GetComponentInChildren<Text>().text = "欄位不可空白";
            ClubLoginHint_Password.SetActive(true);
            ClubLoginPass.text = "";
            //Debug.Log("欄位不可空白");
        }
        if (userName == "")
        {
            ClubLoginHint_Account.GetComponentInChildren<Text>().text = "欄位不可空白";
            ClubLoginHint_Account.SetActive(true);
            ClubLoginAccount.text = "";
            //Debug.Log("欄位不可空白");
        }
        //檢查帳號是否合法 ※帳號欄位改為信箱 所以 CheckName → CheckEmail
        else if (!CheckEmail(userName)) {
            ClubLoginHint_Account.GetComponentInChildren<Text>().text = "帳號格式錯誤";
            ClubLoginHint_Account.SetActive (true);
            ClubLoginAccount.text = "";
            Debug.Log ("帳號格式錯誤");
		} else {
            //ConnectingPanel.SetActive(true); //畫面顯示連線中

            //把帳號傳給UIManager管理
            UIManager.instance.userAccount = userName;

            //儲存此次帳密
            PlayerPrefs.SetString ("USERNAME", userName);
            PlayerPrefs.SetString ("USERPASS", userPass);

            //以下呼叫 API(userName, userPass)
			MainDataManager.Instance.Account =  userName;
			MainDataManager.Instance.Passwd = userPass;
			PlayerPrefs.SetString ("USERNAME", userName);
			PlayerPrefs.SetString ("USERPASS", userPass);
			//YkiApi.Login("0", userName, userPass, LoginCallback);
			MainDataManager.Instance.doLogin(userName, userPass, LoginCallback);
            //Main.Instance.doLogin0(userName, userPass, LoginCallback);
            //StartCoroutine(CheckLoginStatus());

            //UIManager.instance.StartSetEnterLoading();
		}
	}

    //檢查帳號格式 (6-14 小寫英文+數字)
    public bool CheckName(string str)
    {

        if (str.Length < 1) {
            return false;
        }
        else{
            //檢查第一個字元是否為數字
            bool isN = IsNumeric(str.Substring(0, 1));

            //帳號長度 6-14 首位不得位數字
            if (str == "" || str.Length < 6 || isN)
                return false;
            else
                return true;
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

    //定義一個函數,作用:判斷strNumber是否為數字,是數字返回True,不是數字返回False
    public bool IsNumeric(string strNumber)
    {
        Regex NumberPattern = new Regex("[^0-9.-]");
        return !NumberPattern.IsMatch(strNumber);
    }

    public void LoginCallback(WebExceptionStatus status, string result)
	{
		//ConnectingPanel.SetActive(false); //關閉連線畫面
		if (status!=WebExceptionStatus.Success){
            ClubLoginHint_Password.GetComponentInChildren<Text> ().text = "登入失敗: 輸入資訊錯誤";
            ClubLoginHint_Password.SetActive (true);
            ClubLoginPass.text = "";
            Debug.Log("登入失敗: 輸入資訊錯誤");		
		} else {	
			Debug.Log ("登入成功! Token="+result);
			MainDataManager.Instance.Token = result;
			UIManager.instance.StartSetEnterLoading();
			//hide login panel
//			if(Main.Instance.loginPanel && Main.Instance.loginPanel.activeSelf){
//              ClubLoginAccount.text = "";
//				ClubLoginPass.text = "";
//				Main.Instance.loginPanel.SetActive (false);
//			}

			//PlayerPrefs.SetInt ("LOGOUT", 0);
		}
	}

    //點擊錯誤提示區塊 清空欄位並聚焦
    public void ClickHintBlock(Button targetHint) {
        targetHint.gameObject.SetActive(false);
        targetHint.GetComponentInParent<InputField>().ActivateInputField();
    }
}
