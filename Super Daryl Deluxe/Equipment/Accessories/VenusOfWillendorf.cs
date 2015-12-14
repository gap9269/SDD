using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class VenusOfWillendorf : Accessory
    {
        public VenusOfWillendorf()
            : base()
        {
            health = 15;
            strength = 10;
            defense = 10;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Venus of Willendorf";
            level = 7;
            description = "Do not eat.";
            icon = Game1.equipmentTextures[name];
            sellPrice = 20;
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 3);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

            description = "Chef Flex's special recipe. Rubbing it on your body brings you luck.";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Chef Flex's special recipe. Rubbing it on your body brings you luck.";
        }
    }
}
