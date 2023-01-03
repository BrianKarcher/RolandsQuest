using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Model;
using RQ.Model.Interfaces;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SetColorAtom : AtomActionBase
    {
        public string name;
        public Color color;
        private AnimationComponent _animComponent;
        //public TweenToColorInfo _overlayColor = null;
        //private float _endTime;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _animComponent = entity.Components.GetComponent<AnimationComponent>();
            //Color color;
            //color = hideTileFound ? new Color(0f, 1f, 0f) : Color.white;
            _animComponent?.GetSpriteRenderer()?.SetColor(name, color);
        }

        public override void End()
        {
            _animComponent?.GetSpriteRenderer()?.RemoveColor(name);
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }
    }
}
