using RQ.Model.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Common.EventChain
{
    public class EventChain : IDisposable
    {
        private List<IEventTrigger> _triggers;
        private bool _processingChain;
        private int _index;
        private Transform _requester;
        public event EventHandler ChainComplete;
        private string _uniqueId;

        public EventChain()
        {
            _triggers = new List<IEventTrigger>();
            _processingChain = false;
            _index = 0;
            _uniqueId = Guid.NewGuid().ToString();
        }

        public EventChain(IEntity[] chainTriggers) : this()
        {
            foreach (var usableObject in chainTriggers)
            {
                var eventTrigger = usableObject.transform.GetComponents<IEventTrigger>();
                //eventTrigger.EventEnded += Next;
                if (eventTrigger != null)
                    _triggers.AddRange(eventTrigger);
            }
        }

        public void Dispose()
        {
            //StopListening();
        }

        //public void StartListening()
        //{

        //}

        //public void StopListening()
        //{

        //}
 
        public void StartChain(Transform requester)
        {
            Debug.Log("Starting new Event Chain");
            _index = 0;
            _processingChain = true;
            _requester = requester;
            Process(_index);
        }

        public void Add(IEventTrigger trigger)
        {
            _triggers.Add(trigger);
        }

        private void Next(object sender, EventArgs e)
        {
            _triggers[_index].EventEnded -= Next;
            _index++;
            Debug.Log("Next Event " + _index);
            Process(_index);
        }

        private void Process(int index)
        {
            Debug.Log("Processing Event " + index);
            //if (_index >= 0)            
            if (_index > _triggers.Count - 1)
            {
                _index = 0;
                _processingChain = false;
                Debug.Log("Chain Complete");
                ChainComplete(this, EventArgs.Empty);
                return;
            }
            //_index++;
            var trigger = _triggers[_index];
            trigger.EventEnded += Next;
            Debug.Log("EventChain (Process) Firing Trigger");
            trigger.Trigger(_requester);
        }

        //private void Next()
        //{

        //}

        //void trigger_EventEnded(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
