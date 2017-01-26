using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GameInfoUI : UIObject 
{
    private Text kyokuLab;
	private Text reachCountLab;
    private Image reachBan;
	private Text lab_remain;


    void Start () {
        Init();
    }

    public override void Init() {
        if(isInit == false){
			kyokuLab = transform.FindChild("Kyoku").GetComponent<Text>();
			reachCountLab = transform.FindChild("ReachCount").GetComponent<Text>();
			reachBan = transform.FindChild("ReachBan").GetComponent<Image>();
			lab_remain = transform.FindChild("lab_remain").GetComponent<Text>();

            isInit = true;
        }
    }

    public override void Clear()
    {
        base.Clear();

        kyokuLab.text = "";
        reachCountLab.text = "";
        lab_remain.text = "";
        reachBan.enabled = false;
    }

    public void SetKyoku( EKaze kaze, int kyoku ) {
        string kazeStr = ResManager.getString( "kaze_" + kaze.ToString().ToLower() );
        kyokuLab.text = kazeStr + " " + kyoku.ToString() + "局";
    }

    public void SetReachCount(int count) {
        reachCountLab.text = "x" + count.ToString();

        if(!reachBan.enabled)
            reachBan.enabled = true;
    }

    public void SetHonba(int honba) {
        Debug.Log( honba + "本场");
    }

    public void SetRemain(int remain)
    {
        lab_remain.text = "残: " + remain.ToString();
    }
}
