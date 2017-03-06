using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Dice : MonoBehaviour {
    public static Dice Instance;
    public int downForce = -1;
    public float delayFade = 3.5f;
    public GameObject dicePrefab;
    public SaifuriPanel saifuriPanel;

    private int diceNum = 3;
    private Vector3[] diceStartPos;
    private Rigidbody[] rb;
    private Vector3[] forces;
    private string namedChild = "dice";
    private List<int> dicePoints =new List<int>();
    private int totalPoint = 0;
    private Sai[] sais;

    void Start () {
        Instance = this;

        rb = new Rigidbody[diceNum];
        forces = new Vector3[diceNum];
        diceStartPos = new Vector3[diceNum];

        diceStartPos[0] = new Vector3(-0.237f, 0, 0.166f);
        diceStartPos[1] = new Vector3(0.276f, 0, 0.223f);
        diceStartPos[2] = new Vector3(-0.008f, -0.124f, -0.217f);
    }
	

    void OnCollisionEnter(Collision col) {
        if(col.gameObject.tag == "Table")
            Debug.Log("碰到桌子");
    }

    public void ThrowDices(Sai[] sais) {
        ResetDice();

        for (int i = 0; i < diceNum; i++)
        {
            GameObject go = Instantiate(dicePrefab);
            go.transform.SetParent(transform);
            go.transform.localPosition = diceStartPos[i];
            go.transform.localScale = Vector3.one;

            rb[i] = go.GetComponent<Rigidbody>();

            //rb[i].velocity = (new Vector3(Random.Range(0, 2), downForce*2, Random.Range(0, 2)));
            //rb[i].AddTorque(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
            rb[i].AddTorque(300f * 5, -90f, 400f * 10);

            forces[i] = new Vector3(Random.Range(-50, 50), downForce * 100, Random.Range(-50, 50));
            rb[i].AddForce(forces[i]);

            StartCoroutine("CalculateDice", go.transform);
        }

        //StartCoroutine("FadeDices", delayFade );
        StartCoroutine(FadeDices(delayFade, sais));
    }

    IEnumerator FadeDices(float delayTime, Sai[] sais) {
        yield return new WaitForSeconds(delayTime);

        saifuriPanel.SetTotalSaiString(totalPoint);
        //Debug.Log("點數總和 = " + totalPoint);

        sais[0].Num = dicePoints[0];
        sais[1].Num = dicePoints[1];
        sais[2].Num = dicePoints[2];

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject go = transform.GetChild(i).gameObject;
            Destroy(go);
        }

        saifuriPanel.OnEnd();
    }

    void RandomDicePoint() {
        int randomPoint = Random.Range(1, 7) + Random.Range(1, 7) + Random.Range(1, 7);
    }

    int CheckDice(Transform tf)
    {
        // 取得本地旋轉角度
        //Vector3 lRot = tf.localRotation.eulerAngles;

        // 取得世界旋轉角度
        Vector3 wRot = tf.rotation.eulerAngles;

        float faceX = wRot.x;
        float faceZ = wRot.z;

        int xAngle = 0;
        int zAngle = 0;
        int _point = -1;

        if (faceX <= 45 || Mathf.Abs(faceX - 360) <= 45)
            xAngle = 0;
        else if (Mathf.Abs(faceX - 90) <= 45)
            xAngle = 90;
        else if (Mathf.Abs(faceX - 180) <= 45)
            xAngle = 180;
        else if (Mathf.Abs(faceX - 270) <= 45)
            xAngle = 270;
        else
            xAngle = Mathf.RoundToInt(faceX);


        if (faceZ <= 45 || Mathf.Abs(faceZ - 360) <= 45)
            zAngle = 0;
        else if (Mathf.Abs(faceZ - 90) <= 45)
            zAngle = 90;
        else if (Mathf.Abs(faceZ - 180) <= 45)
            zAngle = 180;
        else if (Mathf.Abs(faceZ - 270) <= 45)
            zAngle = 270;
        else
            zAngle = Mathf.RoundToInt(faceZ);

        switch (xAngle)
        {
            case 0:
                if (zAngle == 0)
                    _point = 1;
                else if (zAngle == 90)
                    _point = 3;
                else if (zAngle == 180)
                    _point = 6;
                else if (zAngle == 270)
                    _point = 4;
                else
                    _point = -1;
                break;
            case 90:
                _point = 2;
                break;
            case 180:
                _point = 6;
                break;
            case 270:
                _point = 5;
                break;
            default:
                _point = -1;
                break;
        }

        //Debug.Log("點數 = " + _point + " 世界角度 = " + wRot + "\n");
        return _point;
    }

    IEnumerator CalculateDice(Transform tf)
    {
        yield return new WaitForSeconds(2f);

        int _getPoint = CheckDice(tf);
        dicePoints.Add(_getPoint);
        totalPoint += _getPoint;
    }

    private void ResetDice()
    {
        totalPoint = 0;
        saifuriPanel.ClearTotalSaiString();

        StopCoroutine("CalculateDice");
        StopCoroutine("FadeDices");

        dicePoints.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject go = transform.GetChild(i).gameObject;
            Destroy(go);
        }
    }
}
