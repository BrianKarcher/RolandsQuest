using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem.TK2D {
	
	/// <summary>
	/// Response menu controls for TK2DDialogueUI.
	/// </summary>
	[System.Serializable]
	public class TK2DResponseMenuControls : AbstractUIResponseMenuControls {
		
		/// <summary>
		/// The alert panel. A panel is optional, but you may want one
		/// so you can include a background image, panel-wide effects, etc.
		/// </summary>
		public tk2dUILayout panel;
		
		/// <summary>
		/// The label that will show the portrait image of the PC on the response menu.
		/// </summary>
		public tk2dBaseSprite pcImage;
		
		/// <summary>
		/// The label that will show the name of the PC on the response menu.
		/// </summary>
		public tk2dTextMesh pcName;
		
		/// <summary>
		/// The reminder of the last subtitle.
		/// </summary>
		public TK2DSubtitleControls subtitleReminder;
		
		/// <summary>
		/// The (optional) timer.
		/// </summary>
		public TK2DTimer timer;
		
		/// <summary>
		/// The response buttons.
		/// </summary>
		public TK2DResponseButton[] buttons;
		
		public override AbstractUISubtitleControls SubtitleReminder {
			get { return subtitleReminder; }
		}
		
		private TK2DTimer tk2dTimer = null;
		
		//--- Not used (uses sprite library instead): private Texture2D pcPortraitTexture = null;
		private string pcPortraitName = null;
		
		public override void SetPCPortrait(Texture2D portraitTexture, string portraitName) {
			//pcPortraitTexture = portraitTexture;
			pcPortraitName = portraitName;
		}
		
		/// <summary>
		/// Sets the controls active/inactive, except this method never activates the timer. If the
		/// UI's display settings specify a timeout, then the UI will call StartTimer() to manually
		/// activate the timer.
		/// </summary>
		/// <param name='value'>
		/// Value (<c>true</c> for active; otherwise inactive).
		/// </param>
		public override void SetActive (bool value) {
			SubtitleReminder.SetActive(value && SubtitleReminder.HasText);
			foreach (TK2DResponseButton button in buttons) {
				TK2DDialogueTools.SetControlActive(button, value && button.IsVisible);
			}
			TK2DDialogueTools.SetControlActive(timer, false);
			TK2DDialogueTools.SetControlActive(pcName, value);
			TK2DDialogueTools.SetControlActive(pcImage, value);
			TK2DDialogueTools.SetControlActive(panel, value);
			if (value == true) {
				if (pcImage != null) pcImage.SetSprite(pcPortraitName);
				if ((pcName != null) && (pcPortraitName != null)) pcName.text = pcPortraitName;
			}

		}
		
		/// <summary>
		/// Clears the response buttons.
		/// </summary>
		protected override void ClearResponseButtons() {
			if (buttons != null) {
				for (int i = 0; i < buttons.Length; i++) {
					SetResponseButton(buttons[i], null, null);
					buttons[i].IsVisible = showUnusedButtons;
				}
			}
		}
		
		/// <summary>
		/// Sets the response buttons.
		/// </summary>
		/// <param name='responses'>
		/// Responses.
		/// </param>
		/// <param name='target'>
		/// Target that will receive OnClick events from the buttons.
		/// </param>
		protected override void SetResponseButtons(Response[] responses, Transform target) {
			if ((buttons != null) && (buttons.Length > 0) && (responses != null)) {
				
				// Add explicitly-positioned buttons:
				for (int i = 0; i < responses.Length; i++) {
					if (responses[i].formattedText.position != FormattedText.NoAssignedPosition) {
						int position = Mathf.Clamp(responses[i].formattedText.position, 0, buttons.Length - 1);
						SetResponseButton(buttons[position], responses[i], target);
					}
				}
				
				// Auto-position remaining buttons:
				if (buttonAlignment == ResponseButtonAlignment.ToFirst) {
					
					// Align to first, so add in order to front:
					for (int i = 0; i < Mathf.Min(buttons.Length, responses.Length); i++) {
						if (responses[i].formattedText.position == FormattedText.NoAssignedPosition) {
							int position = Mathf.Clamp(GetNextAvailableResponseButtonPosition(0, 1), 0, buttons.Length - 1);
							SetResponseButton(buttons[position], responses[i], target);
						}
					}
				} else {
					
					// Align to last, so add in reverse order to back:
					for (int i = Mathf.Min(buttons.Length, responses.Length) - 1; i >= 0; i--) {
						if (responses[i].formattedText.position == FormattedText.NoAssignedPosition) {
							int position = Mathf.Clamp(GetNextAvailableResponseButtonPosition(buttons.Length - 1, -1), 0, buttons.Length - 1);
							SetResponseButton(buttons[position], responses[i], target);
						}
					}
				}
			}
		}
		
		private void SetResponseButton(TK2DResponseButton button, Response response, Transform target) {
			if (button != null) {
				button.IsVisible = true;
				button.IsClickable = (response != null);
				button.Target = target;
				button.Response = response;
				if (response != null) {
					button.SetFormattedText(response.formattedText);
				} else if (showUnusedButtons) {
					button.SetUnformattedText(string.Empty);
				}
			}
		}
		
		private int GetNextAvailableResponseButtonPosition(int start, int direction) {
			if (buttons != null) {
				int position = start;
				while ((0 <= position) && (position < buttons.Length)) {
					if (buttons[position].IsClickable) {
						position += direction;
					} else {
						return position;
					}
				}
			}
			return 0;
		}
	
		/// <summary>
		/// Starts the timer.
		/// </summary>
		/// <param name='timeout'>
		/// Timeout duration in seconds.
		/// </param>
		public override void StartTimer(float timeout) {
			if (timer != null) {
				if (tk2dTimer == null) {
					TK2DDialogueTools.SetControlActive(timer, true);
					tk2dTimer = timer.GetComponent<TK2DTimer>();
					TK2DDialogueTools.SetControlActive(timer, false);
				}
				if (tk2dTimer != null) {
					timer.Value = 1;
					tk2dTimer.duration = timeout;
					tk2dTimer.TimeoutHandler -= OnTimeout;
					tk2dTimer.TimeoutHandler += OnTimeout;
					TK2DDialogueTools.SetControlActive(timer, true);
				}
				
			}
		}
		
		/// <summary>
		/// This method is called if the timer runs out. It selects the first response.
		/// </summary>
		public void OnTimeout() {
			// Used to call button click directly. Now sends OnConversationTimeout message.
			//foreach (TK2DResponseButton button in buttons) {
			//	if ((button.Response != null) && button.IsClickable) {
			//		button.OnClick();
			//		return;
			//	}
			//}
			DialogueManager.Instance.SendMessage("OnConversationTimeout");
		}
		
	}

}
