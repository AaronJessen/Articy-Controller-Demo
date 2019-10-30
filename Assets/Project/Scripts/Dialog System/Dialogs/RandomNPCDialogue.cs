using System.Collections;
using System.Collections.Generic;
using Articy.Teleperformance_Test;
using UnityEngine;

public class RandomNPCDialogue : NPCDialog {

    private System.Action _interactionEnd;

    [SerializeField]
    private ArticyHubReference _hubReference;
    private Hub _returnHub;

    private ArticyManager Manager {
        get => ArticyManager.Instance;
    }

    public override void Interact (GameObject source, System.Action InteractionEnd) {
        _returnHub = Manager.LastHub;
        Manager.Goto (_hubReference.articyObject);
        _interactionEnd = InteractionEnd;
        base.Interact (source, OnInteractionEnd);
    }

    private void OnInteractionEnd () {
        Manager.Goto (_returnHub);
        _interactionEnd?.Invoke ();
    }
}