using System.Collections;
using System.Collections.Generic;
using LitJson;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu (menuName = "State Management/Local State Manager")]
public class LocalStateManager : BaseStateManager {
    [SerializeField]
    private bool _freezeStates;

    private string Json {
        get {
            return PlayerPrefs.GetString ("Json", "[]");
        }
        set {
            PlayerPrefs.SetString ("Json", value);
        }
    }
    private List<GlobalVariableState> JsonStates {
        get {
            var states = JsonMapper.ToObject<List<GlobalVariableState>> (Json);
            return states ?? new List<GlobalVariableState> ();
        }
        set {
            Json = JsonMapper.ToJson (value);
        }
    }

    [SerializeField, ListDrawerSettings (OnTitleBarGUI = "Editor_StatesTitleBarGUI")]
    private List<GlobalVariableState> states;
    [SerializeField, ListDrawerSettings (OnTitleBarGUI = "Editor_ItemsTitleBarGUI")]
    private List<Item> items;

#if UNITY_EDITOR
    private void Editor_StatesTitleBarGUI () {
        if (Sirenix.Utilities.Editor.SirenixEditorGUI.ToolbarButton (Sirenix.Utilities.Editor.EditorIcons.X)) {
            states.Clear ();
        }
    }

    private void Editor_ItemsTitleBarGUI () {
        if (Sirenix.Utilities.Editor.SirenixEditorGUI.ToolbarButton (Sirenix.Utilities.Editor.EditorIcons.X)) {
            items.Clear ();
        }
    }
#endif

    public override IEnumerator ReceiveStates () {
        //States will be defined in the inspector
        Debug.Log ("#StateRestore#Recieving states");
        receivedItems = items;
        receivedStates = states;
        yield return new WaitForEndOfFrame ();
        Debug.Log ("#StateRestore#Sates received.");
        Ready = true;
    }

    public override void OnGameStateVariableChanged (string aVariableName, object value) {
        base.OnGameStateVariableChanged (aVariableName, value);
        if (lastState == null)
            return;
        AddState (lastState.Copy () as GlobalVariableState);
    }

    public override void OnItemVariableChanged (string aVariableName, object value) {

        base.OnItemVariableChanged (aVariableName, value);

        if (lastItem == null)
            return;

        AddItem (lastItem.Copy () as Item);
    }

    public void AddState (GlobalVariableState state) {
        if (_freezeStates) {
            return;
        }
        states.Add (state);
    }

    public void AddItem (Item item) {
        if (_freezeStates) {
            return;
        }
        items.Add (item);
    }

    public string GetItemsJson () {
        return JsonMapper.ToJson (items);
    }

    public string GetStatesJson () {
        return JsonMapper.ToJson (states);
    }
}