using UnityEngine;
using System.Collections;

public class AnimationEvent : MonoBehaviour {
    public void MoreBtnUnfocus() {
        Lobby_UIManager.instance.ResetAllBtnColor();
    }

    public void MoreBtnfocus()
    {
        Lobby_UIManager.instance.RemainMoreBtnTextColor();
    }
}
