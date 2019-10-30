using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Articy.Teleperformance_Test.GlobalVariables;
using Articy.Teleperformance_Test;
using Articy.Unity.Interfaces;
using Articy.Unity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Analytics;

public class DialogueManager : ArticySubManager {
    [Required, SerializeField, HideInPrefabAssets]
    protected UIDialogueManager uiDialogueManager;
    [Required, SerializeField]
    protected AudioHandler audioHandler;

    [ShowInInspector, HideInEditorMode, DisableInPlayMode]
    protected DialogueData lastData;
    [SerializeField]
    private bool _alwaysShowButton;

    [SerializeField, Required]
    private GlobalSettings _settings;

#if UNITY_EDITOR
     [SerializeField]
     protected bool editor_SkipDialogues = false;
#endif

    public override int Priority {
        get => 100;
    }

    protected virtual void OnDestroy() {
        if ( uiDialogueManager )
            Destroy( uiDialogueManager.transform.root.gameObject );
    }

    protected void OnDialogue( DialogueFragment dialogueFragment ) {
        lastData = new DialogueData();

        var speaker = dialogueFragment.Speaker;
        if ( speaker is IObjectWithFeatureCharacterPreviewImage characterImage ) {
            var feature = characterImage.GetFeatureCharacterPreviewImage();
            switch ( ArticyGlobalVariables.Default.Session.AvatarIndex ) {
                case 1:
                    lastData.sprite = (feature.Image1 as Asset).LoadAssetAsSprite();
                    break;
                case 2:
                    lastData.sprite = (feature.Image2 as Asset).LoadAssetAsSprite();
                    break;
                case 3:
                    lastData.sprite = (feature.Image3 as Asset).LoadAssetAsSprite();
                    break;
                case 4:
                    lastData.sprite = (feature.Image4 as Asset).LoadAssetAsSprite();
                    break;
                case 5:
                    lastData.sprite = (feature.Image5 as Asset).LoadAssetAsSprite();
                    break;
                case 6:
                    lastData.sprite = (feature.Image6 as Asset).LoadAssetAsSprite();
                    break;
                default:
                    lastData.sprite = (feature.Image7 as Asset).LoadAssetAsSprite();
                    break;

            }
        }
        else if ( speaker is IObjectWithPreviewImage speakerImage ) {
            var asset = speakerImage.PreviewImage.Asset;
            if ( asset != null ) {
                lastData.sprite = asset.LoadAssetAsSprite();
            }
        }

        if ( speaker is IObjectWithDisplayName speakerName ) {
            lastData.name = speakerName.DisplayName;
        }

        if ( dialogueFragment is IObjectWithFeatureImageDescription imageDescription ) {
            var articyObject = imageDescription.GetFeatureImageDescription().Image;
            //Debug.Log( $"Type: {articyObject.GetType()} {articyObject.GetArticyType()}" );
            lastData.descriptionSprite = (articyObject as Asset).LoadAssetAsSprite();
        }

        if ( dialogueFragment is IObjectWithFeatureAudioSource audio ) {
            lastData.clip = (audio.GetFeatureAudioSource()?.AudioClip as Asset)?.LoadAsset<AudioClip>();
        }
        else if ( dialogueFragment is IObjectWithFeatureDoubleAudioSource playerAudio ) {
            var doubleAudioSource = playerAudio.GetFeatureDoubleAudioSource();
            try {
                lastData.clip = ((_settings.data.PlayerSex == Gender.Female ? doubleAudioSource.FemaleClip : doubleAudioSource.MaleClip) as Asset).LoadAsset<AudioClip>();
            }
            catch {
                Debug.Log( "Null reference dialogue" );
            }
        }

        lastData.text = dialogueFragment.Text;
    }

    public override void ManageFlowPlayer( ArticyFlowPlayer player ) {
        if ( ArticyManager.Instance.Dialogue ) {
            player.Play();
        }
        else {
            uiDialogueManager.StartDialogue( player.Play, !lastData.HasAudio || _alwaysShowButton );
            uiDialogueManager.ShowDialogueText( lastData );
            if ( lastData.HasAudio ) {
                audioHandler.PlayClip( lastData.clip, () => uiDialogueManager.ShowNextButton( true ) );
            }
            else {
                audioHandler.StopHandler();
            }
        }
    }

    public override void YieldPlayerController() {
        base.YieldPlayerController();
        uiDialogueManager.EndDialogue();
        audioHandler.StopHandler();
    }

    public override void Traverse( IFlowObject flowObject ) {
        AboutToRequestMain = false;
#if UNITY_EDITOR
         if (editor_SkipDialogues)
             return;
#endif
        if ( flowObject is Dialogue dialogue ) {
            AboutToRequestMain = true;
            return;
        }

        if ( flowObject is DialogueFragment dialogueFragment ) {
            AboutToRequestMain = true;
            OnDialogue( dialogueFragment );
        }
    }

    public override void ViewNext( IList<Branch> branches ) {

    }
}

public class DialogueData {
    public string name = "", text = "";
    public Sprite sprite, descriptionSprite;
    public AudioClip clip;

    public bool HasSprite {
        get => sprite != null;
    }

    public bool HasAudio {
        get => clip != null;
    }

    public bool HasDescriptionImage {
        get => descriptionSprite != null;
    }

    public bool HasText {
        get => !string.IsNullOrEmpty( text );
    }
}