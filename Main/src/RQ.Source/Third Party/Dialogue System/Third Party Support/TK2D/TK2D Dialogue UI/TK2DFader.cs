using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem.TK2D {
	
	/// <summary>
	/// Provides a fade in/out effect for TK2D sprites and text meshes. Makes the TK2D element
	/// fade in/out when the game object is activated.
	/// </summary>
	[AddComponentMenu("Dialogue System/Third Party/2D Toolkit/Effects/Fader")]
	public class TK2DFader : MonoBehaviour {
		
		/// <summary>
		/// The duration in seconds to fade in.
		/// </summary>
		public float fadeInDuration = 0.2f;
		
		/// <summary>
		/// The duration in seconds to show the sprite/text mesh before fading out.
		/// </summary>
		public float showDuration = 1f;
		
		/// <summary>
		/// The duration in seconds to fade out. If <c>0</c>, there is no fade out
		/// and the sprite/text mesh stays visible.
		/// </summary>
		public float fadeOutDuration = 0.2f;
		
		public bool fadeOnEnable = true;
		
		private tk2dTextMesh textMesh = null;
		private tk2dBaseSprite sprite = null;
		
		void Awake() {
			textMesh = GetComponent<tk2dTextMesh>();
			sprite = GetComponent<tk2dBaseSprite>();
		}
		
		/// <summary>
		/// Starts the fade in/out routine when the component is enabled.
		/// </summary>
		void OnEnable() {
			if (fadeOnEnable) StartCoroutine(Play());
		}
		
		public void SetDurations(float fadeInDuration, float showDuration, float fadeOutDuration) {
			this.fadeInDuration = fadeInDuration;
			this.showDuration = showDuration;
			this.fadeOutDuration = fadeOutDuration;
		}
		
		public IEnumerator Play() {
			// Fade in:
			float startTime = DialogueTime.time;
			float endTime = startTime + fadeInDuration;
			while (DialogueTime.time < endTime) {
				SetAlpha(Mathf.InverseLerp(startTime, endTime, DialogueTime.time));
				yield return null;
			}
			SetAlpha(1);
			
			// If no fade out, stop here:
			if (Tools.ApproximatelyZero(fadeOutDuration)) yield break;
			
			// Otherwise wait and then fade out:
			yield return new WaitForSeconds(showDuration);
			startTime = DialogueTime.time;
			endTime = startTime + fadeInDuration;
			while (DialogueTime.time < endTime) {
				SetAlpha(1 - Mathf.InverseLerp(startTime, endTime, DialogueTime.time));
				yield return null;
			}
			SetAlpha(0);			
		}
		
		private void SetAlpha(float alpha) {
			if (textMesh != null) {
				textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);
				textMesh.Commit();
			}
			if (sprite != null) {
				sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
			}
		}
			
	}

}
