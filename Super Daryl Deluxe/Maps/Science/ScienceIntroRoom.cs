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
    class ScienceIntroRoom : MapClass
    {
        static Portal toNorthHall;
        static Portal toScience101;

        public static Portal ToNorthHall { get { return toNorthHall; } }
        public static Portal ToScience101 { get { return toScience101; } }

        Texture2D farBack, rock1, rock2, rock3, rock4, rock5, fore, portalSprite;

        float v1, v2, v3, v4, v5; //Rock float velocities

        float y1, y2, y3, y4, y5; //Rock float Y positions

        Boolean up1, up2, up3, up4, up5;

        int portalFrame, portalFrameDelay;

        public ScienceIntroRoom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2500;
            mapName = "Intro To Science";
            portalFrameDelay = 5;

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            v5 = .3f;
            v1 = -.1f;
            up1 = false;
            v2 = .15f;
            v3 = .1f;
            v4 = -.4f;
            up4 = false;
        }

        public Rectangle getPortalSourceRec()
        {
            if (portalFrame < 7)
            {
                return new Rectangle(520 * portalFrame, 0, 520, 720);
            }
            else
                return new Rectangle(520 * (portalFrame - 7), 720, 520, 720);
        }

        public override void Update()
        {
            base.Update();

            if (!up5)
            {
                v5 += .01f;
                y5 += v5;

                if (v5 >= .7)
                    up5 = true;
            }
            else
            {
                v5 -= .01f;
                y5 += v5;

                if (v5 <= -.7)
                    up5 = false;
            }

            if (!up3)
            {
                v3 += .005f;
                y3 += v3;

                if (v3 >= .3)
                    up3 = true;
            }
            else
            {
                v3 -= .005f;
                y3 += v3;

                if (v3 <= -.3)
                    up3 = false;
            }

            if (!up4)
            {
                v4 += .01f;
                y4 += v4;

                if (v4 >= .5)
                    up4 = true;
            }
            else
            {
                v4 -= .01f;
                y4 += v4;

                if (v4 <= -.5)
                    up4 = false;
            }

            if (!up2)
            {
                v2 += .006f;
                y2 += v2;

                if (v2 >= .5)
                    up2 = true;
            }
            else
            {
                v2 -= .006f;
                y2 += v2;

                if (v2 <= -.5)
                    up2 = false;
            }

            if (!up1)
            {
                v1 += .008f;
                y1 += v1;

                if (v1 >= .45)
                    up1 = true;
            }
            else
            {
                v1 -= .008f;
                y1 += v1;

                if (v1 <= -.45)
                    up1 = false;
            }

            portalFrameDelay--;

            if (portalFrameDelay <= 0)
            {
                portalFrame++;

                if (portalFrame > 20)
                    portalFrame = 0;

                portalFrameDelay = 4;
            }
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Science\background"));
            farBack = content.Load<Texture2D>(@"Maps\Science\FarBack");
            fore = content.Load<Texture2D>(@"Maps\Science\foreground");
            portalSprite = content.Load<Texture2D>(@"Maps\Science\PortalSprite");
            rock1 = content.Load<Texture2D>(@"Maps\Science\rockOne");
            rock2 = content.Load<Texture2D>(@"Maps\Science\rockTwo");
            rock3 = content.Load<Texture2D>(@"Maps\Science\rockThree");
            rock4 = content.Load<Texture2D>(@"Maps\Science\rockFour");
            rock5 = content.Load<Texture2D>(@"Maps\Science\rockFive");
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            base.DrawBackgroundAndParallax(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(farBack, new Rectangle(0, 0, farBack.Width, farBack.Height), Color.White);
            s.End();
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(fore, new Rectangle(0, 0, fore.Width, fore.Height), Color.White);
            s.End();
        }

        public override void Draw(SpriteBatch s)
        {
            s.Draw(portalSprite, new Rectangle(2500 - 520, 0, 520, 720), getPortalSourceRec(), Color.White);

            base.Draw(s);
            s.Draw(rock1, new Vector2(1827, 265 + y1), Color.White);
            s.Draw(rock2, new Vector2(1754, 258 + y2), Color.White);
            s.Draw(rock3, new Vector2(1792, 131 + y3), Color.White);
            s.Draw(rock4, new Vector2(1586, 327 + y4), Color.White);
            s.Draw(rock5, new Vector2(1612, 142 + y5), Color.White);
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toNorthHall = new Portal(0, platforms[0], "IntroToScience");
            toNorthHall.FButtonYOffset = -10;
            toNorthHall.PortalNameYOffset = -10;
            toScience101 = new Portal(2240, platforms[0], "IntroToScience");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toNorthHall, NorthHall.ToScienceIntroRoom);
            portals.Add(toScience101, Science101.ToIntroRoom);
        }
    }
}
