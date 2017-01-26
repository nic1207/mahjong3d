using UnityEngine;
using System.Collections;

[RequireComponent( typeof( LineRenderer ) )]
public class PointCloudGestureRenderer : MonoBehaviour
{
    LineRenderer lineRenderer;
    public PointCloudGestureTemplate GestureTemplate;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
    }

    void Start()
    {
        if( GestureTemplate )
            Render( GestureTemplate );
    }

    public void Blink()
    {
        GetComponent<Animation>().Stop();
        GetComponent<Animation>().Play();
    }

    public bool Render( PointCloudGestureTemplate template )
    {
        if( template.PointCount < 2 )
            return false;

        lineRenderer.SetVertexCount( template.PointCount );

        for( int i = 0; i < template.PointCount; ++i )
            lineRenderer.SetPosition( i, template.GetPosition( i ) );

        return true;
    }
}
