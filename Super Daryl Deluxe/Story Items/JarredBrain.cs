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
    class JarredBrain : StoryItem
    {

        public JarredBrain(int x, int y)
            : base(x, y)
        {
            name = "Jarred Brain";
            pickUpName = "Jarred Brain";
            description = "The jarred brain of a European Swamp Ape.";

            rec = new Rectangle(0, 0, 0, 0);
            icon = Game1.storyItemIcons["Jarred Brain"];
        }
    }
}
