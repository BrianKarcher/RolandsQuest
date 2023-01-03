using RQ.Common.Controllers;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Messaging;
using RQ.Model;
using RQ.Physics;
using RQ.Physics.Components;
using System;

namespace RQ.Entity.StatesV3.Conditions
{
    [Obsolete]
    public class EntityInViewportConfig : StateTransitionConditionBaseConfig
    {
        //private ICameraClass _camera;
        public override void ConditionInit(IStateMachine stateMachine)
        {
            //_camera = stateMachine.GetComponentRepository().Components.GetComponent<ICameraClass>();
            base.ConditionInit(stateMachine);
            //Listen(stateMachine);
        }

        //public void Listen(IStateMachine stateMachine)
        //{
        //    var entity = stateMachine.GetComponentRepository();
        //    if (entity == null)
        //        throw new Exception("Message Received Condition - Entity is empty");

        //    //entity.StartListening("PosInViewport", this.UniqueId, (data) =>
        //    //{
        //    //    SetIsConditionSatisfied(stateMachine, true);
        //    //    // Set up the next state right away if need be, no need to wait a frame
        //    //    stateMachine.CalculateNextState();
        //    //});
        //    stateMachine.GetComponentRepository().StartListening("SetCamera", this.UniqueId, (data) =>
        //    {
        //        _camera = (ICameraClass)data.ExtraInfo;
        //        //SetIsConditionSatisfied(stateMachine, true);
        //        //// Set up the next state right away if need be, no need to wait a frame
        //        //stateMachine.CalculateNextState();
        //    });
        //}

        public override void ConditionExit(IStateMachine stateMachine) 
        {
            base.ConditionExit(stateMachine);
            var entity = stateMachine.GetComponentRepository();
            if (entity == null)
                throw new Exception("Message Received Condition - Entity is empty");
            //entity.StopListening("PosInViewport", this.UniqueId);
            //entity.StopListening("SetCamera", this.UniqueId);
        }

        public override bool TestCondition(IStateMachine stateMachine)
        {
            var entity = stateMachine.GetComponentRepository();
            //MessageDispatcher2.Instance.DispatchMsg("GetCamera", 0f, stateMachine.GetComponentRepository().UniqueId, "Game Controller", null);
            //GameStateController
            var physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            Vector2D pos = physicsComponent.GetWorldPos();
            var camera = GameDataController.Instance.Camera;
            var isInViewport = camera.IsPosInViewport(pos);
            //MessageDispatcher2.Instance.DispatchMsg("IsPosInViewport", 0f, this.UniqueId, "Game Controller", pos);
            return isInViewport; //GetIsConditionSatisfied(stateMachine);
        }
    }
}
