using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class RileysBow : Accessory
    {
        public RileysBow()
            : base()
        {
            health = 2;
            strength = 1;
            defense = 0;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Riley's Bow";
            level = 1;
            description = "The cute pink bow from a sweet old lady's pet rat you stepped on.\n\n";
            sellPrice = .50f;
            icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 3);

            description = "The cute pink bow from a sweet old lady's pet rat you killed.\n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "The cute pink bow from a sweet old lady's pet rat you killed.\n\n";
        }
    }
}
