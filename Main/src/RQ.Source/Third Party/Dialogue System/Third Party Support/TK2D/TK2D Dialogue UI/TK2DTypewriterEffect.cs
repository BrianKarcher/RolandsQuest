using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem.TK2D {

	[AddComponentMenu("Dialogue System/Third Party/2D Toolkit/Effects/Typewriter")]
	public class TK2DTypewriterEffect : MonoBehaviour {
	
		/// <summary>
		/// How fast to "type."
		/// </summary>
		public float charactersPerSecond = 50;
		
		public void OnEnable() {
			StartCoroutine(Play());
		}
		
		public void OnDisable() {
			Stop();
		}
		
		/// <summary>
		/// Plays the typewriter effect.
		/// </summary>
		public IEnumerator Play() {
			tk2dTextMesh textMesh = GetComponent<tk2dTextMesh>();
			if (textMesh == null) yield break;
			string fullText = textMesh.text;
			int length = 0;
			while ((length + 1) < fullText.Length) {
				length++;
				textMesh.text = fullText.Substring(0, length);
				textMesh.Commit();
				float delay = 1 / charactersPerSecond;
				yield return new WaitForSeconds(delay);
			}
			textMesh.text = fullText;
			textMesh.Commit();
		}
		
		public void Stop() {
			StopAllCoroutines();
		}
		
	}

}
