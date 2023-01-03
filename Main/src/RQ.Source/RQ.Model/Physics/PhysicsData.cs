using System;

namespace RQ.Physics
{
    [Serializable]
    public class PhysicsData : BasicPhysicsData
    {
        public override void CopyFrom(BasicPhysicsData from)
        {
            base.CopyFrom(from);
            var physicsData = from as PhysicsData;
        }
    }
}
