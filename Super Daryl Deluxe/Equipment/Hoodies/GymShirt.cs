using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class GymShirt : Hoodie
    {
        public GymShirt()
            : base()
        {
            health = 0;
            strength = 0;
            defense = 2;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Dirty Gym Shirt";
            level = 1;
            description = "Smells like sweat. \n\n";
            ////icon = Game1.equipSheetOne;
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

            description = "Smells like sweat. \n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Smells like sweat. \n\n";
        }
    }
}
