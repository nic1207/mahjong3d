using UnityEngine;
using System.Collections;

/// <summary>
/// This sample demonstrates uses the following Toolbox scripts:
/// - TBFingerDown
/// - TBFingerUp
/// </summary>
public class ToolboxFingerEventsSample : SampleBase
{
    #region Properties exposed to the editor

    public Light light1;
    public Light light2;

    #endregion

    // this is called by TBFingerDown message
    void ToggleLight1()
    {
        light1.enabled = !light1.enabled;
    }

    // this is called by TBFingerUp message
    void ToggleLight2()
    {
        light2.enabled = !light2.enabled;
    }

    #region Misc

    protected override string GetHelpText()
    {
        return @"This sample demonstrates the use of the toolbox scripts TBFingerDown and TBFingerUp. It also shows how you can use the message target property to turn the light on & off.";
    }

    #endregion
}
