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
    public class SwordOfDesperation : Passive
    {
        public SwordOfDesperation(Game1 g)
            : base(g)
        {
            name = "Sword of Desperation";
        }

        public override void LoadPassive()
        {
            base.LoadPassive();
        }

        public override void Update()
        {
            base.Update();

            if (Game1.Player.Health < (Game1.Player.realMaxHealth * .15f))
            {
                if (Game1.Player.specialStrength < 125)
                    Game1.Player.specialStrength = 125;
            }
            else if (Game1.Player.specialStrength == 125)
                Game1.Player.specialStrength = 0;
        }
    }
}
