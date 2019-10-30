#define USE_GLOBALVARIABLES
using System.Collections;
using System.Collections.Generic;
using Articy.Teleperformance_Test;
using Articy.Teleperformance_Test.Features;
using Articy.Teleperformance_Test.GlobalVariables;
using Articy.Unity;
using LitJson;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Analytics;

public class VideoDialogueManager : DialogueManager {

    [SerializeField, Required, HideInPrefabAssets]
    private UIVideoDialogueManager _uiVideoManager;

    private VideoDataFeature _lastData = null;
    private int _currentTimeStamp;
    [ShowInInspector]
    private double[] _timeStamps;
    private string _videoName;

    private double CurrentTimeStamp {
        get => _timeStamps[_currentTimeStamp];
    }

    private string _videoUrl {
        get {
            string url = "";
#if USE_GLOBALVARIABLES
            switch (ArticyGlobalVariables.Default.Session.AvatarIndex) {
                case 1:
                    url = _lastData.VideoURL_1;
                    break;
                case 2:
                    url = _lastData.VideoURL_2;
                    break;
                case 3:
                    url = _lastData.VideoURL_3;
                    break;
                case 4:
                    url = _lastData.VideoURL_14;
                    break;
                case 5:
                    url = _lastData.VideoURL_5;
                    break;
                case 6:
                    url = _lastData.VideoURL_6;
                    break;
                default:
                    url = _lastData.VideoURL_7;
                    break;
            }
#else
            switch (PlayerBodyManager.playerAssignedBody) {

                case PlayerBodyManager.PixBody.PIX01:
                    url = _lastData.VideoURL_1;
                    break;
                case PlayerBodyManager.PixBody.PIX02:
                    url = _lastData.VideoURL_2;
                    break;
                case PlayerBodyManager.PixBody.PIX03:
                    url = _lastData.VideoURL_3;
                    break;
                case PlayerBodyManager.PixBody.PIX04:
                    url = _lastData.VideoURL_14;
                    break;
                case PlayerBodyManager.PixBody.PIX05:
                    url = _lastData.VideoURL_5;
                    break;
                case PlayerBodyManager.PixBody.PIX06:
                    url = _lastData.VideoURL_6;
                    break;
                default:
                    url = _lastData.VideoURL_7;
                    break;
            }
#endif
            return url;
        }
    }
    public override int Priority {
        get => 101;
    }

    private bool LastDialogueFragment {
        get => _currentTimeStamp >= _timeStamps.Length;
    }

    private void OnVideoStart () {
        AnalyticsEvent.CutsceneStart( _videoName );
    }

    private void OnVideoUpdate (float time) {
        if (LastDialogueFragment) {
            return;
        }
        if (CurrentTimeStamp < time) {
            Debug.Log ($"#Video#Passed the timestamp {CurrentTimeStamp}, index: {_currentTimeStamp}. Current time: {time}.");
            uiDialogueManager.Continue ();
            _currentTimeStamp++;
        }
    }

    private void OnVideoEnd () {
        Debug.Log ("onVideoEnd");
        _flowPlayer.Play();
        while ( !LastDialogueFragment ) {
            _flowPlayer.Play();
            _currentTimeStamp++;
        }
        ArticyHubStateCheck.RefreshHubState();
    }

    public override void ManageFlowPlayer (ArticyFlowPlayer player) {
        if (ArticyManager.Instance.Dialogue) {
            var video = ArticyManager.Instance.LastDialogue as IObjectWithFeatureVideoData;
            var dialogue = ArticyManager.Instance.LastDialogue;
            _lastData = video.GetFeatureVideoData ();
            _videoName = ArticyManager.Instance.LastDialogue.DisplayName;
            var jsonTimestamps = _lastData.Timestamps;
            try {
                _timeStamps = JsonMapper.ToObject<double[]> (jsonTimestamps);
            }
            catch (System.Exception e) {
                Debug.LogError ($"#Video#Invalid json format for { ArticyManager.Instance.LastDialogue.name} VideoDialogue " +
                    $"with Technical name: {ArticyManager.Instance.LastDialogue.TechnicalName}. Json: {jsonTimestamps}. Exception {e.Message}");
            }

            player.Play ();
#if UNITY_EDITOR
            if (editor_SkipDialogues) {
                for (int i = 0; i < _timeStamps.Length - 1; i++) {
                    player.Play();
                }
                OnVideoEnd();
                return;
            }
#endif
            _uiVideoManager.showDialogue = true;

            _uiVideoManager.OnVideoUpdate = OnVideoUpdate;
            _uiVideoManager.OnVideoEnd = OnVideoEnd;
            _uiVideoManager.OnVideoStart = OnVideoStart;

            _uiVideoManager.StartVideo (_videoUrl);
        }
        else {
#if UNITY_EDITOR
            if (editor_SkipDialogues) {
                return;
            }
#endif
            uiDialogueManager.StartDialogue (OnDialogueFragmentEnded, false, false);
            uiDialogueManager.ShowDialogueText (lastData);
            if (lastData.HasAudio) {
                audioHandler.PlayClip (lastData.clip);
            }
        }
    }

    private void OnDialogueFragmentEnded () {
        if (_currentTimeStamp < _timeStamps.Length) {
            _flowPlayer.Play ();
        }
    }

    public override void Traverse (IFlowObject flowObject) {
        AboutToRequestMain = false;
        if (flowObject is VideoDialogue) {
            AboutToRequestMain = true;
            _currentTimeStamp = 0;
            return;
        }

        if (flowObject is VideoDialogueFragment videoFragment) {
            AboutToRequestMain = true;
            OnDialogue (videoFragment as DialogueFragment);
        }
    }

    public override void ViewNext (IList<Branch> branches) {

    }
}