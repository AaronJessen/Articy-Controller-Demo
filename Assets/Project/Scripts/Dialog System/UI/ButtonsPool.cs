using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ButtonsPool : InfiniteObjectsPool<BranchObject> {
    [Required]
    public LayoutGroup layoutGroupParent;

    protected override BranchObject InstantiatePoolObject( BranchObject poolObject ) {
        var branch = base.InstantiatePoolObject( poolObject );
        branch.transform.SetParent( layoutGroupParent.transform, false );
        return branch;
    }
}
