using RQ.Physics;
using System;

namespace RQ.Editor.Point
{
    public interface IPointsEditor<POINT> where POINT : IPoint
    {
        event Action Dirty;
        event Action Repaint;
        event Action<int> RemoveAt;
        /// <summary>
        /// First is From, second is To
        /// </summary>
        //event Action<int, int> SwapPointsList;
        void OnEnable(IPointEditor pointEditor, IPoints<POINT> points);
        void OnDisable();
        void OnInspectorGUI(int index);
        void OnSceneGUI();
    }
}
