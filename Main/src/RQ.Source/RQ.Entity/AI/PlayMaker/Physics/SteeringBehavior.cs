using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using UnityEngine;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Physics")]
    [PM.Tooltip("Steering behavior to move the entity.")]
    public class SteeringBehavior : FsmStateAction
    {
        public SteeringBehaviorAtom _sbAtom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            //Debug.Log("SteeringBehavior Enter");
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _sbAtom.Start(_entity);
            Finish();
        }

        public override void OnExit()
        {
            //Debug.Log("SteeringBehavior Exit");
            base.OnExit();
            _sbAtom.End();
        }
    }
}
