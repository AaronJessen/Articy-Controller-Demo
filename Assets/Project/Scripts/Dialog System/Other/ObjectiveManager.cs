using System.Collections;
using System.Collections.Generic;
using Articy.Unity;
using UnityEngine;
using Articy.Teleperformance_Test;
using Sirenix.OdinInspector;

public class ObjectiveManager : ArticySubManager {

    [SerializeField, Required, HideInPrefabAssets]
    private UIObjectiveManager _uiManager;

    public override int Priority {
        get => 99;
    }

    public override void ManageFlowPlayer( ArticyFlowPlayer player ) {
    }

    public override void Traverse( IFlowObject flowObject ) {
        if ( flowObject is MissionFlowFragment missionFlowFragment ) {
            _uiManager.SetObjective( missionFlowFragment.Text );
            Debug.Log( $"#Objective#Setting objective {missionFlowFragment.Text}." );
        }
    }

    public override void ViewNext( IList<Branch> branches ) {

    }
}
