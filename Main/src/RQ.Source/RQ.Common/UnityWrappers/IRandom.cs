namespace RQ.UnityWrappers
{
    /// <summary>
    /// Used for Dependency Injection to choose your randomizer
    /// Or, if you choose, to mock one for testing
    /// </summary>
    public interface IRandom
    {
        float Range(float min, float max);
    }
}
