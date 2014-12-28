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
    class PitfallCover : BreakableObject
    {
        public enum PitfallType
        {
            leaves
        }
        public PitfallType type;

        public PitfallCover(Game1 g, int x, int y, Boolean pass, Boolean fore, PitfallType typ)
            : base(g, x, y, Game1.interactiveObjects["Pitfall"], pass, 1, 0, 0, fore)
        {
            switch (type)
            {
                case PitfallType.leaves:
                    rec = new Rectangle(x, y - sprite.Height, 290, 123);
                    vitalRec = new Rectangle(rec.X + 45, rec.Y + 50, 180, 100);
                    break;
            }
            type = typ;
        }


        public override Rectangle GetSourceRec()
        {
            switch (type)
            {
                case PitfallType.leaves:
                    return new Rectangle(0, 0, 461, 196);
            }

            return new Rectangle();
        }

        public override void Update()
        {
            vitalRec = new Rectangle(rec.X + 75, rec.Y + 50, 120, 100);
            if (!finished)
            {
                if (Game1.Player.VitalRec.Intersects(vitalRec))
                {
                    finished = true;
                    Chapter.effectsManager.AddSmokePoof(new Rectangle(rec.X, rec.Y, rec.Width, rec.Width), 2);
                }
            }

            base.Update();
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (!finished)
            {
                s.Draw(Game1.whiteFilter, vitalRec, Color.Black);
            }
        }
    }

}
