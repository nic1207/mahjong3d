using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class SelectChiiChaPanel : MonoBehaviour
{
    private List<Vector3> kazePosList = new List<Vector3>();
	public List<Image> pais = new List<Image> ();
	public List<Button> buttons = new List<Button> ();
	private List<Hai> kazePaiList = new List<Hai>();
	private Dictionary<int, Image> cachePais = new Dictionary<int, Image> ();

    private float posY = -40f;
    private float leftPosX = -150f;
    private float offset = 100f;

    private int chiiChaIndex = -1;
	//private Hai[] init_hais;


    void Start(){
		Vector3 pos0 = new Vector3 (0, -Screen.height/2, 0);//self
		Vector3 pos1 = new Vector3 (-Screen.width/2, 0, 0);//prev
		Vector3 pos2 = new Vector3 (0, Screen.height/2, 0);//upp
		Vector3 pos3 = new Vector3 (Screen.width/2, 0, 0);//next

		kazePosList.Add (pos0);
		kazePosList.Add (pos1);
		kazePosList.Add (pos2);
		kazePosList.Add (pos3);
    }


    public void Hide()
    {
        kazePaiList.Clear();
        gameObject.SetActive(false);
    }

    public void Show()
    {
		Hai[] init_hais = new Hai[4]{
			new Hai(Hai.ID_TON),//27  %4 = 3
			new Hai(Hai.ID_NAN),//28  %4 = 0
			new Hai(Hai.ID_SYA),//29  %4 = 1
			new Hai(Hai.ID_PE),//30   %4 = 2
        };


        Hai temp;
        for( int i = 0; i < init_hais.Length; i++ )
        {
            int index = Random.Range(0, init_hais.Length);

            temp = init_hais[i];
            init_hais[i] = init_hais[index];
            init_hais[index] = temp;
        }
		

        gameObject.SetActive(true);
		kazePaiList = init_hais.ToList ();
		//cachePais
		//int rd = Utils.GetRandomNum(0,4);
		//OnClickMahjong(rd);
		StartCoroutine (_clickMj());
    }

	private IEnumerator _clickMj()
	{
		yield return new WaitForSeconds (1.0f);
		int rd = Utils.GetRandomNum(0,4);
		//rd = 3;
		OnClickMahjong(rd);
	}


	public void OnClickMahjong(int index)
    {
		for( int i = 0; i < buttons.Count; i++ )
        {
			buttons [i].gameObject.SetActive (false);
        }
		Hai pai = kazePaiList [index];
		int paiID = pai.ID;
		string name = ResManager.getMagjongName (pai.Kind, pai.Num);
		//Debug.Log ("OnClickMahjong("+name+")"+paiID);
		chiiChaIndex = ((Hai.ID_TON - paiID) + 4) % 4;
        //Debug.Log("chiiChaIndex: " + chiiChaIndex.ToString());

		for (int i = 0; i < pais.Count; i++) {
			Hai h = kazePaiList [i];
			pais [i].sprite = ResManager.getMahjongSprite( h.Kind, h.Num);
			pais [i].transform.parent.gameObject.SetActive (true);
			cachePais.Add (h.ID, pais [i]);
		}

        StartCoroutine( MoveMahjongPaiToKaze(paiID, index) );
    }


	IEnumerator MoveMahjongPaiToKaze( int startID , int index)
    {
		//Debug.Log ("startID="+startID);
		yield return new WaitForSeconds(0.3f);
		for( int i = index, id = startID, j = 0; i < index+kazePaiList.Count; i++, id++, j++)
        {
            if( id > Hai.ID_PE )
                id = Hai.ID_TON;

			//Hai h = kazePaiList.Find( mp => mp.ID == id );
			Image img = null;
			cachePais.TryGetValue (id, out img);

            //yield return new WaitForSeconds(0.4f);
			//GameObject go = new GameObject ("Xxx");
			//int xx = chiiChaIndex;
			//Debug.Log ("xx="+xx);
			TweenPosition tweener = TweenPosition.Begin( img.transform.parent.gameObject, 0.8f, kazePosList[j] );
            tweener.style = UITweener.Style.Once;
			//yield return new WaitForSeconds(0.3f);
            if( j == kazePaiList.Count-1 )
                tweener.SetOnFinished( OnMoveEnd );
			
            //yield return new WaitForSeconds(0.2f);
        }
		//yield return new WaitForSeconds(0.3f);
    }

    void OnMoveEnd()
    {
        Invoke("OnEnd", 0.5f);
    }

    void OnEnd()
    {
        Hide();
		EventManager.Instance.RpcSendEvent(UIEventType.On_Select_ChiiCha_End, chiiChaIndex);
    }

}
