using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class Marker : Weapon
    {
        public Marker()
            : base()
        {
            strength = 1;
            health = 0;
            name = "Dried Out Marker";
            moveSpeed = 0;
            jumpHeight = 0;
            level = 1;
            description = "Non-scented. \n\n";
            canHoldTwo = true;
            icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 3);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 4);

            description = "Non-scented. \n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Non-scented. \n\n";
        }
    }
}
