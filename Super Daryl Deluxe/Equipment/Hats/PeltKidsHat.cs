using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class PeltKidsHat : Hat
    {
        public PeltKidsHat()
            : base()
        {
            health = 180;
            strength = 0;
            defense = 0;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Pelt Kid's Hat";
            level = 15; //Make this 4 for the demo
            description = "The charred remains of Pelt Kid's hat. You sick bastard. \n\n";
            icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 5);

            description = "The charred remains of Pelt Kid's hat. You sick bastard. \n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "The charred remains of Pelt Kid's hat. You sick bastard. \n\n";
        }
    }
}
