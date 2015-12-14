using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    class Money:Equipment
    {
        float amount;
        public float Amount { get { return amount; } }

        public Money(float amount)
            : base()
        {
            this.amount = amount;

            name = "Lunch Money";
            description = "Some lunch money!";
            icon = Game1.equipmentTextures["Lunch Money"];
        }
    }
}
