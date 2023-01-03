using RQ.AI;
using RQ.Common.Container;
using RQ.Controller.Sequencer;
using RQ.Entity.AtomAction;
using RQ.Entity.Common;
using RQ.Entity.Components;
using System;
using UnityEngine;
using WellFired;

namespace RQ.AI.Atom.GameManager
{
    [Serializable]
    public class PlaySequenceAtom : AtomActionBase
    {
        public string SequenceName;
        public bool PlayDirectly;
        public SequencerLink _sequencerLink;
        //public USSequencer _sequencer = null;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            if (_sequencerLink == null)
            {
                if (entity == null)
                    throw new Exception("(PlaySequenceAtom) Could not find entity");
                _sequencerLink = entity.Components.GetComponent<SequencerLink>(SequenceName);
            }

            var sequencerChildren = _sequencerLink.GetComponentsInChildren<USTimelineContainer>();
            foreach (var sequencerChild in sequencerChildren)
            {
                if (sequencerChild.AffectedObject == null)
                    continue;
                var affectedObject = sequencerChild.AffectedObject;
                var placeHolder = affectedObject.GetComponent<SequencerPlaceholder>();
                if (placeHolder == null)
                    continue;
                var tag = placeHolder.tag;
                var tagsWithGameObject = GameObject.FindGameObjectsWithTag(tag);
                SpriteBaseComponent target = null;
                foreach (var tagIterate in tagsWithGameObject)
                {
                    var component = tagIterate.GetComponent<SpriteBaseComponent>();
                    if (component != null)
                    {
                        target = component;
                        break;
                    }
                }
                //var target = GameObject.FindGameObjectsWithTag(tag).FirstOrDefault(i => i.GetComponent<SpriteBaseComponent>() != null);
                if (target == null)
                    continue;
                sequencerChild.AffectedObject = target.transform;
            }
            //var main = sequencerChildren.FirstOrDefault(i => i.Index == 3);
            //if (main != null)
            //    main.AffectedObject = EntityContainer._instance.GetMainCharacter().transform;
            //var camera = sequencerChildren.FirstOrDefault(i => i.Index == 4);
            //if (camera != null)
            //    camera.AffectedObject = GameController.Instance?.GetCamera()?.transform;

            Debug.LogError($"Playing Sequence {SequenceName}");

            //if (PlayDirectly)
            //    PlayDirect(_sequencerLink);
            //else
            //    PlayIndirect(_sequencerLink);
            Play(_sequencerLink);
        }

        //public void PlayDirect(SequencerLink sequencer)
        //{
        //    var usSequencer = sequencer.GetSequencer();
        //    usSequencer.PlaybackFinished = (seq) =>
        //    {
        //        _isRunning = false;
        //    };
        //    usSequencer.Play();
        //}

        public void Play(SequencerLink sequencer)
        {
            sequencer.OnSequenceComplete += () => 
            {
                _isRunning = false;
            };
            sequencer.Play();
        }

        public override void End()
        {
            if (PlayDirectly)
                StopDirect(_sequencerLink);
            else
                StopIndirect(_sequencerLink);
        }

        private void StopDirect(SequencerLink sequencer)
        {
            var usSequencer = sequencer.GetSequencer();
            usSequencer.PlaybackFinished = null;
            usSequencer.Stop();
        }

        private void StopIndirect(SequencerLink sequencer)
        {
            sequencer.Stop();
        }

        public override AtomActionResults OnUpdate()
        {
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
