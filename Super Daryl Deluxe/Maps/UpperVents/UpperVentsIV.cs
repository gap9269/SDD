using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class UpperVentsIV : MapClass
    {
        static Portal toUpperVents3Bot;
        static Portal toUpperVents3Top;

        public static Portal ToUpperVents3Bot { get { return toUpperVents3Bot; } }
        public static Portal ToUpperVents3Top { get { return toUpperVents3Top; } }

        Platform door;

        Texture2D foreground, gate, bars;
        LivingLocker locker;

        public UpperVentsIV(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            yScroll = true;

            mapWidth = 2420;
            mapHeight = 1400;
            mapName = "Upper Vents IV";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);

            AddPlatforms();
            AddBounds();
            SetPortals();
            overlayType = 1;
            enemyAmount = 8;

            locker = new LivingLocker(game, new Rectangle(450, -300, 700, 400));
            interactiveObjects.Add(locker);

            door = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1940, 140, 420, 50), false, false, false);
            platforms.Add(door);

            TreasureChest chest = new TreasureChest(Game1.treasureChestSheet, 1390, 140, player, 0, new Textbook(), this);
            treasureChests.Add(chest);

            enemyNamesAndNumberInMap.Add("Fluffles the Rat", 0);
            enemyNamesAndNumberInMap.Add("Vent Bat", 0);
        }
        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_vents");
        }
        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Vents\Upper Vents 4\background"));
            foreground = content.Load<Texture2D>(@"Maps\Vents\Upper Vents 4\foreground");
            gate = content.Load<Texture2D>(@"Maps\Vents\Upper Vents 4\platform");
            Sound.LoadVentZoneSounds();
            bars = content.Load<Texture2D>(@"Maps\Vents\Upper Vents 4\bars");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.FlufflesRat(content); EnemyContentLoader.SharedRatSounds(content);
            EnemyContentLoader.BatEnemy(content);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Fluffles the Rat"] < 4)
            {
                FlufflesTheRat en = new FlufflesTheRat(pos, "Fluffles the Rat", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Fluffles the Rat"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void RespawnFlyingEnemies(Rectangle mapRec)
        {
            base.RespawnFlyingEnemies(mapRec);
            if (enemyNamesAndNumberInMap["Vent Bat"] < 4)
            {
                Bat en = new Bat(pos, "Vent Bat", game, ref player, this, mapRec);

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    enemyNamesAndNumberInMap["Vent Bat"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void Update()
        {
            base.Update();

            PlayAmbience();

            if (enemiesInMap.Count < enemyAmount && game.MapBooleans.chapterOneMapBooleans["upperVentIVSpawned"] == false)
            {
                RespawnGroundEnemies();
                RespawnFlyingEnemies(new Rectangle(50, 200, 2000, 300));
            }

            if (enemiesInMap.Count == enemyAmount && game.MapBooleans.chapterOneMapBooleans["upperVentIVSpawned"] == false)
            {
                game.MapBooleans.chapterOneMapBooleans["upperVentIVSpawned"] = true;
                game.MapBooleans.chapterOneMapBooleans["lockedUpperVentIVDoors"] = true;
                toUpperVents3Bot.IsUseable = false;
                game.Camera.ShakeCamera(5, 15);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(toUpperVents3Bot.PortalRecX - 100, toUpperVents3Bot.PortalRecY + 20, 200, 200), 2);

                Sound.PlayRandomRegularPoof(toUpperVents3Bot.PortalRecX, toUpperVents3Bot.PortalRecY);

                Chapter.effectsManager.foregroundFButtonRecs.Clear();

            }

            if (enemiesInMap.Count == 0 && game.MapBooleans.chapterOneMapBooleans["lockedUpperVentIVDoors"])
            {
                if (platforms.Contains(door))
                {
                    Chapter.effectsManager.AddSmokePoof(new Rectangle(door.Rec.Center.X - 100, door.Rec.Center.Y - 100, 200, 200), 2);
                    Chapter.effectsManager.AddSmokePoof(new Rectangle(toUpperVents3Bot.PortalRecX - 100, toUpperVents3Bot.PortalRecY + 20, 200, 200), 2);

                    Sound.PlayRandomRegularPoof(door.Rec.Center.X - 100, door.Rec.Center.Y - 100);
                    Sound.PlayRandomRegularPoof(toUpperVents3Bot.PortalRecX, toUpperVents3Bot.PortalRecY);

                    platforms.Remove(door);
                }
                toUpperVents3Bot.IsUseable = true;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toUpperVents3Bot = new Portal(50, 630, "Upper Vents IV");
            toUpperVents3Top = new Portal(50, 126, "Upper Vents IV");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toUpperVents3Bot, UpperVentsIII.ToUpperVents4Bot);
            portals.Add(toUpperVents3Top, UpperVentsIII.ToUpperVents4Top);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (platforms.Contains(door))
            {
                s.Draw(bars, new Vector2(0,mapRec.Y), Color.White);
                s.Draw(gate, new Vector2(door.RecX, door.RecY - 36), Color.White);
            }
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            base.DrawBackgroundAndParallax(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.End();

        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.End();

            //s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            //s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White);
            //s.Draw(foreground3, new Vector2(foreground.Width + foreground2.Width, mapRec.Y), Color.White);
        }
    }
}