using System.Collections.Generic;
using RQ.Common;

namespace RQ.Model.Interfaces
{
    public interface ICollisionModifier
    {
        IList<IBaseObject> GetCollisionComponents();
    }
}
