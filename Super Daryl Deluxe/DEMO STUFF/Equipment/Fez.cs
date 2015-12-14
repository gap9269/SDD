using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class FezDemo : Hat
    {
        public FezDemo()
            : base()
        {
            health = 200;
            strength = 0;
            defense = 2;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Fez";
            level = 1;
            description = "Everything about how you obtained this Fez is weird. \n\n";
            sellPrice = .50f;
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
