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
    class JanitorCloset:MapClass
    {
        static Portal toArtHall;

        public static Portal ToArtHall { get { return toArtHall; } }

        Texture2D fore, lights, drip, splashSheet, overlay;

        Dictionary<String, Texture2D> janitorSleeping;
        int janitorFrame;
        int janitorDelay = 5;

        float dropPosY = -50;

        int splashFrame, splashFrameDelay;

        int timeUntilNextDrip;
        float dripVel;

        public JanitorCloset(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1400;
            mapName = "Janitor's Closet";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            TreasureChest janitorsChest = new TreasureChest(Game1.treasureChestSheet, 1100, 624, player, 0, new KeyRing(false), this);
            treasureChests.Add(janitorsChest);
        }

        public override void Update()
        {
            base.Update();

            if (splashFrame < 5)
            {
                splashFrameDelay--;

                if (splashFrameDelay == 0)
                {
                    splashFrame++;
                    splashFrameDelay = 4;
                }
            }

            if (timeUntilNextDrip > 0)
            {
                timeUntilNextDrip--;
            }
            else
            {
                if (dropPosY < 653)
                {
                    dripVel += GameConstants.GRAVITY / 2;
                    dropPosY += dripVel;
                }
                else
                {
                    splashFrame = 0;
                    splashFrameDelay = 4;
                    dropPosY = -50;
                    dripVel = 0;
                    timeUntilNextDrip = 250;
                }
            }
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\School\Closet\background"));

            fore = content.Load<Texture2D>(@"Maps\School\Closet\foreground");
            lights = content.Load<Texture2D>(@"Maps\School\Closet\lightsOn");
            overlay = content.Load<Texture2D>(@"Maps\School\Closet\overlay");
            drip = content.Load<Texture2D>(@"Maps\School\Closet\drip");
            splashSheet = content.Load<Texture2D>(@"Maps\School\Closet\splashSheet");

            if(game.chapterState == Game1.ChapterState.prologue)
                janitorSleeping = ContentLoader.LoadContent(content, "Maps\\School\\Closet\\Janitor");
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.chapterState == Game1.ChapterState.prologue)
            {
                janitorDelay--;

                if (janitorDelay == 0)
                {
                    janitorFrame++;
                    janitorDelay = 20;

                    if (janitorFrame == 4)
                        janitorDelay = 25;

                    if (janitorFrame > 5)
                        janitorFrame = 0;
                }
                s.Draw(janitorSleeping["janitor sleeping" + janitorFrame], new Vector2(876, 461), Color.White);
            }
            else
            {
                s.Draw(lights, new Vector2(404, 172), Color.White);
            }

            s.Draw(drip, new Vector2(1055, dropPosY), Color.White);

            if(splashFrame < 5)
                s.Draw(splashSheet, new Vector2(1000, 620), new Rectangle(400 * splashFrame, 0, 400, 100), Color.White);
        }

        public override void DrawMapOverlay(SpriteBatch s)
        {
            base.DrawMapOverlay(s);

            if(game.chapterState == Game1.ChapterState.prologue)
                s.Draw(overlay, new Vector2(0, 0), Color.White);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
    null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(fore, new Rectangle(0, 0, fore.Width, fore.Height), Color.White);
            s.End();
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toArtHall = new Portal(0, platforms[0], "JanitorsCloset");
            toArtHall.FButtonYOffset = -10;
            toArtHall.PortalNameYOffset = -10;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toArtHall, ArtHall.ToJanitorsCloset);
        }

        public override void AddNPCs()
        {
            base.AddNPCs();
        }
    }
}
