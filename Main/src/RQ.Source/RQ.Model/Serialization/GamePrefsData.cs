namespace RQ.Model.Serialization
{
    public class GamePrefsData
    {
        public float MusicVolume { get; set; }
        public float SoundEffectVolume { get; set; }
        //public Dictionary<InputAction, Dictionary<InputType, InputCommand>> InputCommands { get; set; }
        //public Dictionary<InputAction, InputCommand> ControllerInputCommands { get; set; }
        //public bool IsControllerPresent { get; set; }
        //public int ScreenWidth { get; set; }
        //public int ScreenHeight { get; set; }
        //public bool IsFullScreen { get; set; }

        public GamePrefsData Clone()
        {
            return new GamePrefsData()
            {
                MusicVolume = MusicVolume,
                SoundEffectVolume = SoundEffectVolume
            };
        }

        /// <summary>
        /// Does a value transfer instead of setting the reference
        /// </summary>
        /// <param name="data"></param>
        public void SetGamePrefs(GamePrefsData data)
        {            
            MusicVolume = data.MusicVolume;
            SoundEffectVolume = data.SoundEffectVolume;
        }
    }
}
