using UnityEngine;
using System.Collections;

public class MaxFPS : MonoBehaviour 
{
    public int maxFPS = 60;

    void OnEnable()
    {
        Application.targetFrameRate = maxFPS;
        enabled = false;
    }
}
