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
    public class WallSwitch : Switch
    {
        public WallSwitch(Texture2D tex, Rectangle r)
            : base(tex, r)
        {
            rec = r;
            texture = tex;
        }

        public WallSwitch(Texture2D tex, Rectangle r, int ac)
            : base(tex, r, ac)
        {
            maxTimeActive = ac;
            rec = r;
            texture = tex;
            timedSwitch = true;
        }

        public override Rectangle GetSourceRectangle()
        {
            if (active)
                return new Rectangle(333, 0, 333, 335);
            else
                return new Rectangle(0, 0, 333, 335);

        }
    }
}
