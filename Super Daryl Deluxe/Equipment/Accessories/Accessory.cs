using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class Accessory : Equipment
    {
        public Accessory()
            : base()
        {

        }
    }

    public class JarOfDirt : Accessory
    {
        public JarOfDirt()
            : base()
        {
            health = 2;
            strength = 2;
            defense = 2;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Jar Of Dirt";
            level = 2;
            description = "A sweet-ass jar of dirt.\n\n";
icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 3);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

            description = "A sweet-ass jar of dirt.\n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "A sweet-ass jar of dirt.\n\n";
        }
    }

    public class SoloCup : Accessory
    {
        public SoloCup()
            : base()
        {
            health = 60;
            strength = 20;
            defense = 6;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Solo Cup";
            level = 14;
            description = "It's full of water.\n\n";
icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 3);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

            description = "It's full of water.\n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "It's full of water.\n\n";
        }
    }

    public class BeerGoggles : Accessory
    {
        public BeerGoggles()
            : base()
        {
            health = 0;
            strength = 5;
            defense = 5;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Beer Goggles";
            level = 1;
            description = "Do these things even work?\n\n";
icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 3);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

            description = "Do these things even work?\n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Do these things even work?\n\n";
        }
    }
}
