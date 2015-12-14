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
    public class ImpairedVision : Passive
    {
        public ImpairedVision(Game1 g)
            : base(g)
        {
            name = "Impaired Vision";
            healthModifier = 300;
        }

        public override void LoadPassive()
        {
            base.LoadPassive();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
