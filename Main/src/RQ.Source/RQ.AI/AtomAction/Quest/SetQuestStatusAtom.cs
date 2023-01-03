using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Model;
using System;
using RQ.Animation;
using RQ.Common.Container;
using RQ.Physics.Components;
using UnityEngine;
using RQ.Entity;
using RQ.Model.Game_Data.Quest;
using RQ.Common.Controllers;

namespace RQ.AI.Action
{
    [Serializable]
    public class SetQuestStatusAtom : AtomActionBase
    {
        public QuestConfig QuestConfig;

        public int Id;

        public QuestStatus Status;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            bool found = GameDataController.Instance.Data.QuestDatas.TryGetValue(QuestConfig.UniqueId, 
                out var questData);
            if (!found)
            {
                Debug.LogError($"Could not find quest {QuestConfig.name}");
                return;
            }
            for (int i = 0; i < questData.QuestEntryDatas.Count; i++)
            {
                var questEntry = questData.QuestEntryDatas[i];
                if (questEntry.Id == Id)
                {
                    questEntry.Status = Status;
                    return;
                }
            }
            Debug.LogError($"Could not find quest entry {QuestConfig.name}-{Id}");
        }

        public override void End()
        {
            base.End();
        }

        public override AtomActionResults OnUpdate()
        {
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
