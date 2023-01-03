using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Model.Serialization;

namespace RQ.FSM.V2
{
    /// <summary>
    /// abstract base class to define an interface for a state
    /// </summary>
    public interface IState //where T : class, IRQObject
    {
        //void SetEntity(Transform entity);
        IComponentRepository GetEntity();

        //this will execute when the state is entered
        void Enter();
        void SetupState();

        //this is the states normal update function
        //void Update(IRQObject entity);

        //void FixedUpdate(IRQObject entity);

        //this will execute when the state is exited. 
        void Exit();

        //this executes if the agent receives a message from the 
        //message dispatcher
        bool HandleMessage(Telegram telegram);

        void SetEnabled(bool enable);

        void Complete();
        bool IsComplete();

        //void PhysicsUpdate();

        //int GetState();
        string Name { get; }
        string UniqueId { get; }

        IStateMachine StateMachine { get; set; }
        void Serialize(StateData stateData);
        void Deserialize(StateData stateData);
    }
}
