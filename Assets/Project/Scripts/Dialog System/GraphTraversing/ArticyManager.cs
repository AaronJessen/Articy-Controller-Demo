using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Articy.Teleperformance_Test;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Class in charge of managing the graph flow of ArticyFlowPlayer and calling the different scripts
/// accordingly.
/// </summary>
public class ArticyManager : ArticySubManager, IArticyFlowPlayerCallbacks {

    public static ArticyManager Instance {
        get;
        private set;
    }

    [Required, SerializeField, HideInPrefabAssets]
    private GameObject _masterCanvas;

    //public static System.Action<IList<Branch>> OnMainManagerBranchesUpdated;
    //public static System.Action<IFlowObject> OnMainManagerFlowPaused;
#if UNITY_EDITOR
    [ShowInInspector, HideInPlayMode]
    private ArticySubManager[] editor_addedManagers {
        get => GetComponents<ArticySubManager>();
    }
    [SerializeField]
    private bool editor_CheckOnStart = false;

    private int editor_InfiniteLoopSafe = 0;

    private void LateUpdate() {
        editor_InfiniteLoopSafe = 0;
    }
#endif
    [ShowInInspector, HideInEditorMode]
    private ArticySubManager[] _subManagers;
    [ShowInInspector, HideInEditorMode, BoxGroup( "Status" )]
    private IEnumerable<Branch> currentBranches;

    private bool _continueTraverse;

    private System.Action FinishCallback;

    private IFlowObject _flowObject;
    [ShowInInspector, HideInEditorMode, BoxGroup( "Status" )]
    private IList<Branch> _branches;

    [ShowInInspector, HideInEditorMode, BoxGroup( "Status" )]
    public FlowFragment LastFlowFragment {
        get;
        private set;
    }
    [ShowInInspector, HideInEditorMode, BoxGroup( "Status" )]
    public Dialogue LastDialogue {
        get;
        private set;
    }
    [ShowInInspector, HideInEditorMode, BoxGroup( "Status" )]
    public DialogueFragment LastDialogueFragment {
        get;
        private set;
    }
    [ShowInInspector, HideInEditorMode, BoxGroup( "Status" )]
    public Hub LastHub {
        get;
        private set;
    }

#if UNITY_EDITOR
    [ShowInInspector, HideInEditorMode, DisableInPlayMode, BoxGroup( "Status" )]
    private string LastFlowFragmentName {
        get => LastFlowFragment?.DisplayName ?? "";
    }

    [ShowInInspector, HideInEditorMode, DisableInPlayMode, BoxGroup( "Status" )]
    private string LastDialogueName {
        get => LastDialogue?.DisplayName ?? "";
    }

    [ShowInInspector, HideInEditorMode, DisableInPlayMode, BoxGroup( "Status" )]
    private string LastDialogueFragmentName {
        get => LastDialogueFragment?.Speaker.name ?? "";
    }

    [ShowInInspector, HideInEditorMode, DisableInPlayMode, BoxGroup( "Status" )]
    private string LastHubName {
        get => LastHub?.DisplayName ?? "";
    }

    private IEnumerator Start() {
        yield return new WaitForEndOfFrame();
        if ( editor_CheckOnStart ) {
            _ready = true;
            OnFlowPlayerPaused( _flowPlayer.PausedOn );
            OnBranchesUpdated( _flowPlayer.AvailableBranches );
        }
    }
#endif
    [ShowInInspector, HideInEditorMode, BoxGroup( "Status" )]
    public bool Dialogue {
        get;
        private set;
    }
    [ShowInInspector, HideInEditorMode, BoxGroup( "Status" )]
    public bool CharacterHub {
        get;
        private set;
    }
    [ShowInInspector, HideInEditorMode, BoxGroup( "Status" )]
    public bool FlowFragment {
        get;
        private set;
    }

    [ShowInInspector, HideInEditorMode, BoxGroup( "Status" )]
    private bool _ignoreNextCharacterHub;

    public override int Priority {
        get => 0;
    }
    [ShowInInspector, HideInEditorMode]
    private bool _ready;

    public bool Ready {
        get => _ready;
        set {
            _ready = value;
            OnFlowPlayerPaused( _flowObject );
            OnBranchesUpdated( _branches );
        }
    }

    public void Goto( Hub articyObject ) { //You can only directly move into a Hub since these are Pause points.
        _ignoreNextCharacterHub = true;
        LastHub = articyObject;
        Dialogue = false;
        CharacterHub = true;
        FlowFragment = false;
        _flowPlayer.StartOn = articyObject;
    }

    [Button, HideInEditorMode]
    private void GoTo( string technicalName ) {
        var foundObject = ArticyDatabase.GetObject( technicalName );
        _ignoreNextCharacterHub = true;
        _flowPlayer.StartOn = foundObject;
    }

