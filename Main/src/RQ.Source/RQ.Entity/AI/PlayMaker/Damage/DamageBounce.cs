using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.Components;
using RQ.Physics.Components;
using UnityEngine;
using RQ.Entity.AtomAction.Condition;
using RQ.AI.Conditions;
using RQ.AI.Action;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Damage")]
    [PM.Tooltip("Bounce away from enemy when damaged.")]
    public class DamageBounce : FsmStateAction
    {
        [SerializeField]
        public DamageBounceAtom _damageBounceAtom;
        private IComponentRepository _entity;

        //public override void Reset()
        //{
        //    //gameObject = null;
        //    //storeResult = null;
        //    //everyFrame = false;
        //}

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _damageBounceAtom.Start(_entity);
            Tick();

            //if (!everyFrame)
            //{
            //    Finish();
            //}
        }

        public override void OnExit()
        {
            base.OnExit();
            _damageBounceAtom.End();
        }

        public override void OnUpdate()
        {
            Tick();
        }

        void Tick()
        {
            if (Finished)
                return;
            var result = _damageBounceAtom.OnUpdate();
            if (result == RQ.AI.AtomActionResults.Success)
            {
                Finish();
                //Fsm.Event(Damaged);
            }
        }
    }
}
