using UnityEngine;

namespace RQ.Entity.UI
{
    public class EntityUIBase : MonoBehaviour
    {
        public string GetName()
        {
            if (transform.parent != null)
                return transform.parent.name;
            else
                return name;
        }
    }
}
