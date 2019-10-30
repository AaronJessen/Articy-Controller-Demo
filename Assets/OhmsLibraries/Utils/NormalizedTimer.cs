using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NormalizedTimer : MonoBehaviour {
    public float timer;
    public UnityEvent OnTimerEnd;
    public UnityFloatEvent OnNormalizedTimer, OnTimer;

    private float _currentTime = 0;

    private void OnEnable() {
        _currentTime = 0;
    }

    protected virtual void Update() {
        _currentTime += Time.deltaTime;
        OnTimer.Invoke( timer - _currentTime );
        OnNormalizedTimer.Invoke( _currentTime / timer );
        if( _currentTime >= timer ) {
            OnTimerEnd.Invoke();
            enabled = false;
        }
    }
}
