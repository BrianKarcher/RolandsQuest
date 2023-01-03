using RQ.Entity.Components;
using RQ.Physics.Components;
using System;

namespace RQ.Entity.AtomAction.Action
{
    [Serializable]
    public class PlayKillSoundAtom : PlaySoundOneShotAtom
    {
        public override void Start(IComponentRepository entity)
        {
            var damageComponent = entity.Components.GetComponent<DamageComponent>();
            base._audioClip = damageComponent.GetKillSound();
            base.Start(entity);
        }
    }
}
