using UnityEngine;

namespace RQ.Common.Components
{
    [AddComponentMenu("RQ/Components/Dont Destroy On Load")]
    public class DontDestroyOnLoadComponent : BaseObject
    {
        public override void Awake()
        {
            base.Awake();
        }
    }
}
