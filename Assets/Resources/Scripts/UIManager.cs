using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UIManager : MonoBehaviour {
    public static UIManager instance;

    public Transform foxGameLogoPanel;
    public Transform gameTitlePanel;
    public Transform loginPanel;
    public Transform gameLobbyButton;
    public Image playNowText;
    public GameObject janWanLoginButton;
    public GameObject janWanLoginPanel;

    public Transform enterLoadingPanel;
    public GameObject registerPanel;
    public GameObject rulePanel;
    public GameObject forgotPanel;
    public Image guideImage;
    public Sprite[] guideImages;

    private Animator foxGameLogoAnim;
    private Animator gameTitleAnim;
    private Animator enterLoadingAnim;
    private Animator loginAnim;

    private int guideImageNum;
    private int guideImageIndex = 0;

    //private float gameLobbyBtnHeight = 300f;

    void Start() {

        instance = this;

        if (!foxGameLogoPanel)
            Debug.LogError("No found FoxGameLogoPanel");
        else
            foxGameLogoAnim = foxGameLogoPanel.GetComponent<Animator>();

        if (!gameTitlePanel)
            Debug.LogError("No found GameTitlePanel");
        else
            gameTitleAnim = gameTitlePanel.GetComponent<Animator>();

        if (!loginPanel)
            Debug.LogError("No found LoginPanel");
        else
            loginAnim = loginPanel.GetComponent<Animator>();

        if (!enterLoadingPanel)
            Debug.LogError("No found EnterLoadingPanel");
        else
            enterLoadingAnim = enterLoadingPanel.GetComponent<Animator>();

        if (!registerPanel)
            Debug.LogError("No found RegisterPanel");

        if (!rulePanel)
            Debug.LogError("No found RulePanel");

        //if (!gameLobbyButton)
        //    Debug.LogError("No found GameLobbyButton");
        //else
        //    gameLobbyBtnHeight = gameLobbyButton.GetComponent<LayoutElement>().preferredHeight;

        InitialLoginPanel();

        StartCoroutine("PlayOP");

        guideImageNum = guideImages.Length;
    }

    //遊戲流程
    IEnumerator PlayOP() {
        yield return new WaitForSeconds(1f);
        //播放 Foxgame Logo
        foxGameLogoAnim.SetTrigger("FoxGameLogo");

        yield return new WaitForSeconds(4f);
        //播放 醬玩麻將標題
        gameTitleAnim.SetTrigger("GameTitle");

        yield return new WaitForSeconds(5f);
        //進入登入畫面
        loginAnim.SetTrigger("loginFlag");

        //enterLoadingAnim.SetTrigger("EnterLoading");
        //InvokeRepeating("GuideImages", 0f, 5f);

        //yield return new WaitForSeconds(3f);
        //EnterLoading.instance.StartLoading();
    }

    //登入流程正確 準備進入載入畫面
    public void StartSetEnterLoading() {
        loginPanel.gameObject.SetActive(false);
        StartCoroutine("EntranceLoading");
    }

    IEnumerator EntranceLoading() {
        //顯示載入畫面
        enterLoadingAnim.SetTrigger("EnterLoading");
        InvokeRepeating("GuideImages", 0f, 5f);

        yield return new WaitForSeconds(3f);
        EnterLoading.instance.StartLoading();
    }


    //更換載入畫面教學圖片
    private void GuideImages() {
        guideImageIndex += 1;
        guideImage.sprite = guideImages[guideImageIndex % guideImageNum];
        //Debug.Log("guideImageIndex = " + guideImageIndex);
    }

    //載入畫面載入完畢
    public void SetEnterLoadingDone() {
        enterLoadingAnim.SetBool("EnterLoadingDone", true);
    }

    //點擊醬玩帳號入口鈕
    public void JanWanPlayClick() {
        playNowText.gameObject.SetActive(false);
        gameLobbyButton.GetComponent<LayoutElement>().preferredHeight = 180f;

        janWanLoginButton.SetActive(false);
        janWanLoginPanel.SetActive(true);
    }

    //進入註冊醬玩會員
    public void GoRegisterPage() {
        registerPanel.SetActive(true);
    }

    //離開註冊醬玩會員
    public void ExitRegisterPage()
    {
        registerPanel.SetActive(false);
    }

    //進入服務條款頁
    public void GoRulePage()
    {
        rulePanel.SetActive(true);
    }

    //離開服務條款頁
    public void ExitRulePage()
    {
        rulePanel.SetActive(false);
    }

    private void InitialLoginPanel() {
        playNowText.gameObject.SetActive(true);
        gameLobbyButton.GetComponent<LayoutElement>().preferredHeight = 300f;

        janWanLoginButton.SetActive(true);
        janWanLoginPanel.SetActive(false);
    }

    //進入忘記密碼頁
    public void GoForgotPage() {
        forgotPanel.SetActive(true);
    }

    //離開忘記密碼頁
    public void ExitForgotPage()
    {
        //ForgotUI.instance.OOOO();
        forgotPanel.SetActive(false);
    }



}
