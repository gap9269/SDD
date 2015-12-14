using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class InfiniteLives : Accessory
    {
        public InfiniteLives()
            : base()
        {
            health = 0;
            strength = 0;
            defense = 0;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Infinite Lives";
            level = 10;
            description = "Like a cat- except better.";
            icon = Game1.equipmentTextures[name];
            passiveAbility = PassiveManager.allPassives["Infinite Lives"];

        }
    }
}
