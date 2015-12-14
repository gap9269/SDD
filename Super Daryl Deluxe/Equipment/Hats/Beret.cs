using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class Beret : Hat
    {
        public Beret()
            : base()
        {
            health = 10;
            strength = 0;
            defense = 0;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Lucky Beret";
            level = 7;
            sellPrice = 11.50f;
            description = "Pronounced \"beret\".";
            icon = Game1.equipmentTextures[name];
            passiveAbility = PassiveManager.allPassives["Good Fortune"];

        }
    }
}
