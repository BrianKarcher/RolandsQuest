using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MissingReferencesFinder : MonoBehaviour
{
    [MenuItem("Tools/Show Missing Object References in scene", false, 50)]
    public static void FindMissingReferencesInCurrentScene()
    {
        var objects = GetSceneObjects();
        FindMissingReferences(EditorApplication.currentScene, objects);
    }

    [MenuItem("Tools/Show Missing Object References in all scenes", false, 51)]
    public static void MissingSpritesInAllScenes()
    {
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled)
                continue;
            EditorApplication.OpenScene(scene.path);
            FindMissingReferences(scene.path, GetSceneObjects());
        }
    }

    [MenuItem("Tools/Show Missing Object References in assets", false, 52)]
    public static void MissingSpritesInAssets()
    {
        var allAssets = AssetDatabase.GetAllAssetPaths();

        
        List<GameObject> gameObjects = new List<GameObject>();
        foreach (var asset in allAssets)
        {
            var locatedAsset = AssetDatabase.LoadAssetAtPath(asset, typeof(GameObject)) as GameObject;
            if (locatedAsset == null)
                continue;
            gameObjects.Add(locatedAsset);
        }
        //var objs = allAssets.Select(a => .Where(a => a != null).ToArray();
        FindMissingReferences("Project", gameObjects);
    }

    private static void FindMissingReferences(string context, List<GameObject> objects)
    {
        foreach (var go in objects)
        {
            //List<Component> components;
            var components = go.GetComponents<Component>();

            foreach (var c in components)
            {
                if (!c)
                {
                    Debug.LogError("Missing Component in GO: " + FullPath(go), go);
                    continue;
                }

                SerializedObject so = new SerializedObject(c);
                var sp = so.GetIterator();

                while (sp.NextVisible(true))
                {
                    if (sp.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if (sp.objectReferenceValue == null
                            && sp.objectReferenceInstanceIDValue != 0)
                        {
                            ShowError(context, go, c.GetType().Name, ObjectNames.NicifyVariableName(sp.name));
                        }
                    }
                }
            }
        }
    }

    private static List<GameObject> GetSceneObjects()
    {
        var resources = Resources.FindObjectsOfTypeAll<GameObject>();
        List<GameObject> gameObjects = new List<GameObject>();
        foreach (var resource in resources)
        {
            if (string.IsNullOrEmpty(AssetDatabase.GetAssetPath(resource))
                       && resource.hideFlags == HideFlags.None)
                gameObjects.Add(resource);
        }

        //return Resources.FindObjectsOfTypeAll<GameObject>()
        //    .Where(go => string.IsNullOrEmpty(AssetDatabase.GetAssetPath(go))
        //           && go.hideFlags == HideFlags.None).ToArray();
        return gameObjects;
    }

    private const string err = "Missing Ref in: [{3}]{0}. Component: {1}, Property: {2}";

    private static void ShowError(string context, GameObject go, string c, string property)
    {
        Debug.LogError(string.Format(err, FullPath(go), c, property, context), go);
    }

    private static string FullPath(GameObject go)
    {       
        return go.transform.parent == null
            ? go.name
                : FullPath(go.transform.parent.gameObject) + "/" + go.name;
    }
}