using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class NurseHat : Hat
    {
        public NurseHat()
            : base()
        {
            health = 90;
            strength = 0;
            defense = 2;
            name = "Nurse Hat";
            level = 10;
            description = "Perfect for ze little helper zat you are. Ohoho!";
            icon = Game1.equipmentTextures[name];
            sellPrice = 15;
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 5);

        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
        }
    }
}
