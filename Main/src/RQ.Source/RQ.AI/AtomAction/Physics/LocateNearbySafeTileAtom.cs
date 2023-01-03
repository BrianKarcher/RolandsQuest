using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class LocateNearbySafeTileAtom : AtomActionBase
    {
        public int UnsafeLayerMask;
        public float ExpandIteration = 0.08f;
        private AnimationComponent AnimationComponent { get; set; }
        public Vector2 Value { get; set; }
        public bool Is3D { get; set; }
        private float minX;
        private float maxX;
        private float minY;
        private float maxY;
        //private EntityStatsComponent _entityStatsComponent;
        //private EntityStatsData _entityStatusData;
        //private InputComponent _inputComponent;
        //public bool Value;
        //public bool InvertValue;
        //public ActionTarget ActionTarget = ActionTarget.Self;
        //[SerializeField]
        //public BoolVariableEnum _variable;
        //public string VariableName;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            var pos = entity.transform.position;

            // Expand a box outword from the characters position until a safe tile is located.
            minX = pos.x;
            maxX = pos.x;
            minY = pos.y;
            maxY = pos.y;

            // first check current tile.
            int iterationCount = 0;
            while (iterationCount < 10)
            {
                if (CheckSquare(out var locatedPosition))
                {
                    Value = locatedPosition;
                    Debug.Log($"Located safe spot at {Value}");
                    break;
                }
                ExpandSquare();
                iterationCount++;
            }

            //AnimationComponent = entity.Components.GetComponent<AnimationComponent>();


            //_inputComponent = target.Components.GetComponent<InputComponent>();

        }

        private bool CheckSquare(out Vector2 locatedPosition)
        {
            // Top line
            for (float x = minX; x <= maxX; x += 0.16f)
            {
                if (!PhysicsCast(x, minY))
                {
                    locatedPosition = new Vector2(x, minY);
                    return true;
                }
            }

            // Bottom line
            for (float x = minX; x <= maxX; x += 0.16f)
            {
                if (!PhysicsCast(x, maxY))
                {
                    locatedPosition = new Vector2(x, maxY);
                    return true;
                }
            }

            // left line
            for (float y = minY; y <= maxY; y += 0.16f)
            {
                if (!PhysicsCast(minX, y))
                {
                    locatedPosition = new Vector2(minX, y);
                    return true;
                }
            }

            // right line
            for (float y = minY; y <= maxY; y += 0.16f)
            {
                if (!PhysicsCast(maxX, y))
                {
                    locatedPosition = new Vector2(maxX, y);
                    return true;
                }
            }

            locatedPosition = Vector2.zero;
            return false;
        }

        private void ExpandSquare()
        {
            minX -= ExpandIteration;
            minY -= ExpandIteration;
            maxX += ExpandIteration;
            maxY += ExpandIteration;
        }

        private bool PhysicsCast(float x, float y)
        {
            if (Is3D)
            {
                return UnityEngine.Physics.Raycast(new Vector3(x, y, -10f), new Vector3(0, 0, 1f), UnsafeLayerMask);
            }
            else
            {
                var raycastInfo = UnityEngine.Physics2D.Raycast(new Vector2(x, y), new Vector2(0, 0), 10f, UnsafeLayerMask);
                // RaycastHit2D has an implicit conversion to bool
                return raycastInfo;
            }
        }

        public override AtomActionResults OnUpdate()
        {
            //Value = GetValue();
            //if (InvertValue)
            //    Value = !Value;
            return AtomActionResults.Success;
        }

        //private bool GetValue()
        //{
        //    _entityStatusData = _entityStatsComponent?.GetEntityStats();
        //    var variableComponent = _entity.Components.GetComponent<VariablesComponent>();
        //    switch (_variable)
        //    {
        //        case BoolVariableEnum.IsHiding:
        //            return _entityStatusData.IsHiding;
        //        case BoolVariableEnum.IsInputEnabled:
        //            return _inputComponent.IsInputEnabled();
        //        case BoolVariableEnum.IsAutoStart:
        //            return GameStateController.Instance.AutoStart;
        //        case BoolVariableEnum.BoolVariable:
        //            return variableComponent.Variables.GetBool(VariableName);
        //    }
        //    return false;
        //}
    }
}
