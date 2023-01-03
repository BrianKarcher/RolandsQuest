using UnityEngine;

namespace RQ.Common
{
    public interface IGameData
    {
        LayerMask GetLayerMask(Enum.LevelLayer levelLayer);
    }
}
