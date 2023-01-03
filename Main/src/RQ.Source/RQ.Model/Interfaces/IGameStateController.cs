using RQ.Model.Serialization;

namespace RQ
{
    public interface IGameStateController
    {
        GamePrefsData GetGamePrefs();
        void SaveGamePrefsToFile();
    }
}