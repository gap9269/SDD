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
    class LetterToCleopatra: StoryItem
    {
        public LetterToCleopatra(int x, int y)
            : base(x, y)
        {
            name = "Letter to Cleopatra";
            pickUpName = "a letter to Cleopatra";
            description = "An invitation to join forces, from Napoleon to Julius.";

            texture = Game1.whiteFilter;
            rec = new Rectangle(x, y, texture.Width, texture.Height);
            icon = Game1.storyItemIcons["Letter"];
        }
    }
}
