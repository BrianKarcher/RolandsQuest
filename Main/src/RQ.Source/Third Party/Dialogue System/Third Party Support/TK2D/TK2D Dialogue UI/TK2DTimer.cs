using UnityEngine;
using System;
using System.Collections;

namespace PixelCrushers.DialogueSystem.TK2D {
	
	/// <summary>
	/// Applies a timer effect to a tk2dProgressBar that counts down from 1 to 0. When time is up,
	/// it calls an event handler. Attach this script to your response menu timer.
	/// </summary>
	[AddComponentMenu("Dialogue System/Third Party/2D Toolkit/Effects/Timer")]
	public class TK2DTimer : MonoBehaviour {
		
		/// <summary>
		/// Occurs when the timer is done (i.e., at the end of duration). If the coroutine is
		/// stopped or the control is deactivated, this will never get called.
		/// </summary>
		public event Action TimeoutHandler = null;
		
		/// <summary>
		/// The timer duration.
		/// </summary>
		public float duration = 5f;
		
		private tk2dUIProgressBar progressBar = null;

		/// <summary>
		/// Gets or sets the value of the progress bar.
		/// </summary>
		/// <value>
		/// The value (0-1).
		/// </value>
		public float Value {
			get { return (progressBar != null) ? progressBar.Value : 0; }
			set { if (progressBar != null) progressBar.Value = value; }
		}
		
		/// <summary>
		/// Starts the timer as soon as the game object is activated.
		/// </summary>
		void OnEnable() {
			StartCoroutine(Play());
		}
		
		/// <summary>
		/// Runs the timer.
		/// </summary>
		public IEnumerator Play() {
			progressBar = GetComponent<tk2dUIProgressBar>();
			if (progressBar == null) yield break;
			float startTime = DialogueTime.time;
			float endTime = startTime + duration;
			while (DialogueTime.time < endTime) {
				progressBar.Value = 1 - Mathf.InverseLerp(startTime, endTime, DialogueTime.time);
				yield return null;
			}
			if (TimeoutHandler != null) TimeoutHandler();
		}
			
	}

}
