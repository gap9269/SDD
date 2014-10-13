using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace ISurvived
{
    class PeltIOU : StoryItem
    {
        public PeltIOU(int x, int y)
            : base(x, y)
        {
            name = "Pelt Kid's IOU";
            pickUpName = "Pelt Kid's IOU card";
            description = "A cheap piece of cardboard with 'IOU' scribbled on it.";

            rec = new Rectangle(x, y, texture.Width, texture.Height);
            icon = Game1.storyItemIcons[name];
            texture = Game1.storyItems[name];
        }
    }
}
