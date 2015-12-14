using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class VoiceBox : Accessory
    {
        public VoiceBox()
            : base()
        {
            health = 180;
            strength = 45;
            defense = 115;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Voice Box";
            level = 10;
            description = "Now Daryl can talk! A must-have for those wanting to know the whole story.";
            icon = Game1.equipmentTextures[name];
            passiveAbility = PassiveManager.allPassives["Daryl Can Talk"];

        }
    }
}
