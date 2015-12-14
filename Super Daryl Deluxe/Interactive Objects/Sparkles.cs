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
    public class Sparkles
    {
        public Rectangle rec;
        int frame, timer;

        public Sparkles(int x, int y)
        {
            rec = new Rectangle(x, y, 70, 70);
            timer = 10;
        }

        public void Update()
        {
            timer--;

            if (timer == 0)
            {
                frame++;
                timer = 10;

                if (frame == 6)
                {
                    frame = 1;
                }
            }
        }

        public void Draw(SpriteBatch s)
        {
            s.Draw(Game1.interactiveObjects["Sparkles"], new Rectangle(rec.X, rec.Y, 70, 70), new Rectangle(70 * frame, 0, 70, 70), Color.White);
        }
    }
}
