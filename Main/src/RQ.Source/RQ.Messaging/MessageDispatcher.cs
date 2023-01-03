using PriorityQueues;
//using RQ.Entities;
//using RQ.Entities.Common;
using RQ.Enums;
//using RQ.Logging;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Messaging
{
    [Obsolete]
    [AddComponentMenu("RQ/Components/Message Dispatcher")]
    public class MessageDispatcher
    {
        //to make code easier to read
        //private const float SEND_MSG_IMMEDIATELY = 0.0f;
        //private const int NO_ADDITIONAL_INFO = 0;
        //private const int SENDER_ID_IRRELEVANT = -1;

        //private IMessageHandler _gameController;

        private Dictionary<string, IMessagingObject> _messageHandlers = new Dictionary<string, IMessagingObject>();

        // @todo Have this class be the container
        //private IMessageHandlerContainer _messageHandlerContainer;

        // Singleton instance
        public static readonly MessageDispatcher Instance = new MessageDispatcher();

        //a std::set is used as the container for the delayed messages
        //because of the benefit of automatic sorting and avoidance
        //of duplicates. Messages are sorted by their dispatch time.
        // todo Change this to a SortedSet when Unity finally goes to .NET 4.0......
        //private SortedDictionary<Telegram, bool> PriorityQ;
        //private List<Telegram> PriorityQ;
        // Inserts and removals are very fast with a Linked List, which we
        // will be using often with the messaging system
        //private LinkedList<Telegram> PriorityQ = new LinkedList<Telegram>();
        private PriorityQueue<Telegram> PriorityQ = new PriorityQueue<Telegram>();
        //private List<Telegram> _itemsToRemove = new List<Telegram>();

        //public void SetGameController(IMessageHandler gameController)
        //{
        //    _gameController = gameController;
        //}

        //public void RegisterMessageHandlerContainer(IMessageHandlerContainer messageHandlerContainer)
        //{
        //    _messageHandlerContainer = messageHandlerContainer;
        //}

        public void ResetAll()
        {
            ResetQueue();
            ResetMessageHandlerList();
        }

        public void ResetMessageHandlerList()
        {
            _messageHandlers.Clear();
        }

        public virtual void RegisterMessageHandler(IMessagingObject messageHandler)
        {
            if (_messageHandlers.ContainsKey(messageHandler.UniqueId))
                return;
                //throw new Exception("MessageDispatcher - Unique Id " + messageHandler.UniqueId + " already exists");
            //if (!_messageHandlers.ContainsKey(messageHandler.UniqueId))
                _messageHandlers.Add(messageHandler.UniqueId, messageHandler);
        }

        public bool IsRegistered(IMessagingObject messageHandler)
        {
            return _messageHandlers.ContainsKey(messageHandler.UniqueId);
        }

        public void UnregisterMessageHandler(IMessagingObject messageHandler)
        {
            UnregisterMessageHandler(messageHandler.UniqueId);
        }

        public void UnregisterMessageHandler(string id)
        {
            _messageHandlers.Remove(id);
        }

        //this method is utilized by DispatchMsg or DispatchDelayedMessages.
        //This method calls the message handling member function of the receiving
        //entity, pReceiver, with the newly created telegram
        private bool Discharge(string receiverId, Telegram telegram)
        {
            //get a pointer to the receiver
            //IMessageHandler messageHandler = EntityController._instance.GetEntity(receiverId); //EntityMgr->GetEntityFromID(receiver);
            //IMessageHandler messageHandler = _messageHandlerContainer.GetEntityForHandler(receiverId);
            if (!_messageHandlers.ContainsKey(receiverId))
            {
                //isSuccess = false;
                //throw new Exception("Could not locate id " + receiverId + " in the messaging system for message " + telegram.Msg + ".");
                return false;
            }

            IMessagingObject messageHandler = _messageHandlers[receiverId];

            //make sure the receiver is valid
            if (messageHandler == null)
            {
                throw new Exception("Warning! No Receiver with ID of " + receiverId + " found");


                //return false;
            }

            //isSuccess = true;
            return messageHandler.HandleMessage(telegram);

        }

        // Private constructor enforces the singleton
        private MessageDispatcher()
        {

        }

        public virtual void Awake()
        {
            //PriorityQ = new SortedDictionary<Telegram, bool>();
            //PriorityQ = new List<Telegram>();
            //PriorityQ = new LinkedList<Telegram>();
            //_messageHandlers = new Dictionary<string, IMessagingObject>();
        }

        public void ResetQueue()
        {
            //while()
            //PriorityQ.Dequeue
            PriorityQ.Clear();
        }

        /// <summary>
        /// Removes the messages destined for an entity from the queue
        /// </summary>
        /// <param name="entityId"></param>
        //public void RemoveMessagesForEntity(string entityId)
        //{
        //    LinkedListNode<Telegram> node = PriorityQ.First;

        //    while (node != null)
        //    {
        //        //var nextNode = node.Next;

        //        if (node.Value.ReceiverId == entityId)
        //            PriorityQ.Remove(node);

        //        //node = nextNode;
        //        node = node.Next;
        //    }
        //}

        //copy ctor and assignment should be private
        //private MessageDispatcher(MessageDispatcher&);
        //private static MessageDispatcher operator =(MessageDispatcher rhs) {}

        //send a message to another agent. Receiving agent is referenced by ID.
        public bool DispatchMsg(float delay,
                         string senderId,
                         string receiverId,
                         Telegrams msg,
                         object ExtraInfo)
        {
            return DispatchMsgWithEarlyTermination(delay, senderId, receiverId, msg, ExtraInfo, null, TelegramEarlyTermination.None);
        }

        public bool DispatchMsg(float delay,
                 string senderId,
                 IEnumerable<string> receiverIds,
                 Telegrams msg,
                 object ExtraInfo)
        {
            foreach (var receiverId in receiverIds)
            {
                DispatchMsg(delay, senderId, receiverId, msg, ExtraInfo);
            }
            return false;
        }

        public bool DispatchMsg(float delay,
                 string senderId,
                 string receiverId,
                 Telegrams msg,
                 object ExtraInfo,
                 Action<object> act
                 )
        {
            return DispatchMsgWithEarlyTermination(delay, senderId, receiverId, msg, ExtraInfo, act, TelegramEarlyTermination.None);
        }

        public bool DispatchMsgWithEarlyTermination(float delay,
                 string senderId,
                 string receiverId,
                 Telegrams msg,
                 object ExtraInfo,
                 Action<object> act,
                 TelegramEarlyTermination earlyTermination)
        {
            bool isDispatched = false;
            //create the telegram
            Telegram telegram = CreateTelegram(delay, senderId, receiverId, msg, ExtraInfo, act, earlyTermination); //new Telegram(0, senderId, receiverId, msg, ExtraInfo, earlyTermination);

            //if there is no delay, route telegram immediately                       
            if (delay <= 0.0)
            {
                //Log.Info("Telegram dispatched at time: " + Time.time
                //   + " by " + senderId + " for " + receiverId
                //   + ". Msg is " + msg);
                //#ifdef SHOW_MESSAGING_INFO
                //debug_con << "\nTelegram dispatched at time: " << TickCounter->GetCurrentFrame()
                //     << " by " << sender << " for " << receiver 
                //     << ". Msg is " << msg << "";
                //#endif

                //send the telegram to the recipient
                //bool isSuccess;
                isDispatched = Discharge(receiverId, telegram);
            }

            //else calculate the time when the telegram should be dispatched
            else
            {
                float CurrentTime = UnityEngine.Time.time; //TickCounter->GetCurrentFrame(); 

                telegram.DispatchTime = CurrentTime + delay;

                //and put it in the queue
                PriorityQ.Enqueue(telegram);
                //PriorityQ.AddLast(telegram);
                //Log.Info("Adding item from message queue, count = " + PriorityQ.Count);

                //if (PriorityQ.ContainsKey(telegram))
                //{
                //    Log.Error("Telegram " + telegram.Msg.ToString() + " already exists");
                //}
                //else
                //{
                //    PriorityQ.Add(telegram, true);
                //}
                //PriorityQ.insert(telegram);   

                //#ifdef SHOW_MESSAGING_INFO
                //debug_con << "\nDelayed telegram from " << sender << " recorded at time " 
                //        << TickCounter->GetCurrentFrame() << " for " << receiver
                //        << ". Msg is " << msg << "";
                //#endif
            }
            return isDispatched;
        }

        //public bool DispatchMsgToRelay(float delay,
        //         string senderId,
        //         IMessageRelayComponent receiver,
        //         Telegrams msg,
        //         object ExtraInfo,
        //         params TelegramEarlyTermination[] earlyTermination)
        //{
        //    return DispatchMsg(delay, senderId, receiver.UniqueId, msg, ExtraInfo, earlyTermination);
        //}

        //send a message to all agents.
        //public void DispatchMsgToAll(float delay,
        //                 string senderId,
        //                 Telegrams msg,
        //                 object ExtraInfo,
        //                 params TelegramEarlyTermination[] earlyTermination)
        //{
        //    foreach (var messageHandler in _messageHandlers)
        //    {
        //        DispatchMsg(delay, senderId, messageHandler.Key, msg, ExtraInfo, earlyTermination);
        //    }
        //}

        public Telegram CreateTelegram(float delay,
                         string senderId,
                         string receiverId,
                         Telegrams msg,
                         object ExtraInfo,
                         Action<object> act,
                         TelegramEarlyTermination earlyTermination)
        {
            return new Telegram(0, senderId, receiverId, msg, ExtraInfo, act, earlyTermination);
        }

        //send out any delayed messages. This method is called each time through   
        //the main game loop.
        //  This function dispatches any telegrams with a timestamp that has
        //  expired. Any dispatched telegrams are removed from the queue
        public void DispatchDelayedMessages()
        {
            //RemoveTaggedMessages();
            //first get current time
            float CurrentTime = UnityEngine.Time.time; //TickCounter->GetCurrentFrame(); 

            //List<Telegram> telegramsToRemove = new List<Telegram>();
            //var ListPriorityQ = PriorityQ.Keys.ToList();
            //now peek at the queue to see if any telegrams need dispatching.
            //remove all telegrams from the front of the queue that have gone
            //past their sell by date

            // Iterate through the list
            //var iteration = PriorityQ.First;

            //for (int i = 0; i < ListPriorityQ.Count; i++)
            while (PriorityQ.Count() != 0 && PriorityQ.Peek().DispatchTime < CurrentTime)
            //while (iteration != null)
            //foreach (var item in PriorityQ)
            {
                //var nextIteration = iteration.Next;
                //var telegram = iteration.Value;
                var telegram = PriorityQ.Dequeue();
                //var telegram = ListPriorityQ[i];
                //Telegram telegram = item;
                //if (telegram.DispatchTime < CurrentTime)
                //{
                    //if (telegram.DispatchTime > 0)
                    //{
                        //find the recipient
                        //BaseGameEntity pReceiver = EntityController._instance.GetEntity(telegram.ReceiverId); //EntityMgr->GetEntityFromID(telegram.Receiver);

                        //send the telegram to the recipient
                        //bool isSuccess;
                        Discharge(telegram.ReceiverId, telegram);
            }

            // Remove the telegrams marked for deletion
            //foreach (var item in telegramsToRemove)
            //{
            //    PriorityQ.Remove(item);
            //}
        }

        //public void RemoveByEarlyTermination(string receiverId, Enums.TelegramEarlyTermination earlyTermination)
        //{
        //    // Doing it this way only flips through the list once.
        //    var iteration = PriorityQ.First;

        //    while (iteration != null)
        //    {
        //        var telegram = iteration.Value;
        //        if (telegram.ReceiverId == receiverId && telegram.EarlyTermination.Contains(earlyTermination))
        //        {
        //            PriorityQ.Remove(iteration); // This is an O(1) operation. Do NOT remove telegram, that would be an O(n) operation.
        //        }
        //        iteration = iteration.Next;
        //    }
        //    //var telegramsToRemove = PriorityQ.Where(i => i.ReceiverId == receiverId && i.EarlyTermination == earlyTermination);
        //    //foreach (var telegram in telegramsToRemove)
        //    //{
        //    //    PriorityQ.Remove(telegram);
        //    //}
        //}

        //public void RemoveByEarlyTermination(Enums.TelegramEarlyTermination earlyTermination)
        //{
        //    // Doing it this way only flips through the list once.
        //    var iteration = PriorityQ.First;

        //    while (iteration != null)
        //    {
        //        var telegram = iteration.Value;
        //        if (telegram.EarlyTermination.Contains(earlyTermination))
        //        {
        //            PriorityQ.Remove(iteration); // This is an O(1) operation. Do NOT remove telegram, that would be an O(n) operation.
        //        }
        //        iteration = iteration.Next;
        //    }
        //    //var telegramsToRemove = PriorityQ.Where(i => i.ReceiverId == receiverId && i.EarlyTermination == earlyTermination);
        //    //foreach (var telegram in telegramsToRemove)
        //    //{
        //    //    PriorityQ.Remove(telegram);
        //    //}
        //}

        //public void RemoveFromQueue(Func<Telegram, bool> removeQuery)
        //{
        //    var itemsToRemove = PriorityQ.Where(removeQuery);
        //    _itemsToRemove.AddRange(itemsToRemove);
        //    //foreach(var item in itemsToRemove)
        //    //    PriorityQ.Remove(item);
        //}
        
        public Telegram[] Serialize()
        {
            List<Telegram> messageDatas = new List<Telegram>();
            // Retain the queue so we can set it back later
            var newQueue = PriorityQ.Clone();

            while (PriorityQ.Count() != 0)
            {
                var telegram = PriorityQ.Dequeue();
                telegram.TimeRemaining = telegram.DispatchTime - Time.time;
                messageDatas.Add(telegram);
                //newQueue.Enqueue(telegram);
            }

            PriorityQ = newQueue;

            //var iteration = PriorityQ.First;

            //while (iteration != null)
            //{
            //    Telegram telegram = iteration.Value;
            //    telegram.TimeRemaining = telegram.DispatchTime - Time.time;
            //    messageDatas.Add(telegram);
            //    iteration = iteration.Next;
            //}

            return messageDatas.ToArray();
        }

        public void Deserialize(IEnumerable<Telegram> messageDatas)
        {
            //Dictionary<int, int> entityMap = EntityController._instance.GetEntityMap();
            foreach (var messageData in messageDatas)
            {
                //PriorityQ.AddLast(new Telegram()
                //{
                //     DispatchTime = Time.time + messageData.TimeRemaining,

                //});
                //int senderId = _messageHandlerContainer.GetEntityOnDeserializationForHandler(messageData.SenderId).ID();
                //int receiverId = _messageHandlerContainer.GetEntityOnDeserializationForHandler(messageData.ReceiverId).ID();
                //int senderId = entityMap[messageData.SenderId];
                //int receiverId = entityMap[messageData.ReceiverId];
                DispatchMsgWithEarlyTermination(messageData.TimeRemaining, messageData.SenderId,
                    messageData.ReceiverId, messageData.Msg, messageData.ExtraInfo, null, messageData.EarlyTermination);
            }
        }
    }
}
