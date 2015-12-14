using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class CoalShovelDemo : Weapon
    {
        public CoalShovelDemo()
            : base()
        {
            strength = 46;
            health = 35;
            defense = 3;
            name = "Coal Shovel";
            moveSpeed = 0;
            jumpHeight = 0;
            level = 5;
            description = "An old coal shovel given to Daryl by a furnace worker. \n\n";
            canHoldTwo = false;
            icon = Game1.equipmentTextures[name];
            sellPrice = 3.00f;
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "An old coal shovel given to Daryl by a furnace worker. \n\n";
        }
    }
}
