using RQ.Common.Components;
using RQ.Common.Controllers;
using RQ.Messaging;
using RQ.Model;
using UnityEngine;

namespace RQ.Physics.Components
{
    [AddComponentMenu("RQ/Components/Death Persistence")]
    public class DeathPersistenceComponent : ComponentBase<DeathPersistenceComponent>
    {
        //[SerializeField]
        //private EntityStatsData _entityStats = new EntityStatsData();

        //public EntityStatsData GetEntityStats()
        //{
        //    return _entityStats;
        //}

        //private void SendEntityStats(string senderId)
        //{
        //    // Send the stats to the requester
        //    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId,
        //        senderId, Enums.Telegrams.SetEntityStats, _entityStats);
        //    //return _entityStats;
        //}

        public override bool HandleMessage(Telegram msg)
        {
            if (base.HandleMessage(msg))
                return true;

            switch (msg.Msg)
            {
                case Enums.Telegrams.EntityDied:
                    AddEntityToDeathPersistence();
                    //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, "Game Controller",
                    //    Enums.Telegrams.PersistDeath, null);
                    break;
            }
            return false;
        }

        private void AddEntityToDeathPersistence()
        {
            var deathPersistence = GetSceneDeathData();
            if (deathPersistence == null)
                return;

            deathPersistence.DeadEntities.Add(_componentRepositoryId);
        }

        private SceneDeathData GetSceneDeathData()
        {
            foreach (var deathScene in GameDataController.Instance.Data.SceneDeathDatas)
            {
                if (deathScene.SceneUniqueId == GameDataController.Instance.Data.CurrentSceneUniqueId)
                {
                    return deathScene;
                }
            }
            //return GameDataController.Instance.Data.SceneDeathDatas.FirstOrDefault(i => i.SceneUniqueId
            //    == GameDataController.Instance.Data.CurrentSceneUniqueId);
            return null;
        }

        public override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;
            var deathPersistence = GetSceneDeathData();
            if (deathPersistence == null)
                return;

            //if (deathPersistence.DeadEntities.Contains(_componentRepositoryId))
            //    GameObject.Destroy(transform.gameObject);
            if (deathPersistence.DeadEntities.Contains(_componentRepositoryId))
            {
                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _componentRepositoryId,
                    Enums.Telegrams.Kill, null);
            }
        }
    }
}
