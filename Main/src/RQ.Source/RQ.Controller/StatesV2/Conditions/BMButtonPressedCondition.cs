using RQ.UI;
using System;
using UnityEngine;

namespace RQ.FSM.V2.Conditionals
{
    [Obsolete]
    [AddComponentMenu("RQ/States/Conditions/BM Button Pressed")]
    public class BMButtonPressedCondition : StateTransitionConditionBase//, IMessageHandler
    {
        private ButtonManager _buttonManager = null;
        [SerializeField]
        private string _buttonName;

        private ButtonManager ButtonManager
        {
            get
            {
                if (_buttonManager == null)
                    _buttonManager = GameController.Instance.UIManager.GetButtonManager() as ButtonManager;

                return _buttonManager;
            }
        }

        public override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;

            //_buttonManager = 
        }

        public override bool TestCondition(IStateMachine stateMachine)
        {
            if (!base.TestCondition(stateMachine))
                return false;

            return ButtonManager.ButtonClicked == _buttonName;
        }

        public override void ConditionEnter(IStateMachine stateMachine)
        {
            base.ConditionEnter(stateMachine);
            ButtonManager.Reset();
        }
    }
}
