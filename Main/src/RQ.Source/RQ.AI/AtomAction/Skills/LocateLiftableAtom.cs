using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using System;
using RQ.Animation;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.AI.Action
{
    [Serializable]
    public class LocateLiftableAtom : AtomActionBase
    {
        public float MaxDistance;
        private PhysicsComponent _physicsComponent;
        private AnimationComponent _animationComponent;
        private int _obstacleLayerMask;
        private GameObject _liftableObject;
        private int _layerMask;
        private RaycastHit[] _raycastHit = null;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            if (_raycastHit == null)
                _raycastHit = new RaycastHit[20];
            if (_physicsComponent == null)
                _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            if (_animationComponent == null)
            {
                var animationComponents = entity.Components.GetComponents<AnimationComponent>();
                for (int i = 0; i < animationComponents.Count; i++)
                {
                    var animationComponent = animationComponents[i] as AnimationComponent;
                    if (animationComponent.IsMain())
                    {
                        _animationComponent = animationComponent;
                        break;
                    }
                }
            }
            _liftableObject = null;
            var sqMaxDistance = MaxDistance * MaxDistance;

            GameObject closestEntity = null;
            float closestEntitySqDistance = float.MaxValue;

            //var entities = EntityContainer._instance.GetAllEntities();
            //foreach (var otherEntity in entities)
            //{
            // First check is to make sure the entity has a LiftableComponent
            var raycastHitCount = UnityEngine.Physics.BoxCastNonAlloc(_physicsComponent.GetFeetWorldPosition3(), new Vector3(0.5f, 0.5f, 0.2f), _animationComponent.GetFacingDirectionVector(), _raycastHit, Quaternion.identity, MaxDistance, _layerMask);
            for (int i = 0; i < raycastHitCount; i++)
            {
                var otherEntity = _raycastHit[i].rigidbody?.GetComponent<IComponentRepository>();
                if (otherEntity == null)
                    continue;
                var skillAffectedByComponent = otherEntity.Components.GetComponent<SkillAffectedByComponent>();
                if (skillAffectedByComponent == null)
                    continue;
                if (!skillAffectedByComponent.GetData().DisperseHelix)
                    continue;
                bool inFOV = _physicsComponent.EntityInFOV(otherEntity.gameObject, _animationComponent.GetFacingDirectionVector(), true,
                    _obstacleLayerMask);
                if (!inFOV)
                    continue;

                var distanceSqToEntity =
                    (_physicsComponent.GetFeetWorldPosition3() - otherEntity.transform.position).sqrMagnitude;
                // Too far away to consider?
                if (distanceSqToEntity > sqMaxDistance)
                    continue;

                // Cycle through all to find the closest liftable object
                if (distanceSqToEntity < closestEntitySqDistance)
                {
                    closestEntity = otherEntity.gameObject;
                    closestEntitySqDistance = distanceSqToEntity;
                }
                //break;
            }

            //}
            _liftableObject = closestEntity;
        }

        public override void End()
        {
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Running;
        }

        public void SetObstacleLayerMask(int obstacleLayerMask)
        {
            _obstacleLayerMask = obstacleLayerMask;
        }

        public void SetLayerMask(int layerMask)
        {
            _layerMask = layerMask;
        }

        public GameObject GetLiftableObject()
        {
            return _liftableObject;
        }
    }
}
