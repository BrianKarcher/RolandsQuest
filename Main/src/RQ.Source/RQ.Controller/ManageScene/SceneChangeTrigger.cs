using RQ.Entity.Common;
using UnityEngine;

namespace RQ.Triggers
{
    public class SceneChangeTrigger : MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D other)
        {
            var entity = other.GetComponent<SpriteBaseComponent>();
            if (entity != null)
            {
                // Only the player can trigger a scene change
                if (entity.GetEntityType() == Enums.EntityType.Player)
                {
                    // @todo Change scenes
                }
            }
        }
    }
}
