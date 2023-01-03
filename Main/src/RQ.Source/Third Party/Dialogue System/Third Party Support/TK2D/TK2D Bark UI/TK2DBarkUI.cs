using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem.TK2D {

	/// <summary>
	/// Implements IBarkUI for 2D Toolkit. To use barks with 2D Toolkit, add a TK2DBarkUI to the
	/// actor. Then add a tk2dTextMesh to a child object. Add the Always Fade Camera script if
	/// you want the bark to always face the main camera. Optionally add a TK2DFader if you want
	/// barks to fade in and out.
	/// </summary>
	[AddComponentMenu("Dialogue System/Third Party/2D Toolkit/Bark UI")]
	public class TK2DBarkUI : MonoBehaviour, IBarkUI {
		
		/// <summary>
		/// The text mesh that will display barks.
		/// </summary>
		public tk2dTextMesh textMesh;
		
		/// <summary>
		/// Set <c>true</c> to include the barker's name in the text.
		/// </summary>
		public bool includeName;
		
		/// <summary>
		/// The default color to use if not overridden by an emphasis tag.
		/// </summary>
		public Color defaultColor = Color.white;
		
		/// <summary>
		/// The duration in seconds to show the bark.
		/// </summary>
		public float duration = 4f;
		
		/// <summary>
		/// The duration in seconds to fade in and out.
		/// </summary>
		public float fadeDuration = 0.5f;
		
		/// <summary>
		/// Set <c>true</c> to keep the bark text onscreen until the sequence ends.
		/// </summary>
		public bool waitUntilSequenceEnds;

		private TK2DFader fader = null;

		private float secondsLeft = 0;
		
		/// <summary>
		/// On Awake, find the text mesh if it's not already assigned.
		/// </summary>
		void Awake() {
			if (textMesh == null) textMesh = GetComponentInChildren<tk2dTextMesh>();
		}
		
		/// <summary>
		/// On Start, find the fader and mark the bark UI as not playing.
		/// </summary>
		void Start() {
			if (textMesh != null) {
				fader = textMesh.GetComponent<TK2DFader>();
				if (fader != null) fader.fadeOnEnable = false;
				IsPlaying = false;
                if (textMesh.GetComponent<Renderer>() != null) textMesh.GetComponent<Renderer>().enabled = false;
			}
		}
	
		/// <summary>
		/// Bark the specified subtitle.
		/// </summary>
		/// <param name='subtitle'>
		/// Subtitle to bark.
		/// </param>
		public void Bark(Subtitle subtitle) {
			if ((subtitle != null) && (textMesh != null)) {
				if (includeName) subtitle.formattedText.text = string.Format("{0}: {1}", subtitle.speakerInfo.Name, subtitle.formattedText.text);
				TK2DDialogueTools.SetFormattedText(textMesh, subtitle.formattedText, defaultColor);
                if (textMesh.GetComponent<Renderer>() != null) textMesh.GetComponent<Renderer>().enabled = true;
				StopAllCoroutines();
				if (fader != null) {
					fader.StopAllCoroutines();
					fader.SetDurations(fadeDuration, duration, waitUntilSequenceEnds ? 0 : fadeDuration);
					fader.StartCoroutine(fader.Play());
				}
				secondsLeft = duration;
			}
		}
			
		/// <summary>
		/// Gets or sets a value indicating whether this instance is playing.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is playing; otherwise, <c>false</c>.
		/// </value>
		public bool IsPlaying {
			get { return secondsLeft > 0; }
			set {
				if (value == true) {
					if (secondsLeft <= 0) secondsLeft = duration;
				} else {
					secondsLeft = 0;
				}
			}
		}
		
		/// <summary>
		/// Updates the seconds left and hides the label if time is up.
		/// </summary>
		public void Update() {
			if (secondsLeft > 0) {
				secondsLeft -= Time.deltaTime;
				if ((secondsLeft <= 0) && !waitUntilSequenceEnds) Hide();
			}
		}
		
		void OnBarkEnd(Transform actor) {
			Hide();
		}
		
		private void Hide() {
            if ((textMesh != null) && (textMesh.GetComponent<Renderer>() != null)) textMesh.GetComponent<Renderer>().enabled = false;
		}
		
	}

}