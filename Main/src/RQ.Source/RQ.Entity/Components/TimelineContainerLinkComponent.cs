using RQ.Common.Container;
using RQ.Entity.Common;
using System;
using UnityEngine;
using WellFired;

namespace RQ.Entity.Components
{
    [AddComponentMenu("RQ/Components/Timeline Container Link")]
    [ExecuteInEditMode]
    public class TimelineContainerLinkComponent : MonoBehaviour //ComponentPersistence<TimelineContainerLinkComponent>
    {
        public USTimelineContainer timelineContainer;
        public SpriteBaseComponent Entity;
        public string EntityUniqueId;

        public void Start()
        {
            //base.Start();
            // Keep cross-scene references intact
            if (Entity == null)
            {
                if (String.IsNullOrEmpty(EntityUniqueId))
                    return;
                var entity = EntityContainer._instance.GetEntity(EntityUniqueId);
                if (entity == null)
                {
                    Debug.Log("Entity cannot be found");
                    return;
                }
                Entity = entity as SpriteBaseComponent;
                timelineContainer.AffectedObject = entity.transform;
            }
        }
    }
}
