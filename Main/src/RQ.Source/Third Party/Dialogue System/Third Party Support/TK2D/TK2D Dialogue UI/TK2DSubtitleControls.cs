using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem.TK2D {
	
	/// <summary>
	/// Subtitle UI controls for TK2DDialogueUI. This class is used for NPC subtitles,
	/// NPC reminder subtitles (shown during the response menu), and PC subtitles.
	/// </summary>
	[System.Serializable]
	public class TK2DSubtitleControls : AbstractUISubtitleControls {
		
		/// <summary>
		/// The alert panel. A panel is optional, but you may want one
		/// so you can include a background image, panel-wide effects, etc.
		/// </summary>
		public tk2dUILayout panel;
		
		/// <summary>
		/// The label that will show the portrait image of the speaker.
		/// </summary>
		public tk2dBaseSprite portraitImage;
		
		/// <summary>
		/// The label that will show the name of the speaker.
		/// </summary>
		public tk2dTextMesh portraitName;
		
		/// <summary>
		/// The label that will show the text of the subtitle.
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
		private Color defaultColor = Color.white;
		
		/// <summary>
		/// Have we recorded the alert line's initial color as the default color yet?
		/// </summary>
		private bool haveSavedDefaultColor = false;
		
		public override bool HasText {
			get { return (line != null) && !string.IsNullOrEmpty(line.text); }
		}
		
		public override void SetActive (bool value) {
			TK2DDialogueTools.SetControlActive(line, value);
			TK2DDialogueTools.SetControlActive(portraitImage, value);
			TK2DDialogueTools.SetControlActive(portraitName, value);
			TK2DDialogueTools.SetControlActive(continueButton, value);
			TK2DDialogueTools.SetControlActive(panel, value);
		}
		
		public override void HideContinueButton() {
			TK2DDialogueTools.SetControlActive(continueButton, false);
		}

		public override void SetSubtitle(Subtitle subtitle) {
			if (subtitle != null) {
				string actorName = subtitle.speakerInfo.Name;
				if (!string.IsNullOrEmpty(actorName)) {
					if (portraitImage != null) portraitImage.SetSprite(actorName);
					if (portraitName != null) portraitName.text = actorName;
				}
				CheckDefaultColor();
				if (line != null) TK2DDialogueTools.SetFormattedText(line, subtitle.formattedText, defaultColor);
			}
		}
		
		public override void ClearSubtitle() {
			CheckDefaultColor();
			if (line != null) TK2DDialogueTools.SetPlainText(line, string.Empty, defaultColor);
		}
		
		private void CheckDefaultColor() {
			if (!haveSavedDefaultColor && (line != null)) {
				defaultColor = line.color;
				haveSavedDefaultColor = true;
			}
		}
		
		/// <summary>
		/// Sets the portrait collection.
		/// </summary>
		/// <param name='portraitCollection'>
		/// Portrait collection.
		/// </param>
		public void SetPortraitCollection(tk2dSpriteCollectionData portraitCollection) {
			if (portraitImage != null) portraitImage.SetSprite(portraitCollection, 0);
		}
		
	}
		
}
