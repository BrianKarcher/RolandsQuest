using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem.TK2D {

	/// <summary>
	/// TK2D UI root, implemented as a container for a tk2dUILayout.
	/// </summary>
	[System.Serializable]
	public class TK2DUIRoot : AbstractUIRoot {
		
		private tk2dUILayout UIRootLayout;
		
		public TK2DUIRoot(tk2dUILayout UIRootLayout) {
			this.UIRootLayout = UIRootLayout;
		}
		
		/// <summary>
		/// Shows the root.
		/// </summary>
		public override void Show() {
			if (UIRootLayout != null) UIRootLayout.gameObject.SetActive(true);
		}
		
		/// <summary>
		/// Hides the root.
		/// </summary>
		public override void Hide() {
			if (UIRootLayout != null) UIRootLayout.gameObject.SetActive(false);
		}
		
	}

}
