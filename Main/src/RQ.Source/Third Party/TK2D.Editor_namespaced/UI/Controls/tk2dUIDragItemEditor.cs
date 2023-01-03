using UnityEngine;
using UnityEditor;
using System.Collections;
using tk2d;

namespace tk2dEditor2
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(tk2dUIDragItem))]
    public class tk2dUIDragItemEditor : tk2dUIBaseItemControlEditor
    {
        protected bool hasUIManagerCheckBeenDone = false;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            tk2dUIDragItem dragButton = (tk2dUIDragItem)target;

            if (GUI.changed)
            {
                EditorUtility.SetDirty(dragButton);
            }
        }

    }
}
