using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomSetPanel : MonoBehaviour {
    public Sprite[] _toggleBG;

    private string _roomSec = "秒數隨機";

    //自訂底台秒數UI切換
    public void RoomSecondToogleUI(Toggle _toggle) {
        if (_toggle.isOn) {
            _toggle.GetComponent<Image>().sprite = _toggleBG[0];
            _toggle.GetComponentInChildren<Text>().color = Color.white;
            _roomSec = _toggle.GetComponentInChildren<Text>().text;
        }
        else {
            _toggle.GetComponent<Image>().sprite = _toggleBG[1];
            _toggle.GetComponentInChildren<Text>().color = Color.gray;
        }
    }

    //開始玩
    public void RoomSetDone(Button _btn){
        int _roomDii = _btn.transform.parent.GetComponent<RoomSet>()._Dii;
        int _roomTai = _btn.transform.parent.GetComponent<RoomSet>()._Tai;
        int _roomCirc = _btn.transform.parent.GetComponent<RoomSet>()._Circle;

        Debug.Log("房間設定\n" + _roomSec + "  底: " + _roomDii + "  台: " + _roomTai + "  " + _roomCirc + " 圈 ");    
    }
}
