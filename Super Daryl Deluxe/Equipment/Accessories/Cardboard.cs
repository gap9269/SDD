using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class Cardboard : Accessory
    {
        public Cardboard()
            : base()
        {
            health = 30;
            strength = 18;
            defense = 16;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Cardboard";
            level = 12;
            description = "Just some cardboard.";
            icon = Game1.equipmentTextures[name];
            sellPrice = 15;
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
