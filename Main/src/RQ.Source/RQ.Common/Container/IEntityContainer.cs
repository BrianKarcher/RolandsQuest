using System.Collections.Generic;

namespace RQ.Common.Container
{
    public interface IEntityContainer
    {
        void AddEntities(IEnumerable<IEntity> entities);
        void AddEntity(IEntity entity);
        bool Contains(string id);
        Dictionary<string, IEntity> GetAllEntities();
        Dictionary<string, IEntity> GetBosses();
        IEntity GetCompanionCharacter();
        //IEnumerable<IEntity> GetEntitiesFromTag(string tag);
        IEntity GetEntityFromTag(string tag);
        //IEnumerable<IEntity> GetEntitiesFromTags(params string[] tags);
        IEntity GetEntity(string id);
        T GetEntity<T>(string id) where T : class;
        IEntity GetMainCharacter();
        void RemoveEntity(IEntity entity);
        void ResetEntityList();
        void SetBossCharacter(IEntity repo);
        void SetCompanionCharacter(IEntity repo);
        void SetMainCharacter(IEntity repo);
    }
}