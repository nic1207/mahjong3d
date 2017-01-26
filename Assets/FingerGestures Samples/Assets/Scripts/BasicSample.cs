using UnityEngine;
using System.Collections;

public class BasicSample : SampleBase
{
    public string helpText = "Help text here";
    protected override string GetHelpText()
    {
        return helpText;
    }

    public string statusText = "";
    
    protected override void Start()
    {
        base.Start();
        UI.StatusText = statusText;
    }
}
