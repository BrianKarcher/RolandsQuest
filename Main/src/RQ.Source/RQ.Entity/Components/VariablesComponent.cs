using BehaviorDesigner.Runtime;
using RQ.Common.Components;
using RQ.Model;
using UnityEngine;
using static BehaviorDesigner.Runtime.BehaviorManager;

namespace RQ.Physics.Components
{
    [AddComponentMenu("RQ/Components/Variables")]
    public class VariablesComponent : ComponentPersistence<VariablesComponent>
    {
        [SerializeField]
        private Variables _variables;

        public Variables Variables { get { return _variables; } }

        public override void Awake()
        {
            base.Awake();
            _variables?.Init();
        }
    }
}
