using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class KyokuInfoPanel : MonoBehaviour 
{
	public Image uiPanel;
	public Text lab_kyoku;
	public Text lab_honba;


    void Start()
    {
        if(uiPanel == null)
			uiPanel = GetComponent<Image>();
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show( string kyokuStr, string honbaStr )
    {
        gameObject.SetActive(true);

		//uiPanel.GetComponent<CanvasRenderer>().SetAlpha(0.1f);
		if (uiPanel) {
			uiPanel.CrossFadeAlpha (1, 0.1f, false);
			uiPanel.gameObject.SetActive (true);
		}
		if(lab_kyoku)
        	lab_kyoku.text = kyokuStr;
		if(lab_honba)
        	lab_honba.text = honbaStr;

        if( string.IsNullOrEmpty(honbaStr) ){
			if (lab_kyoku) {
				//lab_kyoku.pivot = UIWidget.Pivot.Center;
				lab_kyoku.transform.localPosition = Vector3.zero;
			}
        }
        else{
			if (lab_kyoku) {
				//lab_kyoku.pivot = UIWidget.Pivot.Right;
				lab_kyoku.transform.localPosition = Vector3.zero;
			}
        }

        StartCoroutine( Show_Internel() );
    }

    IEnumerator Show_Internel()
    {
        float Duration = 0.4f;

        //TweenAlpha.Begin(gameObject, Duration, 1f).method = UITweener.Method.EaseIn;
		if (uiPanel) {
			uiPanel.CrossFadeAlpha (1, Duration, false);
			uiPanel.gameObject.SetActive (true);
		}
        yield return new WaitForSeconds(Duration);

        // stay time.
        yield return new WaitForSeconds(1f);

        //TweenAlpha.Begin(gameObject, Duration, 0f).method = UITweener.Method.EaseOut;
		if (uiPanel) {
			uiPanel.CrossFadeAlpha (0.1f, Duration, false);
			uiPanel.gameObject.SetActive (true);
		}
		yield return new WaitForSeconds(Duration);

        yield return new WaitForSeconds(Duration);
        OnEnd();
    }

    void OnEnd()
    {
        Hide();

		EventManager.Instance.RpcSendEvent(UIEventType.On_UIAnim_End);
    }
}
