using UnityEngine;
using System.Collections;

[RequireComponent( typeof( EmitParticles ) )]
public class EmitParticlesOnSwipe : MonoBehaviour
{
    public bool constrained = false;
    EmitParticles emitter;

    void Awake()
    {
        emitter = GetComponent<EmitParticles>();
    }

    // message sent by SwipeRecognizer
    void OnSwipe( SwipeGesture gesture )
    {
        if( constrained )
        {
            switch( gesture.Direction )
            {
                case FingerGestures.SwipeDirection.Up:
                    emitter.EmitUp();
                    break;

                case FingerGestures.SwipeDirection.Right:
                    emitter.EmitRight();
                    break;

                case FingerGestures.SwipeDirection.Down:
                    emitter.EmitDown();
                    break;

                case FingerGestures.SwipeDirection.Left:
                    emitter.EmitLeft();
                    break;

                default:
                    return;
            }
        }
        else
        {
            Vector3 emitDir = new Vector3( gesture.Move.x, gesture.Move.y, 0 );
            emitter.Emit( emitDir ); 
        }
    }
}
