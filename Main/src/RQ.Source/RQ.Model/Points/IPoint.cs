using UnityEngine;

namespace RQ.Physics
{
    public interface IPoint
    {
        //Vector2D Point { get; set; }
        //float X { get; set; }
        //float Y { get; set; }
        int Id { get; set; }
        void Set(float x, float y);
        Vector2D Get();
        Vector3 ToVector3(float z);
        //void OnInspectorGUI();
    }
}
