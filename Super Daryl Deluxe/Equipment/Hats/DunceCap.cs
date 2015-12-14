using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class DunceCap : Hat
    {
        public DunceCap()
            : base()
        {
            health = 3;
            strength = 0;
            defense = 0;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Dunce Cap";
            level = 1;
            description = "You've gotta be a certain kind of smart to wear this. \n\n";
            ////icon = Game1.equipSheetOne;
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 5);

            description = "You've gotta be a certain kind of smart to wear this. \n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "You've gotta be a certain kind of smart to wear this. \n\n";
        }
    }
}
