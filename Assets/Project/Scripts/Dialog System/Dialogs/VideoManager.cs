using System;
using System.Collections;
using System.Collections.Generic;
using Articy.Teleperformance_Test;
using Articy.Teleperformance_Test.Features;
using Articy.Unity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Analytics;

public class VideoManager : ArticySubManager {
    public override int Priority => 300;
    private VideoDataFeature _lastData = null;
    private string _videoName;
    private string _videoUrl {
        get {
            string url = "";
#if USE_GLOBALVARIABLES
            switch ( ArticyGlobalVariables.Default.Session.AvatarIndex ) {
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
            switch ( PlayerBodyManager.playerAssignedBody ) {

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

    [SerializeField, Required, HideInPrefabAssets]
    private UIVideoDialogueManager _uiVideoManager;

    private void OnVideoStart() {
        AnalyticsEvent.CutsceneStart( _videoName );
    }

    private void OnVideoEnd() {
        _flowPlayer.Play();
        ArticyHubStateCheck.RefreshHubState();
    }

    private void OnVideoUpdate( float obj ) {
    }

    public override void ManageFlowPlayer( ArticyFlowPlayer player ) {
        _uiVideoManager.showDialogue = false;
        var video = ArticyManager.Instance.LastFlowFragment as IObjectWithFeatureVideoData;
        _lastData = video.GetFeatureVideoData();
        _videoName = ArticyManager.Instance.LastFlowFragment.DisplayName;

        _uiVideoManager.OnVideoUpdate = OnVideoUpdate;
        _uiVideoManager.OnVideoEnd = OnVideoEnd;
        _uiVideoManager.OnVideoStart = OnVideoStart;

        _uiVideoManager.StartVideo( _videoUrl );
    }

    public override void Traverse( IFlowObject flowObject ) {
        AboutToRequestMain = flowObject is VideoFlowFragment;
    }

    public override void ViewNext( IList<Branch> branches ) {
    }
}
