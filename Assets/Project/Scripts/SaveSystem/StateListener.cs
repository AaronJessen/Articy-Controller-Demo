using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Articy.Unity;

public class StateListener : MonoBehaviour {
    public string[] states;

    private void Start() {
        var player = FindObjectOfType<ArticyFlowPlayer>();
        Debug.Log( $"#StateListener#Player is {player}" );
        foreach ( var state in states ) {
            ArticyDatabase.DefaultGlobalVariables.Notifications.AddListener( state, OnValueChanged );
            player?.globalVariables.Notifications.AddListener( state, OnValueChanged2 );
        }
    }

    private void OnValueChanged( string name, object value ) {
        Debug.Log( $"#StateListener#Variable: {name} changed to {value} of type {value.GetType()}" );
    }
    private void OnValueChanged2( string name, object value ) {
        Debug.Log( $"#StateListener2#Variable: {name} changed to {value} of type {value.GetType()}" );
    }
}
