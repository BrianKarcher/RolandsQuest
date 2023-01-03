using System.Collections.Generic;
using RQ.Animation;
using RQ.Common;
using RQ.Enums;
using RQ.Messaging;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Hide")]
    public class Hide : Active
    {
        //private ISprite _sprite;
        //private IEnumerable<string> animationComponentUniqueIds;
        private IList<IBaseObject> _animationComponents;
        //private ICollisionComponent _collisionComponent;

        public override void SetupState()
        {
            base.SetupState();
            _animationComponents = _spriteBaseComponent.Components.GetComponents<AnimationComponent>();
            //animationComponentUniqueIds =
            //    _spriteBaseComponent.Components.GetComponents<AnimationComponent>().Select(i =>
            //        i.UniqueId);
            //_collisionComponent = _spriteBaseComponent.Components.GetComponent<CollisionComponent>();
        }

        public override void Enter()
        {
            base.Enter();

            foreach (var animComponent in _animationComponents)
            {
                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId,
                    animComponent.UniqueId, Telegrams.SetRender, false);
            }

            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId,
            //    animationComponentUniqueIds, Telegrams.SetRender, false);
            _collisionComponent.gameObject.SetActive(false);
            //GameObject.SetActiv
            //MessageDispatcher2.Instance.DispatchMsg("EnableGO", 0f, this.UniqueId, _collisionComponent.UniqueId, "0");
            //_physicsComponent.Stop();
            //sprite.GetSteering().TurnOn(behavior_type.wander);
        }

        public override void Exit()
        {
            base.Exit();

            foreach (var animComponent in _animationComponents)
            {
                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId,
                    animComponent.UniqueId, Telegrams.SetRender, true);
            }

            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId,
            //    animationComponentUniqueIds, Telegrams.SetRender, true);
            _collisionComponent.gameObject.SetActive(true);
            //sprite.GetSteering().TurnOff(behavior_type.wander);
        }
    }
}
