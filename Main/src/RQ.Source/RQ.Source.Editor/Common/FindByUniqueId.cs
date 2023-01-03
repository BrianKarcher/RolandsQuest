using RQ.Common;
using UnityEditor;
using UnityEngine;

public class FindByUniqueId : EditorWindow
{
    private string _uniqueId;

    [MenuItem("Tools/Find", false, 50)]
    public static void Find()
    {
        EditorWindow.GetWindow(typeof(FindByUniqueId));
    }

    void OnGUI()
    {
        _uniqueId = EditorGUILayout.TextField("Unique Id", _uniqueId);
        if (GUILayout.Button("Find"))
        {
            var baseObjects = Resources.FindObjectsOfTypeAll<BaseObject>();
            //var baseObjects = GameObject.FindObjectsOfType<BaseObject>();
            BaseObject baseObject = null;
            foreach (var tempBaseObject in baseObjects)
            {
                if (tempBaseObject.UniqueId == _uniqueId)
                {
                    baseObject = tempBaseObject;
                    break;
                }
            }
            if (baseObject == null)
                return;
            //var baseObject = baseObjects.FirstOrDefault(i => i.UniqueId == _uniqueId);

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = baseObject;
            
        }
    }
}