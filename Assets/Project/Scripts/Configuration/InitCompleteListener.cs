using Articy.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InitCompleteListener : MonoBehaviour {

    [Required]
    public ArticyFlowPlayer player;
    [Required, InlineEditor]
    public BaseStateManager manager;
    [Required]
    public TransitionPoint transition;

    public UnityEvent OnLoaded;

    [ShowInInspector, HideInEditorMode]
    private ReadyBehaviour[] _waitObjects;
    [ShowInInspector, HideInEditorMode]
    private int _completed = 0;
    private int RequiredToComplete {
        get => _waitObjects.Length;
    }
    private void Awake() {
        _waitObjects = GetComponents<ReadyBehaviour>();
        for ( int i = 0; i < _waitObjects.Length; i++ ) {
            _waitObjects[i].OnRegister( this );
        }
    }

    public void Step() {
        _completed++;
        if ( _completed == RequiredToComplete ) {
            RestoreComplete();
        }
    }

    private void RestoreComplete() {
        manager.StartListening( player );
        transition.Transition();
        OnLoaded.Invoke();
    }
}

public abstract class ReadyBehaviour : MonoBehaviour {
    private bool _ready;
    private System.Action _readyCallback;
    public bool Ready {
        get => _ready;
        protected set {
            _ready = value;
            if ( _ready ) {
                _readyCallback.Invoke();
            }
        }
    }
    public void OnRegister( InitCompleteListener completeManager ) {
        _readyCallback = completeManager.Step;
    }
}
