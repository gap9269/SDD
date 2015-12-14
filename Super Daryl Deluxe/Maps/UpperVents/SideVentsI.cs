using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class SideVentsI : MapClass
    {

        static Portal toSideVentsII;
        static Portal toUpperVentsI;

        public static Portal ToSideVentsII { get { return toSideVentsII; } }
        public static Portal ToUpperVentsI { get { return toUpperVentsI; } }

        Texture2D foreground, bars;

        public SideVentsI(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            yScroll = false;

            mapWidth = 3500;
            mapHeight = 720;
            mapName = "Side Vents I";
            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            overlayType = 1;
            AddPlatforms();
            AddBounds();
            SetPortals();

            enemyAmount = 5;

            enemyNamesAndNumberInMap.Add("Fluffles the Rat", 0);
            enemyNamesAndNumberInMap.Add("Vent Bat", 0);
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();
            spawnEnemies = true;
        }
        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_vents");
        }
        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Vents\Side Vents 1\background"));
            foreground = content.Load<Texture2D>(@"Maps\Vents\Side Vents 1\foreground");
            Sound.LoadVentZoneSounds();
            bars = content.Load<Texture2D>(@"Maps\Vents\Side Vents 1\bars");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.BatEnemy(content);
            EnemyContentLoader.FlufflesRat(content); EnemyContentLoader.SharedRatSounds(content);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Fluffles the Rat"] < 3)
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
            if (enemyNamesAndNumberInMap["Vent Bat"] < 2)
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

            if (enemiesInMap.Count < enemyAmount && spawnEnemies == true)
            {
                RespawnGroundEnemies();
                RespawnFlyingEnemies(new Rectangle(100, 100, 3300, 300));
            }
            if (enemiesInMap.Count == enemyAmount)
            {
                spawnEnemies = false;

                //--Lock the doors
                if (game.MapBooleans.chapterOneMapBooleans["sideVentDoorsLocked"] == false)
                {
                    game.MapBooleans.chapterOneMapBooleans["sideVentDoorsLocked"] = true;
                    game.Camera.ShakeCamera(5, 15);

                    toSideVentsII.IsUseable = false;
                    toUpperVentsI.IsUseable = false;

                    Chapter.effectsManager.fButtonRecs.Clear();
                    Chapter.effectsManager.foregroundFButtonRecs.Clear();

                    Chapter.effectsManager.AddSmokePoof(new Rectangle(toUpperVentsI.PortalRecX + 50, toUpperVentsI.PortalRecY - 90, 200, 200), 2);
                    Chapter.effectsManager.AddSmokePoof(new Rectangle(toSideVentsII.PortalRecX - 80, toSideVentsII.PortalRecY - 60, 200, 200), 2);

                    Sound.PlayRandomRegularPoof(toUpperVentsI.PortalRecX + 50, toUpperVentsI.PortalRecY - 90);
                    Sound.PlayRandomRegularPoof(toSideVentsII.PortalRecX - 80, toSideVentsII.PortalRecY - 60);
                }
            }

            //--Unlock the doors
            if (enemiesInMap.Count == 0 && game.MapBooleans.chapterOneMapBooleans["sideVentDoorsLocked"] == true && game.MapBooleans.chapterOneMapBooleans["sideVentDoorsUnlocked"] == false)
            {
                game.MapBooleans.chapterOneMapBooleans["sideVentDoorsUnlocked"] = true;
                game.Camera.ShakeCamera(5, 15);
                toSideVentsII.IsUseable = true;
                toUpperVentsI.IsUseable = true;

                Chapter.effectsManager.AddSmokePoof(new Rectangle(toUpperVentsI.PortalRecX + 50, toUpperVentsI.PortalRecY - 90, 200, 200), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(toSideVentsII.PortalRecX - 80, toSideVentsII.PortalRecY - 60, 200, 200), 2);

                Sound.PlayRandomRegularPoof(toUpperVentsI.PortalRecX + 50, toUpperVentsI.PortalRecY - 90);
                Sound.PlayRandomRegularPoof(toSideVentsII.PortalRecX - 80, toSideVentsII.PortalRecY - 60);
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toSideVentsII = new Portal(50, 630, "Side Vents I");
            toUpperVentsI = new Portal(3300, 630, "Side Vents I");

        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();


            portals.Add(toUpperVentsI, UpperVents1.ToSideVents);
            portals.Add(toSideVentsII, SideVentsII.ToSideVentsI);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (toUpperVentsI.IsUseable == false)
            {
                s.Draw(bars, new Vector2(0, 0), Color.White);
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
        }
    }
}
