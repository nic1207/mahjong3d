using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
/// <summary>
/// Which turn.
/// </summary>
public class PanelPlayers : MonoBehaviour {
	public List<Image> Arrows = new List<Image> ();
	public List<Image> Pais = new List<Image>();
	public List<Image> Homes = new List<Image>();
	public List<Image> Listeners = new List<Image>();
	public List<Image> Actions = new List<Image>();
	// Use this for initialization
	void Start () {
		hideAll ();
		//ShowArrow (0);
	}

	public void hideAll() {
		hideAllArrow ();
		hideAllPai ();
		hideAllHome ();
		hideAllListeners ();
		hideAllPon ();
	}


	public void ShowHomeba(int index) {
		hideAllHome ();
		//Debug.Log ("ShowHomeba("+index+")");
		Image im = null;
		if (index < Homes.Count) {
			im = Homes [index];
			im.gameObject.SetActive (true);
		}
	}

	public void ShowPai(int index, Hai h) {
		Image im = null;
		Sprite sp = ResManager.getMahjongSprite (h.Kind, h.Num);
		if (index < Pais.Count) {
			im = Pais [index];
			im.sprite = sp;
			im.transform.parent.parent.gameObject.SetActive (true);
		}
		StartCoroutine (HidePai(index));
	}

	public IEnumerator HidePai(int index) {
		yield return new WaitForSeconds (1.0f);
		Image im = null;
		if (index < Pais.Count) {
			im = Pais [index];
			im.transform.parent.parent.gameObject.SetActive (false);
		}
		StopCoroutine ("HidePai");
	}

	public void ShowArrow(int index) {
		hideAllArrow ();
		//Debug.Log ("Show("+index+")");
		Image im = null;
		if (index < Arrows.Count) {
			im = Arrows [index];
			im.gameObject.SetActive (true);
		}
	}

	public void ShowListener(int index) {
		//Debug.Log ("Show("+index+")");
		Image im = null;
		if (index < Listeners.Count) {
			im = Listeners [index];
			im.gameObject.SetActive (true);
		}
	}

	//秀碰字
	public void ShowPon(int index) {
		Image im = null;
		//Sprite sp = ResManager.getSprite("eff_peng");
        Sprite sp = ResManager.getChiiPonGanSprite(index);
        if (index < Actions.Count) {
			im = Actions [index];
            im.gameObject.GetComponentInChildren<Text>().text = "碰";
            im.sprite = sp;
			im.gameObject.SetActive (true);
		}
		StartCoroutine (HideAction(index));
	}

	//秀槓字
	public void ShowKan(int index) {
		Image im = null;
		//Sprite sp = ResManager.getSprite("eff_gang");
        Sprite sp = ResManager.getChiiPonGanSprite(index);
        if (index < Actions.Count) {
			im = Actions [index];
            im.gameObject.GetComponentInChildren<Text>().text = "槓";
            im.sprite = sp;
			im.gameObject.SetActive (true);
		}
		StartCoroutine (HideAction(index));
	}

	//秀吃字
	public void ShowChii(int index) {
		Image im = null;
        //Sprite sp = ResManager.getSprite("eff_chi");
        Sprite sp = ResManager.getChiiPonGanSprite(index);
        if (index < Actions.Count) {
            Debug.Log("ShowChii.index= "+ index);
			im = Actions [index];
            im.gameObject.GetComponentInChildren<Text>().text = "吃";
            im.sprite = sp;
            im.gameObject.SetActive (true);
		}
		StartCoroutine (HideAction(index));
	}

	//秀胡字
	public void ShowRon(int index) {
		Image im = null;
		//Sprite sp = ResManager.getSprite("eff_hu");
        Sprite sp = ResManager.getChiiPonGanSprite(index);
        if (index < Actions.Count) {
			im = Actions [index];
            im.gameObject.GetComponentInChildren<Text>().text = "胡";
            im.sprite = sp;
			im.gameObject.SetActive (true);
		}
		//StartCoroutine (HideAction(index));
	}

	public IEnumerator HideAction(int index) {
		yield return new WaitForSeconds (2);
		Image im = null;
		if (index < Actions.Count) {
			im = Actions [index];
			im.gameObject.SetActive (false);
		}
		StopCoroutine ("HideAction");
	}

	public void hideAllPon() {
		foreach (Image im in Actions) {
			if(im)
				im.transform.gameObject.SetActive (false);
		}
	}
	public void hideAllPai() {
		foreach (Image im in Pais) {
			if(im)
				im.transform.parent.parent.gameObject.SetActive (false);
		}
	}

	public void hideAllArrow() {
		foreach (Image im in Arrows) {
			im.gameObject.SetActive (false);
		}
	}
	public void hideAllHome() {
		foreach (Image im in Homes) {
			im.gameObject.SetActive (false);
		}
	}

	public void hideAllListeners() {
		foreach (Image im in Listeners) {
			im.gameObject.SetActive (false);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
