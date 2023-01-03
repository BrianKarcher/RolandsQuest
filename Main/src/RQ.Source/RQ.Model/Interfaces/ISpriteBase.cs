using RQ.Serialization;
using System;
namespace RQ.Entity.Components
{
    public interface ISpriteBase
    {
        string UniqueId { get; set; }
        //RQ.Enums.EntityClass GetEntityClass();
        RQ.Enums.EntityType GetEntityType();
        void Reset();

        //RQ.Model.Enums.StatusPersistenceLength GetStatusPersistenceLength();
    }
}
