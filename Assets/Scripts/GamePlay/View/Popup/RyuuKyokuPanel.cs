using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class RyuuKyokuPanel : MonoBehaviour
{
	public Button btn_Continue;
	public Text lab_msg;

    public List<UIPlayerTenbouChangeInfo> playerTenbouList = new List<UIPlayerTenbouChangeInfo>();

    private ERyuuKyokuReason ryuuKyokuReason;
    private AgariUpdateInfo currentAgari;
    private float _delayTime = 2f; // stay UI time to continue;


    void Start(){
		if (btn_Continue) {
			btn_Continue.onClick.AddListener(OnConfirm);
			Text btnTag = btn_Continue.GetComponentInChildren<Text> (true);
			btnTag.text = ResManager.getString ("continue");
		}

        
    }

    IEnumerator AutoContinue(float _delayTime) {
        yield return new WaitForSeconds(_delayTime);

        OnConfirm();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(ERyuuKyokuReason reason, List<AgariUpdateInfo> agariList)
    {
        this.ryuuKyokuReason = reason;
        this.currentAgari = agariList[0];

        gameObject.SetActive(true);

        Show_Internel();
        //OnConfirm ();

        StartCoroutine(AutoContinue(_delayTime));
    }

    void Show_Internel()
    {
		if(lab_msg)
        	lab_msg.text = GetRyuuKyokuReasonString();

        PlayRyuuKyokuVoice();

        bool showTenpai = ryuuKyokuReason == ERyuuKyokuReason.NoTsumoHai;

        var tenbouInfos = currentAgari.tenbouChangeInfoList;
        EKaze nextKaze = currentAgari.manKaze;

        for( int i = 0; i < playerTenbouList.Count; i++ )
        {
            PlayerTenbouChangeInfo info = tenbouInfos.Find( ptci=> ptci.playerKaze == nextKaze );
            playerTenbouList[i].SetInfo( info.playerKaze, info.current, info.changed, info.isTenpai, showTenpai );
            nextKaze = nextKaze.Next();
        }
    }

    string GetRyuuKyokuReasonString()
    {
        return ResManager.getString( ryuuKyokuReason.ToString() );
    }

    void PlayRyuuKyokuVoice()
    {
        ECvType cv = ECvType.RyuuKyoku;
        if( ryuuKyokuReason == ERyuuKyokuReason.NoTsumoHai ){
            cv = ECvType.RyuuKyoku;
        }
        else if( ryuuKyokuReason == ERyuuKyokuReason.HaiTypeOver9 ){
            cv = ECvType.RKK_HaiTypeOver9;
        }
        else if( ryuuKyokuReason == ERyuuKyokuReason.SuteFonHai4 ){
            cv = ECvType.RKK_SuteFonHai4;
        }
        else if( ryuuKyokuReason == ERyuuKyokuReason.KanOver4 ){
            cv = ECvType.RKK_KanOver4;
        }
        else if( ryuuKyokuReason == ERyuuKyokuReason.Reach4 ){
            cv = ECvType.RKK_Reach4;
        }
        else if( ryuuKyokuReason == ERyuuKyokuReason.Ron3 ){
            cv = ECvType.RKK_Ron3;
        }
		GameClientManager.Get().Speak(cv);
    }

    void OnConfirm()
    {
		EventManager.Instance.RpcSendEvent(UIEventType.End_RyuuKyoku);
    }
}
