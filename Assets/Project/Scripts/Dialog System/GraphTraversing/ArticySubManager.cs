using Articy.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class that defines a handler for the Articy tree and can be setted as main manager for the graph.
/// </summary>
public abstract class ArticySubManager : MonoBehaviour {
    [SerializeField, Required/*, ValidateInput( "Validate_FlowPlayer", "ArticyFlowPlayer should be disabled by default." )*/]
    protected ArticyFlowPlayer _flowPlayer;

    //When requesting control, the submanager with the highest Priority value will get it.
    public abstract int Priority {
        get;
    }

    public static ArticySubManager Current;

    public bool AboutToRequestMain {
        get; protected set;
    }

    [ShowInInspector, HideInEditorMode, DisableInPlayMode]
    public bool IsMain {
        get; private set;
    }

    public virtual void SetAsPlayerController() {
        if ( IsMain ) {
            return;
        }
        IsMain = true;
        Current = this;
        //_flowPlayer.enabled = true;
    }

    public virtual void YieldPlayerController() {
        IsMain = false;
    }

    public abstract void Traverse( IFlowObject flowObject );
    public abstract void ViewNext( IList<Branch> branches );
    public abstract void ManageFlowPlayer( ArticyFlowPlayer player );

#if UNITY_EDITOR
    private bool Validate_FlowPlayer( ArticyFlowPlayer player ) {
        if ( !player || UnityEditor.EditorApplication.isPlaying )
            return true;
        return !player.enabled;
    }
#endif
}
