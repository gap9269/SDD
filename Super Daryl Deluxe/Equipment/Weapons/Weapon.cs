using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{


    public class Weapon : Equipment
    {
        protected bool canHoldTwo;

        public bool CanHoldTwo { get { return canHoldTwo; } set { canHoldTwo = value; } }

        public Weapon()
            : base()
        {

        }
    }

    public class MelonMallet : Weapon
    {
        public MelonMallet()
            : base()
        {
            strength = 3;
            health = 0;
            name = "Melon-Mashing Mallet";
            moveSpeed = 0;
            jumpHeight = 0;
            level = 2;
            description = "Used to smash the shit out of all sorts of melons. \n\n";
            canHoldTwo = false;
icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 3);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 4);

            description = "Used to smash the shit out of all sorts of melons. \n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Used to smash the shit out of all sorts of melons. \n\n";
        }
    }

    public class DirtyBrokenHoe : Weapon
    {
        public DirtyBrokenHoe()
            : base()
        {
            strength = 3;
            health = 0;
            name = "Dirty Broken Hoe";
            moveSpeed = 0;
            jumpHeight = 0;
            level = 2;
            description = "An old hoe that has seen better days. \n\n";
            canHoldTwo = false;
icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 3);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 4);

            description = "An old hoe that has seen better days. \n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "An old hoe that has seen better days. \n\n";
        }
    }

    public class HandSaw : Weapon
    {

        public HandSaw()
            : base()
        {
            strength = 35;
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
