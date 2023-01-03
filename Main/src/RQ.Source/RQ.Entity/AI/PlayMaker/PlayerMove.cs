using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.AtomAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Entity.AI.PlayMaker
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Move based on player input.")]
    public class PlayerMove : FsmStateAction
    {
        [SerializeField]
        public PlayerMoveAtom _playerMoveAtom;

        private IComponentRepository _entity;

        public override void OnEnter()
        {
            //Debug.Log("PlayerMove - OnEnter");
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _playerMoveAtom.Start(_entity);

            //if (!everyFrame)
            //{
            //    Finish();
            //}
        }

        public override void OnExit()
        {
            base.OnExit();
            _playerMoveAtom.End();
        }

        public override void OnUpdate()
        {
            Tick();
        }

        void Tick()
        {
            if (Finished)
                return;
            _playerMoveAtom.OnUpdate();
            //if (result == RQ.AI.AtomActionResults.Success)
            //{
            //    Finish();
            //    Fsm.Event(Received);
            //}
        }
    }
}
