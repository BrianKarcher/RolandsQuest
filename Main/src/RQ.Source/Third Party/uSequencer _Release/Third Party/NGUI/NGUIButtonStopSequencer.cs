using UnityEngine;
using System.Collections;
using WellFired;

public class NGUIButtonStopSequencer : MonoBehaviour 
{
	[SerializeField]
	private USSequencer sequenceToStop;

	private void Start() 
	{
		UIEventListener.Get(gameObject).onClick += (go) => { sequenceToStop.Stop(); };
	}
}
