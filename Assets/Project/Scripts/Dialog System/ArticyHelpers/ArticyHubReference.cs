using System.Collections;
using System.Collections.Generic;
using Articy.Teleperformance_Test;
using Articy.Teleperformance_Test.Templates;
using Articy.Unity;
using UnityEngine;

public class ArticyHubReference : ArticyGeneralReference<Hub> {
    [ArticyTypeConstraint (typeof (Hub))]
    public ArticyRef reference;
    protected override ArticyRef BaseReference {
        get => reference;
    }
}