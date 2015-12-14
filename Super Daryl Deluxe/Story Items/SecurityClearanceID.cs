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
    class SecurityClearanceID : StoryItem
    {
        public SecurityClearanceID(int x, int y)
            : base(x, y)
        {
            name = "Security Clearance I.D";
            pickUpName = "a Security Clearance I.D";
            description = "An I.D card that gives you high security clearance in the Theater An Der Wien.";

            rec = new Rectangle(x, y, texture.Width, texture.Height);
            icon = Game1.storyItemIcons[name];
            texture = Game1.storyItems[name];
        }
    }
}
