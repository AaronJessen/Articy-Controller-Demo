using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace OhmsLibraries.SceneManagement {
    public class SceneLoader : MonoBehaviour {

        [SerializeField]
        SceneDataScriptable targetScene;

        private void Awake() {
            DontDestroyOnLoad( this );
        }

        private void OnDestroy() {
            if ( SceneLoaderScriptable.Loader == this ) {
                SceneLoaderScriptable.Loader = null;
            }
        }
    }
}
