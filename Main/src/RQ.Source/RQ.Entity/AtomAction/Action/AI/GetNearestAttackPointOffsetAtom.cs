using RQ.Common.UniqueId;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.Input.Data;
using RQ.Messaging;
using RQ.Physics.Components;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace RQ.AI.Action
{
    public class GetNearestAttackPointOffsetAtom : AtomActionBase
    {
        public string AttackComponentName;
        public bool SameLayer = true;
        public int[] ObstacleLayers;
        public float AdjustDistance;

        private AttackComponent _attackComponent;
        private AIComponent _aiComponent;
        private System.Action Failed;
        // The offset will be used for things like OffsetPursuit, to go to an intelligent point of attack
        public Vector3 Offset;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _entity = entity;
            _attackComponent = _entity.Components.GetComponent<AttackComponent>(AttackComponentName);
            _aiComponent = _entity.Components.GetComponent<AIComponent>();
            if (_aiComponent.Target == null)
                Failed?.Invoke();
            Act();
        }

        public void Act()
        {
            // Todo This assumes four-way attacks only
            //List<Vector2> lst = new List<Vector2>();
            if (SameLayer)
            {
                ObstacleLayers = new int[] { _entity.gameObject.layer };
            }
            Vector3 closestVector = new Vector2();
            float closestDistanceSq = 9999f;
            var currentPos = (Vector3)_entity.transform.position;
            var target = _aiComponent.Target;
            var distance = _attackComponent.AttackData.Distance;
            var vectors = new Vector3[4];
            vectors[0] = new Vector3(target.position.x - distance, target.position.y, target.position.z);
            vectors[1] = new Vector3(target.position.x + distance, target.position.y, target.position.z);
            vectors[2] = new Vector3(target.position.x, target.position.y + distance, target.position.z);
            vectors[3] = new Vector3(target.position.x, target.position.y - distance, target.position.z);
            for (int i = 0; i < 4; i++)
            {
                // Faster than a raycast, check this first
                var sqrDist = (currentPos - vectors[i]).sqrMagnitude;
                if (sqrDist > closestDistanceSq)
                    continue;
                bool los = HasLineOfSight(vectors[i]);
                if (los)
                {
                    closestVector = vectors[i];
                    closestDistanceSq = sqrDist;
                }
            }
            // Couldn't find any
            if (closestDistanceSq > 1000f)
            {
                Failed?.Invoke();
                return;
            }
            var offset = closestVector - target.position;
            var adjustDistanceOffset = offset.normalized * AdjustDistance;
            //Offset = offset.normalized * (offset.magnitude + DistanceOffset);
            Offset = offset + adjustDistanceOffset;
        }

        public bool HasLineOfSight(Vector3 target)
        {
            var currentPos = (Vector3)_entity.transform.position;
            var layerMask = 0;
            for (var i = 0; i < ObstacleLayers.Length; i++)
            {
                layerMask = layerMask | 1 << ObstacleLayers[i];
            }
            //var layerMask = 1 << ObstacleLayer;
            return (!UnityEngine.Physics.Raycast(currentPos, target - currentPos, (target - currentPos).magnitude, layerMask));

        }

        //public override void End()
        //{
        //    base.End();
        //}

        public override AtomActionResults OnUpdate()
        {
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
            //return AtomActionResults.Success;
        }

        public void SetFailedAction(System.Action failed)
        {
            Failed = failed;
        }
    }
}