    protected void Awake() {
        if ( Instance != null ) {
            Debug.Log( $"{GetType()} Instance already in place, deleting {name}" );
            Destroy( gameObject );
            Destroy( _masterCanvas );
            return;
        }

        SetAsPlayerController(); //This is the main manager so this will be started inmediatly
        Instance = this;
        //_subManagers.Insert( 0, this );
        _subManagers = GetComponents<ArticySubManager>();
        AboutToRequestMain = true;
        DontDestroyOnLoad( gameObject );
        DontDestroyOnLoad( _masterCanvas );
    }

    public void OnBranchesUpdated( IList<Branch> aBranches ) {
        if ( !Ready ) {
            _branches = aBranches;
            return;
        }
        if ( aBranches == null ) {
            return;
        }
        Debug.Log( "#ArticyManager#Branches Updates" );
        Debug.Log( $"#ArticyManager#Branch count: {aBranches.Count}" );
        for ( int i = 0; i < _subManagers.Length; i++ ) {
            _subManagers[i].ViewNext( aBranches );
        }

        var mainCandidates = (from s in _subManagers where s.AboutToRequestMain orderby s.Priority select s).ToList();
        var main = mainCandidates.Last();

        Debug.Log( $"#Articy Manager#Main is {main.GetType()}" );

        if ( Current != main ) {
            Current.YieldPlayerController();
        }

        Debug.Log( $"#ArticyManager#Main is {main.GetType().ToString()}" );
        main.SetAsPlayerController();
        main.ManageFlowPlayer( _flowPlayer );
    }

    public void OnFlowPlayerPaused( IFlowObject aObject ) {
        _flowObject = aObject;

        if ( !Ready ) {
            return;
        }

        if ( aObject == null ) {
            return;
        }
        var name = aObject as IObjectWithDisplayName;
        Debug.Log( "#ArticyManager#OnFlowPlayerPaused" );
        Debug.Log( $"#ArticyManager#Object Name: {name?.DisplayName ?? ""} Object Type: {aObject.GetType().ToString()}" );
        for ( int i = 0; i < _subManagers.Length; i++ ) {
            _subManagers[i].Traverse( aObject );
        }
#if UNITY_EDITOR
        editor_InfiniteLoopSafe++;
        _ready = editor_InfiniteLoopSafe <= 20;
        if ( !_ready ) {
            Debug.Log( $"#Failsafe# The editor paused to prevent an infinite loop. ArticyManager is not ready, you can set the variable back on in the editor." );
            UnityEditor.EditorApplication.isPaused = true;
        }
#endif
    }

    public override void Traverse( IFlowObject flowObject ) {
        Dialogue = false;
        CharacterHub = false;
        FlowFragment = false;
        if ( flowObject is FlowFragment flow ) {
            LastFlowFragment = flow;
            FlowFragment = true;
            return;
        }

        if ( flowObject is Hub hub ) {
            if ( LastHub == null ) {
                LastHub = hub;
                ArticyHubStateCheck.OnUpdateHub?.Invoke();
            }
            LastHub = hub;
            CharacterHub = true;
            return;
        }

        if ( flowObject is Dialogue dialogue ) {
            LastDialogue = dialogue;
            Dialogue = true;
            return;
        }

        if ( flowObject is DialogueFragment dialogueFragment ) {
            LastDialogueFragment = dialogueFragment;
            return;
        }

    }

    public override void ViewNext( IList<Branch> branches ) {
        currentBranches = branches;
    }

    public void PlayCharacter( Character character, System.Action endCallback = null ) {
        if ( !CharacterHub ) {
            throw new System.Exception( $"Not currently in a HUB! Last was type of {_flowObject.GetType()}." );
        }
        FinishCallback = endCallback;
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        foreach ( var branch in currentBranches ) {
            if ( branch.Target is IObjectWithFeatureCharacterDialog featureDialogue ) {
                var characterDialogue = featureDialogue.GetFeatureCharacterDialog();
                var branchCharacter = characterDialogue.Character;
                builder.AppendFormat( "{0}, ", (branchCharacter as Character).DisplayName );
                if ( branchCharacter.Equals( character ) ) {
                    _flowPlayer.Play( branch );
                    return;
                }
            }
        }
        throw new System.Exception( $"InvalidCall: The character {character.DisplayName} wasn't found in the current node. Current:{builder.ToString()}" );
    }
    public override void ManageFlowPlayer( ArticyFlowPlayer player ) {
        if ( CharacterHub ) {
            if ( !_ignoreNextCharacterHub ) {
                FinishCallback?.Invoke();
            }
            _ignoreNextCharacterHub = false; 
            return;
        }
        player.Play();
    }

    public void SetReady( bool ready ) {
        Ready = ready;
    }
}