using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class AuthCodeTime : MonoBehaviour {
    public int _countDownSeconds = 5; //倒數秒數
    public static AuthCodeTime instance;

    private DateTime timer;
    private float getTime;
    public bool _statFlag = false;


    void Start() {
        instance = this;
    }

	void FixedUpdate() {
        if (_statFlag && timer.Ticks > 0)
        {
            gameObject.GetComponent<Text>().text = timer.ToString("mm:ss");
            if (Time.fixedTime - getTime >= 1)
            {
                timer = timer.AddSeconds(-1);
                getTime = Time.fixedTime;
                //Debug.Log("Timer = " + timer.ToString("mm:ss") + " ; timer.Ticks = " + timer.Ticks);
            }
        }
        else if(_statFlag && timer.Ticks == 0) {
            gameObject.GetComponent<Text>().text = timer.ToString("mm:ss");
            Debug.Log("Time is up! " + timer.Ticks);
            gameObject.SetActive(false);
            ForgotUI.instance.UnLockSendAuthCodeBtn();
            _statFlag = false;
        }
    }

    public void StartTimer()
    {
        //timer = new DateTime(DateTime.Now.AddSeconds(_countDownSeconds).Ticks  - DateTime.Now.Ticks);
        timer = new DateTime(_countDownSeconds * 10000000);

        gameObject.SetActive(true);
        _statFlag = true;
    }

}
