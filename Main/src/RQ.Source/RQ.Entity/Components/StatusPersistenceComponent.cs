using RQ.Common;
using RQ.Common.Components;
using RQ.Common.Controllers;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Model.Enums;
using System;
using UnityEngine;

namespace RQ.Physics.Components
{
    /// <summary>
    /// Persists the state in a variable by name such as treasure chest "open" state
    /// </summary>
    [AddComponentMenu("RQ/Components/Status Persistence")]
    public class StatusPersistenceComponent : ComponentBase<StatusPersistenceComponent>
    {
        [SerializeField]
        private StateMachine _stateMachine;

        //[SerializeField]
        //private StateBase _state;
        [SerializeField]
        private VariableType _variableType;

        [SerializeField]
        [VariableSelector(VariableTypeField = "_variableType")]
        private string _variableUniqueId;

        public override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;

            var value = GetVariable();

            if (!String.IsNullOrEmpty(value))
            {
                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _stateMachine.UniqueId,
                    Enums.Telegrams.ChangeStateByName, value);
            }
        }

        //public override void OnDestroy()
        //{
        //    SetVariable(_stateMachine.GetCurrentState().name);
        //    base.OnDestroy();
        //}

        //public override bool HandleMessage(Telegram msg)
        //{
        //    if (base.HandleMessage(msg))
        //        return true;

        //    switch (msg.Msg)
        //    {
        //        case Enums.Telegrams.ChangeScene:
        //            SetVariable(_stateMachine.GetCurrentState().name);
        //            break;
        //    }

        //    return false;
        //}

        public string GetVariable()
        {
            return GameDataController.Instance.GetVariable(_variableType, _variableUniqueId);
        }

        public void SetVariable(string value)
        {
            GameDataController.Instance.SetVariable(_variableType, _variableUniqueId, value);
        }
    }
}
