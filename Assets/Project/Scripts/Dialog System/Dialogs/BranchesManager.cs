using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Articy.Unity;
using Articy.Unity.Interfaces;
using Doozy.Engine.Events;
using Doozy.Engine;
using TMPro;
using UnityEngine.UI;
using Articy.Project;
using System.Linq;
using System;
using UnityEngine.Events;

/// <summary>
/// Activated when a decision must be made in the flow. Creates the necessary buttons
/// and moves through the selected branch.
/// </summary>
public class BranchesManager : ArticySubManager {
    [SerializeField, Required, HideInPrefabAssets]
    private UIBranchesManager _uiManager;

    [ShowInInspector, HideInEditorMode, DisableInPlayMode]
    private BranchData _currentData;

    public override int Priority {
        get => 200;
    }

    public void EndDecision( Branch selectedBranch ) {
        _flowPlayer.Play( selectedBranch );
        YieldPlayerController();
    }

    public override void ManageFlowPlayer( ArticyFlowPlayer player ) {
        _uiManager.StartBranches( _currentData );
    }

    public override void YieldPlayerController() {
        base.YieldPlayerController();
        _uiManager.EndBranches();
    }

    public override void Traverse( IFlowObject flowObject ) {

    }

    public override void ViewNext( IList<Branch> branches ) {
        var validBranches = from branch in branches where branch.IsValid select branch;
        _currentData = new BranchData();
        AboutToRequestMain = false;

        if ( validBranches.Count() <= 1 || ArticyManager.Instance.CharacterHub ) {
            return;
        }
        AboutToRequestMain = true;
        Debug.Log( "BranchesManager about to request main." );

        //We create buttons for all posible options
        foreach ( var branch in validBranches ) {
            var menuObject = branch.Target as IObjectWithText;
            var text = menuObject?.Text ?? "";
            _currentData.AddButton( new BranchButtonData { text = text, callback = () => EndDecision( branch ) } );
        }

        //Create the text and show image
        if ( _flowPlayer.CurrentObject.HasReference ) {
            var node = _flowPlayer.CurrentObject.GetObject();
            if ( node is IObjectWithText textObject ) {
                _currentData.description = textObject.Text;
            }

            if ( node is DialogueFragment dialog ) {
                if ( dialog.Speaker is IObjectWithPreviewImage previewImage ) {
                    var asset = previewImage.PreviewImage.Asset;
                    if ( asset != null ) {
                        _currentData.portrait = asset.LoadAssetAsSprite();
                        return;
                    }
                }
            }
        }
    }
}

public class BranchData {
    public List<BranchButtonData> buttonsData = new List<BranchButtonData>();
    public string description;
    public Sprite portrait;

    public void AddButton( BranchButtonData data ) {
        buttonsData.Add( data );
    }
}

public class BranchButtonData {
    public string text;
    public UnityAction callback;
}

