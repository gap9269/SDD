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
    class InBetweenField : MapClass
    {
        static Portal toAnotherSpookyField;
        static Portal toBathroom;
        static Portal toWorkers;

        public static Portal ToWorkers { get { return toWorkers; } }
        public static Portal ToAnotherSpookyField { get { return toAnotherSpookyField; } }
        public static Portal ToBathroom { get { return toBathroom; } }

        Texture2D foreground1, foreground2, backField, barn, sky, clouds, moonHappy, moonAngry, outhouse;

        float cloudPos = 1500;

        float moonFaceAlpha = 0f;
        int moonFaceTimer;
        bool moonIsAngry = false;
        Random randomMoonTime;
        int timeUntilNextMoonFace;
        LivingLocker locker;

        public InBetweenField(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1150;
            mapWidth = 1700;
            mapName = "InBetween Field";

            randomMoonTime = new Random();

            timeUntilNextMoonFace = randomMoonTime.Next(1000, 6000);

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;
            zoomLevel = .9f;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            locker = new LivingLocker(game, new Rectangle(250, 100, 500, 400));
            interactiveObjects.Add(locker);

        }

        public override void LoadContent()
        {
            base.LoadContent();

            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\SpookyField"));
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\SpookyField2"));

            foreground1 = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldFore");
            foreground2 = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldFore2");

            backField = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldBack");
            barn = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldBackBack");

            sky = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldSky");

            clouds = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldClouds");

            moonHappy = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldMoonHappy");

            moonAngry = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldMoonAngry");

            outhouse = content.Load<Texture2D>(@"Maps\Outhouse");

            game.NPCSprites["Trenchcoat Employee"] = content.Load<Texture2D>(@"NPC\Main\trenchcoat");
            Game1.npcFaces["Trenchcoat Employee"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Trenchcoat");

        }
        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Trenchcoat Employee"] = Game1.whiteFilter;
            Game1.npcFaces["Trenchcoat Employee"].faces["Normal"] = Game1.whiteFilter;
        }


        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

        }


        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            locker.Health = 1;
            locker.Finished = false;
            locker.mState = LivingLocker.movestate.flying;

        }

        public override void Update()
        {
            base.Update();

            //CLOUD STUFF
            //1750 is just off the map with the parallax value clouds have
            cloudPos -= .4f;
            if (cloudPos + clouds.Width < 0)
                cloudPos = 1750;


            #region MOON FACES
            if (timeUntilNextMoonFace > 0 && moonFaceTimer <= 0)
                timeUntilNextMoonFace--;

            if (timeUntilNextMoonFace == 0 && moonFaceTimer <= 0)
            {
                moonFaceTimer = 300;

                int angry = randomMoonTime.Next(2);

                if (angry == 1)
                    moonIsAngry = true;
                else
                    moonIsAngry = false;
            }

            if (moonFaceTimer > 0)
            {
                moonFaceTimer--;

                if (moonFaceTimer == 0)
                {
                    timeUntilNextMoonFace = randomMoonTime.Next(1000, 6000);
                }
            }

            if (moonFaceTimer > 0 && moonFaceAlpha != 1)
            {
                if (moonIsAngry)
                    moonFaceAlpha += .01f;
                else
                    moonFaceAlpha += .01f;

                if (moonFaceAlpha > 1)
                    moonFaceAlpha = 1f;
            }
            else if (moonFaceTimer <= 0 && moonFaceAlpha != 0)
            {
                moonFaceAlpha -= .01f;

                if (moonFaceAlpha < 0)
                    moonFaceAlpha = 0f;
            }
            #endregion
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toAnotherSpookyField = new Portal(40, platforms[0], "InBetween Field");
            toWorkers = new Portal(1500, platforms[0], "InBetween Field");
            toBathroom = new Portal(1030, 420 + 253, "InBetween Field");
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(outhouse, new Rectangle(920, 390, outhouse.Width, outhouse.Height), Color.White);
            locker.Draw(s);
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toWorkers, WorkersField.ToAnotherSpookyField);
            portals.Add(toAnotherSpookyField, AnotherSpookyField.ToWorkersField);
            portals.Add(toBathroom, Bathroom.ToLastMap);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }
            }

            s.Draw(foreground1, new Rectangle(0, mapRec.Y, foreground1.Width, foreground1.Height), Color.White);
            s.Draw(foreground2, new Rectangle(foreground1.Width, mapRec.Y, foreground2.Width, foreground2.Height), Color.White);
            s.End();
        }


        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.05f, this, game));

            s.Draw(sky, new Rectangle(0, mapRec.Y + 70, sky.Width, sky.Height), Color.White);

            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.1f, this, game));
            s.Draw(clouds, new Rectangle((int)cloudPos, 0, clouds.Width, clouds.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.1f, this, game));
            s.Draw(barn, new Rectangle(0, mapRec.Y, barn.Width, barn.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.6f, this, game));
            s.Draw(backField, new Rectangle(0, mapRec.Y, backField.Width, backField.Height), Color.White);
            s.End();

        }
    }
}