using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class ArtSmock : Outfit
    {
        public ArtSmock()
            : base()
        {
            health = 25;
            strength = 0;
            defense = 5;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Smock of Desperation";
            level = 7;
            description = "A must-have for all those who are struggling to survive in the art business.";
            icon = Game1.equipmentTextures[name];
            passiveAbility = PassiveManager.allPassives["Shield of Desperation"];
            sellPrice = 11.25f;
        }
    }

}

