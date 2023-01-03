using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Model.Enums;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SetJointAtom : AtomActionBase
    {
        private GameObject _addJointTo;
        private GameObject _connectTo;
        //public ActionTarget AddJointTo = ActionTarget.Self;
        //public ActionTarget ConnectTo = ActionTarget.Self;
        //private IComponentRepository _target;
        public bool SetToNull = false;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            var addJointToRepo = _addJointTo.GetComponent<IComponentRepository>();
            IComponentRepository connectToRepo = null;
            if (_connectTo != null)
                connectToRepo = _connectTo.GetComponent<IComponentRepository>();
            //_target = base.GetTarget(addJointToRepo);
            if (addJointToRepo == null)
            {
                //Debug.LogError("(SetJointAtom) - Could not locate target.");
                return;
            }
            
            if (SetToNull)
            {
                var oldJoint = addJointToRepo.GetComponent<Joint>();
                //Debug.LogError($"Destroying joint {oldJoint.name}");
                GameObject.Destroy(oldJoint);
            }
            else
            {
                //var connectTo = base.GetTarget(addJointToRepo);
                // TODO Allow user to specify the joint, decouple the two entities
                // TODO Program a better way of getting the rigid body, this is error prone and assumes it
                // is at the same level as the ComponentRepository
                // This still isn't clean code
                var rigidBody = connectToRepo.GetComponent<Rigidbody>();
                if (rigidBody == null)
                    rigidBody = connectToRepo.GetComponentInChildren<Rigidbody>();
                var joint = addJointToRepo.gameObject.AddComponent<FixedJoint>();
                //var joint = _target.GetComponent<Joint>();
                //joint.
                joint.connectedBody = SetToNull ? null : rigidBody;
            }
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }

        public void SetAddJointTo(GameObject gameObject)
        {
            _addJointTo = gameObject;
        }

        public void SetConnectedTo(GameObject gameObject)
        {
            _connectTo = gameObject;
        }
    }
}
