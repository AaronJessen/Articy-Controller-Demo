using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Teleperformance_Test;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;

public class NPCDialog : MonoBehaviour, IInteractable {
    [Required, SerializeField]
    protected ArticyCharacterReference characterReference;

    private void Start() {
#if UNITY_EDITOR
        if ( !characterReference.IsValid ) {
            Debug.LogError( $"Character must have a valid value in {name}", characterReference.gameObject );
        }
#endif
    }

    public virtual void Interact( GameObject source, Action OnInteractionEnd ) {
        ArticyManager.Instance.PlayCharacter( characterReference.articyObject, OnInteractionEnd );
    }

#if UNITY_EDITOR
    private void Reset() {
        characterReference = GetComponent<ArticyCharacterReference>();
        if ( !characterReference ) {
            characterReference = gameObject.AddComponent<ArticyCharacterReference>();
        }
    }

    [Button]
    private void Editor_Interact() {
        var player = FindObjectOfType<CharacterMovement>().gameObject;
        Interact( player, () => GetComponent<CharacterMovement>() );
    }
#endif
}
