using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class MikrovsEyePatch : Hat
    {
        public MikrovsEyePatch()
            : base()
        {
            health = 500;
            strength = 0;
            defense = 2;
            name = "Mikrov's Eye Patch";
            level = 10;
            description = "Mikrov's spare eye patch. Exceptionally crusty.";
            passiveAbility = PassiveManager.allPassives["Impaired Vision"];
            icon = Game1.equipmentTextures[name];
            sellPrice = 14.27f;
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
