using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Messaging;
using RQ.Model;
using RQ.Physics;
using RQ.Physics.Components;
using System;

namespace RQ.Controller.StatesV3.Conditions
{
    public class EntityInTilemapBoundsConfig : StateTransitionConditionBaseConfig
    {
        public override bool TestCondition(IStateMachine stateMachine)
        {
            //var tileMap = GameController.Instance.GetSceneSetup().TileMap;
            var camera = GameController.Instance.GetCamera();
            var maxBounds = camera.GetMaxBounds();
            var entity = stateMachine.GetComponentRepository();
            var physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            Vector2D pos = physicsComponent.GetWorldPos();
            bool isInTileMapBounds = pos.x > -0.08f && pos.y > -0.08f && pos.x < maxBounds.x && pos.y < maxBounds.y;
            return isInTileMapBounds;
        }
    }
}
