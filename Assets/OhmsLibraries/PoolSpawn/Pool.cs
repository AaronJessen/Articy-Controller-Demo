using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Text;

public abstract class Pool<T> : MonoBehaviour where T : PoolMonoBehaviour {
#if ODIN_INSPECTOR
    [ValidateInput( "ValidatePoolSize" )]
    public int poolSize = 1;
#else
    public int poolSize = 1;
#endif

    [ShowInInspector, HideInEditorMode, DisableInPlayMode]
    protected List<T> pool;
    protected Queue<T> poolQueue;


#if UNITY_EDITOR
    private int _objectCount = 0;
#if ODIN_INSPECTOR
    private bool ValidatePoolSize( int i ) {
        return i > 0;
    }
    #endif
#endif

    /// <summary>
    /// Generator which returns the unavailable objects in the array.
    /// </summary>
    /// <returns>The unavailable objects.</returns>
    public IEnumerable<T> GetUnavailableObjects() {
        for ( int i = 0; i < pool.Count; i++ ) {
            if ( !pool[i].Available )
                yield return pool[i];
        }
    }

    public void DespawnAll() {
        foreach ( var obj in GetUnavailableObjects() ) {
            obj.Despawn();
        }
    }

    /// <summary>
    /// Generator which returns the available objects in the array.
    /// </summary>
    /// <returns>The available objects.</returns>
    public IEnumerable<T> GetAvailableObjects() {
        for ( int i = 0; i < pool.Count; i++ ) {
            if ( pool[i].Available )
                yield return pool[i];
        }
    }

    public virtual bool RequestPoolMonoBehaviour( out T PoolMonoBehaviour ) {
        if ( poolQueue == null || poolQueue.Count == 0 ) {
            PoolMonoBehaviour = null;
            return false;
        }
        if ( poolQueue.Count == 0 ) {
            PoolMonoBehaviour = null;
            return false;
        }
        PoolMonoBehaviour = poolQueue.Dequeue();
        return PoolMonoBehaviour.Available;
    }

    protected abstract void InstantiateObjects();

    protected virtual T InstantiatePoolObject( T poolObject ) {
        var obj = Instantiate( poolObject );
        #if UNITY_EDITOR
        obj.name = string.Format( "{0}_{1}", obj.name, _objectCount );
        _objectCount++;
        #endif
        return obj;
    }

    /// <summary>
    /// Register an OnSpawn event on all the objects in the pool.
    /// </summary>
    /// <param name="OnSpawn">On spawn.</param>
    public void RegisterOnSpawn( System.Action<PoolMonoBehaviour> OnSpawn ) {
        for ( int i = 0; i < pool.Count; i++ ) {
            pool[i].OnSpawn += OnSpawn;
        }
    }

    /// <summary>
    /// Unregisters an OnSpawn event on all the objects in the pool.
    /// </summary>
    /// <param name="OnSpawn">On spawn.</param>
    public void UnregisterOnSpawn( System.Action<PoolMonoBehaviour> OnSpawn ) {
        for ( int i = 0; i < pool.Count; i++ ) {
            pool[i].OnSpawn -= OnSpawn;
        }
    }

    public void RegisterOnDespawn( System.Action<PoolMonoBehaviour> OnDespawn ) {
        Debug.Log( "Registrando evento" );
        for ( int i = 0; i < pool.Count; i++ ) {
            pool[i].OnDespawn += OnDespawn;
        }
    }

    public void UnregisterOnDespawn( System.Action<PoolMonoBehaviour> OnDespawn ) {
        for ( int i = 0; i < pool.Count; i++ ) {
            pool[i].OnDespawn -= OnDespawn;
        }
    }

    public void UnspawnAll() {
        foreach ( var obj in GetUnavailableObjects() ) {
            obj.Despawn();
        }
    }

    protected void ReturnToQueue( PoolMonoBehaviour o ) {
        poolQueue.Enqueue( o as T );
    }
}

public abstract class PoolMonoBehaviour : MonoBehaviour {
    public event System.Action<PoolMonoBehaviour> OnSpawn, OnDespawn;
    public System.Action<PoolMonoBehaviour> OnPoolReturnRequest;
    //Disponible para el pool
    public virtual bool Available {
        get {
            return !gameObject.activeSelf;
        }
        set {
            gameObject.SetActive( !value );
        }
    }

    public virtual void Spawn() {
        Available = false;
        CallOnSpawnEvent();
    }

    public virtual void Spawn( Vector3 position ) {
        transform.position = position;
        Spawn();
    }

    public virtual void Spawn( Vector3 position, Quaternion rotation ) {
        transform.rotation = rotation;
        Spawn( position );
    }

    public virtual void Despawn() {
        CallOnDespawnEvent();
        Available = true;
    }

    protected void CallOnDespawnEvent() {
        OnPoolReturnRequest.Invoke( this );
        OnDespawn?.Invoke( this );
    }

    protected void CallOnSpawnEvent() {
        OnSpawn?.Invoke( this );
    }
}
