using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using System;
using RQ.Physics.Components;

namespace RQ.AI.AtomAction.Physics
{
    [Serializable]
    public class SetColliderTagsAtom : AtomActionBase
    {
        private string[] _tags;
        public string _collisionComponent;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            var collComponent = entity.Components.GetComponent<CollisionComponent>(_collisionComponent);
            collComponent.GetCollisionData().Tags = _tags;
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }

        public void SetTags(string[] tags)
        {
            _tags = tags;
        }
    }
}
