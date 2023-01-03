using RQ.Physics;
using System;

namespace RQ.Editor.Point
{
    public interface IPointEditor
    {
        event Action Dirty;
        event Action<int> RemoveAt;
        /// <summary>
        /// First is From, second is To
        /// </summary>
        event Action<int, int> SwapPoints;
        void OnEnable();
        void OnInspectorGUI(IPoint point, int index, bool isLast);
    }
}
