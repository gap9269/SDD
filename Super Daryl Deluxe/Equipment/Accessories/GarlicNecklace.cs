using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class GarlicNecklace : Accessory
    {
        public GarlicNecklace()
            : base()
        {
            health = 5;
            strength = 3;
            defense = 3;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Garlic Necklace";
            level = 3;
            description = "A necklace worn to ward of vampires and friends.";
            icon = Game1.equipmentTextures[name];
            sellPrice = 1.25f;
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 3);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

            description = "A necklace worn to ward of vampires and friends.";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "A necklace worn to ward of vampires and friends.";
        }
    }
}
