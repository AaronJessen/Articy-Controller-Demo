using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UIObjectiveManager : MonoBehaviour {

    [SerializeField, Required]
    private TextMeshProUGUI _objectiveText;

    public void SetObjective( string mission ) {
        _objectiveText.text = mission;
    }
}
