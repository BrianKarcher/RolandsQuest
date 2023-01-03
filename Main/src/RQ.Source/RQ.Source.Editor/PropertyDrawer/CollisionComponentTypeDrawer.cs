using RQ.Common;
using RQ.Editor.UnityExtensions;
using RQ.Model.Interfaces;
using RQ.Physics.Components;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RQ.Editor.PropertyDrawer
{
    // Place this file inside Assets/Editor
    [CustomPropertyDrawer(typeof(CollisionComponentTypeAttribute))]
    public class CollisionComponentTypeDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            // Generate a unique ID, defaults to an empty string if nothing has been serialized yet
            //if (prop.stringValue == "")
            //{
            //    Guid guid = Guid.NewGuid();
            //    prop.stringValue = guid.ToString();
            //}

            //prop.

            // Place a label so it can't be edited by accident
            Rect textFieldPosition = position;
            textFieldPosition.height = 16;

            if (prop.isArray)
            {
                for (int i = 0; i < prop.arraySize; i++)
                    DrawPopupField(textFieldPosition, prop.GetArrayElementAtIndex(i), label);
            }
            else
            {
                DrawPopupField(textFieldPosition, prop, label);
            }            
        }

        void DrawPopupField(Rect position, SerializedProperty prop, GUIContent label)
        {
            var collisionModifier = prop.serializedObject.targetObject as ICollisionModifier;

            if (collisionModifier == null)
            {
                Debug.Log("Could not locate Collision Modifier");
                return;
            }

            var collisionComponents = collisionModifier.GetCollisionComponents();
            if (collisionComponents == null)
            {
                Debug.Log("Could not locate Collision Components");
                return;
            }

            var animationTypes = new List<KeyValuePair<CollisionComponent, string>>() { new KeyValuePair<CollisionComponent, string>(null, "Select Collision Component...") };

            foreach (var collisionComponent in collisionComponents)
            {
                animationTypes.Add(new KeyValuePair<CollisionComponent, string>(collisionComponent as CollisionComponent, collisionComponent.GetName()));
            }

            //animationTypes.AddRange(collisionComponents.Select(i => new KeyValuePair<CollisionComponent, string>(i as CollisionComponent, i.GetName())).ToList());

            prop.objectReferenceValue = ControlExtensions.EditorGUIPopup<CollisionComponent>(position, label.text, prop.objectReferenceValue as CollisionComponent, animationTypes);
            //prop.stringValue = ControlExtensions.EditorGUIPopup(position, label.text, prop.stringValue, animationTypes);

            //EditorGUI.Popup(position, )

            //EditorGUILayout.LabelField(position, label, new GUIContent(prop.stringValue));
        }
    }
}
