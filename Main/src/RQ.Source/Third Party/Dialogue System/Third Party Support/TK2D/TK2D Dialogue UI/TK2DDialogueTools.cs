using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem.TK2D {
	
	/// <summary>
	/// Static utility class for TK2D Dialogue UI.
	/// </summary>
	[System.Serializable]
	public static class TK2DDialogueTools {
		
		/// <summary>
		/// Sets a control active.
		/// </summary>
		/// <param name='control'>
		/// Control to set.
		/// </param>
		/// <param name='value'>
		/// <c>true</c> for active; <c>false</c> for inactive.
		/// </param>
		public static void SetControlActive(MonoBehaviour control, bool value) {
			if ((control != null) && (control.gameObject != null)) {
				if ((value == true) && !control.gameObject.activeSelf) {
					control.gameObject.SetActive(true);
				} else if ((value == false) && control.gameObject.activeSelf) {
					control.gameObject.SetActive(false);
				}
			}
		}
		
		/// <summary>
		/// Sets a text mesh using Dialogue System-formatted text.
		/// </summary>
		/// <param name='line'>
		/// The text mesh.
		/// </param>
		/// <param name='formattedText'>
		/// The formatted text.
		/// </param>
		/// <param name='defaultColor'>
		/// Default color to use unless the formatted text contains an emphasis tag that overrides 
		/// the default color.
		/// </param>
		public static void SetFormattedText(tk2dTextMesh line, FormattedText formattedText, Color defaultColor) {
			if (formattedText != null) SetPlainText(line, formattedText.text, (formattedText.emphases.Length > 0) ? formattedText.emphases[0].color : defaultColor);
		}
		
		/// <summary>
		/// Sets a text mesh using plain text.
		/// </summary>
		/// <param name='line'>
		/// The text mesh.
		/// </param>
		/// <param name='text'>
		/// The plain text.
		/// </param>
		/// <param name='color'>
		/// Color.
		/// </param>
		public static void SetPlainText(tk2dTextMesh line, string text, Color color) {
			if (line != null) {
				line.text = text;
				line.color = color;
				line.Commit();
			}
		}
		
	}
		
}
