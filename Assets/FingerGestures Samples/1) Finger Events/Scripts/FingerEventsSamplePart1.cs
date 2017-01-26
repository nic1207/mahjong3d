using UnityEngine;
using System.Collections;

/// <summary>
/// This sample demonstrates the following Finger Event Detectors:
/// - FingerDownDetector
/// - FingerUpDetector
/// - FingerMotionDetector (for the Stationary event)
/// - FingerHoverDetector
/// </summary>
[RequireComponent( typeof( FingerDownDetector ) )]
[RequireComponent( typeof( FingerMotionDetector ) )]
[RequireComponent( typeof( FingerHoverDetector ) )]
[RequireComponent( typeof( FingerUpDetector ) )]
[RequireComponent( typeof( ScreenRaycaster ) )]
public class FingerEventsSamplePart1 : SampleBase
{
    #region Properties exposed to the editor

    public GameObject fingerDownObject;
    public GameObject fingerStationaryObject;
    public GameObject fingerHoverObject;
    public GameObject fingerUpObject;

    public float chargeDelay = 0.5f;
    public float chargeTime = 5.0f;
    public float minSationaryParticleEmissionCount = 5;
    public float maxSationaryParticleEmissionCount = 50;
    public Material highlightMaterial;

    #endregion

    int stationaryFingerIndex = -1;
    Material originalStationaryMaterial;
    Material originalHoverMaterial;

    #region Finger Detector Events (sent by the various FingerEventDetectors on the same object)

    void OnFingerDown( FingerDownEvent e )
    {
        if( e.Selection == fingerDownObject )
            SpawnParticles( fingerDownObject );
    }

    void OnFingerUp( FingerUpEvent e )
    {
        if( e.Selection == fingerUpObject )
            SpawnParticles( fingerUpObject );

        // The finger object contains useful information not available through the event arguments that you might want to use
        FingerGestures.Finger finger = e.Finger;

        Debug.Log( "Finger was lifted up on " + ( e.Selection ? e.Selection.name : "<nothing>" ) + " at " + finger.Position + " and moved " + finger.DistanceFromStart.ToString( "N0" ) + " pixels from its initial position at " + finger.StartPosition +
            ". It was held down for " + e.TimeHeldDown + " seconds" );
    }

    void OnFingerHover( FingerHoverEvent e )
    {
        if( e.Selection == fingerHoverObject )
        {
            // finger entered the object
            if( e.Phase == FingerHoverPhase.Enter )
            {
                UI.StatusText = "Finger entered " + fingerHoverObject.name;

                originalHoverMaterial = fingerHoverObject.GetComponent<Renderer>().sharedMaterial;
                fingerHoverObject.GetComponent<Renderer>().sharedMaterial = highlightMaterial;
            }
            else if( e.Phase == FingerHoverPhase.Exit ) // finger left the object
            {
                UI.StatusText = "Finger left " + fingerHoverObject.name;
                fingerHoverObject.GetComponent<Renderer>().sharedMaterial = originalHoverMaterial;
            }
        }
    }

    void OnFingerStationary( FingerMotionEvent e )
    {
        if( e.Phase == FingerMotionPhase.Started )
        {
            // skip if we're already holding another finger stationary on our object
            if( stationaryFingerIndex != -1 )
                return;

            GameObject selection = e.Selection;

            if( selection == fingerStationaryObject )
            {
                UI.StatusText = "Begin stationary on finger " + e.Finger.Index;

                // remember which finger we're using
                stationaryFingerIndex = e.Finger.Index;

                // remember the original material 
                originalStationaryMaterial = selection.GetComponent<Renderer>().sharedMaterial;

                // change the material to show we've started the stationary state
                selection.GetComponent<Renderer>().sharedMaterial = highlightMaterial;
            }
        }
        else if( e.Phase == FingerMotionPhase.Updated )
        {
            if( e.ElapsedTime < chargeDelay )
                return;

            if( e.Selection == fingerStationaryObject )
            {
                // compute charge progress % (0 to 1)
                float chargePercent = Mathf.Clamp01( ( e.ElapsedTime - chargeDelay ) / chargeTime );

                // compute and apply new particle emission rate based on charge %
                float emissionRate = Mathf.Lerp( minSationaryParticleEmissionCount, maxSationaryParticleEmissionCount, chargePercent );
                stationaryParticleEmitter.minEmission = emissionRate;
                stationaryParticleEmitter.maxEmission = emissionRate;

                // make sure the emitter is turned on
                stationaryParticleEmitter.emit = true;

                UI.StatusText = "Charge: " + ( 100 * chargePercent ).ToString( "N1" ) + "%";
            }
        }
        else if( e.Phase == FingerMotionPhase.Ended )
        {
            if( e.Finger.Index == stationaryFingerIndex )
            {
                float timeStationary = e.ElapsedTime;

                UI.StatusText = "Stationary ended on finger " + e.Finger + " - " + timeStationary.ToString( "N1" ) + " seconds elapsed";

                // turn off the stationary particle emitter when we begin to move the finger, as it's no longer stationary
                StopStationaryParticleEmitter();

                // restore the original material
                fingerStationaryObject.GetComponent<Renderer>().sharedMaterial = originalStationaryMaterial;

                // reset our stationary finger index
                stationaryFingerIndex = -1;
            }
        }
    }

    #endregion

    #region Setup

    protected override string GetHelpText()
    {
        return @"This sample lets you visualize and understand the OnFingerDown, OnFingerStationary and OnFingerUp events.

INSTRUCTIONS:
- Press, hold and release the red and blue spheres
- Press & hold the green sphere without moving for a few seconds
- Move your finger over and out of the cyan OnFingerHover sphere";
    }

    ParticleEmitter stationaryParticleEmitter;

    protected override void Start()
    {
        base.Start();

        if( fingerStationaryObject )
            stationaryParticleEmitter = fingerStationaryObject.GetComponentInChildren<ParticleEmitter>();
    }

    void StopStationaryParticleEmitter()
    {
        stationaryParticleEmitter.emit = false;
        UI.StatusText = "";
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
