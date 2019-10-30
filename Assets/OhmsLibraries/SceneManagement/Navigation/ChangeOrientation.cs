using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OhmsLibraries.SceneManagement.Navigation {
    public class ChangeOrientation : MonoBehaviour {
        private ScreenOrientation original;
        public ScreenOrientation target;
        public bool onDestroyReestablish = true;
        // Use this for initialization
        void Start () {
            original = Screen.orientation;
            Screen.orientation = target;
        }

        private void OnDestroy() {
            if(onDestroyReestablish)
                Screen.orientation = original;
        }
    }
}