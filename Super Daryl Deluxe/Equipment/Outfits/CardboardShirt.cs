using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class CardboardShirt : Outfit
    {
        public CardboardShirt()
            : base()
        {
            health = 0;
            strength = 0;
            defense = 108;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Cardboard Shirt";
            level = 12;
            description = "Constructed from various shoe boxes.";
            sellPrice = 20;
            icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();

        }
    }
}
