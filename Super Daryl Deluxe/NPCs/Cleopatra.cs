using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class Cleopatra : NPC
    {
        //--Constructor for non-Quest NPC
        public Cleopatra(Texture2D sprite, List<String> d, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name, Boolean stat)
            : base(sprite, d, r, play, f, g, mName, name, stat)
        {

        }
        public Boolean chained = false;

        public override Rectangle GetSourceRectangle(int frame)
        {

            if (chained)
            {
                currentDialogueFace = "Chained";
                return new Rectangle(516, 0, 516, 388);
            }
            else
            {
                currentDialogueFace = "Normal";
                return new Rectangle(0, 0, 516, 388);
            }
        }

    }
}
