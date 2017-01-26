using UnityEngine;
using System.Collections;

/// <summary>
/// This sample demonstrates how to use the two-fingers Pinch and Twist gesture events to control the scale and orientation of a rectangle on the screen
/// </summary>
public class PinchTwistSample : SampleBase
{
    public enum InputMode
    {
        PinchOnly,
        TwistOnly,
        PinchAndTwist
    }

    public Transform target;
    public Material twistMaterial;
    public Material pinchMaterial;
    public Material pinchAndTwistMaterial;
    public float pinchScaleFactor = 0.02f;

    bool rotating = false;
    bool pinching = false;
    Material originalMaterial;
    
    bool Rotating
    {
        get { return rotating; }
        set
        {
            if( rotating != value )
            {
                rotating = value;
                UpdateTargetMaterial();
            }
        }
    }

    bool Pinching
    {
        get { return pinching; }
        set
        {
            if( pinching != value )
            {
                pinching = value;
                UpdateTargetMaterial();
            }
        }
    }
    
    #region FingerGestures Messages

    void OnTwist( TwistGesture gesture )
    {
        if( gesture.Phase == ContinuousGesturePhase.Started )
        {
            UI.StatusText = "Twist gesture started";
            Rotating = true;
        }
        else if( gesture.Phase == ContinuousGesturePhase.Updated )
        {
            if( Rotating )
            {
                UI.StatusText = "Rotation updated by " + gesture.DeltaRotation + " degrees";

                // apply a rotation around the Z axis by rotationAngleDelta degrees on our target object
                target.Rotate( 0, 0, gesture.DeltaRotation );
            }
        }
        else
        {
            if( Rotating )
            {
                UI.StatusText = "Rotation gesture ended. Total rotation: " + gesture.TotalRotation;
                Rotating = false;
            }
        }
    }

    void OnPinch( PinchGesture gesture )
    {
        if( gesture.Phase == ContinuousGesturePhase.Started )
        {
            Pinching = true;
        }
        else if( gesture.Phase == ContinuousGesturePhase.Updated )
        {
            if( Pinching )
            {
                // change the scale of the target based on the pinch delta value
                target.transform.localScale += gesture.Delta.Centimeters() * pinchScaleFactor * Vector3.one;
            }
        }
        else
        {
            if( Pinching )
            {
                Pinching = false;
            }
        }
    }

    #endregion

    #region Misc

    void UpdateTargetMaterial()
    {
        Material m;

        if( pinching && rotating )
            m = pinchAndTwistMaterial;
        else if( pinching )
            m = pinchMaterial;
        else if( rotating )
            m = twistMaterial;
        else
            m = originalMaterial;

        target.GetComponent<Renderer>().sharedMaterial = m;
    }

    #endregion

    #region Setup

    protected override string GetHelpText()
    {
        return @"This sample demonstrates how to use the two-fingers Pinch and Rotation gesture events to control the scale and orientation of a rectangle on the screen

- Pinch: move two fingers closer or further apart to change the scale of the rectangle (mousewheel on desktop)
- Rotation: twist two fingers in a circular motion to rotate the rectangle (CTRL+mousewheel on desktop)

";
    }
    protected override void Start()
    {
        base.Start();

        UI.StatusText = "Use two fingers anywhere on the screen to rotate and scale the green object.";

        originalMaterial = target.GetComponent<Renderer>().sharedMaterial;
    }

    #endregion

}
