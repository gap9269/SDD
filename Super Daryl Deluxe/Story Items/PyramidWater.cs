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
    class PyramidWater : StoryItem
    {

        public PyramidWater(int x, int y)
            : base(x, y)
        {
            name = "Pyramid Water";
            pickUpName = "Pyramid Water";
            description = "Thousand year old water from the musty pyramid springs. Try pouring it on stuff!";

            rec = new Rectangle(0, 0, 0, 0);
            icon = Game1.storyItemIcons["Pyramid Water"];
        }
    }
}
