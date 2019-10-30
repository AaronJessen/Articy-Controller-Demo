using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif

public class GameObjectGroup : MonoBehaviour {
    [SerializeField,
        ValidateInput( "Editor_ValidateArray", "Los elementos del grupo idealmente deberían tener el mismo status de activado.", InfoMessageType.Warning ),
        ListDrawerSettings( OnEndListElementGUI = "Editor_ArrayGuiEnd", OnBeginListElementGUI = "Editor_ArrayGuiBegin" )]
    private GameObject[] _group;

    [Button]
    public void DisableGroup() {
        SetActive( false );
    }

    [Button]
    public void EnableGroup() {
        SetActive( true );
    }

    public void SetActive( bool active ) {
        for ( int i = 0; i < _group.Length; i++ ) {
            _group[i].SetActive( active );
        }
    }

#if UNITY_EDITOR
    private bool Editor_ValidateArray( GameObject[] group ) {
        if ( group == null || group.Length == 0 ) {
            return true;
        }
        var first = group[0].activeSelf;
        for ( int i = 1; i < group.Length; i++ ) {
            if( group[i] == null ) {
                continue;
            }
            if ( first != group[i].activeSelf ) {
                return false;
            }
        }
        return true;
    }

    private void Editor_ArrayGuiBegin( int index ) {
        SirenixEditorGUI.BeginHorizontalToolbar();
    }

    private void Editor_ArrayGuiEnd( int index ) {
        if ( index > _group.Length || _group[index] == null ) {
            SirenixEditorGUI.EndHorizontalToolbar();
            return;
        }
        var active = UnityEditor.EditorGUILayout.Toggle( _group[index].activeSelf, GUILayout.Width( 30 ) );
        _group[index].SetActive( active );
        SirenixEditorGUI.EndHorizontalToolbar();
    }

    [Button]
    private void Editor_SelectAll() {
        UnityEditor.Selection.objects = _group;
    }
#endif
}
