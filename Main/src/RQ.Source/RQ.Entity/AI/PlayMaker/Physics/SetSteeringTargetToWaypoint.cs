using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using System.Collections.Generic;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Sets the steering target. Finishes immediately.")]
    public class SetSteeringTargetToWaypoint : FsmStateAction
    {
        //[RequiredField]
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Message Receivers for SetSteeringTarget")]
        public FsmArray _messageReceivers;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Message Receiver for SetSteeringTarget")]
        public FsmGameObject _messageReceiver;

        //[PM.Tooltip("Repeat every frame.")]
        //public bool everyFrame;

        public SetSteeringTargetToWaypointAtom _setSteeringAtom;
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
            List<string> messageReceivers = new List<string>();
            if (_messageReceivers != null)
                _setSteeringAtom._messageReceivers = _messageReceivers.stringValues;
            CheckMessageReceiver(messageReceivers);

            _setSteeringAtom._messageReceivers = messageReceivers.ToArray();
            _setSteeringAtom.Start(_entity);
            //DoGetSpeed();

            //if (!everyFrame)
            //{
            //    Finish();
            //}
            Finish();
        }

        private void CheckMessageReceiver(List<string> messageReceivers)
        {
            if (_messageReceiver.IsNone)
                return;

            var go = _messageReceiver.Value;
            if (go == null)
                return;

            var collisionComponent = go.GetComponent<CollisionComponent>();
            if (collisionComponent == null)
                return;

            var repo = collisionComponent.GetComponentRepository();
            if (repo != null)
                messageReceivers.Add(repo.UniqueId);
        }

        public override void OnExit()
        {
            base.OnExit();
            _setSteeringAtom.End();
        }
    }
}
