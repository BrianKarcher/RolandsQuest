using RQ.Animation.V2;
using RQ.Common;
using RQ.Editor.UnityExtensions;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RQ.Editor.PropertyDrawer
{
    // Place this file inside Assets/Editor
    [CustomPropertyDrawer(typeof(AnimationTypeAttribute))]
    public class AnimationTypeDrawer : UnityEditor.PropertyDrawer
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
            var animator = prop.serializedObject.targetObject as IAnimatedObject;
            //var entity = behaviour.GetComponent<IBaseGameEntity>();
            //if (entity == null)
            //    return;

            //var animator = entity.GetSpriteAnimator2();

            if (animator == null)
            {
                //Debug.Log("Could not locate Animator (as IAnimatedObject)");
                return;
            }
            var animationComponent =  animator.GetAnimationComponent();
            var spriteRenderer = animationComponent.GetSpriteRenderer();
            //var spriteRenderer = animator.GetSpriteRenderer();
            if (spriteRenderer == null)
            {
                //Debug.Log("Could not locate Sprite Renderer");
                return;
            }

            var animations = spriteRenderer.GetStoredSpriteAnimations();
            if (animations == null)
            {
                //Debug.Log("Could not locate Stored Sprite Animations");
                return;
            }

            var animationTypes = new List<KeyValuePair<string,string>>() {new KeyValuePair<string,string>("", "Select animation...")};
            foreach (var animation in animations)
            {
                animationTypes.Add(new KeyValuePair<string, string>(animation.ID, animation.Type));
            }
            //animationTypes.AddRange(animations.Select(i => new KeyValuePair<string,string>(i.ID, i.Type)).ToList());

            prop.stringValue = ControlExtensions.EditorGUIPopup(position, label.text, prop.stringValue, animationTypes);

            EditorGUILayout.LabelField("Animator Id", prop.stringValue);
            //EditorGUI.Popup(position, )

            //EditorGUILayout.LabelField(position, label, new GUIContent(prop.stringValue));
        }
    }
}
