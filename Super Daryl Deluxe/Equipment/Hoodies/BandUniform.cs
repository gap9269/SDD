using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class BandUniform : Hoodie
    {
        public BandUniform()
            : base()
        {
            health = 0;
            strength = 0;
            defense = 7;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Band Uniform";
            level = 7;
            description = "Ballsballsdicksballs \n\n";
            ////icon = Game1.equipSheetOne;
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

            description = "Ballsballsdicksballs \n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Ballsballsdicksballs \n\n";
        }
    }

}

