using RQ.Controller.Actions;
using RQ.Editor.UnityExtensions;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RQ.Editor
{
    [CustomEditor(typeof(ChangeScenes), true)]
    public class ChangeScenesEditor : BaseObjectEditor<ChangeScenes>
    {

        //public void OnSceneGUI()
        //{
        //    var ev = Event.current;
        //}

        public override void OnInspectorGUI()
        {
            GUI.changed = false;
            base.OnInspectorGUI();

            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField("Spawnpoint", GUILayout.Width(75));
            //var SpawnPoints = new List<KeyValuePair<string, string>>();
            //int spawnPointIndex = 0;

            //if (agent.sceneConfig != null)
            //{
            //    SpawnPoints.Add(new KeyValuePair<string, string>("-1", "Select Spawn Point..."));
            //    int index = 1;
            //    // Get list of spawn points
            //    foreach (var spawnPoint in agent.sceneConfig.SpawnPoints.GetPoints())
            //    {
            //        if (spawnPoint.UniqueId == agent.spawnPointUniqueId)
            //        {
            //            spawnPointIndex = index;
            //        }
            //        SpawnPoints.Add(new KeyValuePair<string, string>(spawnPoint.UniqueId, spawnPoint.Name));
            //        index++;
            //    }
            //}

            //spawnPointIndex = EditorGUILayout.Popup(spawnPointIndex, SpawnPoints.Select(i => i.Value).ToArray(), GUILayout.Width(125));

            //var key = SpawnPoints[spawnPointIndex].Key;
            //string uniqueId = key == "-1" ? null : key;

            //agent.spawnPointUniqueId = uniqueId;


            var SpawnPoints = new List<KeyValuePair<string, string>>();
            SpawnPoints.Add(new KeyValuePair<string, string>("-1", "Select Spawn Point..."));

            if (agent.sceneConfig != null)
            {                
                // Get list of spawn points
                foreach (var spawnPoint in agent.sceneConfig.SpawnPoints)
                {
                    SpawnPoints.Add(new KeyValuePair<string, string>(spawnPoint.UniqueId, spawnPoint.Name));
                }
            }

            //spawnPointIndex = EditorGUILayout.Popup(spawnPointIndex, SpawnPoints.Select(i => i.Value).ToArray(), GUILayout.Width(125));

            //var key = SpawnPoints[spawnPointIndex].Key;
            //string uniqueId = key == "-1" ? null : key;

            //agent.spawnPointUniqueId = uniqueId;

            agent.spawnPointUniqueId = ControlExtensions.Popup("Spawnpoint", agent.spawnPointUniqueId, SpawnPoints);

            //EditorGUILayout.EndHorizontal();

            //agent.Scene = EditorGUILayout.ObjectField(agent.Scene, typeof(UnityEngine.Object));

            //_variablesEditor.OnInspectorGUI(agent.Variables);

            //_showScenes = GUILayout.Toggle(_showScenes, "Scenes", "Button");

            //if (_showScenes)
            //{

            //}

            if (GUI.changed)
            {
                Dirty();
            }
        }
    }
}
