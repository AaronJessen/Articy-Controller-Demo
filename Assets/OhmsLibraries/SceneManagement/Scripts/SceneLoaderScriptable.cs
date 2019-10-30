using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace OhmsLibraries.SceneManagement {
    public delegate void SceneDelegate();

    public delegate void SceneSetter();

    [CreateAssetMenu( menuName = "Scene Management/Scene Manager" )]
    public class SceneLoaderScriptable : ScriptableObject {
        public int rootScene = 0;
        public LoadSceneMode loadingMode = LoadSceneMode.Single;
        AsyncOperation loader;
        public static event SceneDelegate SceneLoadingFinish;
        public static event SceneDelegate StartSceneLoading;
        public static event System.Action<float> LoadPercentage;
        public SceneDataScriptable data;
        public Bundle currentBundle;

        private static SceneLoader _loader;

        public static SceneLoader Loader {
            get {
                if ( _loader == null ) {
                    _loader = CreateObject();
                }

                return _loader;
            }
            set { _loader = value; }
        }

        private static SceneLoader CreateObject() {
            GameObject obj = new GameObject( "SceneLoader" );
            return obj.AddComponent<SceneLoader>();
        }


        public void StartLoadAsync( string[] ids, SceneDelegate sceneCallback = null ) {
            var required = ids.Length;
            var current = 0;
            foreach ( var id in ids ) {
                Loader.StartCoroutine( LoadAsync( id, () => {
                    current++;
                    if ( required == current )
                        sceneCallback?.Invoke();
                }
                ) );
            }
        }

        private IEnumerator LoadAsync( string id, SceneDelegate sceneCallback = null ) {
            loader = SceneManager.LoadSceneAsync( id, LoadSceneMode.Single );
            yield return loader;
            sceneCallback?.Invoke();
        }

        public void StartLoading() {
            if ( StartSceneLoading != null ) {
                StartSceneLoading();
            }
        }

        public void LoadScene( SceneDataScriptable scene, LoadSceneMode mode ) {
            foreach ( var sceneId in scene.sceneIds ) {
                SceneManager.LoadScene( sceneId, mode );
            }
            if ( mode == LoadSceneMode.Additive ) {
                SceneManager.SetActiveScene( scene.Scene );
            }

            data = scene;
        }

        public void LoadWithSceneLoader( SceneDataScriptable scene ) {
            data = scene;
            SceneManager.sceneLoaded += LoadSceneAfterLoader;
            SceneManager.LoadScene( scene.loadingSceneId, LoadSceneMode.Single );
        }

        private void LoadSceneAfterLoader( Scene s, LoadSceneMode m ) {
            SceneManager.sceneLoaded -= LoadSceneAfterLoader;
            Loader.StartCoroutine( WaitForScene() );
        }

        private IEnumerator WaitForScene() {
            if ( data.simulatedWaitTime > Time.fixedDeltaTime ) {
                float t = 0;
                float invertedSimulatedTime = 1 / data.simulatedWaitTime;
                Debug.Log( "Waiting simulated time" );
                while ( t < data.simulatedWaitTime ) {
                    yield return new WaitForEndOfFrame();
                    t += Time.deltaTime;
                    if ( LoadPercentage != null ) LoadPercentage.Invoke( t * invertedSimulatedTime * 0.9f );
                }
            }
            else {
                yield return new WaitForFixedUpdate();
            }

            StartLoadAsync( data.sceneIds /*, ()=>UnloadScene(data.loadingSceneId)*/ );
        }

        public void UnloadScene( string target ) {
            SceneManager.UnloadSceneAsync( target );
        }

        public void QuitApplication() {
            Application.Quit();
        }
    }

    public abstract class Bundle : ScriptableObject { }
}