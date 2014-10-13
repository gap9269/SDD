using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class UpperVentsChallengeRoom : MapClass
    {
        static Portal toUpperVents3;

        public static Portal ToUpperVents3 { get { return toUpperVents3; } }

        public UpperVentsChallengeRoom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            yScroll = false;

            mapWidth = 1400;
            mapHeight = 720;
            mapName = "Upper Vents Challenge Room";
            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);

            AddPlatforms();
            AddBounds();
            SetPortals();
            overlayType = 1;
            enemyAmount = 11;

            Textbook book1 = new Textbook(1050, 400,1);
            Textbook book2 = new Textbook(1225, 400,0);

            book1.AbleToPickUp = false;
            book2.AbleToPickUp = false;

            collectibles.Add(book1);
            collectibles.Add(book2);

            enemyNamesAndNumberInMap.Add("Erl The Flask", 0);
            enemyNamesAndNumberInMap.Add("Bat", 0);
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

            if (enemiesInMap.Count < enemyAmount && spawnEnemies)
            {
                RespawnGroundEnemies();
                RespawnFlyingEnemies(new Rectangle(50, 200, mapWidth - 100, 450));
            }

            if (enemiesInMap.Count == enemyAmount && game.MapBooleans.chapterOneMapBooleans["upperVentChallengeSpawned"] == false)
            {
                game.MapBooleans.chapterOneMapBooleans["upperVentChallengeSpawned"] = true;
                game.MapBooleans.chapterOneMapBooleans["enteredUpperVentChallengeRoom"] = true;
                toUpperVents3.ItemNameToUnlock = "Clear to Unlock";
                toUpperVents3.PortalTexture = Game1.lockedPortalTexture;
                game.Camera.ShakeCamera(5, 15);

                spawnEnemies = false;

            }
            if (enemiesInMap.Count == 0 && game.MapBooleans.chapterOneMapBooleans["enteredUpperVentChallengeRoom"] && game.MapBooleans.chapterOneMapBooleans["upperVentChallengeClear"] == false)
            {
                toUpperVents3.ItemNameToUnlock = null;
                toUpperVents3.PortalTexture = Game1.portalTexture;

                game.MapBooleans.chapterOneMapBooleans["upperVentChallengeClear"] = true;

                collectibles[0].AbleToPickUp = true;
                collectibles[1].AbleToPickUp = true;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toUpperVents3 = new Portal(50, 630, "UpperVentsChallengeRoom");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toUpperVents3, UpperVentsIII.ToUpperVentsChallenge);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
    }
}
