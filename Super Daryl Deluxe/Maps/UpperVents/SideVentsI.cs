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

            enemyAmount = 10;

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

            if (enemiesInMap.Count < enemyAmount && spawnEnemies == true)
            {
                RespawnGroundEnemies();
                RespawnFlyingEnemies(new Rectangle(100, 100, 3300, 500));
            }
            if (enemiesInMap.Count == enemyAmount)
            {
                spawnEnemies = false;

                //--Lock the doors
                if (game.MapBooleans.chapterOneMapBooleans["sideVentDoorsLocked"] == false)
                {
                    game.MapBooleans.chapterOneMapBooleans["sideVentDoorsLocked"] = true;
                    toSideVentsII.ItemNameToUnlock = "Clear to Unlock";
                    toUpperVentsI.ItemNameToUnlock = "Clear to Unlock";
                    game.Camera.ShakeCamera(5, 15);
                    toUpperVentsI.PortalTexture = Game1.lockedPortalTexture;
                    toSideVentsII.PortalTexture = Game1.lockedPortalTexture;
                }
            }

            //--Unlock the doors
            if (enemiesInMap.Count == 0 && game.MapBooleans.chapterOneMapBooleans["sideVentDoorsLocked"] == true && game.MapBooleans.chapterOneMapBooleans["sideVentDoorsUnlocked"] == false)
            {
                game.MapBooleans.chapterOneMapBooleans["sideVentDoorsUnlocked"] = true;
                toSideVentsII.ItemNameToUnlock = null;
                toUpperVentsI.ItemNameToUnlock = null;
                game.Camera.ShakeCamera(5, 15);
                toUpperVentsI.PortalTexture = Game1.portalTexture;
                toSideVentsII.PortalTexture = Game1.portalTexture;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toSideVentsII = new Portal(50, 630, "SideVentsI");
            toUpperVentsI = new Portal(3300, 630, "SideVentsI");

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
        }
    }
}
