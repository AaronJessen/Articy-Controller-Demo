using Articy.Teleperformance_Test;
using Articy.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class ArticyGeneralReference<T> : MonoBehaviour where T : ArticyObject {
    //Esto se debe implementar en las clases hijas.
    //[ArticyTypeConstraint( typeof( T ) )]
    //public ArticyRef reference;

    public bool IsValid {
        get; private set;
    }
    [HideInInspector]
    public T articyObject;

    protected abstract ArticyRef BaseReference {
        get;
    }

    protected virtual void Awake() {
        if ( !BaseReference.HasReference ) {
            Debug.LogError( $"Object {name} with script {GetType().ToString()} doesn't have a valid reference", gameObject );
            return;
        }
        articyObject = BaseReference.GetObject<T>();
        IsValid = articyObject != null;
    }

#if UNITY_EDITOR
    private bool HasReference {
        get => BaseReference?.HasReference ?? false;
    }
    [ShowInInspector, ShowIf( "HasReference" )]
    private ArticyRefModes Editor_RefMode {
        get => BaseReference.referencingMode;
    }
    [ShowInInspector, ShowIf( "HasReference" )]
    private uint Editor_RefInstanceId {
        get => BaseReference.instanceId;
    }
#endif
}
