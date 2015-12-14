using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class SuperWeeniePurseDemo : Accessory
    {
        public SuperWeeniePurseDemo()
            : base()
        {
            health = 5;
            strength = 15;
            defense = 15;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Super Weenie Purse";
            level = 4;
            description = "With special padded straps for sensitive weenie shoulders.";
            sellPrice = 1.00f;
            icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 3);

            description = "\"With special padded straps for sensitive weenie shoulders.\" - Trenchcoat Kid";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "\"With special padded straps for sensitive weenie shoulders.\" - Trenchcoat Kid";
        }
    }
}
