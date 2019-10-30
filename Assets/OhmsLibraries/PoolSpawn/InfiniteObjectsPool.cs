using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class InfiniteObjectsPool<T> : ObjectsPool<T> where T : PoolMonoBehaviour {
    [MinValue( 1 )]
    public int extraCreate = 4;
    public override bool RequestPoolMonoBehaviour( out T PoolMonoBehaviour ) {
        var available = base.RequestPoolMonoBehaviour( out PoolMonoBehaviour );
        if ( available ) {
            return true;
        }
        ReinstantiateObjects();
        PoolMonoBehaviour = poolQueue.Dequeue();
        return PoolMonoBehaviour.Available;
    }

    protected void ReinstantiateObjects() {
        if ( evenlyCreate ) {
            for ( int i = 0; i < PoolMonoBehaviours.Length; i++ ) {
                for ( int j = 0; j < extraCreate; j++ ) {
                    var obj = InstantiatePoolObject( PoolMonoBehaviours[i] );
                    obj.Available = true;
                    obj.OnPoolReturnRequest = ReturnToQueue;
                    poolQueue.Enqueue( obj );
                    pool.Add( obj );
                }
            }
        }
        else {
            for ( int i = 0; i < extraCreate; i++ ) {
                var obj = InstantiatePoolObject( PoolMonoBehaviour );
                obj.Available = true;
                obj.OnPoolReturnRequest = ReturnToQueue;
                pool.Add( obj );
                poolQueue.Enqueue( pool[i] );
            }
        }
    }
}
