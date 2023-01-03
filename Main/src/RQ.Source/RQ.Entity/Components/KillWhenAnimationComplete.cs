using RQ.Entity.Common;
using RQ.Entity.Components;
using RQ.Messaging;
using UnityEngine;

namespace RQ.Common.Components
{
    [AddComponentMenu("RQ/Components/Kill when Animation complete")]
    public class KillWhenAnimationComplete : MonoBehaviour
    {
        private long _animCompleteId;
        private IComponentRepository _entity;

        void Start()
        {
            _entity = GetComponent<SpriteBaseComponent>();
            _animCompleteId = MessageDispatcher2.Instance.StartListening("AnimationComplete", _entity.UniqueId, (data) =>
            {
                AnimComplete();
            });
            //var animator = spriteAnimator.GetComponent<AnimationComponent>();
            //if (animator == null)
            //    return;
            //animator.AnimComplete += AnimComplete;
        }

        void OnDestroy()
        {
            if (_entity != null && _entity.gameObject != null)
                MessageDispatcher2.Instance.StopListening("AnimationComplete", _entity.UniqueId, _animCompleteId);
            //if (spriteAnimator == null)
            //    return;
            //var animator = spriteAnimator.GetComponent<ISpriteAnimator>();
            //if (animator == null)
            //    return;
            //animator.AnimComplete -= AnimComplete;
        }

        void AnimComplete()
        {
            GameObject.Destroy(gameObject);
        }
    }
}
