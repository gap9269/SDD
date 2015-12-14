using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class ChiselOfForgottenLove : Weapon
    {
        public ChiselOfForgottenLove()
            : base()
        {
            strength = 40;
            health = 0;
            name = "Chisel of Forgotten Love";
            level = 10;
            description = "It cuts through hearts as easily as it cuts through marble.";
            canHoldTwo = false;
            icon = Game1.equipmentTextures[name];
            sellPrice = 36.45f;

        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
        }
    }
}