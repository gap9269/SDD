using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class ArtistsPaletteDemo : Accessory
    {
        public ArtistsPaletteDemo()
            : base()
        {
            health = 13;
            strength = 7;
            defense = 10;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Artist's Palette";
            level = 7;
            description = "Can also be used as a frisby.";
            icon = Game1.equipmentTextures[name];
            passiveAbility = PassiveManager.allPassives["+5 Experience"];

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
