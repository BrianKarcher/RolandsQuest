using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using UnityEngine;

namespace RQ.Entity.StatesV2.Conditions
{
    [AddComponentMenu("RQ/States/Conditions/Randomizer")]
    public class RandomizerCondition : StateTransitionConditionBase
    {
        [SerializeField]
        private float _percent = 0f;

        //public override void SetEntity(IRQObject entity, IStateMachine stateMachine)
        //{
        //    base.SetEntity(entity, stateMachine);
        //    if (_endTime == 0f)
        //        _endTime = _startTime;
        //    //_entity = entity;
        //    SetTimer();
        //}

        public override bool TestCondition(IStateMachine stateMachine)
        {
            var randomNumber = UnityEngine.Random.Range(0f, 100f);

            //return Time.time > _stateMachine.StateStartTime + _time;

            return randomNumber <= _percent;
        }

        //public override void Reset()
        //{
        //    SetTimer();
        //}

        //public void SetTimer()
        //{
        //    _time = UnityEngine.Random.Range(_startTime, _endTime);
        //}
    }
}
