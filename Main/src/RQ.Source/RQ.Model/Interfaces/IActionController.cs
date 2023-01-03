using RQ.Common;
using UnityEngine;

namespace RQ.Model.Interfaces
{
    public interface IActionController : IBaseObject
    {
        //bool CheckAndRun();
        Transform transform { get; }
    }
}
