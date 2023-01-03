using RQ.Entity.Components;
using RQ.Physics.Components;
using System;

namespace RQ.Entity.AtomAction.Action
{
    [Serializable]
    public class PlayDamageSoundAtom : PlaySoundOneShotAtom
    {
        public override void Start(IComponentRepository entity)
        {
            var damageComponent = entity.Components.GetComponent<DamageComponent>();
            base._audioClip = damageComponent.GetDamageSound();
            base.Start(entity);
        }
    }
}
