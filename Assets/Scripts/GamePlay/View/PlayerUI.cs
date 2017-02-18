using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerUI : UIObject 
{
    private PlayerInfoUI playerInfo; // 玩家信息.
    private TehaiUI tehai; // 手牌.
    private YamaUI yama;   // 牌山.
    private HouUI hou;     // 河.
    private FuuroUI fuuro; //副露.
	private bool _isAI = false;
	private string _name = string.Empty;
	private int _index;

    //private int panelDepth = 0;

	public string Name
	{
		get { return _name;}
		set {
			_name = value;
		}
	}

	public int Index
	{
		get { return _index; }
		set {
			_index = value;
		}
	}
    public PlayerInfoUI Info
    {
        get{ return playerInfo; }
    }
    public YamaUI Yama
    {
        get{ return yama; }
    }
    public TehaiUI Tehai
    {
        get{ return tehai; }
    }
    public HouUI Hou
    {
        get{ return hou; }
    }
    public FuuroUI Fuuro
    {
        get{ return fuuro; }
    }


    void Start () {
        Init();
    }

    public override void BindPlayer(Player p)
    {
        Init();

        base.BindPlayer(p);

        tehai.BindPlayer(p);
        yama.BindPlayer(p);
        hou.BindPlayer(p);
        fuuro.BindPlayer(p);
        playerInfo.BindPlayer(p);
    }

    public override void Init() 
    {
        if( isInit == false ) 
        {
            playerInfo = transform.Find("Info").GetComponent<PlayerInfoUI>();
            tehai = transform.Find("Tehai").GetComponent<TehaiUI>();
			//tehai.gameObject.layer = LayerMask.NameToLayer("PlayerTehai");
            yama = transform.Find("Yama").GetComponent<YamaUI>();
            hou = transform.Find("Hou").GetComponent<HouUI>();
			hou.gameObject.layer = LayerMask.NameToLayer("Hou");
            fuuro = transform.Find("Fuuro").GetComponent<FuuroUI>();

            tehai.Init();
            yama.Init();
            hou.Init();
            fuuro.Init();
            playerInfo.Init();

            isInit = true;
        }
    }

    public override void Clear() 
    {        
        if(!isInit) Init();

        tehai.Clear();
        yama.Clear();
        hou.Clear();   
        fuuro.Clear();
        playerInfo.Clear();
    }

    public override void SetParentPanelDepth(int depth) 
    { 
		/*
        if( panelDepth != depth ){
            tehai.SetParentPanelDepth( depth );
            yama.SetParentPanelDepth( depth );
            hou.SetParentPanelDepth( depth );
            fuuro.SetParentPanelDepth( depth );
            playerInfo.SetParentPanelDepth( depth );

            panelDepth = depth;
        }
        */
    }
    
	public void Speak( ECvType content, Hai h)
    {
		string word = string.Empty;

		switch (content) {
		case ECvType.Throw:
			word = "出牌";
			break;
		case ECvType.Pon://
			word = "碰";
			break;

		case ECvType.Chii://吃
			word = "吃";
			break;
		
		case ECvType.Kan://槓
			word = "槓";
			break;
		case ECvType.Reach://聽
			word = "聽";
			break;
		case ECvType.Ron://胡
			word = "胡";
			break;
		case ECvType.Tsumo://自摸
			word = "自摸";
			break;
		}
		string hname = ResManager.getMagjongName (h.Kind, h.Num);
		//Debug.Log (OwnerPlayer.Name+ " Speak("+hname+")");
		string path = AudioConfig.GetHCVPath (OwnerPlayer.VoiceType, content, h);
		//string path = "Sounds/CV/sm_cvw_b001";
		//Debug.Log(path);
		AudioManager.Get().PlaySFX(path);
		EventManager.Get().SendEvent(UIEventType.Display_Throwpai_Panel, _index, h);
    }
	public void Speak( ECvType content )
	{
		//Debug.Log (OwnerPlayer.Name+ "Speak "+content.ToString());
		string path = AudioConfig.GetCVPath (OwnerPlayer.VoiceType, content);
		//Debug.Log ("Speak("+path+")");
		AudioManager.Get().PlaySFX(path);
		//Debug.LogWarning( type.ToString() + "!!!" );
	}
	public void SetPlayerUIndex(int idx) {
		tehai.SetPlayerUIndex (idx);
	}
	// 手牌
	public void SetTehai(Hai[] hais, bool setLastNew = false) {
        tehai.SetTehai(hais);
    }
    public void PickHai( Hai hai, bool newPicked = false, bool isShow = false ) {
        tehai.AddPai(hai, newPicked, isShow);
    }
    public void PickPai( MahjongPai hai, bool newPicked = false, bool isShow = false ) {
        tehai.AddPai(hai, newPicked, isShow);
    }
    public void SuteHai( int index ){
		//Debug.Log (this._ownerPlayer.Name+" SuteHai("+index+")");
        MahjongPai pai = tehai.SuteHai(index);
		if (pai != null) {
			Hai hai = pai.GetInfo ();
			string name = ResManager.getMagjongName (hai.Kind, hai.Num);
			string wind = ResManager.getString ("kaze_"+_ownerPlayer.JiKaze.ToString().ToLower());
			Debug.Log ("["+wind+"]"+this._ownerPlayer.Name + " 出牌(" + name + ")");
			Speak (ECvType.Throw, hai);
			//Speak( ECvType.Reach );
			AddSuteHai (pai);
		} else {
			Debug.LogError ("沒有這個牌 index"+index);
		}
    }
    public void SortTehai(Hai[] hais, float delay, bool setLastNew = false){
        tehai.SortTehai(hais, delay, setLastNew);
    }
    public void SetTehaiVisiable( bool visiable ) {
        tehai.SetAllHaisVisiable(visiable);
    }
    public void EnableInput(bool isEnable){
        if(isEnable)
            tehai.EnableInput();
        else
            tehai.DisableInput();
    }
    public void SetTehaiStateColor(bool state){
        tehai.SetEnableStateColor(state);
    }

    // 河. 打牌.
    protected void AddSuteHai(MahjongPai hai) {
        hou.AddHai(hai);
    }
	public void SetHouPlayerIndex(int idx) {
		hou.SetPlayerIndex (idx);
	}
    public void SetTedashi(bool isTedashi = true){
        hou.setTedashi(isTedashi);
    }
    public void Reach(bool reach = true) {
        hou.SetReach(reach);
        //playerInfo.SetReach(reach);
    }
    public void SetNaki(bool isNaki = true){
        hou.setNaki(isNaki);
    }
    public void SetShining(bool isShining){
        hou.setShining(isShining);
    }

    // 牌山.
    public void SetYamaHais(Dictionary<int, Hai> yamaHais, int start, int end) {
        yama.SetYamaHais(yamaHais, start, end);
    }
	public void SetYamaPlayerIndex(int idx) {
		yama.SetPlayerIndex (idx);
	}
    public MahjongPai PickUpYamaHai(int index) {
        return yama.PickUp(index);
    }
    public void ShowYamaHai(int index) {
        yama.ShowHai(index);
    }
    public void HideYamaHai(int index) {
        yama.HideHai(index);
    }
    public void ShowAllYamaHais() {
        yama.ShowAllYamaHais();
    }
    public void HideAllYamaHais() {
        yama.HideAllYamaHais();
    }
    public void SetWareme(int index) { 
        yama.SetWareme(index);
    }


    // 副露.
    public void UpdateFuuro(Fuuro[] fuuros) {
        fuuro.UpdateFuuro(fuuros);
    }

    // player info.
    public void SetKaze(EKaze kaze) {
        playerInfo.SetKaze(kaze);
    }
    public void SetTenbou(int point) {
        //playerInfo.SetTenbou(point);
    }
    public void SetOyaKaze(bool isOya) {
        playerInfo.SetOyaKaze(isOya); 
    }


    public static void CollectMahjongPai(MahjongPai pai) 
    {
        ResManager.CollectMahjongPai(pai);
    }
    public static MahjongPai CreateMahjongPai(Transform parent, Vector3 localPos, Hai info, bool isShow = true) 
    {
        if( Hai.IsValidHai(info) == false ){
            Debug.LogError("PlayerUI: Invalid hai for ID == " + info.ID);
            return null;
        }

        GameObject newInst = ResManager.CreateMahjongObject();
        newInst.transform.parent = parent;
        newInst.transform.localScale = Vector3.one;
        newInst.transform.localPosition = localPos;

        // set component info.
        MahjongPai pai = newInst.GetComponent<MahjongPai>();
        if( pai == null ) {
            pai = newInst.AddComponent<MahjongPai>();
        }

        pai.SetInfo(info);
        pai.Init();
        pai.UpdateImage();

        if( isShow ) {
            pai.Show();
        }
        else {
            pai.Hide();
        }

        return pai;
    }

}
