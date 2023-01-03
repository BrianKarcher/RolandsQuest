using RQ.Common;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Model.Interfaces
{
    public interface IWaypointComponent : IBaseObject
    {
        IList<Vector3> GetWaypoints();
    }
}
