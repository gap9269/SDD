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
    class HistoryEntrance : MapClass
    {
        static Portal toIncaVillage;
        static Portal toNorthHall;
        static Portal toMongolCamp;

        public static Portal ToMongolCamp { get { return toMongolCamp; } }
        public static Portal ToNorthHall { get { return toNorthHall; } }
        public static Portal ToIncaVillage { get { return toIncaVillage; } }

        Texture2D foreground, sky, sky2, parallax, clouds, clouds2, door;

        float cloudPosX = 0;
        float doorAlpha;

        public HistoryEntrance(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 4340;
            mapWidth = 6000;
            mapName = "Intro to History";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 0;
            zoomLevel = .33f;
            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Entrance\background"));
            background.Add(content.Load<Texture2D>(@"Maps\History\Entrance\background2"));
            sky = content.Load<Texture2D>(@"Maps\History\Entrance\sky");
            sky2 = content.Load<Texture2D>(@"Maps\History\Entrance\sky2");
            parallax = content.Load<Texture2D>(@"Maps\History\Entrance\parallax");
            clouds = content.Load<Texture2D>(@"Maps\History\Entrance\clouds");
            clouds2 = content.Load<Texture2D>(@"Maps\History\Entrance\clouds2");
            foreground = content.Load<Texture2D>(@"Maps\History\Entrance\foreground");
            door = content.Load<Texture2D>(@"Maps\Music\Entrance\door");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }


        public override void Update()
        {
            base.Update();


            //KEEP CAMERA LOCKED WHEN PLAYER IS AT BOTTOM OF MAP
            //zoomLevel = .23f;
            //if (game.Camera.center.Y > -900)
            //{
            //    game.Camera.center.Y = -899;
            //}

            //zoomLevel = .2133f;
            //if (game.Camera.center.Y > -1050)
            //{
            //    game.Camera.center.Y = -1049;
            //}
            if (game.Camera.center.Y > -420)
            {
                game.Camera.center.Y = -419;
            }

            cloudPosX += 1f;

            if (cloudPosX >= 12000)
                cloudPosX = -6000;


        }

        public override void SetPortals()
        {
            base.SetPortals();

            toIncaVillage = new Portal(5750, platforms[0], "Intro to History");
            toNorthHall = new Portal(3065, platforms[0], "Intro to History");
            toMongolCamp = new Portal(70, platforms[0], "Intro to History");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toNorthHall, NorthHall.ToHistoryIntroRoom);
            portals.Add(toIncaVillage, IncaVillage.ToHistoryIntro);
            portals.Add(toMongolCamp, MongolCamp.ToIntroRoom);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(foreground, new Vector2(mapWidth - foreground.Width, mapRec.Y), Color.White);

            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }
            }

            if (player.VitalRec.X < 3500 && player.VitalRecX > 2790)
            {
                if (doorAlpha < .7f)
                    doorAlpha += .05f;
            }
            else
            {
                if (doorAlpha > 0)
                    doorAlpha -= .05f;
            }

            s.Draw(door, new Vector2(2790, 371), Color.White * doorAlpha);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            base.DrawBackgroundAndParallax(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(sky2, new Vector2(sky.Width, mapRec.Y), Color.White);

            s.Draw(clouds, new Vector2(cloudPosX, mapRec.Y), Color.White);
            s.Draw(clouds2, new Vector2(cloudPosX + clouds.Width, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1, this, game));
            s.Draw(parallax, new Vector2(1768, mapRec.Y), Color.White);
            s.End();
        }
    }
}
