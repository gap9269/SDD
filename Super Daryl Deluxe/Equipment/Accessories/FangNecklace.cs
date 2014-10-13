using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class FangNecklace : Accessory
    {
        public FangNecklace()
            : base()
        {
            health = 3;
            strength = 4;
            defense = 2;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Fang Necklace";
            level = 5;
            description = "A gross necklace made of bat fangs.\n\n";
icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 3);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

            description = "A gross necklace made of bat fangs.\n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "A gross necklace made of bat fangs.\n\n";
        }
    }
}
