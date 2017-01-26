using UnityEngine;
using System.Collections;


public class PlayerInfoUI : UIObject 
{
	private bool _isAI = false;
    private UILabel lab_kaze;
    private UILabel lab_point;
    private UISprite reachBan;
    private GameObject oyaObj;

    Color initColor;

    // Use this for initialization
    void Start () {
        Init();
    }

    public override void Init() {
        if(isInit == false){
            //lab_kaze = transform.Find("Kaze").GetComponent<UILabel>();
            //lab_point = transform.Find("Point").GetComponent<UILabel>();
            //reachBan = transform.Find("ReachBan").GetComponent<UISprite>();
            //initColor = lab_kaze.color;
            //oyaObj = transform.Find( "Oya" ).gameObject;
			Clear ();
            isInit = true;
        }
    }

	public void SetKaze(EKaze kaze) {
		string ww = ResManager.getString( "kaze_" + kaze.ToString().ToLower() );
		//if(!isAI)
		//Debug.Log (this._ownerPlayer.Name+"設定為"+ww+"風");
		//_isAI = isAI;
		//if (_isAI)
		//	lab_point.text = "(電腦)";
		//else
		//	lab_point.text = "(玩家)";
    }

    public void SetOyaKaze(bool isOya) {
        //if( isOya ) {
        //    lab_kaze.color = Color.red;
        //} else {
        //lab_kaze.color = initColor;
        //}
        //oyaObj.SetActive(isOya);
    }

    //public void SetTenbou(int point) {
    //    lab_point.text = point.ToString();
    //}

    //public void SetReach(bool isReach) {
    //    reachBan.enabled = isReach;
    //}

    public override void Clear() {
        //lab_kaze.text = "";
        //lab_point.text = "";
        //reachBan.enabled = false;
        //oyaObj.SetActive(false);
    }
}
