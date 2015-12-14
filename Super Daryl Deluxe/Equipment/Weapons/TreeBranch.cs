using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class TreeBranch : Weapon
    {
        public TreeBranch()
            : base()
        {
            strength = 40;
            health = 0;
            name = "Tree Branch";
            moveSpeed = 0;
            jumpHeight = 0;
            level = 14;
            description = "You could poke an eye out with that thing.";
            canHoldTwo = true;
            icon = Game1.equipmentTextures[name];
            sellPrice = 24.25f;

        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
        }
    }
}