using UnityEngine;
using System.Collections;

public class RecordPreTedasi : MonoBehaviour {
    public static RecordPreTedasi _instance;
    private int _ronPlayerIndex = -1; //胡玩家
    private int _tsumoPlayerIndex = -1; //自摸玩家
    private int _preTedasiPlayerIndex = -1; //上一張牌誰丟出

    void Start() {
        _instance = this;
    }

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
        PreTedasiPlayerIndex = -1;
        TsumoPlayerIndex = -1;
        RonPlayerIndex = -1;
    }
}
