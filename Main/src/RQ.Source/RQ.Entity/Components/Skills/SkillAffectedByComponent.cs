using RQ.Common.Components;
using RQ.Model.Skills;
using UnityEngine;

namespace RQ.Entity.Components
{
    [AddComponentMenu("RQ/Components/Skills/Skill Affected By")]
    public class SkillAffectedByComponent : ComponentPersistence<SkillAffectedByComponent>
    {
        [SerializeField]
        private SkillAffectedByData _data;



        public SkillAffectedByData GetData()
        {
            return _data;
        }
    }
}
