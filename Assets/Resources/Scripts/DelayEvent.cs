using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class DelayEvent : MonoBehaviour {
    [SerializeField]
    private float delayTime;
    [SerializeField]
    private bool autoProcess;

    [SerializeField]
    private UnityEvent process;

    void Start()
    {

        if (this.autoProcess) this.Process();
    }

    public void Process()
    {

        Invoke("InvokeEvent", this.delayTime);
    }

    private void InvokeEvent()
    {

        this.process.Invoke();
    }
}
