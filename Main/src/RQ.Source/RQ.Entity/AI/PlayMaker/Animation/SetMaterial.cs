using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.Components;
using RQ.Physics.Components;
using UnityEngine;
using RQ.AI.Action.Animation;
using RQ.Model.Enums;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Animation")]
    [PM.Tooltip("Set the material for the animation.")]
    public class SetMaterial : FsmStateAction
    {
        public SetMaterialAtom _atom;
        private IComponentRepository _entity;
        public RQ.Model.Enums.ActionTarget ActionTarget = RQ.Model.Enums.ActionTarget.Self;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.Start(_entity);
            //Finish();
        }

        public override void OnPreprocess()
        {
            Fsm.HandleLateUpdate = true;
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            _atom.OnLateUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }
    }
}
