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
    public class SetSteeringTargetToEntity : FsmStateAction
    {
        //[RequiredField]
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Message Receivers for SetSteeringTarget")]
        public FsmArray _messageReceivers;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Message Receiver for SetSteeringTarget")]
        public FsmGameObject _messageReceiver;

        [UIHint(UIHint.FsmBool)]
        public FsmBool _sendToSelf;

        //[PM.Tooltip("Repeat every frame.")]
        //public bool everyFrame;

        public SetSteeringTargetToEntityAtom _setSteeringAtom;
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
            _setSteeringAtom.Start(_entity);
            if (!_sendToSelf.IsNone && _sendToSelf.Value)
            {
                _setSteeringAtom.SendMessage(_entity.UniqueId);
            }
            if (_messageReceivers != null && _messageReceivers.stringValues != null)
            {
                for (int i = 0; i < _messageReceivers.stringValues.Length; i++)
                {
                    var receiver = _messageReceivers.stringValues[i];
                    _setSteeringAtom.SendMessage(receiver);
                }
            }
            SendToMessageReceiver();

            Finish();
        }

        private void SendToMessageReceiver()
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
            if (repo == null)
                return;

            _setSteeringAtom.SendMessage(repo.UniqueId);
        }

        public override void OnExit()
        {
            base.OnExit();
            _setSteeringAtom.End();
        }
    }
}
