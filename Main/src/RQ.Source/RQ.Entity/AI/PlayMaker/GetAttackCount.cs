using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Action;
using UnityEngine;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Returns Success if Button is pressed.")]
    public class GetAttackCount : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when button is pressed.")]
        public FsmEvent Trigger;

        //[PM.Tooltip("Repeat every frame.")]
        //public bool everyFrame;

        public GetAttackCountAtom _attackCountAtom;
        private IComponentRepository _entity;

        //public override void Reset()
        //{
        //    //gameObject = null;
        //    storeResult = null;
        //    everyFrame = false;
        //}

        public override void OnEnter()
        {
            Debug.Log("GetAttackCount Enter");
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _attackCountAtom.Start(_entity);
            DoUpdate();
            //DoGetSpeed();

            //if (!everyFrame)
            //{
            //    Finish();
            //}
        }

        public override void OnExit()
        {
            base.OnExit();
            _attackCountAtom.End();
        }

        public override void OnUpdate()
        {
            DoUpdate();
        }

        private void DoUpdate()
        {
            //Debug.Log("GetAttackCount Update");
            if (_attackCountAtom.OnUpdate() == RQ.AI.AtomActionResults.Success)
                Fsm.Event(Trigger);
        }
    }
}
