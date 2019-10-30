using Articy.Teleperformance_Test;
using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticyCharacterReference : ArticyGeneralReference<Character> {
    [ArticyTypeConstraint( typeof( Character ) )]
    public ArticyRef reference;
    protected override ArticyRef BaseReference {
        get => reference;
    }
}
