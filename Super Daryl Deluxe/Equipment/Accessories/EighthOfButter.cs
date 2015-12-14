using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class EighthOfButter : Accessory
    {
        public EighthOfButter()
            : base()
        {
            health = 18;
            strength = 12;
            defense = 12;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Eighth of Luck Butter";
            level = 8;
            description = "Chef Flex's special recipe. Rubbing it on your body brings you luck.";
            icon = Game1.equipmentTextures[name];
            passiveAbility = PassiveManager.allPassives["A Bit of Luck"];
            sellPrice = 55;
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 3);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

            description = "Chef Flex's special recipe. Rubbing it on your body brings you luck.";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Chef Flex's special recipe. Rubbing it on your body brings you luck.";
        }
    }
}
