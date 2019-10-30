using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif

public class SceneObjectDestroyer : MonoBehaviour {
    [SerializeField, ValueDropdown( "Editor_Scenes" )]
    private string _targetScene;

    [SerializeField]
    private bool _dontDestroy = false;

    private void Start() {
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        if ( _dontDestroy ) DontDestroyOnLoad( gameObject );
    }

    private void SceneManager_activeSceneChanged( Scene current, Scene next ) {
        if ( next.name.Equals( _targetScene ) ) {
            Destroy( gameObject );
        }
    }

    private void OnDestroy() {
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
    }

#if UNITY_EDITOR
    private string[] Editor_Scenes {
        get => ( from scene in EditorBuildSettings.scenes where scene.enabled select GetSceneName( scene ) ).ToArray();
    }

    private string GetSceneName( EditorBuildSettingsScene scene ) {
        string output;
        int index = scene.path.LastIndexOf( '/' );
        output = scene.path.Substring( index + 1 );
        int index2 = output.LastIndexOf( "." );
        output = output.Substring( 0, index2 );
        return output;
    }
#endif
}
