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
    class Scissors : StoryItem
    {
        public Scissors(int x, int y)
            : base(x, y)
        {
            name = "Scissors";
            pickUpName = "a pair of metal scissors";
            description = "A pair of 100% conductable metal scissors.";

            texture = Game1.whiteFilter;
            rec = new Rectangle(x, y, texture.Width, texture.Height);
            icon = Game1.storyItemIcons[name];
        }
    }
}
