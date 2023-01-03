using UnityEngine;
using UnityEditor;
using RQ.Entity.Common;
using RQ.Entity.Components;

namespace RQ.Editor
{
    [CustomEditor(typeof(TimelineContainerLinkComponent), true)]
    public class TimelineContainerLinkComponentEditor : EditorBase
    {
        private TimelineContainerLinkComponent agent;
        //private SceneSetup _sceneSetup;

        public void OnEnable()
        {
            agent = target as TimelineContainerLinkComponent;
            //_sceneSetup = GameObject.FindObjectOfType<SceneSetup>();
        }

        //public void OnSceneGUI()
        //{
        //    var ev = Event.current;
        //}

        public override void OnInspectorGUI()
        {
            GUI.changed = false;
            //base.OnInspectorGUI();
            agent.Entity = EditorGUILayout.ObjectField("Target", agent.Entity, typeof(SpriteBaseComponent)) as SpriteBaseComponent;
            if (agent.Entity != null)
                agent.EntityUniqueId = agent.Entity.UniqueId;
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Unique Id", agent.EntityUniqueId);
            EditorGUI.EndDisabledGroup();
            //EditorGUILayout.ObjectField()

            //agent.LayerMask = EditorGUILayout.LayerField("Layer Mask", agent.LayerMask);

            //var spriteAnimations = agent.GetSpriteAnimations();

            ////if (agent.)

            //var clipNames = new List<KeyValuePair<string, string>>();

            //clipNames.Add(new KeyValuePair<string, string>("", "Select Animation..."));

            //var allClips = agent.GetAllClipNames();
            //if (allClips.Count() != 0)
            //{
            //    foreach (var clip in allClips)
            //    {
            //        clipNames.Add(new KeyValuePair<string, string>(clip, clip));
            //    }

            //    if (GUILayout.Button("Add Animation", GUILayout.Width(100)))
            //    {
            //        spriteAnimations.Add(new Animation.Common.SpriteAnimation());
            //    }

            //    int removeAnimationIndex = -1;

            //    for (int i = 0; i < spriteAnimations.Count(); i++)
            //    {
            //        var spriteAnimation = spriteAnimations[i];
            //        EditorGUILayout.BeginHorizontal();
            //        EditorGUILayout.LabelField("Animation", GUILayout.Width(75));

            //        spriteAnimation.type = (AnimationType)EditorGUILayout.EnumPopup(spriteAnimation.type, GUILayout.Width(75));
            //        //if (_sceneSetup != null && _sceneSetup.SceneConfig != null && _sceneSetup.SceneConfig.Variables != null)
            //        //{                
            //        // Get list of spawn points

            //        //}

            //        spriteAnimation.direction = (Direction)EditorGUILayout.EnumPopup(spriteAnimation.direction, GUILayout.Width(75));

            //        /*agent.Variable = */
            //        spriteAnimation.animationName = ControlExtensions.Popup(String.Empty, spriteAnimation.animationName, clipNames, GUILayout.Width(125));

            //        if (String.IsNullOrEmpty(spriteAnimation.animationName))
            //        {
            //            spriteAnimation.clipId = -1;
            //        }
            //        else
            //        {
            //            spriteAnimation.clipId = agent.GetClipIdByName(spriteAnimation.animationName);
            //        }


            //        if (GUILayout.Button("X", GUILayout.Width(20)))
            //        {
            //            if (EditorUtility.DisplayDialog("Remove animation?", "Remove animation " + spriteAnimation.animationName + " from this Area?", "Yes", "No"))
            //            {
            //                removeAnimationIndex = i;
            //            }
            //        }

            //        EditorGUILayout.EndHorizontal();
            //    }

            //    if (removeAnimationIndex != -1)
            //        spriteAnimations.RemoveAt(removeAnimationIndex);

            //}

            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField("Value", GUILayout.Width(75));
            //agent.Value = EditorGUILayout.TextField(agent.Value);
            //EditorGUILayout.EndHorizontal();

            if (GUI.changed)
            {
                Dirty();
            }
        }
    }
}
