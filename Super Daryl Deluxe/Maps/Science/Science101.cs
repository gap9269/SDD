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
    class Science101 : MapClass
    {
        static Portal toIntroRoom;
        static Portal toScience102;
        static Portal toScience104;

        public static Portal ToIntroRoom { get { return toIntroRoom; } }
        public static Portal ToScience102 { get { return toScience102; } }
        public static Portal ToScience104 { get { return toScience104; } }

        int portalFrame, portalFrameDelay, eyeFrame, eyeFrameDelay;

        MovingPlatform movingPlat;
        List<Vector2> targets;

        MovingPlatform movingPlat2;
        List<Vector2> targets2;

        Vector2 photoPos, equationPos;

        Texture2D elevator, equation, float1, float2, photosynthesis, planet, platform, pyramid, sunStuff, portalSheet, eyeSheet;

        Boolean startEquationMovement = false;
        Boolean startPhotoMovement = false;
        Boolean startEyeAnimation = false;
        float v1, v2; //float velocities

        float y1, y2; //float Y positions

        Boolean up1, up2;

        public Science101(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 3000;
            mapWidth = 4350;
            mapName = "Science 101";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
            targets = new List<Vector2>();
            movingPlat = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1820, -137, 370, 50),
                false, false, false, targets, 3, 100);

            platforms.Add(movingPlat);

            targets2 = new List<Vector2>();
            movingPlat2 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3640, -740, 300, 50),
                true, false, false, targets2, 3, 100);

            platforms.Add(movingPlat2);

            zoomLevel = .85f;
            portalFrameDelay = 4;
            eyeFrameDelay = 4;
            photoPos = new Vector2(0, 500);
            equationPos = new Vector2(3800, 500);

            v1 = -.1f;
            up1 = false;

            eyeFrame = -1;

            Barrel bar = new Barrel(game, 1300, -280 + 155, Game1.interactiveObjects["Barrel"], true, 1, 0, .12f, false, Barrel.BarrelType.ScienceBarrel);
            interactiveObjects.Add(bar);
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            startEquationMovement = false;
            startPhotoMovement = false;
            photoPos = new Vector2(0, 500);
            equationPos = new Vector2(3800, 500);
            startEyeAnimation = false;
            eyeFrame = -1;
            eyeFrameDelay = 5;
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps/Science/101/background"));
            background.Add(content.Load<Texture2D>(@"Maps/Science/101/background2"));

            planet = content.Load<Texture2D>(@"Maps/Science/101/planet");
            elevator = content.Load<Texture2D>(@"Maps/Science/101/elevator");
            equation = content.Load<Texture2D>(@"Maps/Science/101/equation");
            float1 = content.Load<Texture2D>(@"Maps/Science/101/float1");
            float2 = content.Load<Texture2D>(@"Maps/Science/101/float2");
            photosynthesis = content.Load<Texture2D>(@"Maps/Science/101/photosynthesis");
            platform = content.Load<Texture2D>(@"Maps/Science/101/platform");
            pyramid = content.Load<Texture2D>(@"Maps/Science/101/pyramid");
            sunStuff = content.Load<Texture2D>(@"Maps/Science/101/sunStuff");
            portalSheet = content.Load<Texture2D>(@"Maps/Science/101/portalSheet");
            eyeSheet = content.Load<Texture2D>(@"Maps/Science/101/EyeSheet");
        }
        public Rectangle GetEyeSourceRec()
        {
            if (eyeFrame < 15)
                return new Rectangle(eyeFrame * 254, 0, 254, 185);
            else
                return new Rectangle((eyeFrame - 15) * 254, 185, 254, 185);

        }

        public override void Update()
        {
            base.Update();

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

            #region Move the words across the foreground
            if (startPhotoMovement)
                photoPos += new Vector2(4f, -2);

            if (photoPos.X > mapWidth * 5)
            {
                photoPos = new Vector2(0, 500);
            }

            if (player.RecX > 750 && player.RecX < 1300 && !startPhotoMovement)
                startPhotoMovement = true;

            if(startEquationMovement)
                equationPos += new Vector2(-2, -3.5f);

            if(equationPos.X <= -mapWidth * 2)
                equationPos = new Vector2(3800, 500);

            if (player.RecX > 2700 && !startEquationMovement)
                startEquationMovement = true;
            #endregion

            #region Portal and Eye animation
            portalFrameDelay--;

            if (portalFrameDelay <= 0)
            {
                portalFrame++;

                if (portalFrame > 17)
                    portalFrame = 0;

                portalFrameDelay = 4;
            }

            if (player.RecX > 3600 && player.RecX < 3800)
                startEyeAnimation = true;

            if (startEyeAnimation)
            {
                eyeFrameDelay--;

                if (eyeFrameDelay <= 0)
                {
                    eyeFrame++;

                    if (eyeFrame > 250)
                        eyeFrame = 0;

                    eyeFrameDelay = 5;
                }
            }
            #endregion

            if (player.CurrentPlat == movingPlat && game.MapBooleans.prologueMapBooleans["targetsAdded"] == false)
            {
                game.MapBooleans.prologueMapBooleans["targetsAdded"] = true;
            }

            if (player.CurrentPlat == movingPlat2 && game.MapBooleans.prologueMapBooleans["targets2Added"] == false)
            {
                game.MapBooleans.prologueMapBooleans["targets2Added"] = true;
            }

            if (game.MapBooleans.prologueMapBooleans["targetsAdded"] && targets.Count == 0)
            {
                targets.Add(new Vector2(2910, -137));
                targets.Add(new Vector2(1820, -137));
            }

            if (game.MapBooleans.prologueMapBooleans["targets2Added"] && targets2.Count == 0)
            {
                targets2.Clear();
                targets2.Add(new Vector2(3640, -455));
                targets2.Add(new Vector2(3640, -740));
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(platform, new Vector2(movingPlat.RecX - 17, movingPlat.RecY- 40), Color.White);


            s.Draw(eyeSheet, new Rectangle(3886, mapRec.Y + 1416, 254, 185), GetEyeSourceRec(), Color.White);
            s.Draw(elevator, new Vector2(movingPlat2.RecX - 65, movingPlat2.RecY - 100), Color.White);
            s.Draw(portalSheet, new Rectangle(0, mapRec.Y + 1428, 219, 484), new Rectangle(219 * portalFrame, 0, 219, 484), Color.White);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(float1, new Vector2(3746, mapRec.Y + 1564 + y1), Color.White);
            s.Draw(float2, new Vector2(3872, mapRec.Y + 1497 + y2), Color.White);

            s.Draw(photosynthesis, photoPos, Color.White);
            s.Draw(equation, equationPos, Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            base.DrawBackgroundAndParallax(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(Game1.whiteFilter, new Rectangle(0, mapRec.Y, mapRec.Width, mapRec.Height),new Color(25,25,25));
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.75f, this, game));
            s.Draw(pyramid, new Rectangle(2443, mapRec.Y + 1256, pyramid.Width, pyramid.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.8f, this, game));
            s.Draw(sunStuff, new Rectangle(1768, mapRec.Y + 1442, sunStuff.Width, sunStuff.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.95f, this, game));
            s.Draw(planet, new Rectangle(1017, mapRec.Y + 931, planet.Width, planet.Height), Color.White);
            s.End();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toIntroRoom = new Portal(20, platforms[0], "Science101");
            toScience102 = new Portal(4175, -228, "Science101");
            toScience104 = new Portal(4153, -740, "Science101");

            toScience104.FButtonYOffset = -30;
            toScience104.PortalNameYOffset = -30;

            toScience102.FButtonYOffset = -27;
            toScience102.PortalNameYOffset = -27;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toIntroRoom, ScienceIntroRoom.ToScience101);
            portals.Add(toScience102, Science102.ToScience101);
            portals.Add(toScience104, Science104.ToScience101);
        }
    }
}