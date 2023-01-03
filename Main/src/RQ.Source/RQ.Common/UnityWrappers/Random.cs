namespace RQ.UnityWrappers
{
    /// <summary>
    /// Used for wrapping Unit Random calls
    /// </summary>
    public class Random : IRandom
    {
        public float Range(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }
    }
}
