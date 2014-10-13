using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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

            enemyAmount = 10;

            steamForItem = new MapSteam(new Rectangle(880, 180, 300, 100), game, 1, new Vector2(0, 0), false, true, false);

            steam1 = new MapSteam(new Rectangle(1780, 180, 400, 100), game, 1, new Vector2(-30, -10), false, true, false);
            steam2 = new MapSteam(new Rectangle(1780, -220, 400, 100), game, 1, new Vector2(-30, -10), false, true, false);
            steam3 = new MapSteam(new Rectangle(1780, -500, 400, 100), game, 1, new Vector2(-30, -10), false, true, false);
            steam4 = new MapSteam(new Rectangle(1780, -760, 400, 100), game, 1, new Vector2(-30, -10), false, true, false);

            mapHazards.Add(steamForItem);
            mapHazards.Add(steam1);
            mapHazards.Add(steam2);
            mapHazards.Add(steam3);
            mapHazards.Add(steam4);

            stopTunnelSteam = new WallSwitch(Game1.switchTexture, new Rectangle(600, 400, 42, 83), 330);
            stopAllSteam = new WallSwitch(Game1.switchTexture, new Rectangle(2360, -1220, 42, 83));
            switches.Add(stopTunnelSteam);
            switches.Add(stopAllSteam);

            enemyNamesAndNumberInMap.Add("Erl The Flask", 0);
            enemyNamesAndNumberInMap.Add("Bat", 0);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Erl The Flask"] < 5)
            {
                ErlTheFlask en = new ErlTheFlask(pos, "Erl The Flask", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Erl The Flask"]++;
                    enemiesInMap.Add(en);
                }
            }


        }

        public override void RespawnFlyingEnemies(Rectangle mapRec)
        {
            base.RespawnFlyingEnemies(mapRec);
            if (enemyNamesAndNumberInMap["Bat"] < 5)
            {
                Bat en = new Bat(pos, "Bat", game, ref player, this, mapRec);

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    enemyNamesAndNumberInMap["Bat"]++;
                    enemiesInMap.Add(en);
                }
            }
        }

        public override void Update()
        {
            base.Update();


            if (enemiesInMap.Count < enemyAmount && spawnEnemies == true)
            {
                RespawnGroundEnemies();
                RespawnFlyingEnemies(new Rectangle(1580, -800, 500, 1000));
            }

            if (enemiesInMap.Count == enemyAmount)
                spawnEnemies = false;

            CheckSwitch(stopTunnelSteam);
            if(CheckSwitch(stopAllSteam))
            {
                if(stopAllSteam.Active)
                    stopTunnelSteam.Active = true;
            }

            if (stopAllSteam.Active)
            {
                steamForItem.Active = false;
            }

            if (stopTunnelSteam.Active)
            {
                if (stopTunnelSteam.TimeActive == 0)
                {
                    steam1.Active = false;
                    steam2.Active = false;
                    steam3.Active = false;
                    steam4.Active = false;
                }
                if (stopTunnelSteam.TimeActive >= 195)
                    steam1.Active = true;
                if (stopTunnelSteam.TimeActive >= 240)
                    steam2.Active = true;
                if (stopTunnelSteam.TimeActive >= 285)
                    steam3.Active = true;
                if (stopTunnelSteam.TimeActive >= 330)
                    steam4.Active = true;

            }
            else
            {
                steam1.Active = true;
                steam2.Active = true;
                steam3.Active = true;
                steam4.Active = true;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toUpperVents2 = new Portal(50, 630, "UpperVentsV");
            toUpperVents6 = new Portal(2900, -1000, "UpperVentsV");
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
    }
}