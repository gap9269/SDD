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
    public class GhostLight : InteractiveObject
    {

        int lightTime;
        int flickAmount;
        int maxFlick;
        static Random lightRandom;

        public Boolean active, drawBulb, large, flickers;
        public Rectangle lightRec, outsideRec;

        public GhostLight(Game1 g, int x, int y, Boolean active = true, Boolean flickers = false, Boolean drawBulb = true, Boolean large = false)
            : base(g, false)
        {
            if (large)
            {
                rec = new Rectangle(x, y, 1230, 720);
                lightRec = new Rectangle(x + 186, y + 158, 855, 564);
                outsideRec = new Rectangle(x + 86, y + 57, 1065, 663);
            }
            else
            {
                rec = new Rectangle(x, y, 998, 691);
                lightRec = new Rectangle(x + 242, y + 200, 510, 436);
                outsideRec = new Rectangle(x + 139, y + 184, 724, 461);
            }

            lightRandom = new Random();
            maxFlick = lightRandom.Next(2, 8);
            this.drawBulb = drawBulb;
            this.active = active;
            this.large = large;
            this.flickers = flickers;
        }

        public override void Update()
        {
            base.Update();

            if (flickers)
            {
                lightTime--;

                if (lightTime <= 0)
                {
                    active = !active;
                    lightTime = lightRandom.Next(2, 5);
                    flickAmount++;

                    if (flickAmount == maxFlick)
                    {
                        int onOff = lightRandom.Next(2);

                        if (onOff == 0)
                        {
                            lightTime = lightRandom.Next(60, 200);
                            active = true;
                        }
                        else
                        {
                            lightTime = lightRandom.Next(60, 200);
                            active = false;
                        }
                        flickAmount = 0;
                        maxFlick = lightRandom.Next(2, 8);
                    }
                }
            }
        }

        public void UpdatePosition()
        {
            if (large)
            {
                lightRec = new Rectangle(rec.X + 186, rec.Y + 158, 855, 564);
                outsideRec = new Rectangle(rec.X + 86, rec.Y + 57, 1065, 663);
            }
            else
            {
                lightRec = new Rectangle(rec.X + 242, rec.Y + 200, 510, 436);
                outsideRec = new Rectangle(rec.X + 139, rec.Y + 184, 724, 461);
            }
        }

        public override void Draw(SpriteBatch s)
        {
            int largeXOffset = 0;
            int largeYOffset = 0;

            if (large)
            {
                largeXOffset = 116;
                largeYOffset = 64;
            }

            if (drawBulb)
                s.Draw(Game1.interactiveObjects["Ghost Light"], new Rectangle(rec.X + largeXOffset, rec.Y + largeYOffset, 998, 691), new Rectangle(1996, 0, 998, 691), Color.White);

            if (active && game.ChapterTwo.ChapterTwoBooleans["lightsTurnedOn"])
            {
                s.Draw(Game1.interactiveObjects["Ghost Light"], new Rectangle(rec.X + largeXOffset, rec.Y + largeYOffset, 998, 691), new Rectangle(2996, 0, 998, 691), Color.White);
            }
        }

        public void DrawGlow(SpriteBatch s)
        {
            if (active && game.ChapterTwo.ChapterTwoBooleans["lightsTurnedOn"])
            {
                if(large)
                    s.Draw(Game1.interactiveObjects["Ghost Light"], rec, new Rectangle(0, 691, 1230, 720), Color.White);
                else
                    s.Draw(Game1.interactiveObjects["Ghost Light"], rec, new Rectangle(0, 0, 998, 691), Color.White);
            }
        }
    }
}