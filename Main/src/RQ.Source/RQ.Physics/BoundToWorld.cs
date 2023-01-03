using UnityEngine;

namespace RQ
{
    public class BoundToWorld : MonoBehaviour
    {
        protected virtual void FixedUpdate()
        {
            Vector3 pos = transform.position; //transform.position;
            if (pos.x < 0)
                pos.x = 0;
            if (pos.y < 0)
                pos.y = 0;

            transform.position = pos;
        }
    }
}
