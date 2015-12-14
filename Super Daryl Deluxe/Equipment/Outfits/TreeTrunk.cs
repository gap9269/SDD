using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class TreeTrunk : Outfit
    {
        public TreeTrunk()
            : base()
        {
            health = 0;
            strength = 0;
            defense = 150;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Tree Trunk";
            level = 14;
            description = "The lifeless body of a magical tree being that you slaughtered.";
            icon = Game1.equipmentTextures[name];
            sellPrice = 35;
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
