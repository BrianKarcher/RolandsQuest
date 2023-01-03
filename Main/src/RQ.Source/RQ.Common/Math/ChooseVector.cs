using UnityEngine;

namespace RQ.Math
{
    public static class Vector
    {
        /// <summary>
        /// Randomly chooses a vector in any 360 degree direction
        /// </summary>
        /// <returns>The vector.</returns>
        public static Vector3 ChooseVector()
        {
            float angle = UnityEngine.Random.Range(0, 360);
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            return q * Vector3.up;
        }
    }
}
