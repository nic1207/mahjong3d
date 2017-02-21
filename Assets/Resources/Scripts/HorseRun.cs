using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HorseRun : MonoBehaviour {
    public static HorseRun instance;

    [HideInInspector]
    public bool _horseRun = false; //是否正在跑

    [HideInInspector]
    public float _horseLength;

    private Text _HorseText;

    [HideInInspector]
    private bool _passFlag = false; //是否可開放下一得獎者

    void Start() {
        instance = this;

        _HorseText = gameObject.GetComponent<Text>();
    }

	void Update () {
        if (_horseRun) {
            
			if (_HorseText && _HorseText.rectTransform.anchoredPosition.x > _horseLength * (-1))
            {
                _HorseText.rectTransform.anchoredPosition = new Vector2(_HorseText.rectTransform.anchoredPosition.x - HorseLight.instance.RunSpeed, _HorseText.rectTransform.anchoredPosition.y);

                //放行下一得獎者
                if (!_passFlag && _HorseText.rectTransform.anchoredPosition.x < _horseLength * (-1) + 360) {
                    _passFlag = true;
                    CheckTailPass();
                }
            }
            else
            {
                _horseRun = false;
                _passFlag = false;
				if(HorseLight.instance)
                	HorseLight.instance.HorseGoal(name);
            }
        }
	}

    private void CheckTailPass() {
		if (HorseLight.instance) {
			HorseLight.instance._acceptPass = true;
			HorseLight.instance.ReadyToStart ();
		}
    }
}
