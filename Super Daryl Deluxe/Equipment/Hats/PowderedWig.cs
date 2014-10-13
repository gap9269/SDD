using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class PowderedWig : Hat
    {
        public PowderedWig()
            : base()
        {
            health = 12;
            strength = 0;
            defense = 2;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Powdered Wig";
            level = 7;
            description = "Some say this very wig belonged to George Washington himself. \n\n";
           // //icon = Game1.equipSheetOne;
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 2);
            Health += NextRandomStat(0, 5);

            description = "Some say this very wig belonged to George Washington himself. \n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Some say this very wig belonged to George Washington himself. \n\n";
        }
    }
}
