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
    class ChallengeRoomKey : StoryItem
    {

        public ChallengeRoomKey(Boolean inChest)
            : base(inChest)
        {
            name = "Challenge Room Key";
            pickUpName = "a Challenge Room Key!";
            description = "A key that unlocks a Challenge Room";

            rec = new Rectangle(0, 0, 0, 0);
            icon = Game1.storyItemIcons["Gold Key"];
        }

        public ChallengeRoomKey(int x, int y)
            : base(x, y)
        {
            name = "Challenge Room Key";
            pickUpName = "a Challenge Room Key!";
            description = "A key that unlocks a Challenge Room";

            icon = Game1.storyItemIcons["Gold Key"];
            texture = Game1.storyItems["Gold Key"];
            rec = new Rectangle(x, y, 100, 75);

        }
    }
}
