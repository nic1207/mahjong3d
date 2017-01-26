using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for all sample scripts
/// </summary>
[RequireComponent( typeof( SampleUI ) )]
public class SampleBase : MonoBehaviour
{
    protected virtual string GetHelpText()
    {
        return "";
    }

    // reference to the shared sample UI script
    SampleUI ui;
    public SampleUI UI
    {
        get { return ui; }
    }

    protected virtual void Awake()
    {
        ui = GetComponent<SampleUI>();
    }

    protected virtual void Start()
    {
        ui.helpText = GetHelpText();
    }

    #region Utils

    // Convert from screen-space coordinates to world-space coordinates on the Z = 0 plane
    public static Vector3 GetWorldPos( Vector2 screenPos )
    {
        Ray ray = Camera.main.ScreenPointToRay( screenPos );

        // we solve for intersection with z = 0 plane
        float t = -ray.origin.z / ray.direction.z;

        return ray.GetPoint( t );
    }

    #endregion
}
