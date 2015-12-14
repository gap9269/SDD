using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{

    class TogaDemo : Outfit
    {
        public TogaDemo()
            : base()
        {
            health = 0;
            strength = 0;
            defense = 68;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Toga";
            level = 14;
            description = "Julius Caesar's own personal toga. \n\n";
icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

            description = "Julius Caesar's own personal toga. \n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Julius Caesar's own personal toga. \n\n";
        }
    }
}
