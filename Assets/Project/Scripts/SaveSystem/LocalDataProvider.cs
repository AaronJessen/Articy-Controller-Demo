using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using LitJson;

public class LocalDataProvider : MonoBehaviour {

    public bool alreadyLogin = true;

    [InlineEditor]
    public LocalStateManager localManager;
    public static LocalDataProvider Instance;
    public bool FormFilled = true;
    public RegistrationData registrationData;
    [MinMaxSlider(0.1f, 1f)]
    public Vector2 _timeRange = new Vector2( 0.2f, 0.5f );

    public float Time {
        get => Random.Range( _timeRange.x, _timeRange.y );
    }
    private void Awake() {
        Instance = this;
    }

    public string RegistrationJson {
        get => JsonMapper.ToJson( registrationData );
    }
}
