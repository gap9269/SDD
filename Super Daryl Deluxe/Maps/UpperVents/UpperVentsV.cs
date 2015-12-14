using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ISurvived
{
    class UpperVentsV : MapClass
    {
        static Portal toUpperVents2;
        static Portal toUpperVents6;

        public static Portal ToUpperVents2 { get { return toUpperVents2; } }
        public static Portal ToUpperVents6 { get { return toUpperVents6; } }

        MapSteam steamForItem;
        MapSteam steam1;
        MapSteam steam2;
        MapSteam steam3;
        MapSteam steam4;

        WallSwitch stopTunnelSteam;
        WallSwitch stopAllSteam;

        LockerCombo lockerCombo;

        Texture2D foreground;

        public UpperVentsV(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            yScroll = true;
            overlayType = 1;
            mapWidth = 3100;
            mapHeight = 2680;
            mapName = "Upper Vents V";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);

            AddPlatforms();
            AddBounds();
            SetPortals();

            enemyAmount = 6;

            steamForItem = new MapSteam(100, 100, 1020, 210, game, 1, true, true, false, false);

            steam1 = new MapSteam(100, 100, 1820 + 190, 140, game, 1, true, true, false);
            steam2 = new MapSteam(100, 100, 1820 + 190, -220, game, 1, true, true, false);
            steam3 = new MapSteam(100, 100, 1820 + 190, -480, game, 1, true, true, false);
            steam4 = new MapSteam(100, 100, 1820 + 190, -750, game, 1, true, true, false);

            mapHazards.Add(steamForItem);
            mapHazards.Add(steam1);
            mapHazards.Add(steam2);
            mapHazards.Add(steam3);
            mapHazards.Add(steam4);

            stopTunnelSteam = new WallSwitch(Game1.switchTexture, new Rectangle(550, 350, (int)(333 * .8f), (int)(335 * .8f)), 351);
            stopAllSteam = new WallSwitch(Game1.switchTexture, new Rectangle(2210, -1310, (int)(333 * .8f), (int)(335 * .8f)));
            switches.Add(stopTunnelSteam);
            switches.Add(stopAllSteam);

            enemyNamesAndNumberInMap.Add("Fluffles the Rat", 0);
            enemyNamesAndNumberInMap.Add("Vent Bat", 0);
        }
        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_vents");
        }
        public override void LoadContent()
        {
            Sound.LoadVentZoneSounds();
            foreach (MapSteam s in mapHazards)
            {
                s.object_steam_vent_loop = Sound.mapZoneSoundEffects["object_steam_vent_loop"].CreateInstance();
            }
            background.Add(content.Load<Texture2D>(@"Maps\Vents\Upper Vents 5\background"));
            foreground = content.Load<Texture2D>(@"Maps\Vents\Upper Vents 5\foreground");

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
            if (enemyNamesAndNumberInMap["Vent Bat"] < 3)
            {
                Bat en = new Bat(pos, "Vent Bat", game, ref player, this, mapRec, true);

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
                RespawnFlyingEnemies(new Rectangle(1580, -500, 500, 700));
            }

            if (enemiesInMap.Count == enemyAmount)
                spawnEnemies = false;

            if(!stopTunnelSteam.Active)
                CheckSwitch(stopTunnelSteam);

            if (stopAllSteam.Active == false)
            {
                if (CheckSwitch(stopAllSteam))
                {
                    if (stopAllSteam.Active)
                        stopTunnelSteam.Active = true;

                    if (steamForItem.Active)
                        steamForItem.TurnOff();
                }
            }
            if (stopAllSteam.Active)
            {
                if (steamForItem.Active && !steamForItem.CurrentlyEnding)
                    steamForItem.TurnOff();

                if (steam1.Active && !steam1.CurrentlyEnding)
                {
                    steam1.TurnOff();
                }

                if (steam2.Active && !steam2.CurrentlyEnding)
                {
                    steam2.TurnOff();
                }

                if (steam3.Active && !steam3.CurrentlyEnding)
                {
                    steam3.TurnOff();
                }

                if (steam4.Active && !steam4.CurrentlyEnding)
                {
                    steam4.TurnOff();
                }
            }

            if (stopTunnelSteam.Active)
            {
                if (stopTunnelSteam.TimeActive == 0)
                {
                    steam1.TurnOff();
                    steam2.TurnOff();
                    steam3.TurnOff();
                    steam4.TurnOff();
                }

                if (!stopAllSteam.Active)
                {
                    if (stopTunnelSteam.TimeActive >= 195 && !steam1.Active)
                        steam1.TurnOn();
                    if (stopTunnelSteam.TimeActive >= 240 && !steam2.Active)
                        steam2.TurnOn();
                    if (stopTunnelSteam.TimeActive >= 285 && !steam3.Active)
                        steam3.TurnOn();
                    if (stopTunnelSteam.TimeActive >= 350 && !steam4.Active)
                        steam4.TurnOn();
                }
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toUpperVents2 = new Portal(50, 630, "Upper Vents V");
            toUpperVents6 = new Portal(2900, -1000, "Upper Vents V");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();


            portals.Add(toUpperVents2, UpperVentsII.ToUpperVents5);
            portals.Add(toUpperVents6, UpperVentsVI.ToUpperVents5);
        }

        public override void Draw(SpriteBatch s)
        {

            base.Draw(s);
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