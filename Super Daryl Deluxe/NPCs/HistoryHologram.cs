using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class HistoryHologram : NPC
    {

        public Boolean turningOff = false;
        public Boolean finished = false;
        public HistoryHologram(Texture2D sprite, List<String> d, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name, Boolean stat)
            : base(sprite, d, r, play, f, g, mName, name, stat)
        {

        }

        public override Rectangle GetSourceRectangle(int frame)
        {

            if (turningOff)
            {
                return new Rectangle(516 * moveFrame, 1164, 516, 388);
            }
            else
            {
                if(moveFrame < 8)
                    return new Rectangle(516 * moveFrame, 0, 516, 388);
                else if (moveFrame < 16)
                    return new Rectangle(516 * (moveFrame - 8), 388, 516, 388);
                else
                    return new Rectangle(516 * (moveFrame - 16), 776, 516, 388);
            }
        }

        public void TurnOff()
        {
            turningOff = true;
            moveFrame = 0;
            frameDelay = 5;
        }

        public override void Update()
        {
            base.Update();

            frameDelay--;

            if (frameDelay <= 0)
            {
                frameDelay = 5;
                moveFrame++;

                if (moveFrame > 19 && !turningOff)
                    moveFrame = 0;
            }
        }

    }
}
