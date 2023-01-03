using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Physics
{
    [Serializable]
    public class Points<POINT> : /*List<POINT>, */IPoints<POINT>//, IEnumerable
        where POINT : IPoint, new()
    {
        [SerializeField]
        private string _name;
        public string Name { get { return _name; } set { _name = value; } }

        //public string fuck = "fuck me";
        [SerializeField]
        private List<POINT> _pointList = new List<POINT>();

        public IPoint Add(Vector3 vector)
        {
            POINT point = new POINT();
            point.Set(vector.x, vector.y);
            this.Add(point);
            return point;
        }

        public IPoint Add(POINT p)
        {
            int newId = 0;
            if (_pointList != null)
            {
                newId = GetNewPointListId();
                p.Id = newId;
                Debug.Log("Adding new point, Id = " + p.Id);
                _pointList.Add(p);
            }           
            
            return p;
        }

        public int GetNewPointListId()
        {
            int maxId = 0;
            if (_pointList != null && _pointList.Count != 0)
            {
                for (int i = 0; i < _pointList.Count; i++)
                {
                    var id = _pointList[i].Id;
                    if (id > maxId)
                        maxId = id;
                }
                //maxId = _pointList.Max(i => i.Id);
            }
            return maxId + 1;
        }

        public POINT this[int index]
        {
            get
            {
                return _pointList[index];
            }
            set
            {
                _pointList[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return _pointList.Count;
            }
        }

        public void RemoveAt(int index)
        {
            _pointList.RemoveAt(index);
        }

        //public IEnumerator GetEnumerator()
        //{
        //    return _pointsList.GetEnumerator();
        //}

        public List<POINT> GetPoints()
        {
            return _pointList;
        }
    }
}
