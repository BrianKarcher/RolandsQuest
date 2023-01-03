using RQ.Entity.Components;

namespace RQ.AI
{
    public interface IAtomAction
    {
        void Start(IComponentRepository entity);
        void End();
        AtomActionResults OnUpdate();
    }
}
