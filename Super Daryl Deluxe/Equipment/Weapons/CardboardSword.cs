using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class CardboardSword : Weapon
    {
        public CardboardSword()
            : base()
        {
            strength = 28;
            health = 0;
            name = "Cardboard Sword";
            moveSpeed = 0;
            jumpHeight = 0;
            level = 12;
            description = "It's a sword made out of cardboard. Made specially for you.";
            canHoldTwo = true;
            icon = Game1.equipmentTextures[name];
            sellPrice = 20;

        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
        }
    }
}