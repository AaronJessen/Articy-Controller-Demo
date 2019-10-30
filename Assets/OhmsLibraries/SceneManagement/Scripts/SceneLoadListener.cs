using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
namespace OhmsLibraries.SceneManagement {
    public class SceneLoadListener : MonoBehaviour {
        public string percentageFormat = "%{0}";
        public float multiplier = 100f;

        public UnityFloatEvent OnPercentage;
        public UnityStringEvent OnPercentageFormat;

        private void Awake() {
            SceneLoaderScriptable.LoadPercentage += SceneLoaderScriptableOnLoadPercentage;
        }

        private void SceneLoaderScriptableOnLoadPercentage( float obj ) {
            OnPercentage.Invoke( obj * multiplier );
            OnPercentageFormat.Invoke( string.Format( percentageFormat, obj * multiplier ) );
        }

        private void OnDestroy() {
            SceneLoaderScriptable.LoadPercentage -= SceneLoaderScriptableOnLoadPercentage;
        }
    }
}