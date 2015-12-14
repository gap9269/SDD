using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class SuperWeenieSwordDemo : Weapon
    {
        public SuperWeenieSwordDemo()
            : base()
        {
            strength = 46;
            health = 35;
            defense = 3;
            name = "Super Weenie Sword";
            moveSpeed = 0;
            jumpHeight = 0;
            level = 4;
            description = "Don't hurt yourself blowing it up.";
            canHoldTwo = false;
            icon = Game1.equipmentTextures[name];
            sellPrice = 1.00f;

        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "\"Don't hurt yourself blowing it up.\" - Trenchcoat Kid";

        }
    }
}