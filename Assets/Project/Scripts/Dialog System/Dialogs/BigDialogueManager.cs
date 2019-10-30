using System.Collections;
using System.Collections.Generic;
using Articy.Teleperformance_Test;
using Sirenix.OdinInspector;
using UnityEngine;

public class BigDialogueManager : ArticySubManager {
    public override int Priority { get => 102; }
#if UNITY_EDITOR
    public bool editor_Skip = false;
#endif

    [SerializeField]
    private UIBigTextMAnager _uiManager;
    [SerializeField]
    public float _duration = 4f;

    public override void ManageFlowPlayer (Articy.Unity.ArticyFlowPlayer player) {
        var flowFragment = ArticyManager.Instance.LastFlowFragment as BigTextFlowFragment;
        _uiManager.ShowText (flowFragment.Text);
        StartCoroutine(PlayDelayed());
    }

    public override void Traverse (Articy.Unity.IFlowObject flowObject) {
        AboutToRequestMain = false;
#if UNITY_EDITOR
        if (editor_Skip) {
            return;
        }
#endif
        if (flowObject is BigTextFlowFragment) {
            AboutToRequestMain = true;
        }
    }

    public override void ViewNext (IList<Articy.Unity.Branch> branches) { }

    public override void YieldPlayerController(){
        base.YieldPlayerController();
        _uiManager.HideText();
    }

    private IEnumerator PlayDelayed(){
        yield return new WaitForSeconds(_duration);
        _flowPlayer.Play();
    }
}