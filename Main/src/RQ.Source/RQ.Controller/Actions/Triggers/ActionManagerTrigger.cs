using RQ.Entity.Common;
using UnityEngine;

namespace RQ.Controller.Actions.Triggers
{
    [AddComponentMenu("RQ/Action/Triggers/Action Manager Trigger")]
    public class ActionManagerTrigger : MonoBehaviour
    {
        public ActionSequence ActionManager;

        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            var entity = other.GetComponent<SpriteBaseComponent>();
            if (entity != null)
            {
                // Only the player can trigger a scene change
                if (entity.GetEntityType() == Enums.EntityType.Player)
                {
                    ActionManager.CheckAndRun();
                }
            }
        }
    }
}
