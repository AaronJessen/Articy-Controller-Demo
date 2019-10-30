using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Doozy.Engine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class BranchObject : PoolMonoBehaviour {

    [Required, SerializeField]
    private UIButton _doozyButton;
    [Required, SerializeField]
    public TextMeshProUGUI _tmpro;

    public override void Despawn() {
        base.Despawn();
        _doozyButton.OnClick.OnTrigger.Event.RemoveAllListeners();
    }

    public void Spawn( UnityAction branchAction, string branchDescription ) {
        _doozyButton.OnClick.OnTrigger.Event.AddListener( branchAction );
        _tmpro.text = branchDescription;
        base.Spawn();
    }
}
