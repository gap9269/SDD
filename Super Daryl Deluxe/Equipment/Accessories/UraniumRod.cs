using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class UraniumRod : Accessory
    {
        public UraniumRod()
            : base()
        {
            health = 3;
            strength = 1;
            defense = 0;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Uranium Rod";
            level = 3;
            description = "If you hold it long enough you might grow a second belly button.\n\n";
            icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 3);
            description = "If you hold it long enough you might grow a second belly button.\n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "If you hold it long enough you might grow a second belly button.\n\n";
        }
    }
}
