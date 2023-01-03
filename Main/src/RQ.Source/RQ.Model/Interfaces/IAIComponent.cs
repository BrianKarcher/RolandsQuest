using RQ.Common;
using RQ.Physics;
using UnityEngine;

namespace RQ.FSM.V2
{
    public interface IAIComponent : IBaseObject
    {
        Vector2D GetHomePosition();
        float GetHomeSickDistanceSq();
        float GetRunHomeSpeed();
        Transform GetTarget();
        Transform GetWaypoints();
    }
}