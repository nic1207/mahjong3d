using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReceivedMask : MonoBehaviour {
    public Text receivedText;
    public static ReceivedMask instance;

    void Start() {
        instance = this;
    }

    //以傳入品項、數量顯示提示框  (※之後放到獲得獎品的API CB中)
    public void ShowReceived(string _itemType, int _itemNum) {
        receivedText.text = "獲得 " + _itemType + " x " + _itemNum.ToString();
        gameObject.SetActive(true);
    }

    //點擊確定鈕 關閉此畫面
    public void AgreeMsg() {
        gameObject.SetActive(false);
    }
}
