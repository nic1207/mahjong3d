using UnityEngine;
using System.Collections;


public class UIPlayerTenbouChangeInfo : MonoBehaviour
{
    public UILabel lab_kaze;
    public UILabel lab_current;
    public UILabel lab_change;
    public UILabel lab_tenpai;


    public void SetInfo(EKaze kaze, int curTenbou, int changeValue, bool isTenpai = false, bool showTenpai = false)
    {
        lab_kaze.text = ResManager.getString( "kaze_" + kaze.ToString().ToLower() );

        lab_current.text = curTenbou.ToString();

        if( changeValue > 0 ){
            lab_change.color = Color.blue;
            lab_change.text = "+" + changeValue.ToString();
        }
        else if( changeValue < 0 ){
            lab_change.color = Color.red;
            lab_change.text = "" + changeValue.ToString();
        }
        else{
            lab_change.text = "";
        }

        if( isTenpai ){
            lab_tenpai.text = ResManager.getString("is_tenpai");
        }
        else{
            lab_tenpai.text = ResManager.getString("not_tenpai");
        }

        lab_tenpai.gameObject.SetActive( showTenpai );
    }

    public void SetPointInfo( EKaze kaze, int tenbou, int point )
    {
        lab_kaze.text = ResManager.getString( "kaze_" + kaze.ToString().ToLower() );

        lab_current.text = tenbou.ToString();

        if( point > 0 ){
            lab_change.color = Color.blue;
            lab_change.text = "+" + point.ToString() + "pt";
        }
        else if( point < 0 ){
            lab_change.color = Color.red;
            lab_change.text = "" + point.ToString() + "pt";
        }
        else{
            lab_change.text = "";
        }

        lab_tenpai.gameObject.SetActive( false );
    }
}
