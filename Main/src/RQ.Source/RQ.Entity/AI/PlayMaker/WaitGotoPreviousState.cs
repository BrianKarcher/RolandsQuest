using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using UnityEngine;

namespace RQ.Entity.AI.PlayMaker
{
    [ActionCategory("RQ.Time")]
    [PM.Tooltip("Delays a State from finishing by the specified time. NOTE: Other actions continue, but FINISHED can't happen before Time.")]
    public class WaitGotoPreviousState : FsmStateAction
    {
        [RequiredField]
        public FsmFloat time;
        private float timer;

        public override void Reset()
        {
            time = 1f;
        }

        public override void OnEnter()
        {
            if (time.Value <= 0)
            {
                if (Fsm.PreviousActiveState != null)
                {
                    Log("Goto Previous State: " + Fsm.PreviousActiveState.Name);

                    Fsm.GotoPreviousState();
                }

                Finish();
                return;
            }

            timer = 0f;
        }

        public override void OnUpdate()
        {
            // update time


            timer += Time.deltaTime;


            if (timer >= time.Value)
            {
                if (Fsm.PreviousActiveState != null)
                {
                    Log("Goto Previous State: " + Fsm.PreviousActiveState.Name);

                    Fsm.GotoPreviousState();
                }

                Finish();
            }
        }

    }
}
