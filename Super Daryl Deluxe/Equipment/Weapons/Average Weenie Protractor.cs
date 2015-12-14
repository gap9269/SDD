using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class AverageWeenieProtractor : Weapon
    {
        public AverageWeenieProtractor()
            : base()
        {
            strength = 13;
            health = 0;
            name = "Average Weenie Protractor";
            moveSpeed = 0;
            jumpHeight = 0;
            level = 8;
            description = "This has two corners to double your chances of scratching something.";
            canHoldTwo = true;
            icon = Game1.equipmentTextures[name];
            sellPrice = 5;

        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "\"This has two corners to double your chances of scratching something.\" - Trenchcoat Kid";

        }
    }
}