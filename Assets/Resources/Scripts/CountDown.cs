using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountDown : MonoBehaviour {
    private int countDownTime = 0; //CountDown second
    //public bool touchCountDown = false;
	public Text CountDownText;
	public Animator Anim;
    //private Text countDownText;
    //private bool countDownFlag = false;
    //private float startTime;
    //private int remainTime;
    //private Animator anim;

    // Use this for initialization
    void Start () {
        //CountDownText = this.GetComponentInChildren<Text>(true);
        //Anim = this.GetComponent<Animator>();
		gameObject.SetActive (false);
        //startTime = Time.time;
    }
	/*
	// Update is called once per frame
	void FixedUpdate() { 
        if(touchCountDown)
        {
            //StartCountDown();
            touchCountDown = false;
            countDownFlag = true;
            anim.enabled = true;
        }
            
        if (countDownFlag) {
            remainTime = (int) ((countDownTime + startTime) - (int) Time.time);
            countDownText.text = remainTime.ToString();

            if (remainTime == 0) {
                countDownFlag = false;
                anim.enabled = false;
            }
                
        }
    }
	*/

	public void Show(int num) {
		countDownTime = num;
		//Debug.Log("countDownTime = " + countDownTime);
		gameObject.SetActive (true);
		CountDownText.text = countDownTime.ToString ();
		StopCoroutine ("countDown");
		transform.localScale = Vector3.one;
		StartCoroutine ("countDown");

    }

	public void Hide() {
		countDownTime = 0;
		//Debug.Log("countDownTime = " + countDownTime);
		gameObject.SetActive (false);
		CountDownText.text = countDownTime.ToString();
		StopCoroutine ("countDown");
		//transform.localScale = Vector3.one;
		//StartCoroutine ("countDown");
	}

	private IEnumerator countDown() {
		while (countDownTime > 0) {
			yield return new WaitForSeconds (1);
			countDownTime--;
			if (countDownTime > 0 && CountDownText != null && countDownTime < 10) {
				Anim.enabled = true;
				gameObject.SetActive (true);
				CountDownText.text = countDownTime.ToString ();
			} else {
				Anim.enabled = false;
				gameObject.SetActive (false);
				StopCoroutine ("countDown");
			}
			//yield return new WaitForSeconds (1);
		}

	}
}
