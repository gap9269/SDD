using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class SuperWeenieHatDemo : Hat
    {
        public SuperWeenieHatDemo()
            : base()
        {
            health = 200;
            strength = 0;
            defense = 2;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Super Weenie Hat";
            level = 4;
            description = "I'm surprised you don't already own one of these.";
            sellPrice = 1.00f;
            icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 5);

            description = "\"I'm surprised you don't already own one of these.\" - Trenchcoat Kid";

        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "\"I'm surprised you don't already own one of these.\" - Trenchcoat Kid";

        }
    }
}
