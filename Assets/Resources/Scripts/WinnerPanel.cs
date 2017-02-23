using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinnerPanel : MonoBehaviour {
    //public static WinnerPanel instance;
    public Image _head;
    public Text _name;
    public GameObject _main;
    public GameObject _winnerEffect;
    public GameObject _ronObject;
    public GameObject _tsumoObject;
    public PanelPlayers panelPlayers;

    private int _winnerIndex = -1;
    private bool isTsumo = false;

    void Start() {
        //instance = this;

        Hide();
    }

    public void Initialize() {
        int _ronPlayer = RecordPreTedasi._instance.RonPlayerIndex;
        int _tsumonPlayer = RecordPreTedasi._instance.TsumoPlayerIndex;

        _winnerIndex = _tsumonPlayer == -1 ? _ronPlayer : _tsumonPlayer;
        isTsumo = _tsumonPlayer == -1 ? false : true;

        _head.sprite = panelPlayers.Photos[_winnerIndex].sprite;
        _name.text = panelPlayers.Names[_winnerIndex].text;

        Show(isTsumo);
    }

    private void Show(bool _isTsumo) {
        _winnerEffect.SetActive(true);//背景特效
        _ronObject.SetActive(!_isTsumo);
        _tsumoObject.SetActive(_isTsumo);

        _main.SetActive(true);
    }

    public void Hide() {
        _winnerEffect.SetActive(false); //背景特效
        _main.SetActive(false);
    }
        
}
