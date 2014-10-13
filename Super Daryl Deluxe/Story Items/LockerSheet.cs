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
    class LockerSheet : StoryItem
    {
        public LockerSheet(int x, int y)
            : base(x, y)
        {
            name = "Piece of Paper";
            pickUpName = "a Piece of Paper";
            description = "A crumpled up piece of paper. It has a bunch of numbers \nwritten on it.";

            icon = Game1.storyItemIcons["Locker Combo"];
            texture = Game1.storyItems["Locker Combo"];
            rec = new Rectangle(x, y, texture.Width, texture.Height);

        }

        public LockerSheet(Boolean inChest)
            : base(inChest)
        {
            name = "Piece of Paper";
            pickUpName = "a Piece of Paper";
            description = "A crumpled up piece of paper. It has a bunch of numbers \nwritten on it.";

            rec = new Rectangle(0, 0, 0, 0);
            icon = Game1.storyItemIcons["Locker Combo"];
            texture = Game1.storyItems["Locker Combo"];
        }
    }
}
