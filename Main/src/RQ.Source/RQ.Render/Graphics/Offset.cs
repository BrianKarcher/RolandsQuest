using UnityEngine;

namespace RQ.Render.Graphics
{
    [AddComponentMenu("RQ/Graphics/Offset")]
    public class Offset : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _offset = Vector3.zero;

        public void Start()
        {
            transform.localPosition = transform.localPosition + _offset;
            //this.transform.localPosition = new Vector3(transform.localPosition.x + _offset.x,
            //    transform.localPosition.y + _offset.y, transform.localPosition.z);
        }
    }
}
