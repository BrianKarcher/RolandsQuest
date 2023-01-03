using System.Collections.Generic;
using UnityEngine;

namespace RQ.Physics
{
    public interface IPoints<POINT> /*: IList<POINT>*/// : IEnumerable
        where POINT : IPoint
    {
        POINT this[int index] { get; set; }
        int Count { get; }
        string Name { get; set; }
        //List<POINT> Points { get; set; }
        IPoint Add(Vector3 vector);
        IPoint Add(POINT p);
        void RemoveAt(int index);
        List<POINT> GetPoints();
    }
}
