using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu( menuName = "GlobalSettings" )]
public class GlobalSettings : ScriptableObject {

    public RegistrationData data;
    [ListDrawerSettings( HideAddButton = true, HideRemoveButton = true ), MinValue( 0f ), ValidateInput( "Editor_ValidateSizeArray" ), SerializeField]
    private float[] _textSizes = new float[4];

    public float TextSize {
        get => _textSizes[data.FontSize];
    }

    public void SetVolume( float volume ) {
        data.Volume = volume;
    }

#if UNITY_EDITOR
    private bool Editor_ValidateSizeArray( float[] array ) {
        float previous = float.MinValue;
        for ( int i = 0; i < array.Length; i++ ) {
            if ( array[i] < previous ) {
                return false;
            }
        }
        return true;
    }

    public float[] Editor_TextSizes {
        get => _textSizes;
    }
#endif
}


