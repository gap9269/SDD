using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class AverageWeenieCalculator : Accessory
    {
        public AverageWeenieCalculator()
            : base()
        {
            health = 18;
            strength = 12;
            defense = 11;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Average Weenie Calculator";
            level = 8;
            description = "Perfect for the least common denominator.";
            icon = Game1.equipmentTextures[name];
            sellPrice = 5;
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 3);

            description = "\"Perfect for the least common denominator.\" - Trenchcoat Kid";

        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "\"Perfect for the least common denominator.\" - Trenchcoat Kid";

        }
    }
}
