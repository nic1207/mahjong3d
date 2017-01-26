using UnityEngine;
using System.Collections;


public class PlayerInputButton : UIButton
{
    public int index;

    protected UILabel lab_tag;
    public UILabel TagLabel
    {
        get{
            if(lab_tag == null){
                lab_tag = GetComponentInChildren<UILabel>();
            }
            return lab_tag;
        }
    }


    public void SetTag( string tag )
    {
        TagLabel.text = tag;
    }

    public void SetEnable(bool state)
    {
        isEnabled = state;

        TagLabel.color = state? Color.red : Color.gray;
    }

}
