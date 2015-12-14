using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class TreeTop : Hat
    {
        public TreeTop()
            : base()
        {
            health = 200;
            strength = 0;
            defense = 5;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Tree Top";
            level = 14;
            description = "Full of fun fungi.";
            icon = Game1.equipmentTextures[name];
            sellPrice = 35;
        }
    }
}
