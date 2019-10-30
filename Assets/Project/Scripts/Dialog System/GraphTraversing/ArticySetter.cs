using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Articy.Unity;
using LitJson;
using System;

public class ArticySetter : MonoBehaviour {
    public ArticyFlowPlayer player;

    private void Start() {

        //Obtaining this data from web service.
        Debug.Log( "Testing Serialization" );
        var states = new GameState[] {
            new GameState { Id = "GameState.foundBonny", Value = true },
            new GameState { Id = "GameState.id", Value = "Hola" },
            new GameState{ Id = "Gamestate.count", Value = 10 }
        };
        foreach ( var state in states ) {
            Debug.Log( state.ToString() );
        }
        var json = JsonMapper.ToJson( states );
        Debug.Log( $"Output json: {json}" );
        Debug.Log( "Deserializing json" );
        var jsonStates = JsonMapper.ToObject<GameState[]>( json );
        foreach ( var state in jsonStates ) {
            Debug.Log( state.ToString() );
        }

        player.globalVariables.SetVariableByGameState(states[0]);
    }



}
 
public class GameState {
    public string Id;
    public object Value;
	public DateTime CompletedDate;

    public override string ToString() {
        return $"id: {Id} value: {Value} type: {Value.GetType()}";
    }
}

public class GameGenericState<T> {
    public string id;
    public T value;
}

[System.Serializable]
public class GameBoolState : GameGenericState<bool> { }
[System.Serializable]
public class GameStringState : GameGenericState<string> { }
[System.Serializable]
public class GameIntState : GameGenericState<int> { }

