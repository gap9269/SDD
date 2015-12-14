using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class LoinCloth : Outfit
    {
        public LoinCloth()
            : base()
        {
            health = 10;
            strength = 3;
            defense = 75;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Loincloth";
            level = 10;
            description = "It protects your whole body.";
            icon = Game1.equipmentTextures[name];
            sellPrice = 11.34f;

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
