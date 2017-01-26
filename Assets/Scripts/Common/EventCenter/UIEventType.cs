
public enum UIEventType 
{
    #region event id
    // pick a tsumo hai. 
    PickTsumoHai,
    PickRinshanHai,

    // 捨牌の選択 
    Select_SuteHai,
    // 捨牌 
    SuteHai,
    // リーチ 
    Reach,
    // ポン 
    Pon,
    // チー(左) 
    Chii_Left,
    // チー(中央) 
    Chii_Center,
    // チー(右) 
    Chii_Right,
    // 大明槓 
    DaiMinKan,
    // 加槓 
    Kakan,
    // 暗槓 
    Ankan,
    // ロンのチェック 
    Ron_Check,
    // ツモあがり 
    Tsumo_Agari,
    // ロンあがり 
    Ron_Agari,
    // 流し 
    Nagashi,
    #endregion

    DisplayMenuList,
    HideMenuList,

    DisplayKyokuInfo,

    On_UIAnim_End, // callback of all EActionType, and other system animations.

    // ゲームの開始 
    Start_Game,

    // 局の開始 
    Start_Kyoku,

    Init_Game,

    // Saifuri
    Select_ChiiCha,
    On_Select_ChiiCha_End,

    Init_PlayerInfoUI,

    Select_Wareme,
    On_Select_Wareme_End,

    // 配牌
    HaiPai,

    SetYama_BeforeHaipai,
    SetUI_AfterHaipai,

    Display_Agari_Panel,
	Display_Arrow_Panel,//顯示目前輪到誰介面
	Display_Throwpai_Panel,//顯示丟出什麼牌介面
	Display_Homeba_Panel,//顯示莊家圖示
	Display_Countdown_Panel,//顯示莊家圖示
    // 流局 
    RyuuKyoku,

    // 局の終了 
    End_Kyoku,
    End_RyuuKyoku,

    // ゲームの終了 
    End_Game,

}
