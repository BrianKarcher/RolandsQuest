using RQ.Common;
using UnityEngine;

namespace RQ.Model.Interfaces
{
    public interface IAnimationComponent : IBaseObject
    {
        Vector3 GetFacingDirectionVector();
        Direction GetFacingDirection();
    }
}
