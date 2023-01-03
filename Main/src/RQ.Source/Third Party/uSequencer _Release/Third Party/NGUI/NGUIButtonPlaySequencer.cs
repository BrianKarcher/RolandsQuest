using UnityEngine;
using System.Collections;
using WellFired;

public class NGUIButtonPlaySequencer : MonoBehaviour 
{
	[SerializeField]
	private USSequencer sequenceToPlay;

	private void Start() 
	{
		UIEventListener.Get(gameObject).onClick += (go) => { sequenceToPlay.Play(); };
	}
}
