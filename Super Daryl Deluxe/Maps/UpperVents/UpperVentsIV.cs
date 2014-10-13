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
            enemyAmount = 9;

            door = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1940, 140, 420, 50), false, false, false);
            platforms.Add(door);

            TreasureChest chest = new TreasureChest(Game1.treasureChestSheet, 1190, 140, player, 0, "Spin Slash", this);
            treasureChests.Add(chest);

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
            if (enemyNamesAndNumberInMap["Bat"] < 4)
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

            if (enemiesInMap.Count < enemyAmount && game.MapBooleans.chapterOneMapBooleans["upperVentIVSpawned"] == false)
            {
                RespawnGroundEnemies();
                RespawnFlyingEnemies(new Rectangle(50, 250, 2000, 400));
            }

            if (enemiesInMap.Count == enemyAmount && game.MapBooleans.chapterOneMapBooleans["upperVentIVSpawned"] == false)
            {
                game.MapBooleans.chapterOneMapBooleans["upperVentIVSpawned"] = true;
                game.MapBooleans.chapterOneMapBooleans["lockedUpperVentIVDoors"] = true;
                toUpperVents3Bot.ItemNameToUnlock = "Clear to Unlock";
                toUpperVents3Bot.PortalTexture = Game1.lockedPortalTexture;
                game.Camera.ShakeCamera(5, 15);
            }

            if (enemiesInMap.Count == 0 && game.MapBooleans.chapterOneMapBooleans["lockedUpperVentIVDoors"])
            {
                if (platforms.Contains(door))
                    platforms.Remove(door);

                toUpperVents3Bot.ItemNameToUnlock = null;
                toUpperVents3Bot.PortalTexture = Game1.portalTexture;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toUpperVents3Bot = new Portal(50, 630, "UpperVentsIV");
            toUpperVents3Top = new Portal(50, 126, "UpperVentsIV");
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
        }
    }
}