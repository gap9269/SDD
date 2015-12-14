using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class GoblinToe : Accessory
    {
        public GoblinToe()
            : base()
        {
            health = 20;
            strength = 12;
            defense = 12;
            name = "Goblin Toe";
            level = 9;
            description = "It's great that you decided to pick this up.";
            icon = Game1.equipmentTextures[name];
            sellPrice = 9.75f;
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 3);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
        }
    }
}
