using System;
using UnityEngine;

namespace RQ.Common.UniqueIdInt
{
    [ExecuteInEditMode]
    public class UniqueIdIntBehaviour : MonoBehaviour
    {
        [UniqueIdentifierInt] // Treat this special in the editor.
        public string UniqueId; // A String representing our Guid

        void OnEnable()
        {
            if (!Application.isPlaying)
            {
                if (String.IsNullOrEmpty(this.UniqueId))
                    UniqueId = Guid.NewGuid().ToString();
                UniqueIdIntRegistry.Register(this.UniqueId, this.GetInstanceID());
            }
        }

        void OnDestroy()
        {
            if (!Application.isPlaying)
            {
                UniqueIdIntRegistry.Deregister(this.UniqueId);
            }
        }

        void Update()
        {
            if (!Application.isPlaying)
            {
                if (this.GetInstanceID() != UniqueIdIntRegistry.GetInstanceId(this.UniqueId))
                {
                    UniqueId = Guid.NewGuid().ToString();
                    UniqueIdIntRegistry.Register(this.UniqueId, this.GetInstanceID());
                }
            }
        }
    }
}
