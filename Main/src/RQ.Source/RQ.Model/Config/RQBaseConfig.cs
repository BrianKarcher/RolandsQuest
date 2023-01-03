using RQ.Common.UniqueId;
using RQ.Model.Interfaces;
using UnityEngine;

namespace RQ.Common.Config
{
    public class RQBaseConfig : ScriptableObject, IRQConfig
    {
        [UniqueIdentifier]
        public string UniqueId;

        public string GetUniqueId()
        {
            return UniqueId;
        }
        //protected StateMachine _stateMachine;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
