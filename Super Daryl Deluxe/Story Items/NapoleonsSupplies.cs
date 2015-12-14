using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class NapoleonsSupplies : StoryItem
    {
        public NapoleonsSupplies(int x, int y)
            : base(x, y)
        {
            name = "Napoleon's Supplies";
            pickUpName = "some supplies";
            description = "Barrels full of white flags.";

            rec = new Rectangle(0, 0, 0, 0);
            icon = Game1.storyItemIcons["Napoleon's Supplies"];
        }
    }
}
