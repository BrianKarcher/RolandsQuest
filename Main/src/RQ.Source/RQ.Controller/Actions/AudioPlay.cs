using RQ.Messaging;
using RQ.Model.Audio;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Audio Play")]
    public class AudioPlay : ActionBase
    {
        public AudioClip AudioClip;
        public float Volume = 1;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            var playOneShotData = new PlayOneShotData()
            {
                Clip = AudioClip,
                Volume = Volume
            };

            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, "Game Controller",
                Enums.Telegrams.PlayOneShot, playOneShotData);
            //GameController._instance.AudioSource.PlayOneShot(AudioClip);
        }
    }
}
