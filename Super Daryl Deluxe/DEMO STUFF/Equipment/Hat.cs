using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class PartyHatDemo : Hat
    {
        public PartyHatDemo()
            : base()
        {
            health = 200;
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
