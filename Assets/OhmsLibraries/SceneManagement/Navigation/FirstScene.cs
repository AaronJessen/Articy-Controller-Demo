using System.Collections;
using System.Collections.Generic;
using OhmsLibraries.SceneManagement;
using UnityEngine;
namespace OhmsLibraries.SceneManagement.Navigation {
    public class FirstScene : MonoBehaviour {
        public SceneDataScriptable scene;

        private void Awake() {
            scene.sceneManager.data = scene;    
        }
    }
}