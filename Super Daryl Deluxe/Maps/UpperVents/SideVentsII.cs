using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class SideVentsII : MapClass
    {

        static Portal toSideVentsI;

        public static Portal ToSideVentsI { get { return toSideVentsI; } }


        WallSwitch firstSwitch;
        WallSwitch lastSwitch;
        MapSteam steam;
        TreasureChest chester;

        public SideVentsII(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            yScroll = true;
            overlayType = 1;
            mapWidth = 4300;
            mapHeight = 1680;
            mapName = "Side Vents II";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);

            AddPlatforms();
            AddBounds();
            SetPortals();

            enemyAmount = 5;

            firstSwitch = new WallSwitch(Game1.switchTexture, new Rectangle(4000, 500, 42, 83), 470);
            lastSwitch = new WallSwitch(Game1.switchTexture, new Rectangle(450, -300, 42, 83));

            switches.Add(firstSwitch);
            switches.Add(lastSwitch);

            steam = new MapSteam(new Rectangle(650, -500, 100, 400), game, 1, new Vector2(0, 0), false, false, true);
            mapHazards.Add(steam);

            chester = new TreasureChest(Game1.treasureChestSheet, 100, -100, player, 0, new ChallengeRoomKey(true), this);
            treasureChests.Add(chester);

            enemyNamesAndNumberInMap.Add("Erl The Flask", 0);
            enemyNamesAndNumberInMap.Add("Bat", 0);
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();
            spawnEnemies = true;
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Erl The Flask"] < 6)
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

            CheckSwitch(firstSwitch);
            if (CheckSwitch(lastSwitch))
                firstSwitch.Active = true;

            if (firstSwitch.Active)
                steam.Active = false;
            else
                steam.Active = true;

            lastSwitch.Active = firstSwitch.Active;

            if (enemiesInMap.Count < enemyAmount && spawnEnemies == true)
            {
                RespawnGroundEnemies();
                RespawnFlyingEnemies(new Rectangle(650, -500, 800, 400));
            }
            if (enemiesInMap.Count == enemyAmount)
                spawnEnemies = false;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toSideVentsI = new Portal(4100, 630, "SideVentsII");

        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();


            portals.Add(toSideVentsI, SideVentsI.ToSideVentsII);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
    }
}
