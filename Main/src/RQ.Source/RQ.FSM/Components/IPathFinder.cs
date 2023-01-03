using RQ.Model.Physics;
using System;
using UnityEngine;

namespace RQ.FSM.Components
{
    public interface IPathFinder
    {
        void StartPathProcessing(Vector3 pos, Vector3 target, int graphMask);
        event Action<PathfindingData> OnPathComplete;
    }
}
