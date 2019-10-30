using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class SimpleTimer : MonoBehaviour {

    public float timer;
    public bool countOnStart;
    public bool realTime;
    public UnityEvent OnTimerEnd = new UnityEvent();
    private float _currentTimer;

    private void Start() {
        if( countOnStart ) {
            StartCountdown();
        }
    }
    
    public void StartCountdown() {
        StartCoroutine( Begin() );
    }

    private IEnumerator Begin() {
        if( realTime ) {
            yield return new WaitForSecondsRealtime( timer );
        }
        else {
            yield return new WaitForSeconds( timer );
        }
        OnTimerEnd.Invoke();
    }
}

public class TimeEvents {
    public int second;
    public UnityEvent OnTime;
}
