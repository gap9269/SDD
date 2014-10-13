using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class UsedTissue : Accessory
    {
        public UsedTissue()
            : base()
        {
            health = 2;
            strength = 0;
            defense = 0;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Used Tissue";
            level = 1;
            description = "A tissue covered in snot.\n\n";
            ////icon = Game1.equipSheetOne;
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 3);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

            description = "A tissue covered in snot.\n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "A tissue covered in snot.\n\n";
        }
    }
}
