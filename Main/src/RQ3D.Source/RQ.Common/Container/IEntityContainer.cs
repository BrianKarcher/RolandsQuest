using System;
using System.Collections.Generic;
using System.Text;
using RQ.Common.Components;
using RQ.Messaging;

namespace RQ.Common.Container
{
    public interface IEntityContainer
    {
        void AddEntities(IEnumerable<IMessagingObject> entities);
        void AddEntity(IMessagingObject entity);
        bool Contains(string id);
        Dictionary<string, IMessagingObject> GetAllEntities();
        Dictionary<string, IMessagingObject> GetBosses();
        IMessagingObject GetCompanionCharacter();
        //IEnumerable<IEntity> GetEntitiesFromTag(string tag);
        IMessagingObject GetEntityFromTag(string tag);
        //IEnumerable<IEntity> GetEntitiesFromTags(params string[] tags);
        IMessagingObject GetEntity(string id);
        T GetEntity<T>(string id) where T : class;
        IMessagingObject GetMainCharacter();
        void RemoveEntity(IMessagingObject entity);
        void ResetEntityList();
        void SetBossCharacter(IMessagingObject repo);
        void SetCompanionCharacter(IMessagingObject repo);
        void SetMainCharacter(IMessagingObject repo);
    }
}
