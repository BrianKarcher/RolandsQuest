using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem.TK2D {
	
	/// <summary>
	/// GUI controls for TK2DDialogueUI's alert message.
	/// </summary>
	[System.Serializable]
	public class TK2DAlertControls : AbstractUIAlertControls {
		
		/// <summary>
		/// The alert panel. A panel is optional, but you may want one
		/// so you can include a background image, panel-wide effects, etc.
		/// </summary>
		public tk2dUILayout panel;
		
		/// <summary>
		/// The text mesh used to show the alert message text.
		/// </summary>
		public tk2dTextMesh line;
		
		/// <summary>
		/// The continue button. This button is only required if DisplaySettings.waitForContinueButton 
		/// is <c>true</c> -- in which case this button should send "OnContinue" to the UI when clicked.
		/// </summary>
		public tk2dUIItem continueButton;
		
		/// <summary>
		/// The default color to use except when an emphasis tag overrides it.
		/// </summary>
		private Color defaultColor;
		
		/// <summary>
		/// Have we recorded the alert line's initial color as the default color yet?
		/// </summary>
		private bool haveSavedDefaultColor = false;
		
		/// <summary>
		/// Is an alert currently showing?
		/// </summary>
		/// <value>
		/// <c>true</c> if showing; otherwise, <c>false</c>.
		/// </value>
		public override bool IsVisible {
			get { return (line != null) && (line.gameObject.activeSelf); }
		}
		
		/// <summary>
		/// Sets the alert controls active.
		/// </summary>
		/// <param name='value'>
		/// <c>true</c> for active.
		/// </param>
		public override void SetActive(bool value) {
			TK2DDialogueTools.SetControlActive(line, value);
			TK2DDialogueTools.SetControlActive(panel, value);
		}
		
		/// <summary>
		/// Sets the alert message and begins the fade in/out routine if a TK2DFader is attached.
		/// </summary>
		/// <param name='message'>
		/// Alert message.
		/// </param>
		/// <param name='duration'>
		/// Duration to show message.
		/// </param>
		public override void SetMessage(string message, float duration) {
			if (line != null) {
				if (!haveSavedDefaultColor) {
					defaultColor = line.color;
					haveSavedDefaultColor = true;
				}
				TK2DDialogueTools.SetFormattedText(line, FormattedText.Parse(message, DialogueManager.MasterDatabase.emphasisSettings), defaultColor);
				SetFadeDuration(line.gameObject, duration);
				if (panel != null) SetFadeDuration(panel.gameObject, duration);
			}
		}
		
		private void SetFadeDuration(GameObject go, float duration) {
			if (go != null) {
				TK2DFader fader = go.GetComponent<TK2DFader>();
				if (fader != null) {
					fader.SetDurations(fader.fadeInDuration, duration, fader.fadeOutDuration);
					alertDoneTime = Mathf.Max(alertDoneTime, DialogueTime.time + fader.fadeInDuration + duration + fader.fadeOutDuration);
					if (go.activeInHierarchy) {
						fader.StopAllCoroutines();
						fader.StartCoroutine(fader.Play());
					}
				}
			}
		}
		
	}
		
}
