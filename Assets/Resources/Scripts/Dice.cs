using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Dice : MonoBehaviour {

    public int downForce = -65;
    public float delayFade = 3.5f;
    public GameObject dicePrefab;
    public int diceNum = 3;
    public Text dicePoint;

    private Vector3[] diceStartPos;
    private Rigidbody[] rb;
    //private Vector3[] forces;
    private string namedChild = "dice";


    void Start () {
        rb = new Rigidbody[diceNum];
        //forces = new Vector3[diceNum];
        diceStartPos = new Vector3[diceNum];

        for (int i = 0; i < diceNum; i++)
        {
            diceStartPos[i] = new Vector3(1.5f * i, 0.02f * i, -4.8f);
        }
    }
	

    void OnCollisionEnter(Collision col) {
        if(col.gameObject.tag == "Table")
            Debug.Log("碰到桌子");
    }

    public void ThrowDices() {
        StopCoroutine("FadeDices");

        for (int i = 0; i < diceNum; i++)
        {
            GameObject go = Instantiate(dicePrefab, diceStartPos[i], Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)))) as GameObject;
            go.transform.SetParent(transform);
            go.transform.localPosition = diceStartPos[i];
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
            go.transform.localScale = Vector3.one;

            rb[i] = go.GetComponent<Rigidbody>();

            rb[i].velocity = (new Vector3(Random.Range(0, 2), downForce*2, Random.Range(0, 2)));
            rb[i].AddTorque(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));

            //forces[i] = new Vector3(Random.Range(-500, 500), downForce * 800, Random.Range(0, 500));
            //rb[i].AddForce(forces[i]);
        }

        StartCoroutine("FadeDices", delayFade);
    }

    IEnumerator FadeDices(float delayTime) {
        yield return new WaitForSeconds(delayTime);

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject go = transform.GetChild(i).gameObject;
            Destroy(go);
        }

        RandomDicePoint();
    }

    void RandomDicePoint() {
        int randomPoint = Random.Range(1, 7) + Random.Range(1, 7) + Random.Range(1, 7);
        dicePoint.text = "Dice Point : " + randomPoint;
    }
}
