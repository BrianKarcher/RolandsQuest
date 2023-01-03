using RQ.Audio;
using RQ.Model.Audio;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Audio Play 2")]
    public class AudioPlay2 : ActionBase
    {
        [SerializeField]
        PlaySoundInfo _soundInfo = null;

        private AudioComponent _audioComponent;

        public override void InitAction()
        {
            base.InitAction();
            var entity = GetEntity();
            if (entity == null)
                return;
            _audioComponent = entity.Components.GetComponent<AudioComponent>();
        }

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            AudioHelper.PlaySound(this.UniqueId, _soundInfo, _audioComponent);
        }
    }
}
