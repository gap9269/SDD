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
    class OldBattery : StoryItem
    {
        public OldBattery()
            : base(false)
        {
            name = "Old Battery";
            pickUpName = "an Old Battery";
            description = "A cheap, old battery.";

            rec = new Rectangle(0, 0, 0, 0);
            icon = Game1.storyItemIcons["Old Battery"];
        }
    }
}
