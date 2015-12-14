using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class CardboardHat : Hat
    {
        public CardboardHat()
            : base()
        {
            health = 120;
            strength = 0;
            defense = 0;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Cardboard Hat";
            level = 12;
            description = "It might look like an ordinary box, but I assure you it's a hat.";
            icon = Game1.equipmentTextures[name];
            sellPrice = 20;
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 5);

            description = "\"The propeller runs on brain activity, in case you're wondering why it's stuck.\" - Trenchcoat Kid";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "\"The propeller runs on brain activity, in case you're wondering why it's stuck.\" - Trenchcoat Kid";
        }
    }
}
