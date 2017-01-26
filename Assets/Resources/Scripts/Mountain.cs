using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Mountain : MonoBehaviour {
    public Transform[] startPos;
    public Text diceText;
    public Image[] highLightMark;

    private int playerNum = 4;
    private int allCardsNum = 136; //不含花牌
    private int randomHost; //隨機莊家
    private int randomDicePoint; //隨機骰子點數

    private int startTakePos;

    // Use this for initialization
    void Start () {
       // DecideHostAndDice();
    }

    public void DecideHostAndDice() {
        for (int i = 0; i < playerNum; i++) {
            startPos[i].parent.gameObject.SetActive(true);
        }

        randomHost = Random.Range(0, 4); //隨機決定莊家
        HighLight(randomHost);

        randomDicePoint = Random.Range(1, 7) + Random.Range(1, 7) + Random.Range(1, 7);
        diceText.text = "Dice Point :" + randomDicePoint;

        startTakePos = (randomHost + randomDicePoint - 1) % 4;
        TakeHostFrontCards();
    }

    void TakeHostFrontCards() {
        //startPos[startTakePos].parent.gameObject.SetActive(false);

        //for (int i = 0; i < 4; i++)
        //{
        //    startPos[startTakePos].parent.GetChild(randomDicePoint * 2 + i).position += new Vector3(0,1f,0);
        //}
        
    }

    // 輪到哪位玩家 頭頂提示三角形亮起 
    void HighLight(int index)
    {
        Color colorAlpha = new Color(1, 1, 1, 0.4f);
        Color colorHighLight = new Color(1, 0.9f, 0.7f, 1f);

        for (int i = 0; i < playerNum; i++)
        {
            if (i == index)
                highLightMark[i].color = colorHighLight;
            else
                highLightMark[i].color = colorAlpha;
        }
    }
}
