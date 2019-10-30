using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class TMPSetterUtil : MonoBehaviour {
    public TextMeshProUGUI textMesh;

    [ValidateInputAttribute( "ValidateFormat", defaultMessage: "Posiblemente haga falta el formato",
        messageType: InfoMessageType.Warning )]
    public string format = "{0}";

    private void Reset() {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private bool ValidateFormat( string f ) {
        return f.Contains( "{0" );
    }

    public void SetFloat( float t ) {
        textMesh.text = string.Format( format, t );
    }

    public void SetInt( int t ) {
        textMesh.text = string.Format( format, t );
    }
}