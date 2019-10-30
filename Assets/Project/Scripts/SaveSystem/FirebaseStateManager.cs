using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu( menuName = "State Management/Firebase State Manager" )]
public class FirebaseStateManager : BaseStateManager {
    private FirebaseHandler _handler;

    public override void Init() {
        if ( _handler != null ) {
            Destroy( _handler.gameObject );
            //We take the newest handler in case there are other Monobehaviours referencing other objects in the scene.
        }
        _handler = FindObjectOfType<FirebaseHandler>();
        DontDestroyOnLoad( _handler.gameObject );
    }

    public override IEnumerator ReceiveStates() {
        Debug.Log( "#Firebase#Requesting game states" );
        _handler.RequestGameStates();
        yield return new WaitUntil( FirebaseWrapper.CheckOperationStatus( FirebaseWrapper.OperationGameState ) );
        yield return new WaitForEndOfFrame();
        Debug.Log( "#Firebase#Received game states" );
        receivedStates = _handler.LastGameStates;

        Debug.Log( "#Firebase#Requesting Items" );
        _handler.RequestItems();
        yield return new WaitUntil( FirebaseWrapper.CheckOperationStatus( FirebaseWrapper.OperationItems ) );
        yield return new WaitForEndOfFrame();
        Debug.Log( "#Firebase#Received Items" );
        receivedItems = _handler.LastItems;

        Ready = true;
    }

    public override void OnGameStateVariableChanged( string aVariableName, object value ) {
        base.OnGameStateVariableChanged( aVariableName, value );
        if ( lastState == null )
            return;
        _handler.SaveGameState( lastState );
    }

    public override void OnItemVariableChanged( string aVariableName, object value ) {
        base.OnItemVariableChanged( aVariableName, value );
        if ( lastItem == null )
            return;
        _handler.SaveItem( lastItem );
    }
}
