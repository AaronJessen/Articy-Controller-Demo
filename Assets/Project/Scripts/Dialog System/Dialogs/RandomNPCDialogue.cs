using System.Collections;
using System.Collections.Generic;
using Articy.Project;
using UnityEngine;

/// <summary>
/// Starts a dialogue from the selected NPC which is not part of the main flow
/// so we go to a selected node in the graph and return to the original afterwards.
/// </summary>
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