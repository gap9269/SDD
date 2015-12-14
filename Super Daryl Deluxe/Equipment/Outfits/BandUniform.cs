using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class BandUniform : Outfit
    {
        public BandUniform()
            : base()
        {
            health = 0;
            strength = 0;
            defense = 38;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Band Uniform";
            level = 7;
            description = "Pending";
            icon = Game1.equipmentTextures[name];
            sellPrice = 8.00f;
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

            description = "Pending";

        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Pending";

        }
    }

}

