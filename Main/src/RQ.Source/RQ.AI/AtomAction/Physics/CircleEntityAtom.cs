using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class CircleEntityAtom : AtomActionBase
    {
        private GameObject _target;
        public float Radius;
        public float RotateSpeed;
        //private EntityStatsComponent _entityStatsComponent;
        //private EntityStatsData _entityStatusData;
        //private InputComponent _inputComponent;
        //public bool Value;
        //public bool InvertValue;
        //public ActionTarget ActionTarget = ActionTarget.Self;
        //[SerializeField]
        //public BoolVariableEnum _variable;
        //public string VariableName;
        private float _angle;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            //_angle = Vector2.Angle(_target.transform.position, entity.transform.position);
            //var signedAngle = Vector2.SignedAngle(entity.transform.position, _target.transform.position);
            var dir = (entity.transform.position - _target.transform.position);
            var signedAngle = Vector2.SignedAngle(Vector2.right, dir);
            //_angle =  * Mathf.Deg2Rad;
            _angle = signedAngle * Mathf.Deg2Rad;
            //_angle = Vector2.Angle(entity.transform.position, _target.transform.position);
            //Vector3.Cross(entity.transform.position, _target.transform.position);
        }

        public override AtomActionResults OnUpdate()
        {
            var time = Time.deltaTime;
            _angle += RotateSpeed * time;

            var unitVector = new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle));

            var offset = unitVector * Radius;
            var newPosition = _target.transform.position + offset;
            _entity.transform.position = new Vector3(newPosition.x, newPosition.y, _entity.transform.position.z);
            //Value = GetValue();
            //if (InvertValue)
            //    Value = !Value;
            return AtomActionResults.Running;
        }

        public void SetTarget(GameObject target)
        {
            _target = target;
        }
    }
}
