//using RQ.Animation;
//using RQ.Entity.Common;
//using RQ.FSM;
//using RQ.FSM.Door;
//using RQ.Serialization;
//using System;
//using System.Collections.Generic;

//namespace RQ.Entity.Sprites
//{
//    [Serializable]
//    public class Door : BaseGameEntity<Door>
//    {
//        public Enum.Door StartState;
//        //public DoorRenderer Animator;

//        //public override void Awake()
//        //{
//        //    //base.SetSpriteAnimator(Animator);
//        //    base.Awake();
//        //}

//        public override void Deserialize(EntitySerializedData entityData, Dictionary<int, int> entityMap)
//        {
//            base.Deserialize(entityData, entityMap);
//            //var state = StateFactory.CreateDoorState(entityData.CurrentState);
//            //GetStateMachine().SetCurrentState(state);
//        }

//        public virtual void OpenDoor()
//        {
//            //GetStateMachine().ChangeState(Open.Instance);
//        }
//    }
//}
