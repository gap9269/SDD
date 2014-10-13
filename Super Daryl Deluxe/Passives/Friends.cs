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
    public class Friends : Passive
    {

        public Friends(Game1 g)
            : base(g)
        {
            name = "You Have Friends!";
        }

        public override void LoadPassive()
        {
            base.LoadPassive();
        }
    }
}
