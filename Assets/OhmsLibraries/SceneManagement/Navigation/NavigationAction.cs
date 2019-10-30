using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OhmsLibraries.SceneManagement.Navigation {
    public abstract class NavigationAction : ScriptableObject {
        public abstract void Action();
    }
}