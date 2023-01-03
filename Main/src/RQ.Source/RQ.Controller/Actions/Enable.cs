using RQ.Common.Container;
using RQ.Entity.Components;
using RQ.Model.Serialization;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Entity/Enable")]
    public class Enable : ActionBase
    {
        [SerializeField]
        private bool _enableOnEnter;
        [SerializeField]
        private bool _enableOnExit = true;

        [SerializeField]
        private Transform _transform;
        //public AudioClip AudioClip;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            if (_transform == null)
                _transform = GetEntity().transform;
            _transform.gameObject.SetActive(_enableOnEnter);
            //var componentRepository = _transform.GetComponent<IComponentRepository>();
            //componentRepository.Init();
        }

        public override void ActExit(Component otherRigidBody)
        {
            base.ActExit(otherRigidBody);
            _transform.gameObject.SetActive(_enableOnExit);
        }

        public override void Serialize(Serialization.EntitySerializedData entitySerializedData)
        {
            base.Serialize(entitySerializedData);
            var enableData = new EnableData();
            if (_transform != null)
            {
                var repo = _transform.GetComponent<IComponentRepository>();
                if (repo == null)
                {
                    Debug.LogError("No Component Repository in " + _transform.name + ", cannot serialize Enable");
                }
                else
                {
                    enableData.TargetUniqueId = repo.UniqueId;
                }                
            }
            base.SerializeComponent(entitySerializedData, enableData);
        }

        public override void Deserialize(Serialization.EntitySerializedData entitySerializedData)
        {
            base.Deserialize(entitySerializedData);
            var enableData = base.DeserializeComponent<EnableData>(entitySerializedData);
            if (enableData == null)
                return;
            if (!string.IsNullOrEmpty(enableData.TargetUniqueId))
            {
                // TODO Change this to a deep search, right now it only searches for the Component Repo's.
                _transform = EntityContainer._instance.GetEntity(enableData.TargetUniqueId).transform;
            }
        }
    }
}
