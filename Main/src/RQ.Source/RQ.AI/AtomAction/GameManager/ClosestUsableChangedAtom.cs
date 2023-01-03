//using RQ.Entity.AtomAction;
//using RQ.Entity.Components;
//using System;
//using RQ.Common.Controllers;
//using UnityEngine;

//namespace RQ.AI.AtomAction.GameManager
//{
//    [Serializable]
//    public class ClosestUsableChangedAtom : AtomActionBase
//    {
//        private string _closestUsable;

//        public override void Start(IComponentRepository entity)
//        {
//            base.Start(entity);
//            Debug.Log("Entering AppStart State");
//            GameController.Instance.AppStart();
//        }

//        public override void End()
//        {
//        }

//        public override AtomActionResults OnUpdate()
//        {
//            var currentObject = GameDataController.Instance.Data.UsableContainer.CurrentUsableObject;
//            if (_closestUsable != currentObject)
//            {
//                _closestUsable = currentObject;
//                _isRunning = false;
//                //base.SetIsConditionSatisfied(true);
//            }

//            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
//        }
//    }
//}
