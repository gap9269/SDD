using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class Karma:Equipment
    {
        int amount;
        public int Amount { get { return amount; } }

        public Karma(int amount)
            : base()
        {
            this.amount = amount;

            name = "Karma";
            description = "karma";
            icon = Game1.equipmentTextures["Karma"];
        }
    }
}

