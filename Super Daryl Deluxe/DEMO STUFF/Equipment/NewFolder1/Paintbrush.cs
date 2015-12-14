using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class PaintbrushDemo : Weapon
    {
        public PaintbrushDemo()
            : base()
        {
            strength = 22;
            health = 0;
            name = "Paintbrush of Destruction";
            moveSpeed = 0;
            jumpHeight = 0;
            level = 8;
            description = "Paint your foes into oblivion.";
            canHoldTwo = true;
            icon = Game1.equipmentTextures[name];
            passiveAbility = PassiveManager.allPassives["Sword of Desperation"];
        }
    }
}