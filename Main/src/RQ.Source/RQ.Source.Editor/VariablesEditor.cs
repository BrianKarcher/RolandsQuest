using RQ.Model;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RQ.Editor
{
    public class VariablesEditor
    {
        private bool _showVariables = false;
        private EditorBase _editorBase;

        public VariablesEditor(EditorBase editorBase)
        {
            _editorBase = editorBase;
        }

        public void OnInspectorGUI(List<Variable> variables)
        {
            _showVariables = GUILayout.Toggle(_showVariables, "Variables", "Button");

            if (_showVariables)
            {
                if (GUILayout.Button("Create Variable", GUILayout.Width(120)))
                {
                    CreateVariable(variables);
                    //moveUp = layer;
                    _editorBase.Repaint();
                }
                int deleteVariable = -1;
                for (int i = 0; i < variables.Count; i++)
                {
                    var variable = variables[i];
                    GUILayout.Space(7);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Variable", GUILayout.Width(175));
                    
                    if (GUILayout.Button("D", GUILayout.Width(30)))
                    {
                        if (EditorUtility.DisplayDialog("Delete Variable?", "Are you sure you want to delete the variable " + variable.Name + "?", "Yes", "No"))
                        {
                            deleteVariable = i;
                        }
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Name", GUILayout.Width(50));
                    variable.Name = EditorGUILayout.TextField(variable.Name ?? string.Empty, GUILayout.Width(80));
                    GUILayout.Label("Value", GUILayout.Width(50));
                    variable.Value = EditorGUILayout.TextField(variable.Value ?? string.Empty, GUILayout.Width(80));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    //GUILayout.Label("Persistence", GUILayout.Width(75));
                    //variable.Persistence = (StatusPersistenceLength)EditorGUILayout.EnumPopup(variable.Persistence, GUILayout.Width(80));
                    GUILayout.EndHorizontal();
                }
                if (deleteVariable != -1)
                {
                    DeleteVariable(variables, deleteVariable);

                    _editorBase.Repaint();
                }
            }
        }

        private void DeleteVariable(List<Variable> variables, int index)
        {
            variables.RemoveAt(index);
            _editorBase.Dirty();
        }

        private void CreateVariable(List<Variable> variables)
        {
            variables.Add(new Variable() { UniqueId = Guid.NewGuid().ToString() });
            _editorBase.Dirty();
        }
    }
}
