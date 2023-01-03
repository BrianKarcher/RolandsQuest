using System.Collections.Generic;

namespace RQ.Physics
{
    public interface IPointsList<POINTS, POINT>
        where POINTS : IPoints<POINT>
        where POINT : IPoint
    {
        List<POINTS> PointsList
        {
            get;
            set;
        }
        //POINT this[int index] { get; set; }
        //int Count { get; }
        //string Name { get; set; }
        ////List<POINT> Points { get; set; }
        //void Add(Vector3 vector);
        //void Add(POINT p);
        //void RemoveAt(int index);
    }
}
