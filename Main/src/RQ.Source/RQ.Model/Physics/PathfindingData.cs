using System.Collections.Generic;
using UnityEngine;

namespace RQ.Model.Physics
{
    public struct PathfindingData
    {
        public List<Vector3> Path { get; }
        /** Start Point exactly as in the path request */
        public Vector3 OriginalStartPoint { get; }

        /** End Point exactly as in the path request */
        public Vector3 OriginalEndPoint { get; }

        public PathfindingData(List<Vector3> path, Vector3 originalStartPoint, Vector3 originalEndPoint)
        {
            Path = path;
            OriginalStartPoint = originalStartPoint;
            OriginalEndPoint = originalEndPoint;
        }
    }
}
