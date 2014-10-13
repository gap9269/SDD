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
    class AssortedNuts : StoryItem
    {
        public AssortedNuts(int x, int y)
            : base(x, y)
        {
            name = "Assorted Nuts";
            pickUpName = "a can of assorted nuts";
            description = "An old, moldy can of assorted nuts.";

            icon = Game1.storyItemIcons[name];
            texture = Game1.storyItems[name];
            rec = new Rectangle(x, y, texture.Width, texture.Height);
        }
    }
}
