using RQ.Model.Enums;
using System;
using System.Collections.Generic;

namespace RQ.Model.Serialization.FSM
{
    [Serializable]
    public class StateTransitionRecordData
    {
        public List<ConditionExpressionData> ConditionExpressionDatas { get; set; }
        public string Name { get; set; }
        public bool Previous { get; set; }
        public bool Active { get; set; }
        public ConditionType ConditionType { get; set; }
    }
}
