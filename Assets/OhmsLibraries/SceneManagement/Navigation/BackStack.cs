using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OhmsLibraries.SceneManagement.Navigation {
    public class BackStack : MonoBehaviour {
        public delegate void BackStackAction();
        public static Stack<BackStackAction> backStack = new Stack<BackStackAction>();

        private void Start() {
    #if UNITY_IOS
            this.enabled = false;
    #endif
            if ( backStack != null ) {
                return;
            }
            backStack = new Stack<BackStackAction>();
        }

        private void Update() {
            if ( Input.GetKeyDown( KeyCode.Escape ) ) {
                if ( backStack.Count == 0 ) {
    #if UNITY_EDITOR
                    EditorApplication.isPaused = true;
    #else
                    Application.Quit();
    #endif
                    return;
                }
                backStack.Pop()();
            }
        }

        public static void Pop() {
    #if UNITY_EDITOR
            if ( backStack.Count == 0 ) {
                Debug.LogError( "Empty BackStack!" );
                return;
            }
            Debug.Log( "Pop: " + backStack.Peek().Method.Name );
    #endif
            backStack.Pop()();
        }

        public void PopAction() {
            Pop();
        }

        public static void Push( BackStackAction action ) {
    #if UNITY_EDITOR
            Debug.Log( "Pushing action: " + action.Method.Name );
    #endif
            backStack.Push( action );
    #if UNITY_EDITOR
            Debug.Log( "Stack Count: " + backStack.Count );
    #endif
        }

        public static void PopDelete( BackStackAction action ) {
            if ( backStack.Peek() == action ) {
                backStack.Pop();
            }
        }

        public void Clear() {
            backStack.Clear();
        }
    }
}