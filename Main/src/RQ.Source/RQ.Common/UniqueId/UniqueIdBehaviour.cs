using System;
using UnityEngine;

namespace RQ.Common.UniqueId
{
    [ExecuteInEditMode]
    public class UniqueIdBehaviour : MonoBehaviour
    {
        [UniqueIdentifier] // Treat this special in the editor.
        public string UniqueId; // A String representing our Guid

        void OnEnable()
        {
            if (!Application.isPlaying)
            {
                if (String.IsNullOrEmpty(this.UniqueId))
                    UniqueId = Guid.NewGuid().ToString();
                UniqueIdRegistry.Register(this.UniqueId, this.GetInstanceID());
            }
        }

        void OnDestroy()
        {
            if (!Application.isPlaying)
            {
                UniqueIdRegistry.Deregister(this.UniqueId);
            }
        }

        void Update()
        {
            if (!Application.isPlaying)
            {
                if (this.GetInstanceID() != UniqueIdRegistry.GetInstanceId(this.UniqueId))
                {
                    UniqueId = Guid.NewGuid().ToString();
                    UniqueIdRegistry.Register(this.UniqueId, this.GetInstanceID());
                }
            }
        }
    }
}
