using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chronometer : MonoBehaviour
{
	public float CurrentTime { get; private set; }
	public UnityFloatEvent OnTimeUpdate;
	public UnityEvent OnReset;

	private void Awake()
	{
		ResetTime();
	}

	// Update is called once per frame
	void Update ()
	{
		CurrentTime += Time.deltaTime;
		OnTimeUpdate.Invoke(CurrentTime);
	}

	public void ResetTime()
	{
		CurrentTime = 0;
		OnReset.Invoke();
	}
}
