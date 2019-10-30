using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PhysicsSimulationPool : Pool<PhysicsSimulatedObject> {
    public PhysicsSimulatedObject physicsObject;
    protected override void InstantiateObjects() {
        pool = new List<PhysicsSimulatedObject>( poolSize );
        poolQueue = new Queue<PhysicsSimulatedObject>();
        for ( int i = 0; i < poolSize; i++ ) {
            var pgo = Instantiate( physicsObject );
            pool.Add( pgo );
            poolQueue.Enqueue( pgo );
            pgo.Available = true;
            pgo.OnPoolReturnRequest = ReturnToQueue;
        }
    }

    public void InitPool() {
        InstantiateObjects();
    }
}
