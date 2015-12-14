using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    public class UnlockAllSkills : Passive
    {
        public UnlockAllSkills(Game1 g)
            : base(g)
        {
            name = "Unlock All Skills";
        }
    }

    public class SummonBrownies : Passive
    {
        public SummonBrownies(Game1 g)
            : base(g)
        {
            name = "Summon Brownies";
        }
    }

    public class ExperienceBoostMax : Passive
    {
        public ExperienceBoostMax(Game1 g)
            : base(g)
        {
            name = "Experience Boost - Max";
        }
    }

    public class DarylCanTalk : Passive
    {
        public DarylCanTalk(Game1 g)
            : base(g)
        {
            name = "Daryl Can Talk";
        }
    }

    public class InfiniteLivesPassive : Passive
    {
        public InfiniteLivesPassive(Game1 g)
            : base(g)
        {
            name = "Infinite Lives";
        }
    }
}
