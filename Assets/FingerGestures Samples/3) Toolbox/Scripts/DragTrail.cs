using UnityEngine;
using System.Collections;

/// <summary>
/// Attaches a trailing effect when the object is being dragged.
/// </summary>
public class DragTrail : MonoBehaviour 
{
	public LineRenderer lineRendererPrefab;
	LineRenderer lineRenderer;

	void Start()
	{
		lineRenderer = Instantiate( lineRendererPrefab, transform.position, transform.rotation ) as LineRenderer;
		lineRenderer.transform.parent = this.transform;
		lineRenderer.enabled = false;
	}
        
	void Update()
	{
		if( lineRenderer.enabled )
		{
			// update position of the line's end point
			lineRenderer.SetPosition( 1, transform.position );
		}
	}

    #region FingerGestures Events

    void OnDrag( DragGesture gesture )
    {
        if( gesture.Phase == ContinuousGesturePhase.Started )
        {
            // initialize the line renderer
            lineRenderer.enabled = true;
            lineRenderer.SetPosition( 0, transform.position );
            lineRenderer.SetPosition( 1, transform.position );

            // keep end point width in sync with object's current scale
            lineRenderer.SetWidth( 0.01f, transform.localScale.x );
        }
        else if( gesture.Phase == ContinuousGesturePhase.Ended )
        {
            lineRenderer.enabled = false;
        }
    }

    #endregion
}
