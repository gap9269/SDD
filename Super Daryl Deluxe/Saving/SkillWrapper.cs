using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    [Serializable]
    public class SkillWrapper
    {
        public int level;
        public int experience;
        public int experienceUntilLevel;
        public float damage;
        public int fullCooldown;

        public SkillWrapper() { }

        public SkillWrapper(int l, int e, int el, float d, int fc)
        {
            level = l;
            experience = e;
            experienceUntilLevel = el;
            damage = d;
            fullCooldown = fc;
        }

    }
}
