using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{

    public class HandSawDemo : Weapon
    {

        public HandSawDemo()
            : base()
        {
            strength = 25;
            health = 0;
            name = "Hand Saw";
            moveSpeed = 0;
            jumpHeight = 0;
            level = 14;
            description = "It's said that the previous owner of this handsaw will haunt anyone who holds it.";
            canHoldTwo = true;

            passiveAbility = PassiveManager.allPassives["Ghost in the Saw"];

            icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 3);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 4);

            description = "It's said that the previous owner of this handsaw will haunt anyone who holds it.";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "It's said that the previous owner of this handsaw will haunt anyone who holds it.";
        }
    }
}
