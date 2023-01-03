using RQ.Common.Container;
using UnityEngine;

namespace RQ.Controller.StatesV2.GameManager
{
    [AddComponentMenu("RQ/States/State/Game Manager/AppStart")]
    public class AppStart : GMBase
    {
        [SerializeField]
        private string _uiBeginMessage;

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Entering AppStart State");
            GameController.Instance.AppStart();

            Complete();
        }
    }
}
