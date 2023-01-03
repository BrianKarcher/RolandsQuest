using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using UnityEngine;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Call Finallly when the timer runs out. Persists the timer if the state exits.")]
    public class WaitPersist : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.FsmFloat)]
        [PM.Tooltip("The time to wait")]
        public FsmFloat time;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("The variable to use for persistence")]
        public FsmFloat storeResult;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("True when the time is up.")]
        public FsmBool StoreTimesUp;

        [UIHint(UIHint.FsmEvent)]
        [PM.Tooltip("Event to call when the wait finishes.")]
        public FsmEvent FinishEvent;
        
        //[PM.Tooltip("Repeat every frame.")]
        //public bool everyFrame;

        //public KillSelfAtom _killSelfAtom;
        private IComponentRepository _entity;

        //public override void Reset()
        //{
        //    //gameObject = null;
        //    storeResult = null;
        //    everyFrame = false;
        //}

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            if (storeResult.IsNone)
                return;

            //storeResult.Value = time.Value;
            //_killSelfAtom.Start(_entity);
            //DoGetSpeed();

            //if (!everyFrame)
            //{
            //    Finish();
            //}
        }

        public override void OnExit()
        {
            base.OnExit();
            //_killSelfAtom.End();
        }

        public override void OnUpdate()
        {
            if (storeResult.IsNone)
                return;
            storeResult.Value += Time.deltaTime;
            if (storeResult.Value > time.Value)
            {
                StoreTimesUp.Value = true;
                Finish();
                if (FinishEvent != null)
                    Fsm.Event(FinishEvent);
            }
            //Tick();
        }

        //private void Tick()
        //{
        //    //if (storeResult.IsNone) return;
        //    //_getSpeedAtom.OnUpdate();
        //    //storeResult.Value = _getSpeedAtom.Speed;
        //}
    }
}
