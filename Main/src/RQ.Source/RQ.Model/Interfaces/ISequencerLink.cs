using System;

namespace RQ.Model.Interfaces
{
    public interface ISequencerLink
    {
        string name { get; set; }
        void Play();
        void Pause();
        void Stop();
        event Action OnSequenceComplete;
    }
}
