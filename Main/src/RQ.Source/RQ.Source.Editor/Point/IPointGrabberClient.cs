using UnityEngine;

namespace RQ.Editor
{
    public interface IPointGrabberClient
    {
        void SetPosition(Vector3 pos);

        void PointGrabberWindowClosed();
    }
}
