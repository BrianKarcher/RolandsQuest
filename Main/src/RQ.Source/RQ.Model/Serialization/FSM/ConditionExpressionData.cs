using System;

namespace RQ.Model.Serialization
{
    [Serializable]
    public class ConditionExpressionData
    {
        public string Condition2UniqueId { get; set; }
        public bool IsNot { get; set; }
    }
}
