using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class SuperWeenieShirtDemo : Outfit
    {
        public SuperWeenieShirtDemo()
            : base()
        {
            health = 0;
            strength = 0;
            defense = 68;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Super Weenie Shirt";
            level = 4;
            description = "Weenies love flowers.";
            sellPrice = 1.00f;
            icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

            description = "\"Weenies love flowers.\" - Trenchcoat Kid";


        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "\"Weenies love flowers.\" - Trenchcoat Kid";


        }
    }
}
