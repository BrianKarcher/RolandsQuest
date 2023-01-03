using RQ.Common.Container;
using RQ.Entity.Common;
using RQ.Messaging;
using RQ.Model.Serialization.FSM;
using RQ.Serialization;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Entity/Enable Objects")]
    public class EnableObjectsState : AnimatorState
    {
        [SerializeField]
        private GameObject[] _objects;
        [SerializeField]
        private bool _enable = true;

        public override void Enter()
        {
            base.Enter();
            if (_objects != null)
            {
                foreach (var ourObject in _objects)
                {
                    ourObject.SetActive(_enable);
                }
            }
            Complete();
        }
    }
}
