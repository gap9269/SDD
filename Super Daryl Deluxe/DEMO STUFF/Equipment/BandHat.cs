using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class BandHatDemo : Hat
    {
        public BandHatDemo()
            : base()
        {
            health = 200;
            strength = 0;
            defense = 0;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Band Hat";
            level = 7;
            sellPrice = 4.00f;
            description = "This one time, at Band Hat camp... \n\n";
icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 5);

            description = "This one time, at Band Hat camp... \n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "This one time, at Band Hat camp... \n\n";
        }
    }
}
