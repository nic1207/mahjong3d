using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This sample helps visualize the following finger events:
/// - Finger Down
/// - Finger Move (Started/Updated/Ended)
/// - Finger Stationary (Started/Updated/Ended)
/// - Finger Up
/// </summary>
[RequireComponent( typeof( FingerDownDetector ) )]
[RequireComponent( typeof( FingerMotionDetector ) )]
[RequireComponent( typeof( FingerUpDetector ) )]
[RequireComponent( typeof( ScreenRaycaster ) )]
public class FingerEventsSamplePart2 : SampleBase
{
    #region Properties exposed to the editor

    public LineRenderer lineRendererPrefab;
    public GameObject fingerDownMarkerPrefab;
    public GameObject fingerMoveBeginMarkerPrefab;
    public GameObject fingerMoveEndMarkerPrefab;
    public GameObject fingerUpMarkerPrefab;

    #endregion

    #region Utility class that represent a single finger path

    class PathRenderer
    {
        LineRenderer lineRenderer;

        // passage points
        List<Vector3> points = new List<Vector3>();

        // list of marker objects currently instantiated
        List<GameObject> markers = new List<GameObject>();

        public PathRenderer( int index, LineRenderer lineRendererPrefab )
        {
            lineRenderer = Instantiate( lineRendererPrefab ) as LineRenderer;
            lineRenderer.name = lineRendererPrefab.name + index;
            lineRenderer.enabled = true;

            UpdateLines();
        }

        public void Reset()
        {
            points.Clear();
            UpdateLines();
            
            // destroy markers
            foreach( GameObject marker in markers )
                Destroy( marker );

            markers.Clear();
        }

        public void AddPoint( Vector2 screenPos )
        {
            AddPoint( screenPos, null );
        }

        public void AddPoint( Vector2 screenPos, GameObject markerPrefab )
        {
            Vector3 pos = SampleBase.GetWorldPos( screenPos );

            if( markerPrefab )
                AddMarker( pos, markerPrefab );

            points.Add( pos );
            UpdateLines();
        }

        GameObject AddMarker( Vector2 pos, GameObject prefab )
        {
            GameObject instance = Instantiate( prefab, pos, Quaternion.identity ) as GameObject;
            instance.name = prefab.name + "(" + markers.Count + ")";
            markers.Add( instance );
            return instance;
        }

        void UpdateLines()
        {
            lineRenderer.SetVertexCount( points.Count );
            for( int i = 0; i < points.Count; ++i )
                lineRenderer.SetPosition( i, points[i] );
        }
    }

    #endregion

    // one PathRenderer per finger
    PathRenderer[] paths;

    #region Sample Setup

    protected override void Start()
    {
        base.Start();

        UI.StatusText = "Drag your fingers anywhere on the screen";

        // create one PathRenderer per finger
        paths = new PathRenderer[FingerGestures.Instance.MaxFingers];
        for( int i = 0; i < paths.Length; ++i )
            paths[i] = new PathRenderer( i, lineRendererPrefab );
    }

    protected override string GetHelpText()
    {
        return @"This sample lets you visualize the FingerDown, FingerMoveBegin, FingerMove, FingerMoveEnd and FingerUp events.

INSTRUCTIONS:
Move your finger accross the screen and observe what happens.

LEGEND:
- Red Circle = FingerDown position
- Yellow Square = FingerMoveBegin position
- Green Sphere = FingerMoveEnd position
- Blue Circle = FingerUp position";
    }

    #endregion

    #region FingerGesture - FingerEventDetector messages

    void OnFingerDown( FingerDownEvent e )
    {
        PathRenderer path = paths[e.Finger.Index];
        path.Reset();
        path.AddPoint( e.Finger.Position, fingerDownMarkerPrefab );
    }

    void OnFingerMove( FingerMotionEvent e )
    {
        PathRenderer path = paths[e.Finger.Index];

        if( e.Phase == FingerMotionPhase.Started )
        {
            UI.StatusText = "Started moving " + e.Finger;
            path.AddPoint( e.Position, fingerMoveBeginMarkerPrefab );
        }
        else if( e.Phase == FingerMotionPhase.Updated )
        {
            path.AddPoint( e.Position );
        }
        else
        {
            UI.StatusText = "Stopped moving " + e.Finger;
            path.AddPoint( e.Position, fingerMoveEndMarkerPrefab );
        }
    }

    void OnFingerUp( FingerUpEvent e )
    {
        PathRenderer path = paths[e.Finger.Index];
        path.AddPoint( e.Finger.Position, fingerUpMarkerPrefab );
        UI.StatusText = "Finger " + e.Finger + " was held down for " + e.TimeHeldDown.ToString( "N2" ) + " seconds";
    }


    #endregion
}
