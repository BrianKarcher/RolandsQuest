using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RQ.Editor.UnityExtensions;
using RQ.Controller.Actions.Conditionals;

namespace RQ.Editor
{
    [CustomEditor(typeof(SetSceneVariable), true)]
    public class SetSceneVariableEditor : EditorBase
    {
        private SetSceneVariable agent;
        private SceneSetup _sceneSetup;

        public void OnEnable()
        {
            agent = target as SetSceneVariable;
            _sceneSetup = GameObject.FindObjectOfType<SceneSetup>();
        }

        //public void OnSceneGUI()
        //{
        //    var ev = Event.current;
        //}

        public override void OnInspectorGUI()
        {
            GUI.changed = false;
            base.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField("Variable", GUILayout.Width(75));

            var SceneVariables = new List<KeyValuePair<string, string>>();

            SceneVariables.Add(new KeyValuePair<string, string>("", "Select Scene Variable..."));
            if (_sceneSetup != null && _sceneSetup.SceneConfig != null && _sceneSetup.SceneConfig.Variables != null)
            {                
                // Get list of spawn points
                foreach (var variable in _sceneSetup.SceneConfig.Variables)
                {
                    SceneVariables.Add(new KeyValuePair<string, string>(variable.UniqueId, variable.Name));
                }
            }

            agent.Variable = ControlExtensions.Popup("Variable", agent.Variable, SceneVariables, GUILayout.Width(125));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Value", GUILayout.Width(75));
            agent.Value = EditorGUILayout.TextField(agent.Value);
            EditorGUILayout.EndHorizontal();

            if (GUI.changed)
            {
                Dirty();
            }
        }
    }
}
