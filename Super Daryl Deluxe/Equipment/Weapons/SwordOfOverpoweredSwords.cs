using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class SwordOfOverpoweredSwords : Weapon
    {
        public SwordOfOverpoweredSwords()
            : base()
        {
            strength = 457;
            health = 0;
            name = "The Sword of Overpowered Swords";
            moveSpeed = 0;
            jumpHeight = 0;
            level = 8;
            description = "Wield this blade and gods will bow before you.";
            canHoldTwo = false;
            icon = Game1.equipmentTextures[name];
            passiveAbility = PassiveManager.allPassives["Unlock All Skills"];
        }
    }
}