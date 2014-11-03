using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class Fez : Hat
    {
        public Fez()
            : base()
        {
            health = 4;
            strength = 0;
            defense = 0;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Fez";
            level = 2;
            description = "Everything about how you obtained this Fez is weird. \n\n";
            icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 5);

            description = "Everything about how you obtained this Fez is weird. \n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Everything about how you obtained this Fez is weird. \n\n";
        }
    }
}
