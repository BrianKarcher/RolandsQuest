using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Messaging;
using RQ.Model.Item;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Entity.StatesV3.Conditions
{
    //[AddComponentMenu("RQ/States/Conditions/Animation Complete")]
    public class ItemUsedConditionConfig : StateTransitionConditionBaseConfig
    {
        [SerializeField]
        private ItemConfig _itemConfig;
        private Dictionary<string, long> _itemUsedIds = new Dictionary<string, long>();
        private Action<Telegram2> _itemUsedDelegate;
        private string _itemUsedKeyName;

        public override void ConditionEnter(IStateMachine stateMachine)
        {
            base.ConditionEnter(stateMachine);
            var entity = stateMachine.GetEntity().GetComponent<IComponentRepository>();
            if (entity == null)
                throw new Exception("ItemUsedCondition - Entity is empty");

            //Listen(stateMachine);
        }

        public override void ConditionInit(IStateMachine stateMachine)
        {
            base.ConditionInit(stateMachine);
            if (String.IsNullOrEmpty(_itemUsedKeyName))
            {
                var entity = stateMachine.GetComponentRepository();
                _itemUsedKeyName = entity.UniqueId;
            }
            if (_itemUsedDelegate == null)
            {
                _itemUsedDelegate = (data) =>
                {
                    var itemUniqueId = (string) data.ExtraInfo;
                    // Make sure it is the right item
                    if (itemUniqueId != _itemConfig.UniqueId)
                        return;
                    SetIsConditionSatisfied(stateMachine, true);
                };
            }
            Listen(stateMachine);
        }

        public void Listen(IStateMachine stateMachine)
        {
            var entity = stateMachine.GetEntity().GetComponent<IComponentRepository>();
            if (entity == null)
                throw new Exception("Message Received Condition - Entity is empty");

            // TODO This is HORRIBLE, move this logic elsewhere
            MessageDispatcher2.Instance.DispatchMsg("ListenToItemUse", 0f, entity.UniqueId,
                "Inventory Controller", _itemConfig.UniqueId);

            var id = MessageDispatcher2.Instance.StartListening("ItemUsed", entity.UniqueId, _itemUsedDelegate);
            _itemUsedIds[_itemUsedKeyName] = id;
            //entity.StartListening("ItemUsed", this.UniqueId, _itemUsedDelegate);
        }

        public override void ConditionExit(IStateMachine stateMachine)
        {
            base.ConditionExit(stateMachine);
            var entity = stateMachine.GetEntity().GetComponent<IComponentRepository>();
            if (entity == null)
                throw new Exception("Message Received Condition - Entity is empty");
            MessageDispatcher2.Instance.DispatchMsg("RemoveItemUseListener", 0f, entity.UniqueId,
                "Inventory Controller", _itemConfig.UniqueId);

            MessageDispatcher2.Instance.StopListening("ItemUsed", entity.UniqueId, _itemUsedIds[_itemUsedKeyName]);
            _itemUsedIds.Remove(_itemUsedKeyName);
            //entity.StopListening("ItemUsed", this.UniqueId);
        }

        public override bool TestCondition(IStateMachine stateMachine)
        {
            // TODO Make this event-based to speed it up.
            return GetIsConditionSatisfied(stateMachine);
        }
    }
}
