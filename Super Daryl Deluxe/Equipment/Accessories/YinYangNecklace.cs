﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class YinYangNecklace : Accessory
    {
        public YinYangNecklace()
            : base()
        {
            health = 5;
            strength = 5;
            defense = 5;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Yin-Yang Necklace";
            level = 4;
            description = "Find true balance with this necklace that would make any Taoist squeal with joy.\n\n";
            icon = Game1.equipmentTextures[name];
            sellPrice = .05f;
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 3);

            description = "Find true balance with this necklace that would make any Taoist squeal with joy.\n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Find true balance with this necklace that would make any Taoist squeal with joy.\n\n";
        }
    }
}
