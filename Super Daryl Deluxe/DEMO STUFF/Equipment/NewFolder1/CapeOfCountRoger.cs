using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class CapeOfCountRogerDemo : Outfit
    {
        public CapeOfCountRogerDemo()
            : base()
        {
            health = 0;
            strength = 0;
            defense = 60;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Cape of Count Roger";
            level = 5;
            description = "Bite necks in style!";
            icon = Game1.equipmentTextures[name];
            sellPrice = 5.00f;
            passiveAbility = PassiveManager.allPassives["Curse of Count Roger"];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

            description = "Bite necks in style!";

        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Bite necks in style!";

        }
    }
}
