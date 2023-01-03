using RQ.Enums;
using RQ.Messaging;
using RQ.Physics;
using UnityEngine;
using RQ.Extensions;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Change Direction")]
    public class ChangeDirection : ActionBase
    {
        private enum ChangeDirectionTo
        {
            Target = 0,
            Custom = 1
        }

        [SerializeField]
        private ChangeDirectionTo _changeDirectionTo = ChangeDirectionTo.Target;

        [SerializeField]
        private Direction _direction = Direction.Up;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            Process(_changeDirectionTo);
        }

        private void Process(ChangeDirectionTo changeDirectionTo)
        {
            Direction direction = Direction.Up;
            switch (changeDirectionTo)
            {
                case ChangeDirectionTo.Target:
                    var target = _aiComponent.Target;
                    if (target == null)
                    {
                        if (_aiComponent.Targets != null && _aiComponent.Targets.Count != 0)
                            target = _aiComponent.Targets[0];
                        //target = _aiComponent.Targets.FirstOrDefault();
                    }
                    if (target == null)
                        return;
                    var lookAtVector = (Vector2D) target.transform.position - GetEntity().transform.position;
                    direction = lookAtVector.GetDirection();                    
                    break;
                case ChangeDirectionTo.Custom:
                    direction = _direction;
                    break;
            }
            SetDirection(direction);
        }

        private void SetDirection(Direction direction)
        {
            foreach (var animationComponent in _animationComponents)
            {
                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId,
                    animationComponent.UniqueId, Telegrams.SetFacingDirection, direction);
            }
        }
    }
}
