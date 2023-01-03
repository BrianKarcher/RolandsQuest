using RQ.AI.Action;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Damaged")]
    public class Damaged : ActionBase
    {
        public DamagedAtom _atom;
        //private IComponentRepository _entity;
        //[SerializeField]
        //private USSequencer _sequence = null;
        //[SerializeField]
        //private ComponentRepository _sequencer = null;
        //[Obsolete]
        //[SerializeField]
        //private SequencerLink _sequencerLink = null;
        //public SceneConfig sceneConfig;
        ////[HideInInspector]
        ////public int SpawnPointInstanceId;
        //[HideInInspector]
        //public string spawnPointUniqueId;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            _atom.Start(GetEntity());

            //_sequencerLink = _sequencer.Components.GetComponent<SequencerLink>();
            //var name = _sequencerLink != null ? _sequencerLink.name : _sequence.name;
            //Debug.Log("Playing uSequence " + name);
            //if (_sequencerLink != null)
            //{
            //    //if (!_sequencerLink.IsPlaying())
            //        _sequencerLink.Play();
            //}
            //else if (!_sequence.IsPlaying)
            //    _sequence.Play();
            //GameController._instance.ChangeScene(sceneConfig, spawnPointUniqueId);
        }

        public override void ActExit(Component otherRigidBody)
        {
            base.ActExit(otherRigidBody);
            _atom.End();
        }

        //public override void Serialize(EntitySerializedData entitySerializedData)
        //{
        //    base.Serialize(entitySerializedData);
        //    var data = new PlaySequenceData();
        //    data.SequencerLinkUnqiueId = _sequencer.UniqueId;
        //    base.SerializeComponent(entitySerializedData, data);
        //}

        //public override void Deserialize(EntitySerializedData entitySerializedData)
        //{
        //    base.Deserialize(entitySerializedData);
        //    var data = base.DeserializeComponent<PlaySequenceData>(entitySerializedData);
        //    _sequencer = EntityContainer._instance.GetEntity<IComponentRepository>(data.SequencerLinkUnqiueId) as ComponentRepository;
        //    //_sequencerLink = SceneSetup.Instance.GetSequencerLink(data.SequencerLinkUnqiueId);
        //    if (_sequencer != null)
        //        _sequencerLink = _sequencer.Components.GetComponent<SequencerLink>();
        //}
    }
}
