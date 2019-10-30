using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Articy.Unity;
using Articy.Teleperformance_Test.GlobalVariables;
using UnityEngine.Analytics;
using System;

public class GlobalSettingsSetter : ReadyBehaviour {
    [SerializeField, Required]
    private GlobalSettings _settings;
    [SerializeField, Required]
    private VolumeHandler _volumeHandler;
    [SerializeField, Required]
    private TMPSizeHandler _fontHandler;

    public static bool isVolumeDirty, isFontDirty;


    private void Awake() {
        var firebase = FindObjectOfType<FirebaseHandler>();
        firebase.RetrieveLoginData();
    }

    public void OnVolumeChanged( float volume ) {
        _settings.data.Volume = volume;
        isVolumeDirty = true;
    }

    public void OnTextSizeChanged( int index ) {
        _settings.data.FontSize = index;
        isFontDirty = true;
    }

    public void ApplySettings() {
        ArticyGlobalVariables.Default.Session.PlayerName = _settings.data.FirstName;
        ArticyGlobalVariables.Default.Session.AvatarIndex = _settings.data.AvatarID;

        Analytics.SetUserId( _settings.data.CompletePhone );
        Analytics.SetUserBirthYear( DateTime.Today.Year - _settings.data.Age );
        Analytics.SetUserGender( _settings.data.PlayerSex );

        PlayerBodyManager.playerAssignedBody = (PlayerBodyManager.PixBody) Mathf.Clamp(_settings.data.AvatarID - 1, 0, 6);
        //ArticyDatabase.Localization.Language = _settings.data.Language;
        _volumeHandler.SetVolume((float) _settings.data.Volume);
        _fontHandler.SetSize( _settings.data.FontSize );
        Ready = true;
    }
}
