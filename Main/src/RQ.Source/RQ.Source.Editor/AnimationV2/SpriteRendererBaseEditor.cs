using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RQ.Editor.UnityExtensions;
using RQ.AnimationV2;
using RQ.Editor.GUIStyles;

namespace RQ.Editor
{
    [CustomEditor(typeof(SpriteRendererBase2), true)]
    public class SpriteRendererBaseV2Editor : EditorBase
    {
        private SpriteRendererBase2 agent;
        //private SceneSetup _sceneSetup;

        public void OnEnable()
        {
            agent = target as SpriteRendererBase2;
            //_sceneSetup = GameObject.FindObjectOfType<SceneSetup>();
        }

        //public void OnSceneGUI()
        //{
        //    var ev = Event.current;
        //}

        public override void OnInspectorGUI()
        {
            GUI.changed = false;
            base.OnInspectorGUI();

            var spriteAnimationTypes = agent.GetThisStoredSpriteAnimations();
            //if (spriteAnimationTypes != null)

            //if (agent.)

            var clipNames = new List<KeyValuePair<string, string>>();

            clipNames.Add(new KeyValuePair<string, string>("", "Select Animation..."));

            var allClips = agent.GetAllClipNames();
            if (allClips.Length != 0)
            {
                foreach (var clip in allClips)
                {
                    clipNames.Add(new KeyValuePair<string, string>(clip, clip));
                }

                if (GUILayout.Button("Add Animation Type", GUILayout.Width(130)))
                {
                    //spriteAnimations.Add(new Animation.Common.SpriteAnimation());
                    var newType = new SpriteAnimationType(); //KeyValuePair<string, List<SpriteAnimation>>(string.Empty, new List<SpriteAnimation>());
                    newType.ID = Guid.NewGuid().ToString();
                    newType.SpriteAnimations = new List<SpriteAnimation>();
                    spriteAnimationTypes.Add(newType);
                }

                int removeAnimationTypeIndex = -1;

                for (int i = 0; i < spriteAnimationTypes.Count; i++)
                {
                    GUILayout.Space(4.0f);
                    GUILayout.BeginVertical(Styles.CreateInspectorStyle());

                    var spriteAnimationType = spriteAnimationTypes[i];
                    EditorGUILayout.BeginHorizontal();
                    //EditorGUILayout.LabelField("Animation", GUILayout.Width(75));
                    //spriteAnimation.Key = 
                    //var newKey = EditorGUILayout.TextField("Type", spriteAnimationType.Key);
                    spriteAnimationType.Type = EditorGUILayout.TextField("Type", spriteAnimationType.Type);

                    if (GUILayout.Button("", Styles.CreateTilemapDeleteItemStyle()))
                    {
                        if (EditorUtility.DisplayDialog("Remove animation type?", "Remove animation type " + spriteAnimationType.Type + "?", "Yes", "No"))
                        {
                            removeAnimationTypeIndex = i;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.LabelField("ID", spriteAnimationType.ID);

                    if (GUILayout.Button("Add Animation", GUILayout.Width(130)))
                    {
                        //spriteAnimations.Add(new Animation.Common.SpriteAnimation());
                        var newAnimation = new SpriteAnimation(); //KeyValuePair<string, List<SpriteAnimation>>(string.Empty, new List<SpriteAnimation>());
                        spriteAnimationType.SpriteAnimations.Add(newAnimation);
                    }

                    var removeAnimationIndex = -1;

                    for (int k = 0; k < spriteAnimationType.SpriteAnimations.Count; k++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        var spriteAnimation = spriteAnimationType.SpriteAnimations[k];

                        //spriteAnimation.type = (AnimationType)EditorGUILayout.EnumPopup(spriteAnimation.type, GUILayout.Width(75));
                        //if (_sceneSetup != null && _sceneSetup.SceneConfig != null && _sceneSetup.SceneConfig.Variables != null)
                        //{                
                        // Get list of spawn points

                        //}

                        spriteAnimation.Direction = (Direction)EditorGUILayout.EnumPopup(spriteAnimation.Direction, GUILayout.Width(75));

                        /*agent.Variable = */
                        spriteAnimation.AnimationName = ControlExtensions.Popup(String.Empty, spriteAnimation.AnimationName, clipNames, GUILayout.Width(200));

                        if (String.IsNullOrEmpty(spriteAnimation.AnimationName))
                        {
                            spriteAnimation.ClipId = -1;
                        }
                        else
                        {
                            spriteAnimation.ClipId = agent.GetClipIdByName(spriteAnimation.AnimationName);
                        }
                        if (GUILayout.Button("", Styles.CreateTilemapDeleteItemStyle()))
                        {
                            if (EditorUtility.DisplayDialog("Remove animation?", "Remove animation " + spriteAnimation.AnimationName + "?", "Yes", "No"))
                            {
                                removeAnimationIndex = k;
                            }
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                    if (removeAnimationIndex != -1)
                        spriteAnimationType.SpriteAnimations.RemoveAt(removeAnimationIndex);

                    //if (newKey != spriteAnimationType.Type)
                    //{
                    //    // Need to create a new KVP to rekey for the type change
                    //    spriteAnimationTypes[i] = new SpriteAnimationType()
                    //    {
                    //        Type = newKey,
                    //        SpriteAnimations = spriteAnimationType.SpriteAnimations
                    //    };
                    //}




                    GUILayout.EndVertical();
                }

                if (removeAnimationTypeIndex != -1)
                    spriteAnimationTypes.RemoveAt(removeAnimationTypeIndex);

            }

            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField("Value", GUILayout.Width(75));
            //agent.Value = EditorGUILayout.TextField(agent.Value);
            //EditorGUILayout.EndHorizontal();

            if (GUI.changed)
            {
                Dirty();
            }
        }

        //public GUIStyle GetStyle(string name)
        //{
        //    return tk2dExternal.Skin.Inst.GetStyle(name);
        //}
    }
}
