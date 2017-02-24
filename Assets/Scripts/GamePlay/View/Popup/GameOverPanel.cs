using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class GameOverPanel : MonoBehaviour 
{
    public Text lab_reachbou;
    public Button btn_Continue;
    public List<UIPlayerTenbouChangeInfo> playerTenbouList = new List<UIPlayerTenbouChangeInfo>();

    private AgariUpdateInfo currentAgari;


    void Start(){
		if (btn_Continue) {
			btn_Continue.onClick.AddListener(OnClickConfirm);
			Text btnTag = btn_Continue.GetComponentInChildren<Text> (true);
			btnTag.text = ResManager.getString ("replay");
		}
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show( List<AgariUpdateInfo> agariList )
    {
        currentAgari = agariList[0];

        gameObject.SetActive(true);

        Show_Internel();
    }

    void Show_Internel()
    {
		if(lab_reachbou)
        	lab_reachbou.text = "x" + currentAgari.reachBou.ToString();

        var tenbouInfos = currentAgari.tenbouChangeInfoList;
        EKaze nextKaze = currentAgari.manKaze;

        for( int i = 0; i < playerTenbouList.Count; i++ )
        {
            PlayerTenbouChangeInfo info = tenbouInfos.Find( ptci=> ptci.playerKaze == nextKaze );
            playerTenbouList[i].SetPointInfo( info.playerKaze, info.current, info.changed );
            nextKaze = nextKaze.Next();
        }
    }


    void OnClickConfirm()
    {
        Hide();

		GameClientManager.Get().Restart();
    }

}
