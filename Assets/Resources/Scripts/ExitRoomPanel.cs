using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExitRoomPanel : MonoBehaviour {
    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
