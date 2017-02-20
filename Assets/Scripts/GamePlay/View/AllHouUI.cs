using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllHouUI : MonoBehaviour {
    public static AllHouUI instance;
    public const int Max_Lines = 6;
    public const int MaxCoutPerLine = 12;
    //public Vector2 AlignLeftLocalPos = new Vector2(-150, 0);
    public float HaiPosOffsetX = 0.64f;
    public Transform[] _alllineParents;

    //private List<Transform> lineParents;
    private int _curLine = 0;
    private float _curLineRightAligPosX = 0f;
    public int _cuurIndex = 0;

    // Use this for initialization
    void Start () {
        instance = this;

        Init();
    }

    private void Init() {
        //lineParents = new List<Transform>(Max_Lines);
        //for (int i = 0; i < Max_Lines; i++)
        //{
        //    Transform line = transform.FindChild("Line_" + (i + 1));
        //    if (line != null)
        //    {
        //        lineParents.Add(line);
        //    }
        //}


        //_curLineRightAligPosX = AlignLeftLocalPos.x;
    }

    public Transform GiveHouParent() {
        int parentIndex = _cuurIndex / MaxCoutPerLine;
        return _alllineParents[parentIndex];
    }

	public float GiveHouPoistion(int _allallHaisCount) {
        int _lineIndex = _cuurIndex % MaxCoutPerLine;

        _curLineRightAligPosX = MahjongPai.Width + HaiPosOffsetX * _lineIndex;

        _cuurIndex++;
        return _curLineRightAligPosX;
    }

    public void Clear() {

    }
}
