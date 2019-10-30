using Doozy.Engine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIBranchesManager : MonoBehaviour {
    [Required, SerializeField, BoxGroup( "UI" )]
    private TextMeshProUGUI _description;
    [Required, SerializeField, BoxGroup( "UI" )]
    private Image _portrait;

    [Required, SerializeField]
    private ButtonsPool _pool;

    public string Description {
        get => _description.text;
        set => _description.text = value;
    }

    public Sprite Portrait {
        get => _portrait.sprite;
        set => _portrait.sprite = value;
    }

    public void StartBranches( BranchData data ) {
        GameEventMessage.SendEvent( "ChoiceStart" );
        Description = data.description;
        Portrait = data.portrait;
        for ( int i = 0; i < data.buttonsData.Count; i++ ) {
            AddButton( data.buttonsData[i].text, data.buttonsData[i].callback );
        }
    }

    public void EndBranches() {
        GameEventMessage.SendEvent( "ChoiceEnd" );
        Debug.Log( $"Dispawned all pool, hiding window" );
        _pool.DespawnAll();
    }

    public void AddButton( string text, UnityAction OnClicked ) {
        if ( _pool.RequestPoolMonoBehaviour( out BranchObject option ) ) {
            option.Spawn( OnClicked, text );
        }
    }
}
