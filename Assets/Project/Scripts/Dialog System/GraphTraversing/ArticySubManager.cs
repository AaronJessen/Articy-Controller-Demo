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

    //TODO: Is this really necessary? In most cases there will be only one manager requesting control.
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

    /// <summary>
    /// Yields control of the articy player. All ending login must go here.
    /// </summary>
    public virtual void YieldPlayerController() {
        IsMain = false;
    }

    /// <summary>
    /// Receives the current node and decides if this manager should request
    /// to control the articy player.
    /// </summary>
    /// <param name="flowObject">The current node in the graph</param>
    public abstract void Traverse( IFlowObject flowObject );
    /// <summary>
    /// Receives the following branches and decides if this manager should
    /// request to contorl de articy player.
    /// </summary>
    /// <param name="branches"></param>
    public abstract void ViewNext( IList<Branch> branches );
    /// <summary>
    /// Called when this manager takes control of the flow player. All intialization login
    /// should go here.
    /// </summary>
    /// <param name="player"></param>
    public abstract void ManageFlowPlayer( ArticyFlowPlayer player );

#if UNITY_EDITOR
    private bool Validate_FlowPlayer( ArticyFlowPlayer player ) {
        if ( !player || UnityEditor.EditorApplication.isPlaying )
            return true;
        return !player.enabled;
    }
#endif
}
