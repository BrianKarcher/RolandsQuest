using UnityEngine;
using System.Collections;
using WellFired;

public class NGUIButtonPauseSequencer : MonoBehaviour 
{
	[SerializeField]
	private USSequencer sequenceToPause;

	private void Start() 
	{
		UIEventListener.Get(gameObject).onClick += (go) => { sequenceToPause.Pause(); };
	}
}
