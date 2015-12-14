using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class RobeOfChampions : Outfit
    {
        public RobeOfChampions()
            : base()
        {
            health = 185;
            strength = 35;
            defense = 290;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Robe of Champions";
            level = 5;
            description = "You'll be a god walking among men when you wear this.";
            icon = Game1.equipmentTextures[name];

            passiveAbility = PassiveManager.allPassives["Experience Boost - Max"];
        }
    }
}
