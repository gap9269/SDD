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
    class EmptyBottle : StoryItem
    {

        public EmptyBottle(int x, int y)
            : base(x, y)
        {
            name = "Empty Bottle";
            pickUpName = "Empty Bottle";
            description = "Unlike it's older counterparts, this bottle never runs out of precious liquid once it's filled.";

            rec = new Rectangle(0, 0, 0, 0);
            icon = Game1.storyItemIcons["Empty Bottle"];
        }
    }
}
