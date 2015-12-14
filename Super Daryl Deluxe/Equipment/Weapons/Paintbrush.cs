using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class Paintbrush : Weapon
    {
        public Paintbrush()
            : base()
        {
            strength = 5;
            health = 0;
            name = "Paintbrush of Destruction";
            moveSpeed = 0;
            jumpHeight = 0;
            level = 8;
            description = "Paint your foes into oblivion.";
            canHoldTwo = true;
            icon = Game1.equipmentTextures[name];
            passiveAbility = PassiveManager.allPassives["Sword of Desperation"];
            sellPrice = 8.20f;

        }
    }
}