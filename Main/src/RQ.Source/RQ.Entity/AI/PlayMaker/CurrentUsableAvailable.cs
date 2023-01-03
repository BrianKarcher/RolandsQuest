using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Action;
using RQ.Entity.AtomAction.Action;
using UnityEngine;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Fires an event if there is a usable object within range of the player.")]
    public class CurrentUsableAvailable : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when button is pressed.")]
        public FsmEvent Trigger;

        //[PM.Tooltip("Repeat every frame.")]
        //public bool everyFrame;

        public CurrentUsableAvailableAtom _currentUsableAvailableAtom;
        private IComponentRepository _entity;

        //public override void Reset()
        //{
        //    //gameObject = null;
        //    storeResult = null;
        //    everyFrame = false;
        //}

        public override void OnEnter()
        {
            Debug.Log("GetCurrentUsable Enter");
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _currentUsableAvailableAtom.Start(_entity);
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
            _currentUsableAvailableAtom.End();
        }

        public override void OnUpdate()
        {
            DoUpdate();
            //DoGetSpeed();
        }

        private void DoUpdate()
        {
            //Debug.Log("GetCurrentUsable Enter");
            if (_currentUsableAvailableAtom.OnUpdate() == RQ.AI.AtomActionResults.Success)
                Fsm.Event(Trigger);
        }

        //void DoGetSpeed()
        //{
        //    if (storeResult.IsNone) return;
        //    _getSpeedAtom.OnUpdate();
        //    storeResult.Value = _getSpeedAtom.Speed;
        //}
    }
}
