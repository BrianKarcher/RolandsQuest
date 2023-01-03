using System;

namespace RQ.Entity.Assets
{
    [Serializable]
    public class PlayerLevelData
    {
        public int ExperienceNeeded;
        public int HP;
        public int SP;
        public float PhysicalAttackPower;
        public float MPAttackPower;
        public float PhysicalDefense;
        public float MagicalDefense;
    }
}
