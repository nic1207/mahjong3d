using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AgariPlayer : MonoBehaviour {
    public GameObject _effect;
    public GameObject _glow;
    public Image _crow;
    public Image _head;
    public Image _flag;
    public Image _coinBG;
    public Sprite[] _winOrLose;
    public Text _name;
    public Text _coin;
    public Text _caculate;

    private Color _coinYellow = new Color(0.92f, 0.82f, 0.447f);
    private Color _coinPurple = new Color(0.45f, 0.207f, 0.627f);
    private Color _coinGray = new Color(0.788f, 0.792f, 0.792f);


    public void SetPlayerWinUI() {
        _effect.SetActive(true);
        _glow.SetActive(true);
        _crow.enabled = true;
        _flag.enabled = true;
        _flag.sprite = _winOrLose[0];
        SetCoinBlockWin();
    }

    public void SetPlayerLoseUI()
    {
        _effect.SetActive(false);
        _glow.SetActive(false);
        _crow.enabled = false;
        _flag.enabled = true;
        _flag.sprite = _winOrLose[1];
        SetCoinBlockLose();
    }

    public void SetPlayerDrawUI()
    {
        _effect.SetActive(false);
        _glow.SetActive(false);
        _crow.enabled = false;
        _flag.enabled = false;
        SetCoinBlockDraw();
    }

    public void AddCoin(int _int) {
        _caculate.text = "+" + _int;
    }

    public void ReduceCoin(int _int)
    {
        _caculate.text = "-" + _int;
    }

    public void ClearCoin()
    {
        _caculate.text = "";
    }

    private void SetCoinBlockWin() {
        _coinBG.color = _coinYellow;
        _coin.color = Color.red;
    }

    private void SetCoinBlockLose()
    {
        _coinBG.color = _coinPurple;
        _coin.color = Color.white;
    }

    private void SetCoinBlockDraw()
    {
        _coinBG.color = _coinGray;
        _coin.color = Color.gray;
    }

    public void HideEffect() {
        _effect.SetActive(false);
    }
}
