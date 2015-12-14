using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class HatOfBrownieSummoning : Hat
    {
        public HatOfBrownieSummoning()
            : base()
        {
            health = 325;
            strength = 50;
            defense = 100;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Hat of Brownie Summoning";
            level = 10;
            description = "Press \"Enter\" to summon a tray of brownies.";
            icon = Game1.equipmentTextures[name];
            passiveAbility = PassiveManager.allPassives["Summon Brownies"];
        }
    }
}
