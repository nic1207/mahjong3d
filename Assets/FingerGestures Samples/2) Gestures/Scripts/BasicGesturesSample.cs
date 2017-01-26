using UnityEngine;
using System.Collections;

/// <summary>
/// This sample demonstrates the use of the following gestures via messages sent by their 
/// respective GestureRecognizers (on the same object)
/// - Swipe (SwipeRecognizer)
/// - Tap & DoubleTap (TapRecognizer)
/// - Drag (DragRecognizer)
/// - LongPress (LongPressRecognizer)
/// </summary>
public class BasicGesturesSample : SampleBase
{
    #region Gesture event messages sent by the various gesture recognizers

    // spin the yellow cube when swipping it
    void OnSwipe( SwipeGesture gesture )
    {
        // make sure we started the swipe gesture on our swipe object
        //   we use the object the swipe started on, instead of the current one
        GameObject selection = gesture.StartSelection;

        if( selection == swipeObject )
        {
            UI.StatusText = "Swiped " + gesture.Direction + " with finger " + gesture.Fingers[0] +
                " (velocity:" + gesture.Velocity + ", distance: " + gesture.Move.magnitude + " )";

            Debug.Log( UI.StatusText );

            SwipeParticlesEmitter emitter = selection.GetComponentInChildren<SwipeParticlesEmitter>();
            if( emitter )
                emitter.Emit( gesture.Direction, gesture.Velocity );
        }
    }

    void OnTap( TapGesture gesture )
    {
        if( gesture.Selection == tapObject )
        {
            SpawnParticles( tapObject );
            UI.StatusText = "Tapped with finger " + gesture.Fingers[0];
        }
    }
    
    void OnDoubleTap( TapGesture gesture )
    {
        if( gesture.Selection == doubleTapObject )
        {
            SpawnParticles( doubleTapObject );
            UI.StatusText = "Double-Tapped with finger " + gesture.Fingers[0];
        }
    }

    void OnLongPress( LongPressGesture gesture )
    {
        if( gesture.Selection == longPressObject )
        {
            SpawnParticles( longPressObject );
            UI.StatusText = "Performed a long-press with finger " + gesture.Fingers[0];
        }
    }

    

    int dragFingerIndex = -1;

    void OnDrag( DragGesture gesture )
    {
        // first finger
        FingerGestures.Finger finger = gesture.Fingers[0];

        if( gesture.Phase == ContinuousGesturePhase.Started )
        {
            // dismiss this event if we're not interacting with our drag object
            if( gesture.Selection != dragObject )
                return;

            UI.StatusText = "Started dragging with finger " + finger;

            // remember which finger is dragging dragObject
            dragFingerIndex = finger.Index;

            // spawn some particles because it's cool.
            SpawnParticles( dragObject );
        }
        else if( finger.Index == dragFingerIndex )  // gesture in progress, make sure that this event comes from the finger that is dragging our dragObject
        {
            if( gesture.Phase == ContinuousGesturePhase.Updated )
            {
                // update the position by converting the current screen position of the finger to a world position on the Z = 0 plane
                dragObject.transform.position = GetWorldPos( gesture.Position );
            }
            else
            {
                UI.StatusText = "Stopped dragging with finger " + finger;

                // reset our drag finger index
                dragFingerIndex = -1;

                // spawn some particles because it's cool.
                SpawnParticles( dragObject );

            }
        }
    }

    #endregion

    #region Properties exposed to the editor

    public GameObject longPressObject;
    public GameObject tapObject;
    public GameObject doubleTapObject;
    public GameObject swipeObject;
    public GameObject dragObject;

    #endregion


    #region Misc

    protected override string GetHelpText()
    {
        return @"This sample demonstrates some of the supported single-finger gestures:

- Drag: press the red sphere and move your finger to drag it around  

- LongPress: keep your finger pressed on the cyan sphere for a few seconds

- Tap: press & release the purple sphere 

- Double Tap: quickly press & release the green sphere twice in a row

- Swipe: press the yellow sphere and move your finger in one of the four cardinal directions, then release. The speed of the motion is taken into account.";
    }

    #endregion 

    #region Utils

    void SpawnParticles( GameObject obj )
    {
        ParticleEmitter emitter = obj.GetComponentInChildren<ParticleEmitter>();
        if( emitter )
            emitter.Emit();
    }

    #endregion
}
