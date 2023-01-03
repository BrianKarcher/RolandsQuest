using RQ.Messaging;
using RQ.Model;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Effect/Lightning")]
    public class Lightning : AnimatorState
    {
        [SerializeField]
        private float _duration;
        [SerializeField]
        private Color _tweenToColor;
        //public override void SetupState()
        //{
        //    base.SetupState();
        //    if (!Application.isPlaying)
        //        return;
            
        //}

        //private const string StateName = "ShootObject";

        public override void Enter()
        {
            base.Enter();
            //var tweenColor = TweenColor.current as TweenColor;
            //var currentColor = tweenColor.value;
            var tweenToWhiteColorInfo = new TweenToColorInfo(Color.white, 0f, 0f);
            MessageDispatcher2.Instance.DispatchMsg("TweenToColor", 0f, this.UniqueId, 
                "Graphics Engine", tweenToWhiteColorInfo);
            var tweenToCurrentColorInfo = new TweenToColorInfo(_tweenToColor, 0f, _duration);
            MessageDispatcher2.Instance.DispatchMsg("TweenToColor", 0f, this.UniqueId,
                "Graphics Engine", tweenToCurrentColorInfo);
            //var tween = TweenColor.Begin(_overlayColorWindow.gameObject, 0f,
            //    colorInfo.Color);
        }

        //public override void Exit()
        //{
        //    base.Exit();
            
        //}

        //public override void Serialize(Serialization.EntitySerializedData entitySerializedData)
        //{
        //    base.Serialize(entitySerializedData);
        //    //if (!entitySerializedData.Datas.ContainsKey(GetName()))
        //    base.SerializeComponent(entitySerializedData, Data);
        //}

        //public override void Deserialize(Serialization.EntitySerializedData entitySerializedData)
        //{
        //    base.Deserialize(entitySerializedData);
        //    Data = base.DeserializeComponent<SmartFollowData>(entitySerializedData);
            
        //    //Data = Persistence.DeserializeObject<SmartFollowData>(entitySerializedData.Datas[GetName()]);
        //}
    }
}
