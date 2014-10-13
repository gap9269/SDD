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
    class Beer : StoryItem
    {
        public Beer(int x, int y)
            : base(x, y)
        {
            name = "Beer";
            pickUpName = "some beer";
            description = "Some beer from Chelsea's Party.";

            icon = Game1.storyItemIcons[name];
            texture = Game1.storyItems[name];
            rec = new Rectangle(x, y, 78, 64);
        }
    }
}
