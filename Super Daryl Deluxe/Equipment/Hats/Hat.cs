using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class Hat : Equipment
    {
        public Hat()
            : base()
        {

        }
    }

    public class GardeningHat : Hat
    {
        public GardeningHat()
            : base()
        {
            health = 3;
            strength = 0;
            defense = 0;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Gardening Hat";
            level = 2;
            description = "Que? \n\n";
            icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 5);

            description = "Que? \n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Que? \n\n";
        }
    }

    public class ScarecrowHat : Hat
    {
        public ScarecrowHat()
            : base()
        {
            health = 25;
            strength = 0;
            defense = 0;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Scarecrow Hat";
            level = 5;
            description = "Stolen from a spooky scarecrow. \n\n";
icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 5);

            description = "Stolen from a spooky scarecrow. \n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Stolen from a spooky scarecrow. \n\n";
        }
    }

    public class PartyHat : Hat
    {
        public PartyHat()
            : base()
        {
            health = 100;
            strength = 0;
            defense = 0;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Party Hat";
            level = 14;
            description = "Ain't no party like a Party Hat party!\n\n";
icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 5);

            description = "Ain't no party like a Party Hat party! \n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Ain't no party like a Party Hat party! \n\n";
        }
    }
}
