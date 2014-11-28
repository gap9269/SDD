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
    class KeyRing : StoryItem
    {
        public KeyRing(int x, int y)
            : base(x, y)
        {
            name = "Key Ring";
            pickUpName = "Key Ring";
            description = "The janitor's key ring. It should open up all of the doors in the school.";

            icon = Game1.storyItemIcons[name];
            texture = Game1.storyItems[name];
            rec = new Rectangle(x, y, texture.Width, texture.Height);

        }

        public KeyRing(Boolean inChest)
            : base(inChest)
        {
            name = "Key Ring";
            pickUpName = "Key Ring";
            description = "The janitor's key ring. It should open up all of the doors in the school.";

            rec = new Rectangle(0, 0, 0, 0);
            icon = Game1.storyItemIcons[name];
        }
    }
}
