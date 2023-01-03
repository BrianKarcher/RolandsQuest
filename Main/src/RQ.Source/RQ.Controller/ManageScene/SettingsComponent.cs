//using RQ.Messaging;
//using RQ.Model.Serialization;
//using UnityEngine;

//namespace RQ.Controller.ManageScene
//{
//    [AddComponentMenu("RQ/Components/Settings Component")]
//    public class SettingsComponent : MessagingObject
//    {
//        public string SaveMessage;
//        public string CancelMessage;

//        // Need to reset the prefs if Cancel is clicked.
//        private GamePrefsData _originalGamePrefsData;


//        public override void OnEnable()
//        {
//            base.OnEnable();
//            _originalGamePrefsData = GameStateController.Instance.GetGamePrefs().Clone();
//        }

//        public void SetMusicVolume(float volume)
//        {
//            GameStateController.Instance.GetGamePrefs().MusicVolume = volume;
//            MessageDispatcher2.Instance.DispatchMsg("UpdateVolume", 0f, this.UniqueId, null, null);
//        }

//        public void SetSoundEffectVolume(float volume)
//        {
//            GameStateController.Instance.GetGamePrefs().SoundEffectVolume = volume;
//            MessageDispatcher2.Instance.DispatchMsg("UpdateVolume", 0f, this.UniqueId, null, null);
//        }

//        public void Save()
//        {
//            GameStateController.Instance.SaveGamePrefsToFile();
//            // New original in case Settings gets reentered
//            //_originalGamePrefsData = GameStateController.Instance.GetGamePrefs().Clone();
//            MessageDispatcher2.Instance.DispatchMsg(SaveMessage, 0f, this.UniqueId, "Game Controller", null);
//        }

//        public void Cancel()
//        {
//            GameStateController.Instance.GetGamePrefs().SetGamePrefs(_originalGamePrefsData);
//            // Volume got reset, send the update to revert to the original value
//            MessageDispatcher2.Instance.DispatchMsg("UpdateVolume", 0f, this.UniqueId, null, null);
//            MessageDispatcher2.Instance.DispatchMsg(CancelMessage, 0f, this.UniqueId, "Game Controller", null);
//        }
//    }
//}
