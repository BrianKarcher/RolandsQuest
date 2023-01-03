using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.AtomAction.Physics;

namespace RQ.AI.PlayMaker.Physics
{
    [ActionCategory("RQ.Physics")]
    [PM.Tooltip("Set the tags for a collider.")]
    public class SetColliderTags : FsmStateAction
    {
        [UIHint(UIHint.Tag)]
        public FsmString[] Tags;
        //[RequiredField]
        //public string[] Tags;


        public SetColliderTagsAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            var tags = new string[Tags.Length];
            for (int i = 0; i < Tags.Length; i++)
            {
                tags[i] = Tags[i].Value;
            }
            _atom.SetTags(tags);
            //_atom.SetTags(Tags);
            _atom.Start(_entity);

            Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }
    }
}
