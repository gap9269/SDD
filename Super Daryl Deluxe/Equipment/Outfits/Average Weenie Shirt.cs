using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class AverageWeenieShirt : Outfit
    {
        public AverageWeenieShirt()
            : base()
        {
            health = 0;
            strength = 0;
            defense = 40;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Average Weenie Shirt";
            level = 8;
            description = "A dorky shirt made dorkier by being on you.";
            icon = Game1.equipmentTextures[name];
            sellPrice = 5;
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

            description = "You're basically a real scientist now.";

        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "You're basically a real scientist now.";

        }
    }
}
