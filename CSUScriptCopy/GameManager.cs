using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {

    public List<GameObject> cardList;
    public List<GameObject> takenList;
    public GameObject[] cards;
    //public Transform player_firstCardPos;
    //public Transform upPlayer_firstCardPos;
    //public Transform oppositePlayer_firstCardPos;
    //public Transform downPlayer_firstCardPos;
    public Vector3 cardOffsetPos = new Vector3(0.63f ,0f ,0f);

    //private Transform[] firstCardPos;
    private List<Vector3> eachCardPos = new List<Vector3>();
    private List<Transform> playerCards;   
    //private List<Vector3> cardPositions = new List<Vector3>();

    // Use this for initialization
    void Start () {
        Initialized();
    }
	

    void Initialized() {
        cards = Resources.LoadAll<GameObject>("Cards");

        cardList = cards.ToList();

        //GameObject cardToBuild = cardList[Random.Range(0, cardList.Count)];
        //Debug.Log("Total Num = " + cardList.Count);
        //GameObject newCard = Instantiate(cardToBuild, transform.position, Quaternion.identity) as GameObject;

        // Create All Cards
        //foreach (GameObject card in cardList) {
        //    Instantiate(card, transform.position, Quaternion.identity);
        //}

        //firstCardPos = new Transform[] { player_firstCardPos, upPlayer_firstCardPos, oppositePlayer_firstCardPos, downPlayer_firstCardPos };

        playerCards = new List<Transform> { GameObject.Find("PlayerCards").transform, GameObject.Find("UpPlayerCards").transform, GameObject.Find("OppositePlayerCards").transform, GameObject.Find("DownPlayerCards").transform };

        for (int i = 0; i < 17; i++) {
            eachCardPos.Add(cardOffsetPos * i);
        }

        TakeCards();
    }

    void TakeCards() {
        //Set Player Cards Positions
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                //GameObject instance = Instantiate(RandomCards(), firstCardPos[j].position + cardOffsetPos * i, Quaternion.identity) as GameObject;
                GameObject instance = Instantiate(RandomCards(), eachCardPos[i], Quaternion.identity) as GameObject;
                instance.transform.SetParent(playerCards[j]);
                instance.transform.localPosition = eachCardPos[i];
                instance.transform.localRotation = Quaternion.Euler(Vector3.zero); 
            }
        }
        //Debug.Log("剩餘 " + cardList.Count + " 張牌");
        Debug.Log("這副牌有 " + playerCards[0].childCount + " 張牌");
        //Arrang(0);
    }

    GameObject RandomCards() {
        int randomIndex = Random.Range(0, cardList.Count);
        GameObject randomCard = cardList[randomIndex];
        takenList.Add(randomCard);
        cardList.RemoveAt(randomIndex);
        //Debug.Log("Total Num = " + cardList.Count);
        return randomCard;
    }

    public void ResetGame() {
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < playerCards[i].childCount; j++)
            {
                Destroy(playerCards[i].GetChild(j).gameObject);
            }
        }

        cardList.AddRange(takenList);
        takenList.Clear();
        TakeCards();

        //Debug.Log("Total Num = " + cardList.Count);
        //Arrang(0);
    }

    public void ArrangButton(int index) {
        Arrang(index);
    }

    public void ArrangButton()
    {
        for (int i = 0; i < 4; i++) {
            Arrang(i);
        }
    }

    //Arrange Cards
    void Arrang(int index) {
        List<Transform> card_Sol = new List<Transform>();
        List<Transform> card_Wan = new List<Transform>();
        List<Transform> card_Ton = new List<Transform>();
        List<Transform> card_Zi = new List<Transform>();
        List<Transform> card_Others = new List<Transform>();

        foreach (Transform child in playerCards[index])
        {
            if (child.CompareTag("Sol"))
            {
                card_Sol.Add(child);
                //Debug.Log("找到索 = " + child.name);
            }

            else if (child.CompareTag("Wan"))
            {
                card_Wan.Add(child);
                //Debug.Log("找到萬 = " + child.name);
            }

            else if (child.CompareTag("Ton"))
            {
                card_Ton.Add(child);
                //Debug.Log("找到筒 = " + child.name);
            }

            else if (child.CompareTag("Zi"))
            {
                card_Zi.Add(child);
                //Debug.Log("找到字 = " + child.name);
            }
            else
            {
                card_Others.Add(child);
            }
        }
        card_Sol.Sort((x, y) => { return x.name.Substring(0, 4).CompareTo(y.name.Substring(0, 4)); });
        card_Wan.Sort((x, y) => { return x.name.Substring(0, 4).CompareTo(y.name.Substring(0, 4)); });
        card_Ton.Sort((x, y) => { return x.name.Substring(0, 4).CompareTo(y.name.Substring(0, 4)); });
        card_Zi.Sort((x, y) => { return x.name.Substring(0, 4).CompareTo(y.name.Substring(0, 4)); });


        for (int i = 0; i < card_Sol.Count; i++)
        {
            card_Sol[i].localPosition = eachCardPos[i];
        }
        for (int i = 0; i < card_Wan.Count; i++)
        {
            card_Wan[i].localPosition = eachCardPos[card_Sol.Count + i];
        }
        for (int i = 0; i < card_Ton.Count; i++)
        {
            card_Ton[i].localPosition = eachCardPos[card_Sol.Count + card_Wan.Count + i];
        }
        for (int i = 0; i < card_Zi.Count; i++)
        {
            card_Zi[i].localPosition = eachCardPos[card_Sol.Count + card_Wan.Count + card_Ton.Count + i];
        }
    }

    //Player Take Card
    public void TakeCard() {
        if (playerCards[0].childCount < 17) {
            GameObject instance = Instantiate(RandomCards(), eachCardPos[16], Quaternion.identity) as GameObject;
            instance.transform.SetParent(playerCards[0]);
            instance.transform.localPosition = eachCardPos[16] + Vector3.right/4;
            instance.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

    }
}
