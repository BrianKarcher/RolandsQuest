using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Extensions;
using RQ.Messaging;
using RQ.Model.Interfaces;
using RQ.Model.Physics;
using RQ.Physics;
using RQ.Physics.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class BoxCastAtom : AtomActionBase
    {
        public Vector2 _offset;
        public Vector2 _size;
        public float _angle;
        public float _distance;
        public bool _sameLayer;
        public int _layer;
        public string[] _targetTags;
        public bool _rotateOffsetByFacingDirection;

        private List<ColliderSearchData> colliderData = new List<ColliderSearchData>();

        //public bool SameLayer = true;
        //public int Layer;
        //public string AnimationType;
        private IAnimationComponent _animComponent;
        //private IComponentRepository _entity;
        //private bool _isRunning;
        //private long _killSelfIndex;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);

            var colliderSearch = new ColliderSearch();
            var pos = _entity.transform.position;
            _animComponent = entity.Components.GetComponent<IAnimationComponent>();
            
            var facingDirectionVector = _animComponent.GetFacingDirectionVector();
            //animationComponent.

            var offset = (Vector3)_offset;
            if (_rotateOffsetByFacingDirection)
            {
                offset = RotateOffsetByFacingDirection(offset);
            }

            var itemsHit = UnityEngine.Physics.BoxCastAll(pos + offset, _size, facingDirectionVector, Quaternion.identity, _distance);
            var collisionComponent = entity.Components.GetComponent<CollisionComponent>();
            colliderSearch.ColliderScrub(collisionComponent, itemsHit, _targetTags, colliderData);
            //colliderData = scrub;
            //var itemsHit = colliderSearch.BoxCastAll(_entity, pos + _offset, _size, _angle, _distance, _sameLayer, _layer);

        }

        private Vector3 RotateOffsetByFacingDirection(Vector3 offset)
        {
            var facingDirection = _animComponent.GetFacingDirection();
            var rotationAngle = facingDirection.GetDirectionAngle();
            offset = offset.RotateAroundAxis(rotationAngle, Vector3.forward);
            return offset;
        }

        //public override void StartListening(IComponentRepository entity)
        //{
        //    _killSelfIndex = MessageDispatcher2.Instance.StartListening("AnimationComplete", entity.UniqueId, (data) =>
        //    {
        //        //var animation = _animComponent.Get
        //        if ((string)data.ExtraInfo != AnimationType)
        //            return;
        //        _isRunning = false;
        //    });
        //}

        //public override void StopListening(IComponentRepository entity)
        //{
        //    //MessageDispatcher2.Instance.StopListening("AnimationComplete", entity.UniqueId, _animationCompleteIndex);
        //}

        //public override void End()
        //{
        //}

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }

        public IList<ColliderSearchData> GetColliderData()
        {
            return colliderData;
        }
    }
}
