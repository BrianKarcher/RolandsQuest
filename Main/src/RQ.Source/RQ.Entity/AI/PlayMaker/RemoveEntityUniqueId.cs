using System;
using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Physics.Components;
using RQ.Extensions;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Records the entity in an array.")]
    public class RemoveEntityUniqueId : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        //[PM.Tooltip("The speed, or in technical terms: velocity magnitude")]
        public FsmArray storeArray;

        //[RequiredField]
        //[UIHint(UIHint.Variable)]
        ////[PM.Tooltip("The speed, or in technical terms: velocity magnitude")]
        //public FsmString storeResult;

        [RequiredField]
        [UIHint(UIHint.Variable)]
        //[PM.Tooltip("The speed, or in technical terms: velocity magnitude")]
        public FsmGameObject gameObject;

        //[PM.Tooltip("Repeat every frame.")]
        //public bool everyFrame;

        //public KillSelfAtom _killSelfAtom;
        //private IComponentRepository _entity;

        //public override void Reset()
        //{
        //    //gameObject = null;
        //    storeResult = null;
        //    everyFrame = false;
        //}

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            var collisionComponent = gameObject.Value.GetComponent<CollisionComponent>();
            if (collisionComponent == null)
            {
                Finish();
                return;
            }
            var repo = collisionComponent.GetComponentRepository();
            var uniqueId = repo.UniqueId;
            //_entity = rqSM.GetComponentRepository();
            //var uniqueid = _entity.UniqueId;
            //Array.
            if (storeArray.IsNone)
            {
                Finish();
                return;
            }
            var index = Array.IndexOf(storeArray.stringValues, uniqueId);

            if (index > -1)
            {
                storeArray.stringValues.RemoveAt(index);
                //Array.Resize(ref storeArray.stringValues, storeArray.stringValues.Length + 1);
                //storeArray.stringValues[storeArray.stringValues.Length - 1] = uniqueId;
                //storeArray.stringValues = storeArray.stringValues.Where(i => i != uniqueId).ToArray();

            }
            //if (!storeResult.IsNone)
            //    storeResult.Value = uniqueId;
            //_killSelfAtom.Start(_entity);
            //DoGetSpeed();

            //if (!everyFrame)
            //{
            //    Finish();
            //}
            Finish();
        }

        //public override void OnExit()
        //{
        //    base.OnExit();
        //    _killSelfAtom.End();
        //}

        //public override void OnUpdate()
        //{
        //    Tick();
        //}

        //private void Tick()
        //{
        //    //if (storeResult.IsNone) return;
        //    //_getSpeedAtom.OnUpdate();
        //    //storeResult.Value = _getSpeedAtom.Speed;
        //}
    }
}
