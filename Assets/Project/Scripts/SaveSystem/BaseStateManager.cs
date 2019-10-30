using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Articy.Unity;
using Articy.Teleperformance_Test;
using UnityEngine.Analytics;
using System.Linq;

//Se va llamar desde una escena de inicialización.
public abstract class BaseStateManager : ScriptableObject {

    private const string GAME_STATE_LISTENER = "GameState.*";

    private const string ITEM_LISTENER = "Items.*";

    public System.Action<List<GlobalVariableState>, List<Item>> OnReady;
    [ShowInInspector, HideInEditorMode]
    protected List<GlobalVariableState> receivedStates;
    [ShowInInspector, HideInEditorMode]
    protected List<Item> receivedItems;

    [ShowInInspector, HideInEditorMode, DisableInPlayMode]
    protected GlobalVariableState lastState;
    [ShowInInspector, HideInEditorMode, DisableInPlayMode]
    protected Item lastItem;

    [SerializeField]
    private bool _ignoreFirstState = false;
    private bool _stateIgnored = false;

    private bool _ready = false;
    private float _timeSinceLastSave;

    private int _missionAttempts;

    public int LastMission {
        get => _lastMission;
        set {
            Debug.Log( $"#Analytics# Last Mission val:{value} last: {_lastMission}" );
            if ( value != _lastMission && _lastMission != -1 ) {
                OnMissionChange( _lastMission, value );
            }
            _lastMission = value;
        }
    }

    public int LastObjective {
        get => _lastObjective; set => _lastObjective = value;
    }

    private int _lastMission = -1;
    private int _lastObjective = -1;

    protected bool Ready {
        get => _ready;
        set {
            if ( value ) {
                Debug.Log( $"#StateRestore#Ready {value}" );
                OnReady?.Invoke( receivedStates, receivedItems );
            }
            _ready = value;
        }
    }

    public virtual void Init() {
    }

    public abstract IEnumerator ReceiveStates();

    public virtual void StartListening( ArticyFlowPlayer player ) {
        if ( _ignoreFirstState && !_stateIgnored ) {
            return;
        }

        _timeSinceLastSave = Time.time;
        player.globalVariables.Notifications.AddListener( GAME_STATE_LISTENER, OnGameStateVariableChanged );
        player.globalVariables.Notifications.AddListener( ITEM_LISTENER, OnItemVariableChanged );
        Debug.Log( $"{name} started listening to {GAME_STATE_LISTENER} and {ITEM_LISTENER}" );


    }

    public virtual void RemoveListeners( ArticyFlowPlayer player ) {
        player.globalVariables.Notifications.RemoveListener( GAME_STATE_LISTENER, OnGameStateVariableChanged );
        player.globalVariables.Notifications.RemoveListener( ITEM_LISTENER, OnItemVariableChanged );
        lastItem = null;
        lastState = null;
    }

    public virtual void OnGameStateVariableChanged( string aVariableName, object value ) {
        Debug.Log( $"State {aVariableName} changed value to {value} on {name}" );

        var lastFlowFragment = ArticyManager.Instance.LastFlowFragment;
        var saveData = lastFlowFragment as IObjectWithFeatureSaveSceneData;
        bool state = false;
        state = (bool)value;

        if ( saveData == null && !state ) {
            lastState = null;
            return;
        }

        lastState = new GlobalVariableState() {
            SceneId = saveData.GetFeatureSaveSceneData().Scene.ToString(),
            SceneTag = (SceneTransitionDestination.DestinationTag)System.Enum.Parse( typeof( SceneTransitionDestination.DestinationTag ), saveData.GetFeatureSaveSceneData().DestinationTag.ToString() ),
            NodeTechnicalName = lastFlowFragment.TechnicalName,
            CompletedTime = Time.time - _timeSinceLastSave,
            Id = aVariableName.Replace( "GameState.", "" ),
            targetLocation = saveData.GetFeatureSaveSceneData().TargetLocation.ToString(),
            MissionAttempts = _missionAttempts
        };

        _timeSinceLastSave = Time.time;

        StateReader.GetCurrentMissionAndObjective( lastState.Id, out int mission, out int objective );
        LastMission = lastState.MissionIndex = mission;
        LastObjective = objective;

    }

    public virtual void OnItemVariableChanged( string aVariableName, object value ) {
        Debug.Log( $"Item {aVariableName} changed value to {value} on {name}" );

        bool state = false;
        state = (bool)value;

        if ( !state ) {
            lastItem = null;
            return;
        }

        lastItem = new Item() {
            Id = aVariableName.Replace( "Items.", "" )
        };
    }

    protected virtual void OnMissionChange( int previous, int current ) {
        var previousName = $"Mission_{previous}";
        var currentName = $"Mission_{current}";
        var time = GetMissionTime( previous );
        var attempts = _missionAttempts;

        var args = new Dictionary<string, object> {
            ["Index"] = previousName,
            ["Time"] = time,
            ["Attempts"] = attempts
        };

        //Analytics.CustomEvent( "MissionComplete", args );
        var currentResult = AnalyticsEvent.LevelStart( currentName );
        Debug.Log( $"#Analytics# OnMissionChange. Current: {currentName}, result: {currentResult}" );

        if ( previous <= 0 )
            return;

        var previousResult = AnalyticsEvent.LevelComplete( previousName, args );
        _missionAttempts = 0;
        Debug.Log( $"#Analytics# OnMissionChange. Previous: {previousName}, result: {previousResult}" );
    }

    private double GetMissionTime( int mission ) {
        return ( from state in receivedStates where state.MissionIndex == mission select state.CompletedTime ).Sum();
    }

    public void MissionFailed() {
        _missionAttempts++;
    }
}
