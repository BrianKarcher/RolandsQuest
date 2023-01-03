using RQ.Model.Audio;
using RQ.Model.Interfaces;
using UnityEngine;

namespace RQ.Model.Item
{
    public interface IItemConfig : IRQConfig
    {
        PlaySoundInfo AcquireSound { get; }
        string AcquireText { get; }
        string Description { get; }
        Texture GridTexture { get; }
        ItemClass ItemClass { get; }
        string Title { get; }
        ItemType Type { get; }
        int Value { get; }
    }
}