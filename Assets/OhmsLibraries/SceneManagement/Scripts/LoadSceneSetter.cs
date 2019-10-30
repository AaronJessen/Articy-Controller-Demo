using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace OhmsLibraries.SceneManagement {
    public class LoadSceneSetter : MonoBehaviour {
        [HideInInspector] public int sceneToLoad;
        [SerializeField] LoadSceneMode loadingMode = LoadSceneMode.Single;
        [SerializeField] SceneLoaderScriptable scriptableObject;
        [SerializeField] SceneDataScriptable sceneData;
        [SerializeField] Image show;
        [SerializeField] bool isDefault;

        void Start() {
            if ( isDefault )
                SetData();
        }

        public void SetData() {
            scriptableObject.data = sceneData;
            scriptableObject.loadingMode = loadingMode;
        }
    }
}
