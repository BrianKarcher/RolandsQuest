using System.Collections.Generic;

namespace RQ.Model.ObjectPool
{
    public enum ObjectPoolType
    {
        None = 0,
        IEntityList = 1,
        StringList = 2,
        HashSetString = 3,
        QueueActionTelegram2 = 4,
        RaycastHit2DList = 5,
        Vector3List = 6,
        IntList = 7,
        ListOfMessageEvents = 8
    }

    public class ObjectPoolTypeComparer : IEqualityComparer<ObjectPoolType>
    {

        public bool Equals(ObjectPoolType x, ObjectPoolType y)
        {

            return x == y;

        }

        public int GetHashCode(ObjectPoolType x)
        {

            return (int)x;

        }

    }
}
