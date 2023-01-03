using RQ.Common;
using RQ.Entity.Components;
using RQ.Model;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RQ.Controller.Actions.Triggers
{
    [AddComponentMenu("RQ/Action/Triggers/Basic Action Trigger")]
    public class BasicActionTrigger : MonoBehaviour
    {
        [FormerlySerializedAs("OnTriggerEnter")]
        public bool ActOnTriggerEnter = true;
        [SerializeField]
        protected bool _checkTag = false;
        [SerializeField]
        [Tag]
        private string[] _tags;
        [SerializeField]
        [Tag]
        private string[] _anythingBut;

        private IEnumerable<IAction> _actions;

        public void Awake()
        {
            _actions = GetComponents<IAction>();
            //if (_actions != null)
            //{
            //    _actions = _actions.Where(i => i.isActiveAndEnabled).ToList();
            //}
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            ProcessTrigger(other.tag, other.attachedRigidbody, true);
        }

        public virtual void OnCollisionEnter(Collision other)
        {
            ProcessTrigger(other.collider.tag, other.collider.attachedRigidbody, true);
        }

        private void ProcessTrigger(string otherTag, Component otherRigidBody, bool isEnter)
        {
            if (!ActOnTriggerEnter)
                return;
            var act = true;
            if (_checkTag)
            {
                var tagsContains = Array.IndexOf(_tags, otherTag) > -1;
                var anythingButContains = Array.IndexOf(_anythingBut, otherTag) > -1;
                //if (_tags.Length != 0 && !_tags.Contains(otherTag))
                if (_tags.Length != 0 && !tagsContains)
                    act = false;
                //if (_anythingBut.Length != 0 && _anythingBut.Contains(otherTag))
                if (_anythingBut.Length != 0 && anythingButContains)
                    act = false;
            }
            //bool act = false;
            //if (!_checkTag)
            //{
            //    act = true;
            //    //Act(other.attachedRigidbody);
            //}
            //// TODO Add tag property
            //else if (_tags.Contains(otherTag)) //if (other.tag == "Player")
            //{
            //    act = true;
            //    //Act(other.attachedRigidbody);
            //}
            //else if (_anythingBut.Any() && !_anythingBut.Contains(otherTag))
            //{
            //    act = true;
            //}
            if (act)
            {
                if (isEnter)
                    Act(otherRigidBody);
                else
                    ActExit(otherRigidBody);
            }
        }

        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            ProcessTrigger(other.tag, other.attachedRigidbody, true);
        }

        public virtual void OnCollisionEnter2D(Collision2D other)
        {
            ProcessTrigger(other.collider.tag, other.collider.attachedRigidbody, true);
        }

        public virtual void OnTriggerExit(Collider other)
        {
            ProcessTrigger(other.tag, other.attachedRigidbody, false);
        }

        public virtual void OnCollisionExit(Collision other)
        {
            ProcessTrigger(other.collider.tag, other.collider.attachedRigidbody, false);
        }

        public virtual void OnTriggerExit2D(Collider2D other)
        {
            ProcessTrigger(other.tag, other.attachedRigidbody, false);
        }

        public virtual void OnCollisionExit2D(Collision2D other)
        {
            ProcessTrigger(other.collider.tag, other.collider.attachedRigidbody, false);
        }

        public virtual void Act(Component otherRigidBody)
        {
            var entity = otherRigidBody.GetComponent<IComponentRepository>();
            foreach (var action in _actions)
            {
                if (!action.isActiveAndEnabled)
                    continue;
                action.SetComponentRepository(entity);
                action.InitAction();
                action.Act(otherRigidBody);
            }
        }

        public virtual void ActExit(Component otherRigidBody)
        {
            var entity = otherRigidBody.GetComponent<IComponentRepository>();
            foreach (var action in _actions)
            {
                if (!action.isActiveAndEnabled)
                    continue;
                action.SetComponentRepository(entity);
                action.InitAction();
                action.ActExit(otherRigidBody);
            }
        }
    }
}
