using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HudPanel : MonoBehaviour {
	public Text KyokoTxt;
	public Text RemainTxt;
	public Text WindTxt;
	public Text honbaTxt;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetKyokoTxt(string str,string hb) {
		if (KyokoTxt)
			KyokoTxt.text = str;
		if(honbaTxt)
			honbaTxt.text = hb;
	}

	public void SetRemainCount(int num) {
		if (RemainTxt)
			RemainTxt.text = "剩餘"+num.ToString()+"張";
	}

	public void setWindTxt(string str) {
		if (WindTxt && WindTxt.text=="")
			WindTxt.text = str;
	}
}
