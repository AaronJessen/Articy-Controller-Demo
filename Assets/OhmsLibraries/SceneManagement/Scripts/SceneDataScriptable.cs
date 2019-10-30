using System;
using UnityEngine;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using OhmsLibraries.SceneManagement.Navigation;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace OhmsLibraries.SceneManagement {
    [CreateAssetMenu( menuName = "Scene Management/Scene Data" )]
    public class SceneDataScriptable : NavigationAction {
        public bool loadingScreen;
        [ShowIf( "loadingScreen" )]
        public float simulatedWaitTime = 1f;
        [OnValueChanged( "LoadSceneModeChanged" )]
        public bool addToStack;
        [OnValueChanged( "LoadSceneModeChanged" ), HideIf( "loadingScreen" )]
        public LoadSceneMode mode;
        public string levelName;
        [ValueDropdown( "e_currentScenes" ), ValidateInput( "ValidateSceneId", "Scene is not in build settings and will cause an exception at runtime if used." ), Required]
        public string[] sceneIds;
        [ValueDropdown( "e_currentScenes" ), ValidateInput( "ValidateSceneId", "Scene is not in build settings and will cause an exception at runtime if used." ),
         ShowIf( "loadingScreen" )]
        public string loadingSceneId;
#if UNITY_EDITOR

        private void LoadSceneModeChanged( LoadSceneMode m ) {
            addToStack &= m != LoadSceneMode.Additive;
        }

        private void LoadSceneModeChanged( bool m ) {
            if ( mode == LoadSceneMode.Additive ) {
                addToStack = false;
                Debug.LogErrorFormat( "{0} cannot have addToStack set to true while LoadSceneMode is addtive", name );
            }
        }

        private string[] e_currentScenes {
            get {
                return (from scene in EditorBuildSettings.scenes where scene.enabled select GetSceneName( scene )).ToArray();
            }
        }

        private bool ValidateSceneId( string[] ids ) {
            foreach ( var id in ids ) {
                if ( !e_currentScenes.Contains( id ) ) return false;
            }
            return true;
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
        [Required]
        public SceneLoaderScriptable sceneManager;

        public Scene Scene {
            get {
                return SceneManager.GetSceneByName( sceneIds[0] );
            }
        }

        public override void Action() {
            LoadScene();
        }

#if UNITY_EDITOR
        private void Reset() {
            sceneManager = Resources.FindObjectsOfTypeAll<SceneLoaderScriptable>().FirstOrDefault();
        }
#endif

        public void LoadScene() {
            if ( sceneManager.data.addToStack ) {
                BackStack.Push( sceneManager.data.LoadScene );
            }

            if ( loadingScreen ) {
                sceneManager.LoadWithSceneLoader( this );
            }
            else {
                sceneManager.LoadScene( this, mode );
            }
        }

        public void LoadScene( bool overrideAddToStack ) {
            if ( overrideAddToStack ) {
                BackStack.Push( sceneManager.data.LoadScene );
            }
            sceneManager.LoadScene( this, mode );
        }

        public void SetAsActiveScene() {

        }
    }
}