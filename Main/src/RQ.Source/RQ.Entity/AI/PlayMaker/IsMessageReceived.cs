using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using UnityEngine;
using RQ.Entity.AtomAction.Condition;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Gets whether a message has been received.")]
    public class IsMessageReceived : FsmStateAction
    {
        //[RequiredField]
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when message is received.")]
        public FsmEvent Received;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("The sender game object to store")]
        public FsmGameObject storeSender;

        public bool GotoPreviousState;

        //[PM.Tooltip("Repeat every frame.")]
        //public bool everyFrame;

        [SerializeField]
        public IsMessageReceivedAtom _isMessageReceivedAtom;
        private IComponentRepository _entity;

        //public override void Reset()
        //{
        //    base.Reset();
        //    if (_isMessageReceivedAtom.Message == "VictoryPose")
        //    {
        //        Debug.LogError($"(IsMessageReceived.Reset()) VictoryPose called - FSM = {base.Fsm.Name}, State = {base.State.Name}, IsRunning = {_isMessageReceivedAtom.IsRunning()}");
        //    }
        //    //gameObject = null;
        //    //storeResult = null;
        //    //everyFrame = false;
        //}

        public override void OnEnter()
        {
            base.OnEnter();

            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _isMessageReceivedAtom.Received += _isMessageReceivedAtom_Received;
            _isMessageReceivedAtom.Start(_entity);
            //if (_isMessageReceivedAtom.Message == "VictoryPose")
            //{
            //    Debug.LogError($"(IsMessageReceived.OnEnter()) VictoryPose called - FSM = {base.Fsm.Name}, State = {base.State.Name}, IsRunning = {_isMessageReceivedAtom.IsRunning()}");
            //}
            Tick();

            //if (!everyFrame)
            //{
            //    Finish();
            //}
        }

        private void _isMessageReceivedAtom_Received(object sender, System.EventArgs e)
        {
            //if (_isMessageReceivedAtom.Message == "VictoryPose")
            //{
            //    Debug.LogError($"(IsMessageReceived._isMessageReceivedAtom_Received()) VictoryPose called - FSM = {base.Fsm.Name}, State = {base.State.Name}, IsRunning = {_isMessageReceivedAtom.IsRunning()}");
            //}
            if (!storeSender.IsNone)
                storeSender.Value = _isMessageReceivedAtom.GetSenderGameObject();
            Finish();
            RunEvent();
            //Fsm.Event(Received);
            //throw new System.NotImplementedException();
        }

        public override void OnExit()
        {
            base.OnExit();
            _isMessageReceivedAtom.Received -= _isMessageReceivedAtom_Received;
            _isMessageReceivedAtom.End();
            //if (_isMessageReceivedAtom.Message == "VictoryPose")
            //{
            //    Debug.LogError($"(IsMessageReceived.OnExit()) VictoryPose called - FSM = {base.Fsm.Name}, State = {base.State.Name}, IsRunning = {_isMessageReceivedAtom.IsRunning()}");
            //}
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Tick();
        }

        void Tick()
        {
            if (Finished)
                return;
            if (!base.Entered)
                return;
            var result = _isMessageReceivedAtom.OnUpdate();
            if (result == RQ.AI.AtomActionResults.Success)
            {
                if (GotoPreviousState)
                {
                    if (Fsm.PreviousActiveState != null)
                    {
                        //Log("Goto Previous State: " + Fsm.PreviousActiveState.Name);
                        Fsm.GotoPreviousState();
                        return;
                    }
                }
                if (!storeSender.IsNone)
                    storeSender.Value = _isMessageReceivedAtom.GetSenderGameObject();
                RunEvent();
                Finish();                
            }
        }

        private void RunEvent()
        {
            //if (_isMessageReceivedAtom.Message == "VictoryPose")
            //{
            //    Debug.LogError($"(IsMessageReceived.RunEvent()) VictoryPose called - FSM = {base.Fsm.Name}, State = {base.State.Name}, IsRunning = {_isMessageReceivedAtom.IsRunning()}");
            //}
            Fsm.Event(Received);
        }
    }
}
