using RQ.Physics;
using System;

namespace RQ.AI
{
    [Serializable]
    public class LoopingPointsUsingPoint : LoopingPoints<Point> { }

    [Serializable]
    public class LoopingPoints<POINT> : Points<POINT>, IPoints<POINT> 
        where POINT : IPoint, new()
    {
        //public string Name;
        //std::list<Vector2D>            m_WayPoints;
        //private LinkedList<Vector2D> _wayPoints;
        //public List<Point> Points { get; set; }

        //points to the current point
        //private LinkedListNode<Vector2D> _curWaypoint;
        public int CurrentPoint;


        //flag to indicate if the path should be looped
        //(The last waypoint connected to the first)
        public bool Looped;

        public LoopingPoints()
        {

        }
    }
}
