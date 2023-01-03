using RQ.AI;
using RQ.Entity;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class StandingOnColliderAtom : AtomActionBase
    {
        private int Layer;
        private int CancelLayer;
        private PhysicsComponent _physicsComponent;
        private PlayerComponent _playerComponent;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            if (_physicsComponent == null)
                _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            if (_playerComponent == null)
                _playerComponent = entity.Components.GetComponent<PlayerComponent>();
        }

        public override AtomActionResults OnUpdate()
        {
            Tick();
            //Value = GetValue();
            //if (InvertValue)
            //    Value = !Value;
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }

        private void Tick()
        {
            var feetPos = _physicsComponent.GetFeetWorldPosition3() + new Vector3(0, 0, -0.2f); // + new Vector3(0,0,-2f);
            // Do four raycasts to make sure the entity is on water
            // Top left
            if (!PhysicsCast(feetPos + new Vector3(-0.05f, 0.05f)))
                return;
            // Top right
            if (!PhysicsCast(feetPos + new Vector3(0.05f, 0.05f)))
                return;
            // Bottom left
            if (!PhysicsCast(feetPos + new Vector3(-0.05f, -0.05f)))
                return;
            // Bottom right
            if (!PhysicsCast(feetPos + new Vector3(0.05f, -0.05f)))
                return;

            // TODO Fix this, falling should be based on some variable
            // Don't fall if joined to an object?
            var joint = _entity.GetComponent<Joint>();
            if (joint != null)
                return;

            _isRunning = false;
        }

        private bool PhysicsCast(Vector3 pos)
        {
            var layer = Layer | CancelLayer;

            //var physicsData = _physicsComponent.GetPhysicsData();
            //bool hit = UnityEngine.Physics.Raycast(new Ray(pos, Vector3.forward), out var raycastHit, UnityEngine.Mathf.Infinity, layer);
            bool hit = UnityEngine.Physics.Raycast(new Ray(pos, Vector3.forward), out var raycastHit, 0.6f, layer);
            if (!hit)
                return false;
            if (CancelLayer == 0)
                return true;
            // If the layer hit was the Cancel Layer, then return false
            //var gameObjectLayerMask = 1 << raycastHit.transform.gameObject.layer;
            var gameObjectLayerMask = 1 << raycastHit.collider.gameObject.layer;
            if ((gameObjectLayerMask & CancelLayer) == CancelLayer)
                return false;
            return true;
        }
        public void SetLayer(LayerMask layer)
        {
            Layer = 1 << layer.value;
            //Layer = layer;
        }
        public void SetCancelLayer(LayerMask layer)
        {
            CancelLayer = 1 << layer.value;
            //CancelLayer = layer;
        }

        public void DrawGizmos()
        {
            var feetPos = _physicsComponent.GetFeetWorldPosition3();
            Gizmos.color = Color.red;
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(new Ray(feetPos + new Vector3(-0.05f, 0.05f), Vector3.forward * 0.5f));
            Gizmos.DrawRay(new Ray(feetPos + new Vector3(0.05f, 0.05f), Vector3.forward * 0.5f));
            Gizmos.DrawRay(new Ray(feetPos + new Vector3(-0.05f, -0.05f), Vector3.forward * 0.5f));
            Gizmos.DrawRay(new Ray(feetPos + new Vector3(0.05f, -0.05f), Vector3.forward * 0.5f));
            // Do four raycasts to make sure the entity is on water
            // Top left
            //if (!PhysicsCast(feetPos + new Vector3(-0.05f, 0.05f)))
            //    return;
            //// Top right
            //if (!PhysicsCast(feetPos + new Vector3(0.05f, 0.05f)))
            //    return;
            //// Bottom left
            //if (!PhysicsCast(feetPos + new Vector3(-0.05f, -0.05f)))
            //    return;
            //// Bottom right
            //if (!PhysicsCast(feetPos + new Vector3(0.05f, -0.05f)))
            //    return;
        }
    }
}
