namespace RQ.Controller.ManageScene
{
    public interface IDisplayDebugInfo
    {
        int FirePressCount { get; set; }
        string StoryScene { get; set; }
        int AverageFPS { get; set; }
    }
}
