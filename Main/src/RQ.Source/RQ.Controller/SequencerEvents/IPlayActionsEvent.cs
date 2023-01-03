using UnityEngine;

namespace RQ.Controller.SequencerEvents
{
    public interface IPlayActionsEvent
    {
        GameObject AffectedObject { get; }
    }
}
