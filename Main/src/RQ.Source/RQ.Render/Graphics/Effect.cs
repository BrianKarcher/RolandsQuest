using UnityEngine;
using System.Collections;
using System;
using RQ.Messaging;

namespace RQ
{
	public class Effect : MessagingObject
	{
		public bool IsFinished {get;set;}
        //private float _killTimerLength;
		public float KillTimer;
        protected Action<Effect> _callBack;

		//public float KillTimer
		//{
		//	get
		//	{
		//		return _killTimerLength;
		//	}
		//	set
		//	{
		//		_killTimerLength = value;
		//		_killTimer = new Stopwatch();
		//	}
		//}

        protected bool _isActive { get; set; }

        //private Stopwatch _killTimer;
		
		public override void Awake ()
		{
            this.UniqueId = Guid.NewGuid().ToString();
            base.Awake();
			KillTimer = 0f;
			IsFinished = false;
			_callBack = null;            
		}

		protected void Begin()
		{
            if (KillTimer != 0f)
            {
                MessageDispatcher.Instance.DispatchMsg(KillTimer, this.UniqueId, 
                    this.UniqueId, Enums.Telegrams.Kill, null);
            }
		}
	}
}