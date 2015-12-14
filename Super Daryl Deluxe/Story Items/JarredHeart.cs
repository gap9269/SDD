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
    class JarredHeart : StoryItem
    {

        public JarredHeart(int x, int y)
            : base(x, y)
        {
            name = "Jarred Heart";
            pickUpName = "Jarred Heart";
            description = "Yes, it still beats, but can it love?";

            rec = new Rectangle(0, 0, 0, 0);
            icon = Game1.storyItemIcons["Jarred Heart"];
        }
    }
}
