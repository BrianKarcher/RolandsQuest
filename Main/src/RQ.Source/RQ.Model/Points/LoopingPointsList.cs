using RQ.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Physics
{
    [Serializable]
    public class LoopingPointsListUsingPoint : LoopingPointsList<LoopingPointsUsingPoint, Point> { }

    [Serializable]
    public class LoopingPointsList<POINTS, POINT> : IPointsList<POINTS, POINT> 
        where POINTS : IPoints<POINT> 
        where POINT : IPoint
    {
        [HideInInspector]
        [SerializeField]
        private List<POINTS> _pointsList;

        public List<POINTS> PointsList 
        { 
            get 
            {
                return _pointsList; 
            }
            set
            {
                _pointsList = value;
            }
        }
    }
}
