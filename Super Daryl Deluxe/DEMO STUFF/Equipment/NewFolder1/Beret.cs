using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class BeretDemo : Hat
    {
        public BeretDemo()
            : base()
        {
            health = 200;
            strength = 0;
            defense = 2;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Lucky Beret";
            level = 7;
            description = "Pronounced \"beret\".";
            icon = Game1.equipmentTextures[name];
            passiveAbility = PassiveManager.allPassives["Good Fortune"];

        }
    }
}
