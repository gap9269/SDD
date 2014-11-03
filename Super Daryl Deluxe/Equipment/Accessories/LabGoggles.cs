using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class LabGoggles : Accessory
    {
        public LabGoggles()
            : base()
        {
            health = 3;
            strength = 1;
            defense = 0;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Lab Goggles";
            level = 3;
            description = "Now you can see in SCIENCE VISION! \n\n";
            icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 5);

            description = "Now you can see in SCIENCE VISION! \n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Now you can see in SCIENCE VISION! \n\n";
        }
    }
}
