using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MessagePanel : MonoBehaviour {
    public Transform _msgContent;
    public Text[] playerMsgs;
    public float stayTime = 1.5f;

    private GameObject _selectPanel;
    private List<Toggle> msgItem = new List<Toggle>();
    private List<Text> msgItemText = new List<Text>();
    private string _msg;

    void Start() {
        Init();
    }

    private void Init() {

        _selectPanel = transform.Find("Panel").gameObject;

        if (_msgContent) {
            for (int i = 0; i < _msgContent.childCount; i++)
            {
                msgItem.Add( _msgContent.GetChild(i).GetComponentInChildren<Toggle>());
                msgItemText.Add(_msgContent.GetChild(i).GetComponentInChildren<Text>());
                msgItem[i].isOn = i != 0 ? false: true;
            }

            if(msgItemText[0])
                _msg = msgItemText[0].text;
        }
    }

    public void SaveCuurText(int _itemIndex) {
        if (msgItem[_itemIndex].isOn)
            _msg = msgItemText[_itemIndex].text;
    }

    public void SendMessage() {
        //Debug.Log("_msg = " + _msg);
        ExitMessagePanel();

        ShowPlayerMsg(0, _msg);
    }

    public void ShowMessagePanel()
    {
        _selectPanel.SetActive(true);
    }
    public void ExitMessagePanel() {
        _selectPanel.SetActive(false);
    }

    public void ShowPlayerMsg(int _index, string _msg) {
        if (playerMsgs[_index]) {
            playerMsgs[_index].text = _msg;
            playerMsgs[_index].transform.parent.gameObject.SetActive(true);

            StartCoroutine( HideMsgDialog(_index) );
        }
    }


    IEnumerator HideMsgDialog(int _playerIndex)
    {
        yield return new WaitForSeconds(stayTime);

        if (playerMsgs[_playerIndex])
            playerMsgs[_playerIndex].transform.parent.gameObject.SetActive(false);

    }
}
