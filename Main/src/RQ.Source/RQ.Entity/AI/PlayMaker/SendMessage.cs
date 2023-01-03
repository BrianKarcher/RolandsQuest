using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using System.Collections.Generic;
using RQ.Common.Container;
using UnityEngine;
using RQ.Model.Interfaces;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Sends a message.")]
    public class SendMessage : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Where to send the message.")]
        public FsmArray Targets;

        [UIHint(UIHint.Variable)]
        public FsmString Target;

        [UIHint(UIHint.Variable)]
        public FsmGameObject TargetGO;

        [UIHint(UIHint.Tag)]
        [PM.Tooltip("Filter by Tag.")]
        public FsmString collideTag;

        public bool Log = false;

        public SendMessageAtom _sendMessageAtom;
        private IComponentRepository _entity;

        public override void Reset()
        {
            base.Reset();
            if (_sendMessageAtom != null)
            _sendMessageAtom._finishOnFirstMessageSent = true;
        }

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            if (Log)
            {
                Debug.LogError($"(SendMessage) OnEnter called for {_sendMessageAtom.Message} for {_entity.name}");
            }
            //_sendMessageAtom.SetDebugEvent(() =>
            //{
            //    Debug.LogError($"SendMessage VictoryPose called in FSM {base.Fsm.Name} State {base.Fsm.ActiveStateName}");
            //});
            if (!collideTag.IsNone)
                _sendMessageAtom.collideTag = collideTag.Value;

            //var receiverCount = 
            //if (Targets.stringValues != null)
            //    receivers.AddRange(Targets.stringValues);

            //_sendMessageAtom.TargetUniqueIds = receivers.ToArray();
            if (Targets.stringValues != null)
                _sendMessageAtom.TargetUniqueIds = Targets.stringValues;

            _sendMessageAtom.Start(_entity);
            if (!Target.IsNone)
                _sendMessageAtom.Process(Target.Value);

            if (!TargetGO.IsNone)
            {
                var go = TargetGO.Value;
                if (go != null)
                {
                    IComponentRepository repo;
                    repo = go.GetComponent<IComponentRepository>();
                    if (repo == null)
                    {
                        // Connect to any RQ Component so we can obtain the Component Repository - our core objective
                        IComponentBase collisionComponent = go.GetComponent<IComponentBase>();
                        if (collisionComponent != null)
                        {
                            repo = collisionComponent.GetComponentRepository();

                            //receivers.Add(repo.UniqueId);
                        }
                    }
                    if (repo != null)
                        _sendMessageAtom.Process(repo.UniqueId);
                }
            }

            if (_sendMessageAtom.IsFinished())
            {
                if (Log)
                {
                    Debug.LogError($"(SendMessage) OnStart_Finish called for {_sendMessageAtom.Message} for {_entity.name}");
                }
                Finish();
            }
        }

        public override void OnUpdate()
        {
            if (Log)
            {
                Debug.LogError($"(SendMessage) OnUpdate called for {_sendMessageAtom.Message} for {_entity.name}");
            }
            base.OnUpdate();
            _sendMessageAtom.OnUpdate();
            if (_sendMessageAtom.IsFinished())
                Finish();
        }

        public override void OnExit()
        {
            if (Log)
            {
                Debug.LogError($"(SendMessage) OnExit called for {_sendMessageAtom.Message} for {_entity.name}");
            }
            base.OnExit();
            _sendMessageAtom.End();
        }
    }
}
