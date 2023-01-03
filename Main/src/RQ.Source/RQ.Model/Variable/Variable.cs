using RQ.Model.Enums;
using System;

namespace RQ.Model
{
    [Serializable]
    public class Variable
    {
        public string Name;
        public string Value;
        public string UniqueId;
        //public StatusPersistenceLength Persistence;

        public Variable Clone()
        {
            return new Variable()
            {
                Name = Name,
                Value = Value,
                UniqueId = UniqueId
                //Persistence = Persistence
            };
        }
    }
}
