using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class QuarterOfButter : Accessory
    {
        public QuarterOfButter()
            : base()
        {
            health = 50;
            strength = 30;
            defense = 34;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Fourth of Luck Butter";
            level = 15;
            description = "1/4 stick of Chef Flex's special Luck Butter recipe.";
            icon = Game1.equipmentTextures[name];
            sellPrice = 150.25f;
            //passiveAbility = PassiveManager.allPassives["A Bit of Luck"];
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
