namespace RQ.UnityWrappers
{
    public class Time : ITime
    {
        public float time()
        {
            return UnityEngine.Time.time;
        }
    }
}
