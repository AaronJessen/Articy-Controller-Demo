using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class PreciseTimer : MonoBehaviour {
    public UnityFloatEvent OnTimerUpdate = new UnityFloatEvent();
    public UnityEvent OnTimerEnd= new UnityEvent();

    public float CurrentTime { get; private set; }

    public float timer;
    [SerializeField, DisableInPlayMode]
    private bool _restartOnEnd;

    // Update is called once per frame
    void Update() {
        if(CurrentTime >= timer ) {
            enabled = _restartOnEnd;
            OnTimerEnd.Invoke();
            ResetTimer();
            return;
        }
        CurrentTime += Time.deltaTime;
        OnTimerUpdate.Invoke( timer - CurrentTime );
    }

    public void ResetTimer() {
        CurrentTime = 0;
    }
}
