using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Articy.Unity;
using Articy.Teleperformance_Test;
using Sirenix.OdinInspector;

public class ArticyBranchReference : ArticyGeneralReference<FlowFragment>
{
    [ArticyTypeConstraint(typeof(FlowFragment))]
    public ArticyRef reference;

    protected override ArticyRef BaseReference { get => reference; }

    [SerializeField, MinValue(0), ValidateInput( "Editor_BranchIndexValid" ), HorizontalGroup("Index"), ShowIf( "HasBranches" )]
    private int _branchIndex;

#if UNITY_EDITOR
    [ShowInInspector, HorizontalGroup( "Index", Width = 30), LabelText("Max"), ShowIf( "HasBranches" )]
    private int Editor_BranchCount {
        get {
            var fragment = reference.GetObject<FlowFragment>();
            if ( fragment != null ) {
                var pins = fragment.OutputPins;
                return pins.Count;
            }
            return -1;
        }
    }

    private bool HasBranches {
        get=>Editor_BranchCount != -1;
    }

    private bool Editor_BranchIndexValid(int index ) {
        if( HasBranches ) {
            var count = Editor_BranchCount;
            return index < count;
        }
        else {
            return true;
        }
    }
#endif
}
