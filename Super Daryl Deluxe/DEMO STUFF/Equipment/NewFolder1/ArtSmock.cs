using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class ArtSmockDemo : Outfit
    {
        public ArtSmockDemo()
            : base()
        {
            health = 0;
            strength = 0;
            defense = 68;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Smock of Desperation";
            level = 7;
            description = "A must-have for all those who are struggling to survive in the art business.";
            icon = Game1.equipmentTextures[name];
            passiveAbility = PassiveManager.allPassives["Shield of Desperation"];

        }
    }

}

