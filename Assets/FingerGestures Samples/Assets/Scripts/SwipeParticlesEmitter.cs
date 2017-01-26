using UnityEngine;
using System.Collections;

public class SwipeParticlesEmitter : MonoBehaviour 
{
    public ParticleEmitter emitter;
    public float baseSpeed = 4.0f;
    public float swipeVelocityScale = 0.001f;

    void Start()
    {
        if( !emitter )
            emitter = GetComponent<ParticleEmitter>();

        emitter.emit = false;
    }

    public void Emit( Vector3 heading, float swipeVelocity )
    {
        // orient our emitter towards the swipe direction
        emitter.transform.rotation = Quaternion.LookRotation( heading );

        Vector3 localEmitVelocity = emitter.localVelocity;
        localEmitVelocity.z = baseSpeed * swipeVelocityScale * swipeVelocity;
        emitter.localVelocity = localEmitVelocity;

        // fire away!
        emitter.Emit();
    }

    public static Vector3 GetSwipeDirectionVector( FingerGestures.SwipeDirection direction )
    {
        switch( direction )
        {
            case FingerGestures.SwipeDirection.Up:
                return Vector3.up;
                
            case FingerGestures.SwipeDirection.UpperRightDiagonal:
                return 0.5f * ( Vector3.up + Vector3.right );
                
            case FingerGestures.SwipeDirection.Right:
                return Vector3.right;
                
            case FingerGestures.SwipeDirection.LowerRightDiagonal:
                return 0.5f * ( Vector3.down + Vector3.right );
                
            case FingerGestures.SwipeDirection.Down:
                return Vector3.down;
                
            case FingerGestures.SwipeDirection.LowerLeftDiagonal:
                return 0.5f * ( Vector3.down + Vector3.left );
                
            case FingerGestures.SwipeDirection.Left:
                return Vector3.left;
                
            case FingerGestures.SwipeDirection.UpperLeftDiagonal:
                return 0.5f * ( Vector3.up + Vector3.left );
        }

        Debug.LogError( "Unhandled swipe direction: " + direction );
        return Vector3.zero;
    }

    public void Emit( FingerGestures.SwipeDirection direction, float swipeVelocity )
    {
        Vector3 heading = GetSwipeDirectionVector( direction );
        Emit( heading, swipeVelocity );
    }
}
