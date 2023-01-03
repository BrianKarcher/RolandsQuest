using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem.TK2D {

	/// <summary>
	/// This is an implementation of ITextFieldUI for TK2D.
	/// </summary>
	public class TK2DTextFieldUI : MonoBehaviour, ITextFieldUI {

		/// <summary>
		/// The panel (optional).
		/// </summary>
		public tk2dUILayout panel;

		/// <summary>
		/// The label.
		/// </summary>
		public tk2dTextMesh label;

		/// <summary>
		/// The text input.
		/// </summary>
		public tk2dUITextInput textInput;

		public KeyCode acceptKey = KeyCode.Return;

		public KeyCode cancelKey = KeyCode.Escape;

		/// <summary>
		/// This field records the delegate that must be called when the player accepts
		/// the input in the text field (e.g., by pressing the Return key).
		/// </summary>
		private AcceptedTextDelegate acceptedText = null;

		/// <summary>
		/// If the text field starts with the accept key in the down position,
		/// we need to ignore the first accept key event. Otherwise it will
		/// immediately accept the input.
		/// </summary>
		private bool ignoreFirstAccept = false;
		
		/// <summary>
		/// If the text field starts with the cancel key in the down position,
		/// we need to ignore the first cancel key event. Otherwise it will
		/// immediately cancel the input.
		/// </summary>
		private bool ignoreFirstCancel = false;
		
		void Start() {
			Hide();
		}
		
		/// <summary>
		/// Starts the text input field.
		/// </summary>
		/// <param name="labelText">The label text.</param>
		/// <param name="text">The current value to use for the input field.</param>
		/// <param name="maxLength">Max length, or <c>0</c> for unlimited.</param>
		/// <param name="acceptedText">The delegate to call when accepting text.</param>
		public void StartTextInput(string labelText, string text, int maxLength, AcceptedTextDelegate acceptedText) {
			Show();
			if (label != null) TK2DDialogueTools.SetPlainText(label, text, label.color);
			if (textInput != null) {
				textInput.maxCharacterLength = (maxLength != 0) ? maxLength : 1024;
				textInput.emptyDisplayText = text;
				textInput.Text = text;
				textInput.SetFocus(true);
				ignoreFirstAccept = (acceptKey != KeyCode.None) && Input.GetKeyDown(acceptKey);
				ignoreFirstCancel = (cancelKey != KeyCode.None) && Input.GetKeyDown(cancelKey);
			}
			this.acceptedText = acceptedText;
		}

		public void Update() {
			if ((acceptKey != KeyCode.None) && Input.GetKeyDown(acceptKey)) {
				if (ignoreFirstAccept) {
					ignoreFirstAccept = false;
				} else {
					AcceptTextInput();
				}
			} else if ((cancelKey != KeyCode.None) && Input.GetKeyDown(cancelKey)) {
				if (ignoreFirstCancel) {
					ignoreFirstCancel = false;
				} else {
					CancelTextInput();
				}
			}
		}
		
		/// <summary>
		/// Cancels the text input field.
		/// </summary>
		public void CancelTextInput() {
			Hide();
		}
		
		/// <summary>
		/// Accepts the text input and calls the accept handler delegate.
		/// </summary>
		public void AcceptTextInput() {
			if (acceptedText != null) {
				if (textInput != null) acceptedText(textInput.Text);
				acceptedText = null;
			}
			Hide();
		}

		private void Show() {
			if (panel != null) TK2DDialogueTools.SetControlActive(panel, true);
			if (label != null) TK2DDialogueTools.SetControlActive(label, true);
			if (textInput != null) TK2DDialogueTools.SetControlActive(textInput, true);
		}

		private void Hide() {
			if (label != null) TK2DDialogueTools.SetControlActive(label, false);
			if (textInput != null) TK2DDialogueTools.SetControlActive(textInput, false);
			if (panel != null) TK2DDialogueTools.SetControlActive(panel, false);
		}

	}

}
