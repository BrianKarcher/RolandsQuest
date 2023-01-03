using System;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WellFired.Shared
{
	public partial class UnityEditorHelper : IUnityEditorHelper
	{
		#if UNITY_EDITOR
		public void AddUpdateListener(Action listener)
		{
			listeners += listener;

			// We always remove and the re add it, so we can ensure we never add it twice.
			EditorApplication.update -= Update;
			EditorApplication.update += Update;
		}
		
		public void RemoveUpdateListener(Action listener)
		{
			listeners -= listener;
		}

		private Action listeners = delegate { };

		private void Update()
		{
			listeners();
		}
		#endif

		public bool IsPrefab(GameObject testObject)
		{
#if UNITY_EDITOR
			return PrefabUtility.GetPrefabParent(testObject) == null && PrefabUtility.GetPrefabObject(testObject) != null;
#endif
			return false;
		}
	}
}