using Articy.Teleperformance_Test;
using Articy.Teleperformance_Test.Templates;
using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticyItemReference : ArticyGeneralReference<ItemEntity>
{
    [ArticyTypeConstraint( typeof( ItemEntity ) )]
    public ArticyRef reference;
    protected override ArticyRef BaseReference {
        get => reference;
    }
}
