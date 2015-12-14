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
    class PyramidKeyStoryItem : StoryItem
    {

        public PyramidKeyStoryItem(int x, int y)
            : base(x, y)
        {
            name = "Pyramid Key";
            description = "An ancient key made of sandstone";

            texture = Game1.storyItems[name];
            rec = new Rectangle(x, y, texture.Width, texture.Height);
            icon = Game1.storyItemIcons[name];
        }
    }
}
