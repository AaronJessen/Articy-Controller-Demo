using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class AudioHandler : MonoBehaviour {
    private AudioSource _source;
    private void Awake () {
        _source = GetComponent<AudioSource> ();
    }

    public void PlayClip (AudioClip clip, System.Action OnComplete = null) {
        StopHandler ();
        _source.clip = clip;
        _source.Play ();
        if (OnComplete != null) {
            StartCoroutine (WaitForEnd (clip.length, OnComplete));
        }

    }

    public void StopHandler () {
        if (_source.isPlaying) {
            _source.Stop ();
        }
    }

    private IEnumerator WaitForEnd (float waitTime, System.Action OnComplete) {
        yield return new WaitForSeconds (waitTime);
        OnComplete.Invoke ();
    }

#if UNITY_EDITOR
    private void Reset () {
        var source = GetComponent<AudioSource> ();
        if (source) {
            source.playOnAwake = false;
        }
    }
#endif
}