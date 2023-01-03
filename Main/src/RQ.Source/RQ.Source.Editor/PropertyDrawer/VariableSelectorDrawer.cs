using RQ.Common;
using RQ.Editor.UnityExtensions;
using RQ.Model;
using RQ.Model.Enums;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RQ.Editor.PropertyDrawer
{
    // Place this file inside Assets/Editor
    [CustomPropertyDrawer(typeof(VariableSelectorAttribute))]
    public class VariableSelectorDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            // Generate a unique ID, defaults to an empty string if nothing has been serialized yet
            //if (prop.stringValue == "")
            //{
            //    Guid guid = Guid.NewGuid();
            //    prop.stringValue = guid.ToString();
            //}

            // Place a label so it can't be edited by accident
            Rect textFieldPosition = position;
            textFieldPosition.height = 16;
            DrawLabelField(textFieldPosition, prop, label);
        }

        void DrawLabelField(Rect position, SerializedProperty prop, GUIContent label)
        {
            var attr = this.attribute as VariableSelectorAttribute;
            var variableTypeProp = prop.serializedObject.FindProperty(attr.VariableTypeField);
            //var animator = prop.serializedObject.targetObject as IAnimatedObject;
            //var entity = behaviour.GetComponent<IBaseGameEntity>();
            //if (entity == null)
            //    return;

            //var animator = entity.GetSpriteAnimator2();

            //var gameController = GameObject.FindObjectOfType<GameController>();
            var sceneSetup = GameObject.FindObjectOfType<SceneSetup>();
            List<Variable> variables;
            if (variableTypeProp == null)
            {
                variables = sceneSetup.GameConfig.Variables;
            }
            else
            {
                switch ((VariableType)variableTypeProp.enumValueIndex)
                {
                    case VariableType.Scene:
                        //var sceneSetup = GameObject.FindObjectOfType<SceneSetup>();
                        variables = sceneSetup.SceneConfig.Variables;
                        break;
                    default:
                        variables = sceneSetup.GameConfig.Variables;
                        break;
                }
            }

            var variableList = new List<KeyValuePair<string,string>>() {new KeyValuePair<string,string>("", "Select variable...")};
            foreach (var variable in variables)
            {
                variableList.Add(new KeyValuePair<string, string>(variable.UniqueId, variable.Name));
            }
            //variableList.AddRange(variables.Select(i => new KeyValuePair<string, string>(i.UniqueId, i.Name)).ToList());

            //string currentValue = prop.stringValue == null ? Guid.Empty : new Guid(prop.stringValue);

            prop.stringValue = ControlExtensions.EditorGUIPopup(position, label.text, prop.stringValue, variableList);

            //EditorGUI.Popup(position, )

            //EditorGUILayout.LabelField(position, label, new GUIContent(prop.stringValue));
        }
    }
}
