using UnityEngine;
using System;
using System.Collections;

namespace PixelCrushers.DialogueSystem.TK2D {
	
	/// <summary>
	/// A TK2D response button for TK2DDialogueUI. Add this component to every response button in 
	/// the dialogue UI. The button should have, at minimum, a tk2dUIItem. If you also assign a
	/// tk2dTextMesh to the label, it will contain the response text. This script adds
	/// Dialogue System-specific functionality to the tk2dUIItem button.
	/// </summary>
	[AddComponentMenu("Dialogue System/Third Party/2D Toolkit/Response Button")]
	public class TK2DResponseButton : MonoBehaviour {

		/// <summary>
		/// The text mesh that will display the response text.
		/// </summary>
		public tk2dTextMesh label;
		
		/// <summary>
		/// The default color for response text, which can be overridden by emphasis tags.
		/// </summary>
		public Color defaultColor = Color.white;
		
		/// <summary>
		/// Gets or sets the response text.
		/// </summary>
		/// <value>
		/// The text.
		/// </value>
		public string Text {
			get { 
				return (label != null) ? label.text : string.Empty; 
			}
			set { 
				if (label != null) {
					label.text = value; 
				} else {
					if (DialogueDebug.LogErrors) Debug.LogError(string.Format("{0}: No tk2dTextMesh is assigned on {1}", DialogueDebug.Prefix, name));
				}
			}
		}
		
		/// <summary>
		/// Indicates whether the button is shown or not.
		/// </summary>
		/// <value>
		/// <c>true</c> if visible; otherwise, <c>false</c>.
		/// </value>
		public bool IsVisible { get; set; }
		
		/// <summary>
		/// Indicates whether the button is clickable or not. 
		/// </summary>
		/// <value>
		/// <c>true</c> if clickable; otherwise, <c>false</c>.
		/// </value>
		public bool IsClickable { get; set; }
		
		/// <summary>
		/// Gets or sets the response associated with this button. If the player clicks this 
		/// button, this response is sent back to the dialogue system.
		/// </summary>
		/// <value>
		/// The response.
		/// </value>
		public Response Response { get; set; }
		
		/// <summary>
		/// Gets or sets the target that will receive click notifications.
		/// </summary>
		/// <value>
		/// The target.
		/// </value>
		public Transform Target { get; set; }
		
		/// <summary>
		/// The main button object.
		/// </summary>
		private tk2dUIItem button = null;
		
		void Awake() {
			if (button == null) button = GetComponent<tk2dUIItem>() ?? GetComponentInChildren<tk2dUIItem>();
			if (label == null) label = GetComponentInChildren<tk2dTextMesh>();
		}
		
		void Start() {
			button.OnClick += OnClick;
		}

		/// <summary>
		/// When enabled, enable or disable the collider depending on whether the button is clickable.
		/// </summary>
		void OnEnable() {
            if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = IsClickable;
		}
		
		/// <summary>
		/// Sets the button's text using the specified formatted text.
		/// </summary>
		/// <param name='formattedText'>
		/// The formatted text for the button label.
		/// </param>
		public void SetFormattedText(FormattedText formattedText) {
			if (label != null) TK2DDialogueTools.SetFormattedText(label, formattedText, defaultColor);
		}
		
		/// <summary>
		/// Sets the button's text using plain text.
		/// </summary>
		/// <param name='unformattedText'>
		/// Unformatted text for the button label.
		/// </param>
		public void SetUnformattedText(string unformattedText) {
			Text = unformattedText;
			SetColor(defaultColor);
		}
		
		private void SetColor(Color currentColor) {
			if (label != null) {
				label.color = currentColor;
			} else {
				if (DialogueDebug.LogErrors) Debug.LogError(string.Format("{0}: No tk2dTextMesh is assigned to {1}", DialogueDebug.Prefix, name));
			}
		}
	
		/// <summary>
		/// Handles a button click by calling the response handler.
		/// </summary>
		public void OnClick() {
			if (Target != null) Target.SendMessage("OnClick", Response, SendMessageOptions.RequireReceiver);
		}
		
	}

}