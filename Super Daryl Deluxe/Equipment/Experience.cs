using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class Experience : Equipment
    {
        int amount;
        public int Amount { get { return amount; } }

        public Experience(int amount)
            : base()
        {
            this.amount = amount;

            name = "Experience";
            description = "Exp";
            icon = Game1.equipmentTextures["Experience"];
        }
    }
}

