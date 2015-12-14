using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class MonkeysPaw : Accessory
    {
        public MonkeysPaw()
            : base()
        {
            health = 40;
            strength = 20;
            defense = 15;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Monkey's Paw";
            level = 15;
            description = "This paw vows to grant three wishes and strangle all birds.";
            sellPrice = 25.50f;
            icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 3);

        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
        }
    }
}
