using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem.TK2D {
	
	/// <summary>
	/// Contains all dialogue (conversation) controls for a TK2D Dialogue UI.
	/// </summary>
	[System.Serializable]
	public class TK2DDialogueControls : AbstractDialogueUIControls {
		
		/// <summary>
		/// The alert panel. A panel is optional, but you may want one
		/// so you can include a background image, panel-wide effects, etc.
		/// </summary>
		public tk2dUILayout panel;
		
		/// <summary>
		/// The NPC subtitle controls.
		/// </summary>
		public TK2DSubtitleControls npcSubtitle;
		
		/// <summary>
		/// The PC subtitle controls.
		/// </summary>
		public TK2DSubtitleControls pcSubtitle;
		
		/// <summary>
		/// The response menu controls.
		/// </summary>
		public TK2DResponseMenuControls responseMenu;
		
		public override AbstractUISubtitleControls NPCSubtitle { 
			get { return npcSubtitle; }
		}
		
		public override AbstractUISubtitleControls PCSubtitle {
			get { return pcSubtitle; }
		}
		
		public override AbstractUIResponseMenuControls ResponseMenu {
			get { return responseMenu; }
		}
		
		public override void ShowPanel() {
			TK2DDialogueTools.SetControlActive(panel, true);
		}
		
		public override void SetActive (bool value) {
			base.SetActive(value);
			TK2DDialogueTools.SetControlActive(panel, value);
		}
		
		/// <summary>
		/// Sets the sprite collection to use for portrait images.
		/// </summary>
		/// <param name='portraitCollection'>
		/// Portrait collection.
		/// </param>
		public void SetPortraitCollection(tk2dSpriteCollectionData portraitCollection) {
			if (npcSubtitle != null) npcSubtitle.SetPortraitCollection(portraitCollection);
			if (pcSubtitle != null) pcSubtitle.SetPortraitCollection(portraitCollection);
		}
		
	}
		
}
