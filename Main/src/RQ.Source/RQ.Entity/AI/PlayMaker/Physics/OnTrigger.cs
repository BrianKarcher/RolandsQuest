using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using System.Collections.Generic;
using RQ.Common.Container;
using UnityEngine;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Detects a collision event.")]
    public class OnTrigger : FsmStateAction
    {
        //[UIHint(UIHint.Variable)]
        //[PM.Tooltip("Where to send the message.")]
        //public FsmArray Targets;

        //[UIHint(UIHint.Variable)]
        //public FsmString Target;

        [PM.Tooltip("The type of trigger event to detect.")]
        public Trigger2DType trigger;

        [UIHint(UIHint.Variable)]
        public FsmGameObject _storeCollider;

        //[UIHint(UIHint.FsmBool)]
        //public FsmBool triggerEnter;

        [UIHint(UIHint.Tag)]
        [PM.Tooltip("Filter by Tag.")]
        public FsmString collideTag;

        [UIHint(UIHint.FsmEvent)]
        public FsmEvent sendEvent;

        public OnTriggerAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();

            if (trigger == Trigger2DType.OnTriggerEnter2D)
                _atom.triggerType = "enter";
            else if (trigger == Trigger2DType.OnTriggerExit2D)
                _atom.triggerType = "exit";
            _atom.collideTag = collideTag.Value;
            if (trigger == Trigger2DType.OnTriggerEnter2D)
            {
                _atom.SetCollided((go) =>
                {
                    _storeCollider.Value = go;
                    Fsm.Event(sendEvent);
                });
            }
            if (trigger == Trigger2DType.OnTriggerExit2D)
            {
                _atom.SetExited((go) =>
                {
                    _storeCollider.Value = go;
                    Fsm.Event(sendEvent);
                });
            }
            _atom.Start(_entity);
            //if (!triggerEnter.IsNone && !triggerEnter.Value)
            //    Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }
    }
}
