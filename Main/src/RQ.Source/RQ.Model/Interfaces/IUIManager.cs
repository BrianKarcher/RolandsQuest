using RQ.Model.Enums;
using RQ.Model.UI;
using System.Collections.Generic;
using RQ.Model.Interfaces;
using RQ.UI;
using UnityEngine;

namespace RQ.Controller.UI
{
    public interface IUIManager
    {
        //void EnablePanelButtons(IEnumerable<RQ.Enum.Panels> panels, bool state);
        void EnablePanelButtons(IEnumerable<string> panels, bool state);

        //void ShowPanel(RQ.Enum.Panels panel, bool state);
        void ShowPanel(string panel, bool state);
        void MessageBoxOkClicked();
        void DisplayModal(string text);
        ISequencerLink CurrentSequence { get; set; }
        SaveSlotData ClickedSaveSlotData { get; set; }
        IButtonManager GetButtonManager();
        void ClearPersistenceGrid();
        //void ClearInputControlGrid();
        void SelectContinueButtion();
        void ClearTogglesForPanel(string panel);
        void SetupPersistenceGrid(string mainLabelText, SaveOrLoad savePanelState,
            bool newGameSlot);
        //void SetupInputControlGrid();
        void SetModalText(string text);
        void SetupModal(bool hasCancelButton);
        void HoverContinueButton();
        //void AddFollowingLabel(string text, Transform target);
        IHudController GetHudController();
    }
}
