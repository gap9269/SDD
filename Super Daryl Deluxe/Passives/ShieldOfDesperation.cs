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
    public class ShieldOfDesperation : Passive
    {
        public ShieldOfDesperation(Game1 g)
            : base(g)
        {
            name = "Shield of Desperation";
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
                if (Game1.Player.specialDefense < 300)
                    Game1.Player.specialDefense = 300;
            }
            else if (Game1.Player.specialDefense == 300)
                Game1.Player.specialDefense = 0;
        }
    }
}
