using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class AutomaticFirstAid : Accessory
    {
        public AutomaticFirstAid()
            : base()
        {
            health = 25;
            strength = 7;
            defense = 15;
            name = "Automatic First-Aid";
            level = 11;
            description = "Batteries not included.";
            icon = Game1.equipmentTextures[name];
            passiveAbility = PassiveManager.allPassives["Automated Healing"];
            sellPrice = 17;
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 3);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
        }
    }
}
