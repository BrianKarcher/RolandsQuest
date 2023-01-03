using System;
using UnityEngine;
using UnityEditor;
using RQ.Messaging;

namespace RQ.Editor
{
    [CustomEditor(typeof(MessagingObject), true)]
    public class MessagingObjectEditor : BaseObjectEditor
    {
        private MessagingObject _smAgent;

        public override void OnEnable()
        {
            base.OnEnable();
            _smAgent = target as MessagingObject;

            //States = agent.GetComponentsInChildren<IState>().ToList();
            //agent.
            //agent.get
            //States = new List<IState>();
            //foreach (Transform trans in agent.transform)
            //{
            //    var state = trans.GetComponent<IState>();
            //    if (state != null)
            //        States.Add(state);
            //}

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

            //var spriteAnimations = agent.GetSpriteAnimations();

            //if (agent.)

            //var clipNames = new List<KeyValuePair<string, string>>();

            //clipNames.Add(new KeyValuePair<string, string>("", "Select Animation..."));

            //var allClips = agent.GetAllClipNames();
            //if (allClips.Count() != 0)
            //{
            //    foreach (var clip in allClips)
            //    {
            //        clipNames.Add(new KeyValuePair<string, string>(clip, clip));
            //    }

            //if (GUILayout.Button("Add Transition Record", GUILayout.Width(150)))
            //{
            //    agent.Records.Add(new StateTransitionRecord());
            //    //spriteAnimations.Add(new Animation.Common.SpriteAnimation());
            //}

            //if (agent.Records == null)
            //    agent.Records = new List<StateTransitionRecord>();

            //int removeTransitionIndex = -1;

            //for (int i = 0; i < agent.Records.Count(); i++)
            //{
                //var transitionRecord = agent.Records[i];
                //EditorGUILayout.BeginHorizontal();
                //EditorGUILayout.LabelField("Initial State", GUILayout.Width(80));

                //if (StateItems != null)
                //{
                    //string currentState = transitionRecord.CurrentState == null ? string.Empty : transitionRecord.CurrentState.name;
                    //string newCurrentState = ControlExtensions.Popup(currentState, StateItems, GUILayout.Width(100));
                    //if (String.IsNullOrEmpty(newCurrentState))
                    //    transitionRecord.CurrentState = null;
                    //else
                    //    transitionRecord.CurrentState = States.FirstOrDefault(j => j.name == newCurrentState) as StateBase; //StateItems.First(j => j.Key == newCurrentState);

                //_smAgent.InitialState = CreateStatePopup("Initial State", _smAgent.InitialState);

                //}
                //transitionRecord.CurrentState = EditorGUILayout.ObjectField(transitionRecord.CurrentState, typeof(StateBase), GUILayout.Width(120)) as StateBase;

                //EditorGUILayout.LabelField("to", GUILayout.Width(20));

                //transitionRecord.TargetState = CreateStatePopup(transitionRecord.TargetState);

                //string targetState = transitionRecord.TargetState == null ? string.Empty : transitionRecord.TargetState.name;
                //ControlExtensions.Popup(targetState, StateItems, GUILayout.Width(100));
                //transitionRecord.TargetState = EditorGUILayout.ObjectField(transitionRecord.TargetState, typeof(StateBase), GUILayout.Width(120)) as StateBase;
                //EditorGUILayout.EndHorizontal();

                //GUILayout.Toggle()
                //conditionSelected = GUILayout.Toggle(conditionSelected, "Conditions", EditorStyles.toolbarDropDown, GUILayout.ExpandWidth(true));

                //if (conditionSelected)
                //{
                //    EditorGUI.indentLevel++;

                //    EditorGUILayout.LabelField("Hello", GUILayout.Width(100));

                //    EditorGUI.indentLevel--;
                //}
                //spriteAnimation.type = (AnimationType)EditorGUILayout.EnumPopup(spriteAnimation.type, GUILayout.Width(75));
                //if (_sceneSetup != null && _sceneSetup.SceneConfig != null && _sceneSetup.SceneConfig.Variables != null)
                //{                
                // Get list of spawn points

                //}

                //spriteAnimation.direction = (Direction)EditorGUILayout.EnumPopup(spriteAnimation.direction, GUILayout.Width(75));

                /*agent.Variable = */
                //spriteAnimation.animationName = ControlExtensions.Popup(spriteAnimation.animationName, clipNames, GUILayout.Width(125));

                //if (String.IsNullOrEmpty(spriteAnimation.animationName))
                //{
                //    spriteAnimation.clipId = -1;
                //}
                //else
                //{
                //    spriteAnimation.clipId = agent.GetClipIdByName(spriteAnimation.animationName);
                //}


            if (GUILayout.Button("Create RQName2"))
            {
                _smAgent.SetRqName2(Guid.NewGuid().ToString());
                Dirty();
            }

                
            //}

            //if (removeAnimationIndex != -1)
            //    spriteAnimations.RemoveAt(removeAnimationIndex);

            //}

            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField("Value", GUILayout.Width(75));
            //agent.Value = EditorGUILayout.TextField(agent.Value);
            //EditorGUILayout.EndHorizontal();
        }
    }
}
