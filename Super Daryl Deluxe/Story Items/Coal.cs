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
    class Coal : StoryItem
    {

        public Coal(Boolean inChest)
            : base(inChest)
        {
            name = "Coal";
            pickUpName = "a chunk of coal";
            description = "A chunk of dirty coal. Try eating it.";

            rec = new Rectangle(0, 0, 0, 0);
            icon = Game1.storyItemIcons["Coal"];
        }

        public Coal(int x, int y)
            : base(x, y)
        {
            name = "Coal";
            pickUpName = "a chunk of coal";
            description = "A chunk of dirty coal. Try eating it.";

            icon = Game1.storyItemIcons["Coal"];
            texture = Game1.storyItems["Coal"];
            rec = new Rectangle(x, y, 70, 70);

        }
    }
}
