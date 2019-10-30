using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Doozy.Engine;
using Doozy.Engine.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueManager : MonoBehaviour {
    [Required, SerializeField, BoxGroup ("UISettings")]
    private GameObject _container;
    [Required, SerializeField, BoxGroup ("UISettings")]
    private Image _portrait;
    [Required, SerializeField, BoxGroup ("UISettings")]
    private Button _nextButton;
    [Required, SerializeField, BoxGroup ("UISettings")]
    private TextMeshProUGUI _name, _dialogue;
    [SerializeField, BoxGroup ("UISettings"), MinValue (1)]
    private int _maxCharacterCount = 200;
    [MinValue (0.01f), SerializeField, BoxGroup ("Text Animation")]
    private float _showTextTime = 2f;
    [SerializeField, BoxGroup ("Text Animation")]
    private Ease _showTextEase = Ease.Linear;

    private Queue<string> _wordQueue = new Queue<string> ();
    private TweenerCore<string, string, StringOptions> _dialogueTween;
    private string _currentDialogue;
    private System.Action ShowCompletecallback;

    private const string POPUP_NAME = "DialogueImage";
    private const string IMAGE_NAME = "Image";

    private UIPopup _currentPopup;

    public void StartDialogue (System.Action CompleteCallback, bool showNextButton = true, bool manageGameEvent = true) {
        if (manageGameEvent)
            GameEventMessage.SendEvent ("DialogueStart");
        ShowCompletecallback = CompleteCallback;
        _maxCharacterCount = _dialogue.maxVisibleCharacters;
        ShowNextButton(showNextButton);
    }

    public void ShowNextButton (bool show) {
        _nextButton.gameObject.SetActive (show);
        _nextButton.interactable = show;
    }

    public void ShowDialogueText (DialogueData data) {
        Debug.Log ($"#Dialogue#Data text: '{data.text}' and bool {data.HasText}");

        if (data.HasSprite) {
            _portrait.gameObject.SetActive (true);
            SetPortrait (data.sprite);
        }
        else {
            _portrait.gameObject.SetActive (false);
        }
        if (data.HasDescriptionImage) {
            _currentPopup?.Hide ();
            _currentPopup = UIPopup.GetPopup (POPUP_NAME);
            SetDescriptionImage (data.descriptionSprite);
        }
        SetDialogue (data.text, data.name);
        ShowDialogueTween (CutDialog ());
    }

    [Button]
    private void Editor_Popup () {
        _currentPopup = UIPopup.GetPopup (POPUP_NAME);
        _currentPopup.Show ();
    }

    private void SetPortrait (Sprite portrait) {
        _portrait.gameObject.SetActive (true);
        _portrait.sprite = portrait;
    }

    private void SetDescriptionImage (Sprite description) {
        _currentPopup.Data.SetImagesSprites (description);
        _currentPopup.Show ();
    }

    private void SetDialogue (string dialogue, string name) {
        Debug.Log ($"Showing text {dialogue}", gameObject);
        _currentDialogue = dialogue;
        _name.text = name;
        _wordQueue = new Queue<string> (_currentDialogue.Split (' '));
    }

    public void Continue () {
        if (_dialogueTween != null && _dialogueTween.IsPlaying ()) { //The animation is still playing, we finish it.
            Debug.Log ("IsPLaying");
            _dialogueTween.Complete ();
            return;
        }
        if (_wordQueue.Count > 0) { //There are words still in the queue, so we show them.
            Debug.Log ("word Queue");
            ShowDialogueTween (CutDialog ());
            return;
        }
        _currentPopup?.Hide ();
        ShowCompletecallback.Invoke ();
    }

    public void EndDialogue () {
        GameEventMessage.SendEvent ("DialogueEnd"); //Events for Doozy UI
    }

    /// <summary>
    /// Method used if the string is too large to fit inside TMPro.
    /// </summary>
    /// <returns></returns>
    private string CutDialog () {
        StringBuilder strBuilder = new StringBuilder (_maxCharacterCount);
        while (_wordQueue.Count > 0 && strBuilder.Length < _maxCharacterCount) { //While there are words to show and we don't surpass the limit.
            var nextCount = strBuilder.Length + _wordQueue.Peek ().Length;
            if (nextCount > _maxCharacterCount) {
                break; //The next word won't fit, so we exit.
            }
            strBuilder.Append ($" {_wordQueue.Dequeue()}");
        }
        return strBuilder.ToString ();
    }

    private void ShowDialogueTween (string dialog) {
        _dialogue.text = ""; //Clean the text so the animation shows correctly.
        _dialogueTween = _dialogue.DOText (dialog, _showTextTime);
        _dialogueTween.SetEase (_showTextEase);
    }

}