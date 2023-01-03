using RQ.Controller.UI;
using UnityEngine;
namespace RQ.UI
{
    public class ButtonManager : MonoBehaviour, IButtonManager
    {
        //public event EventHandler DialogOkClikedEvent;
        private IUIManager _uiManager;
        public string ButtonClicked { get; set; }
        //public event Action DialogOkDelegate;
        //public event Action DialogCancelDelegate;

        void Awake()
        {
            if (_uiManager == null)
                _uiManager = GetComponent<IUIManager>();
        }

        public void OkCliked()
        {
            ButtonClicked = "Ok";
        }

        public void CancelClicked()
        {
            ButtonClicked = "Cancel";
        }

        public void LoadClicked()
        {
            ButtonClicked = "Load";
        }

        public void Reset()
        {
            ButtonClicked = string.Empty;
        }

        //public void NewGameClicked()
        //{
        //    _uiManager.ShowMessageBox("Hi!");
        //}

        //public void LoadGameClicked()
        //{
        //    Log.Info("Load Game clicked");
        //}

        //public void DialogOkClicked()
        //{
        //    // @todo Have this go through Messaging
        //    //if (DialogOkDelegate != null)
        //    //    DialogOkDelegate();
        //}

        //public void DialogCancelClicked()
        //{
        //    // @todo Have this go through Messaging
        //    //if (DialogCancelDelegate != null)
        //    //    DialogCancelDelegate();
        //}

        //public void ExitClicked()
        //{
        //    Application.Quit();
        //}

        public void MessageBoxOkClicked()
        {
            _uiManager.MessageBoxOkClicked();
            //UILabel label;
            //label.type
            //if (_eventDelegate != null)
            //    _eventDelegate();
            //if (_isTypewriter)
            //if (_uiManager.ConversationTypewriter.isActive)
            //{
            //    _uiManager.ConversationTypewriter.Finish();
            //    //_isTypewriter = false; // Typewriting effect has finished
            //}
            //else
            //{
            //    if (_uiManager.NguiDialogUI != null)
            //        _uiManager.NguiDialogUI.OnContinue(); // Let the dialogue system know the Continue button was clicked.
            //}
        }

        //public void MainMenu_SaveClicked()
        //{
        //    Log.Info("Save clicked");
        //    GameController._instance.SaveGame("Test.dat");
        //    //Persistence.SaveGame("Test.dat", gameData);
        //    _uiManager.ShowMessageBox("Game saved");
        //}

        //public void MainMenu_LoadClicked()
        //{
        //    Persistence.LoadGame("Test.dat");
        //}
    }
}
