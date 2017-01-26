using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIYakuItem : MonoBehaviour
{
    public Text lab_name;
	public Text lab_han;


    public void SetYaku( string key, int han )
    {
		if(lab_name)
        	lab_name.text = ResManager.getString(key);
		if(lab_han)
			lab_han.text = han.ToString() + ResManager.getString( "han" );
    }

    public void SetYakuMan( string key, bool doubleYakuman )
    {
		if(lab_name)
        	lab_name.text = ResManager.getString(key);

        if( doubleYakuman == true )
            lab_han.text = ResManager.getString("double") + ResManager.getString("yakuman");
        else
            lab_han.text = ResManager.getString("yakuman");
    }

}
