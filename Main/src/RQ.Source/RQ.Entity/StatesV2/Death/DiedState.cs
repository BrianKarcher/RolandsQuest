using RQ.Messaging;
using UnityEngine;
using RQ.Physics.Components;
using RQ.Entity.Components;
using RQ.Enums;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Death/Died")]
    public class DiedState : AnimatorState
    {
        public override void Enter()
        {
            base.Enter();
            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _spriteBase.UniqueId, 
                Enums.Telegrams.EntityDied, null);
            MessageDispatcher2.Instance.DispatchMsg("EntityDied", 0f, 
                this.UniqueId, _spriteBase.UniqueId, null);
            ProcessDropItems();
            Complete();
            //StateMachine.GetComponentRepository().
            //_animationComponent.GetSpriteAnimator().SetColor(_color);
            //_sprite.Stop();
            //sprite.GetSteering().TurnOn(behavior_type.wander);
        }

        private void ProcessDropItems()
        {
            var entityStatsComponent = GetEntity().Components.GetComponent<EntityStatsComponent>();
            var entityStats = entityStatsComponent.GetEntityStats();
            if (entityStats.DropItems == null || entityStats.DropItems.Length == 0)
                return;
            float increment = 0;
            var pick = Random.Range(0f, 1f);
            for (int i = 0; i < entityStats.DropItems.Length; i++)
            {
                var dropItem = entityStats.DropItems[i];
                increment += dropItem.Chance;
                if (pick <= increment)
                {
                    // This is the drop item, create it
                    var newObject = (Instantiate(dropItem.Item, _physicsComponent.GetFeetPosition3(), Quaternion.identity) as GameObject);
                    var newTansform = newObject.GetComponent<IComponentRepository>();
                    //GameObject.Instantiate(dropItem.Item, _physicsComponent.GetFeetPosition3(), Quaternion.identity);
                    var level = _floorComponent.GetLevel();
                    if (newObject != null)
                    {
                        //var thisLevel = GetLevel();
                        var collisionComponents = newTansform.Components.GetComponents<CollisionComponent>();
                        if (collisionComponents != null)
                        {
                            foreach (var collisionComponent in collisionComponents)
                            {
                                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, collisionComponent.UniqueId,
                                    Telegrams.SetLevelHeight, level);
                            }
                        }
                    }
                    break;
                }
            }
            
            //_damageComponent.DropItems
        }
    }
}
