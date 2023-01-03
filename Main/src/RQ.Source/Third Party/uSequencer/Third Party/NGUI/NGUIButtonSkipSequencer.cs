using UnityEngine;
using System.Collections;
using WellFired;

public class NGUIButtonSkipSequencer : MonoBehaviour 
{
	[SerializeField]
	private USSequencer sequenceToSkip;

	private void Start() 
	{
		UIEventListener.Get(gameObject).onClick += (go) => { sequenceToSkip.SkipTimelineTo(sequenceToSkip.Duration); };
	}
}
