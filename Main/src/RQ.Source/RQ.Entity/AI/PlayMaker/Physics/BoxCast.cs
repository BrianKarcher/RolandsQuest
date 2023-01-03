using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.Model.Interfaces;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Records results from a Box Cast.")]
    public class BoxCast : FsmStateAction
    {
        //[RequiredField]
        //[UIHint(UIHint.Variable)]
        //[PM.Tooltip("The speed, or in technical terms: velocity magnitude")]
        //public FsmFloat storeResult;

        //[PM.Tooltip("Repeat every frame.")]
        //public bool everyFrame;

        [UIHint(UIHint.Tag)]
        [PM.Tooltip("Filter by Tag.")]
        public FsmString collideTag;

        [UIHint(UIHint.Layer)]
        [PM.Tooltip("Filter by Tag.")]
        public FsmInt collideLayer;

        [PM.Tooltip("Event to send if the trigger event is detected.")]
        public FsmEvent sendEvent;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Store the GameObject that collided with the Owner of this FSM.")]
        public FsmArray storeCollider;

        private ICollisionComponent _collisionComponent;
        //public FsmGameObject storeCollider;


        public BoxCastAtom _boxCastAtom;
        private IComponentRepository _entity;

        public override void Reset()
        {
            //trigger = Trigger2DType.OnTriggerEnter2D;
            //collideTag = "Untagged";
            sendEvent = null;
            storeCollider = null;
        }

        //public override void OnPreprocess()
        //{
        //    Fsm.HandleTriggerEnter2D = true;
        //    //switch (trigger)
        //    //{
        //    //    case Trigger2DType.OnTriggerEnter2D:
        //    //        Fsm.HandleTriggerEnter2D = true;
        //    //        break;
        //    //    case Trigger2DType.OnTriggerStay2D:
        //    //        Fsm.HandleTriggerStay2D = true;
        //    //        break;
        //    //    case Trigger2DType.OnTriggerExit2D:
        //    //        Fsm.HandleTriggerExit2D = true;
        //    //        break;
        //    //}
        //}

        //public override void DoTriggerEnter2D(Collider2D other)
        //{
        //    //if (trigger == Trigger2DType.OnTriggerEnter2D)
        //    //{
        //    if (other.gameObject.tag == collideTag.Value)
        //    {
        //        Debug.Log("DoTriggerEnter " + other.name);
        //        //StoreCollisionInfo(other);
        //        //Fsm.Event(sendEvent);
        //    }
        //    //}
        //}

        //public void StartListening(IComponentRepository entity)
        //{
        //    MessageDispatcher2.Instance.StartListening("TriggerEnter", entity.UniqueId, (data) =>
        //    {
        //        Debug.Log("Trigger Message " + entity.name + " " + data.SenderId);
        //    });
        //}

        //public void StopListening()
        //{

        //}

        //public void StoreCollisionInfo(string uniqueId)
        //{
        //    if (storeCollider.stringValues.Contains(uniqueId))
        //        return;
        //    Array.Resize(ref storeCollider.stringValues, storeCollider.stringValues.Length + 1);
        //    storeCollider.stringValues[storeCollider.stringValues.Length - 1] = uniqueId;
        //    //var values = storeCollider.stringValues.ToList();
        //}

        public override void OnEnter()
        {
            base.OnEnter();
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _collisionComponent = _entity.Components.GetComponent<ICollisionComponent>();

            //Debug.Log("Layer " + collideLayer);

            _boxCastAtom._targetTags = new string[] { collideTag.Value };
            _boxCastAtom._layer = collideLayer.Value;
            _boxCastAtom.Start(_entity);
            var colliderData = _boxCastAtom.GetColliderData();
            if (colliderData != null)
            {
                string[] tempUniqueIds = new string[colliderData.Count];
                for (int i = 0; i < colliderData.Count; i++)
                {
                    tempUniqueIds[i] = colliderData[i].EntityUniqueId;
                }
                storeCollider.stringValues = tempUniqueIds;

                //storeCollider.stringValues = colliderData.Select(i => i.EntityUniqueId).ToArray();
            }


            //if (!everyFrame)
            //{
            //    Finish();
            //}
            Finish();
        }



        //public override void OnExit()
        //{
        //    base.OnExit();
        //    //_killSelfAtom.End();
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
