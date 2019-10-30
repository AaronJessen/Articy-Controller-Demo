using Articy.Teleperformance_Test;
using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticyFlowFragmentReference : ArticyGeneralReference<FlowFragment>
{
    [ArticyTypeConstraint( typeof( FlowFragment ) )]
    public ArticyRef reference;
    protected override ArticyRef BaseReference {
        get => reference;
    }
}
