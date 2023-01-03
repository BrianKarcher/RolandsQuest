using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using UnityEngine;

namespace RQ.FSM.V3.Conditionals
{
    [AddComponentMenu("RQ/States/Conditions/BM Button Pressed")]
    public class SceneSetupExistsConditionConfig : StateTransitionConditionBaseConfig//, IMessageHandler
    {
        public override bool TestCondition(IStateMachine stateMachine)
        {
            var sceneSetup = GameObject.FindObjectOfType<SceneSetup>();

            return sceneSetup != null;
        }
    }
}
