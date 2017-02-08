using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HorseRun : MonoBehaviour {
    public static HorseRun instance;

    [HideInInspector]
    public bool _horseRun = false;

    [HideInInspector]
    public float _horseLength;

    private Text _HorseText;

    [HideInInspector]
    private bool _passFlag = false; //通知是否可開放下一得獎者

    void Start() {
        instance = this;

        _HorseText = gameObject.GetComponent<Text>();
    }

	void Update () {
        if (_horseRun) {
            
            if (_HorseText.rectTransform.anchoredPosition.x > _horseLength * (-1))
            {
                _HorseText.rectTransform.anchoredPosition = new Vector2(_HorseText.rectTransform.anchoredPosition.x - 2, _HorseText.rectTransform.anchoredPosition.y);

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
                HorseLight.instance.HorseGoal(name);
            }
        }
	}

    private void CheckTailPass() {
        HorseLight.instance._acceptPass = true;
        HorseLight.instance.ReadyToStart();
    }
}
