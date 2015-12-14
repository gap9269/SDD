using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class ComposersWand : Weapon
    {
        public ComposersWand()
            : base()
        {
            strength = 13;
            health = 0;
            name = "Conductor's Wand";
            moveSpeed = 0;
            jumpHeight = 0;
            level = 7;
            description = "A flimsy stick that musicians use for something. \n\n";
            canHoldTwo = true;
            icon = Game1.equipmentTextures[name];
            sellPrice = 6.00f;
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "A flimsy stick that musicians use for something. \n\n";
        }
    }
}