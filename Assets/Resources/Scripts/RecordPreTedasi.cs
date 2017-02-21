using UnityEngine;
using System.Collections;

public class RecordPreTedasi : MonoBehaviour {
    public static RecordPreTedasi _instance;
    //private string _preTedasiPlayer; //上一張牌誰丟出
    //private string _tsumoPlayer; //自摸玩家
    //private string _ronPlayer; //胡玩家
    private int _ronPlayerIndex = 4; //胡玩家
    private int _tsumoPlayerIndex = 4; //自摸玩家
    private int _preTedasiPlayerIndex = 4; //上一張牌誰丟出

    void Start() {
        _instance = this;
    }

    //public string PreTedasiPlayer
    //{
    //    get { return _preTedasiPlayer; }
    //    set { _preTedasiPlayer = value; }
    //}

    public int PreTedasiPlayerIndex
    {
        get { return _preTedasiPlayerIndex; }
        set { _preTedasiPlayerIndex = value; }
    }

    public int TsumoPlayerIndex
    {
        get { return _tsumoPlayerIndex; }
        set { _tsumoPlayerIndex = value; }
    }

    public int RonPlayerIndex
    {
        get { return _ronPlayerIndex; }
        set { _ronPlayerIndex = value; }
    }

    public void Clear(){
        PreTedasiPlayerIndex = 4;
        TsumoPlayerIndex = 4;
        RonPlayerIndex = 4;
    }
}
