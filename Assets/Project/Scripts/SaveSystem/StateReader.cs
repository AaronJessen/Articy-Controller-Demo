using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Articy.Unity;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using Articy.Teleperformance_Test.GlobalVariables;

public class StateReader : ReadyBehaviour {

    [Required]
    public ArticyFlowPlayer player;
    [Required]
    public MapUIManager _mapManager;
    [Required, InlineEditor]
    public BaseStateManager manager;
    [Required]
    public TransitionPoint transition;

    public int currentState;

    private const string REGEX = @"\D+(?<mission>\d+).(?<objective>\d+)";

    private void Awake() {
        manager.OnReady += StartReading;
        DontDestroyOnLoad( gameObject );
    }

    private void Start() {
		Debug.Log($"Starting manager {manager.name}");
        manager.Init();
        StartCoroutine( manager.ReceiveStates() );
    }

    private void OnDestroy() {
        manager.RemoveListeners( player );
    }

    private void StartReading( List<GlobalVariableState> states, List<Item> items ) {
        Debug.Log( "#StateRestore#Reading variables" );
        Queue<GlobalVariableState> stateQueues = new Queue<GlobalVariableState>( states );
        GlobalVariableState state = null;

        while ( stateQueues.Count != 0 ) {
            state = stateQueues.Dequeue();
            Debug.Log( $"#StateRestore#Dequed state {state.CompleteId}" );
            
            AddState( state );
        }

        foreach ( var item in items ) {
            AddState( item );
        }

        ApplyState( state );
        Ready = true;
    }

    private void AddState( ArticyVariable state ) {
        Debug.Log( $"#StateRestore#Adding state {state.CompleteId}" );
        player.globalVariables.SetVariableByString( state.CompleteId, true );
    }

    private void ApplyState( GlobalVariableState state ) {

        if ( state != null ) {
            if ( ArticyDatabase.IsObjectAvailable( state.NodeTechnicalName ) ) {
                Debug.Log( $"#StateRestore#Applying {state.NodeTechnicalName}" );
                player.StartOn = ArticyDatabase.GetObject( state.NodeTechnicalName );
            }
#if UNITY_EDITOR
            else {
                Debug.LogError( $"Technical name:{state.NodeTechnicalName} for state {state.Id} is not available in the database. Maybe it doesn't exist." );
                UnityEditor.EditorApplication.isPaused = true;
            }
#endif
            transition.newSceneName = state.SceneId;
            transition.transitionDestinationTag = state.SceneTag;
            ArticyGlobalVariables.Default.Session.TargetLocation = state.targetLocation;
            GetCurrentMissionAndObjective( state.Id, out int mission, out int objective );
            manager.LastMission = mission;
            manager.LastObjective = objective;
        }
    }

    public static void GetCurrentMissionAndObjective(string name, out int mission, out int objective ) {
        Regex missionRegex = new Regex( REGEX, RegexOptions.IgnoreCase );
        var match = missionRegex.Match( name );

#if UNITY_EDITOR
        if ( !match.Success ) {
            Debug.LogError( $"{name} has an incorrect format in its name." );
            mission = objective = -1;
            return;
        }
#endif
        mission = int.Parse( match.Groups["mission"].Value );
        objective = int.Parse( match.Groups["objective"].Value );
    }
    

#if UNITY_EDITOR
    [Button( Style = ButtonStyle.Box )]
    private void Editor_TestRegex( string test ) {
        Regex missionRegex = new Regex( REGEX, RegexOptions.IgnoreCase );
        var match = missionRegex.Match( test );
        if ( !match.Success ) {
            Debug.LogError( $"{test} has an incorrect format in its name." );
        }
        else {
            var mission = int.Parse( match.Groups["mission"].Value );
            var objective = int.Parse( match.Groups["objective"].Value );
            Debug.Log( $"Succes matching for {test}. Mission {mission} Objective {objective}" );
        }
    }


#endif
}
