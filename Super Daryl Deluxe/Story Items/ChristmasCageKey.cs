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
    class ChristmasCageKey : StoryItem
    {

        public ChristmasCageKey(int x, int y)
            : base(x, y)
        {
            name = "Christmas Cage Key";
            pickUpName = "Christmas Cage Key";
            description = "A key that locks the door on Christmas Spirit.";

            rec = new Rectangle(x, y, 70,70);
            icon = Game1.storyItemIcons["Silver Key"];
        }
    }
}
