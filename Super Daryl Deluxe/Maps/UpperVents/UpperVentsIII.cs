using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class UpperVentsIII : MapClass
    {
        static Portal toUpperVents2;
        static Portal toUpperVents4Bot;
        static Portal toUpperVents4Top;
        static Portal toUpperVentsChallenge;
        static Portal toBathroom;

        public static Portal ToBathroom { get { return toBathroom; } }
        public static Portal ToUpperVentsChallenge { get { return toUpperVentsChallenge; } }
        public static Portal ToUpperVents2 { get { return toUpperVents2; } }
        public static Portal ToUpperVents4Bot { get { return toUpperVents4Bot; } }
        public static Portal ToUpperVents4Top { get { return toUpperVents4Top; } }

        MovingPlatform floor;
        MovingPlatform ceiling;

        List<Vector2> floorTargets;
        List<Vector2> ceilingTargets;

        public UpperVentsIII(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            yScroll = true;
            zoomLevel = .85f;

             mapWidth = 9700;
            mapHeight = 2500;
            mapName = "Upper Vents III";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            overlayType = 1;
            AddPlatforms();
            AddBounds();
            SetPortals();

            enemyAmount = 15;

            floorTargets = new List<Vector2>();
            ceilingTargets = new List<Vector2>();

            floor = new MovingPlatform(Game1.platformTextures.ElementAt(1).Value, new Rectangle(7518, -380, 800, 50), false, false, false, floorTargets, 5, 10);
            ceiling = new MovingPlatform(Game1.platformTextures.ElementAt(1).Value, new Rectangle(7518, -800, 800, 50), false, false, false, ceilingTargets, 3, 50);

            platforms.Add(floor);
            platforms.Add(ceiling);

            TreasureChest chest = new TreasureChest(Game1.treasureChestSheet, 8500, -400, player, 0, new BronzeKey(), this);
            treasureChests.Add(chest);
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();
            spawnEnemies = true;
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            switch (game.chapterState)
            {
                case Game1.ChapterState.chapterOne:
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
                        enemiesInMap.Add(en);
                    }
                    break;
            }

        }

        public override void Update()
        {
            base.Update();
            zoomLevel = .85f;
            if (enemiesInMap.Count < enemyAmount && spawnEnemies == true)
                RespawnGroundEnemies();
            if (enemiesInMap.Count == enemyAmount)
                spawnEnemies = false;

            if (player.CurrentPlat == floor && floor.Velocity.Y == 0 && game.MapBooleans.chapterOneMapBooleans["VentLowered"] == false)
            {
                floor.Velocity = new Vector2(0, GameConstants.GRAVITY);
                ceiling.Velocity = new Vector2(0, GameConstants.GRAVITY);

                game.Camera.ShakeCamera(5, 5);
            }

            if (floor.Velocity.Y != 0 && game.MapBooleans.chapterOneMapBooleans["VentLowered"] == false)
            {
                floor.Velocity += new Vector2(0, GameConstants.GRAVITY);
                ceiling.Velocity += new Vector2(0, GameConstants.GRAVITY);
            }

            if (floor.Position.Y >= 0 && game.MapBooleans.chapterOneMapBooleans["VentLowered"] == false)
            {
                floor.Velocity = new Vector2(0, 0);
                ceiling.Velocity = new Vector2(0, 0);
                game.MapBooleans.chapterOneMapBooleans["VentLowered"] = true;
                game.Camera.ShakeCamera(5, 10);
            }

            if (game.MapBooleans.chapterOneMapBooleans["VentLowered"] == true)
            {
                floor.Position = new Vector2(7518, 0);
                ceiling.Position = new Vector2(7518, -400);
            }

            toUpperVentsChallenge.PortalRecY = (int)ceiling.Position.Y - toUpperVentsChallenge.PortalRec.Height;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toUpperVents2 = new Portal(610, -120, "UpperVentsIII");
            toUpperVents4Top = new Portal(9500, -400, "UpperVentsIII");
            toUpperVents4Bot = new Portal(9100, 0, "UpperVentsIII");
            toUpperVentsChallenge = new Portal(7900, -800, "UpperVentsIII", "Challenge Room Key");
            toBathroom = new Portal(8700, 0, "UpperVentsIII");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toUpperVents2, UpperVentsII.ToUpperVents3);
            portals.Add(toUpperVents4Bot, UpperVentsIV.ToUpperVents3Bot);
            portals.Add(toUpperVents4Top, UpperVentsIV.ToUpperVents3Top);
            portals.Add(toUpperVentsChallenge, UpperVentsChallengeRoom.ToUpperVents3);
            portals.Add(toBathroom, Bathroom.ToLastMap);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
    }
}