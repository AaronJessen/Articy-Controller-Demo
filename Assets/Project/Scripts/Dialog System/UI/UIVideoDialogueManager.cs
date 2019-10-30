using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Sirenix.OdinInspector;
using Doozy.Engine;

public class UIVideoDialogueManager : MonoBehaviour {
    [SerializeField, ValidateInput( "Validate_VideoPlayer" )]
    private VideoPlayer _player;

    [HideInInspector]
    public bool showDialogue;

    public System.Action OnVideoStart, OnVideoEnd;
    public System.Action<float> OnVideoUpdate;

    private void Awake() {
        //Debug.Log( $"#Video#Setting video callbacks." );
        _player.prepareCompleted += OnVideoReady;
        _player.loopPointReached += OnVideoEnded;
        _player.errorReceived += _player_errorReceived;
    }

    private void Start() {
        enabled = false;
    }

    public void StartVideo( string url ) {
        //Debug.Log( $"#Video#Preparing video with url {url}." );
        _player.url = url;
        
        _player.Prepare();
        
    }

    private void _player_errorReceived( VideoPlayer source, string message ) {
        OnVideoEnded( null );
    }

    private void OnVideoReady( VideoPlayer source ) {
        //Debug.Log("#Video#Video ready");
        GameEventMessage.SendEvent( showDialogue ? "VideoStart" : "VideoStart_NoDialogue" );
        OnVideoStart?.Invoke();
        source.Play();
        enabled = true;
    }

    private void OnVideoEnded( VideoPlayer source ) {
        //Debug.Log( "Ending video." );
        source.Stop();
        OnVideoEnd?.Invoke();
        enabled = false;
        GameEventMessage.SendEvent( "VideoEnd" );
    }

    private void Update() {
        //ConsoleProDebug.Watch( "#Video#Time", _player.time.ToString() );
        OnVideoUpdate( (float) _player.time );
    }

#if UNITY_EDITOR
    private bool Validate_VideoPlayer( VideoPlayer player ) {
        player.source = VideoSource.Url;
        player.playOnAwake = false;
        return true;
    }

    [Button]
    private void EndVideo() {
        _player.time = _player.length - 2f;
    }
#endif
}
