using UnityEditor;
using RQ.Common;

namespace RQ.Editor
{
    public class BaseObjectEditor<T> : EditorBase where T : class
    {
        protected T agent;

        public virtual void OnEnable()
        {
            agent = target as T;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Dirty();
        }
    }

    [CustomEditor(typeof(BaseObject), true)]
    public class BaseObjectEditor : BaseObjectEditor<BaseObject>
    {
        //private BaseObject agent;

        //public virtual void OnEnable()
        //{
        //    agent = target as BaseObject;
        //}

        //public override void OnInspectorGUI()
        //{
        //    base.OnInspectorGUI();
        //    Dirty();
        //}
    }
}
