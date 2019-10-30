using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine;
using TMPro;

public class UIBigTextMAnager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _tmp;

    public void ShowText(string text){
        GameEventMessage.SendEvent("BlackText");
        _tmp.text = text;
    }

    public void HideText(){
        GameEventMessage.SendEvent("Exit");
    }
}
