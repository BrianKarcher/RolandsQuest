using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem.TK2D {
	
	/// <summary>
	/// This component implements IDialogueUI using 2D Toolkit. It's based on AbstractDialogueUI
	/// and compiles the TK2D versions of the controls defined in TK2DSubtitleControls, 
	/// TK2DResponseMenuControls, TK2DAlertControls, etc.
	///
	/// To use this component, build a 2D Toolkit UI layout (or use a pre-built one in the Prefabs folder)
	/// and assign the UI control properties. You can save a TK2DDialogueUI as a prefab and 
	/// assign the prefab or an instance to the DialogueManager.
	/// 
	/// If you use portrait images, create a sprite collection and assign it to this component.
	/// </summary>
	[AddComponentMenu("Dialogue System/Third Party/2D Toolkit/Dialogue UI")]
	public class TK2DDialogueUI : AbstractDialogueUI {
		
		/// <summary>
		/// The GUI root.
		/// </summary>
		public tk2dUILayout UIRootLayout;
		
		/// <summary>
		/// The sprite collection for portrait image sprites.
		/// </summary>
		public tk2dSpriteCollectionData portraitCollection;
		
		/// <summary>
		/// The dialogue controls.
		/// </summary>
		public TK2DDialogueControls dialogue;
		
		/// <summary>
		/// The QTE (Quick Time Event) indicators.
		/// </summary>
		public tk2dBaseSprite[] qteIndicators;
		
		/// <summary>
		/// The alert message controls.
		/// </summary>
		public TK2DAlertControls alert;
		
		private TK2DUIRoot tk2dUIRoot;
		
		private TK2DQTEControls tk2dQTEControls;
		
		public override AbstractUIRoot UIRoot {
			get { return tk2dUIRoot; }
		}

		public override AbstractDialogueUIControls Dialogue {
			get { return dialogue; }
		}
		
		public override AbstractUIQTEControls QTEs {
			get { return tk2dQTEControls; }
		}
		
		public override AbstractUIAlertControls Alert {
			get { return alert; }
		}
		
		/// <summary>
		/// Sets up the component.
		/// </summary>
		public override void Awake() {
			base.Awake();
			FindControls();
		}
		
		/// <summary>
		/// Makes sure we have a root layout and logs warnings if any critical controls are unassigned.
		/// </summary>
		private void FindControls() {
			if (UIRootLayout == null) UIRootLayout = GetComponentInChildren<tk2dUILayout>();
			tk2dUIRoot = new TK2DUIRoot(UIRootLayout);
			tk2dQTEControls = new TK2DQTEControls(qteIndicators);
			if (DialogueDebug.LogErrors) {
				if (UIRootLayout == null) Debug.LogError(string.Format("{0}: TK2DDialogueUI can't find UI Root Layout and won't be able to display dialogue.", DialogueDebug.Prefix));
				if (DialogueDebug.LogWarnings) {
					if (dialogue.npcSubtitle.line == null) Debug.LogWarning(string.Format("{0}: TK2DDialogueUI NPC Subtitle Line needs to be assigned.", DialogueDebug.Prefix));
					if (dialogue.pcSubtitle.line == null) Debug.LogWarning(string.Format("{0}: TK2DDialogueUI PC Subtitle Line needs to be assigned.", DialogueDebug.Prefix));
					if (dialogue.responseMenu.buttons.Length == 0) Debug.LogWarning(string.Format("{0}: TK2DDialogueUI Response buttons need to be assigned.", DialogueDebug.Prefix));
					if (alert.line == null) Debug.LogWarning(string.Format("{0}: TK2DDialogueUI Alert Line needs to be assigned.", DialogueDebug.Prefix));
				}
			}
		}
		
		public override void Start() {
			base.Start();
			if (dialogue != null) dialogue.SetPortraitCollection(portraitCollection);
		}
		
	}

}
