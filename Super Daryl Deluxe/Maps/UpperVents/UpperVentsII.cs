using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class UpperVentsII : MapClass
    {
        static Portal toUpperVents1;
        static Portal toUpperVents3;
        static Portal toUpperVents5;

        public static Portal ToUpperVents5 { get { return toUpperVents5; } }
        public static Portal ToUpperVents1 { get { return toUpperVents1; } }
        public static Portal ToUpperVents3 { get { return toUpperVents3; } }

        public UpperVentsII(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            yScroll = true;
            overlayType = 1;
            mapWidth = 3100;
            mapHeight = 2680;
            mapName = "Upper Vents II";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);

            AddPlatforms();
            AddBounds();
            SetPortals();

            enemyAmount = 10;

            Ladder lad = new Ladder(1760, -160, 820);
            ladders.Add(lad);

            StackOfBoxes boxes = new StackOfBoxes(game, 2200, 650, Game1.interactiveObjects["StoneStatue"], false, 8, 10, 0, false);
            interactiveObjects.Add(boxes);
        }

        public override void RespawnFlyingEnemies(Rectangle mapRec)
        {
            base.RespawnFlyingEnemies(mapRec);

            Bat en = new Bat(pos, "Bat", game, ref player, this, mapRec);

            Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
            if (testRec.Intersects(player.Rec))
            {
            }
            else
            {
                enemiesInMap.Add(en);
            }

        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();
            spawnEnemies = true;
        }

        public override void Update()
        {
            base.Update();

            if (enemiesInMap.Count < enemyAmount && spawnEnemies == true)
                RespawnFlyingEnemies(new Rectangle(100, -700, 2900, 300));
            if (enemiesInMap.Count == enemyAmount)
                spawnEnemies = false;
        }


        public override void SetPortals()
        {
            base.SetPortals();

            toUpperVents1 = new Portal(50, -350, "UpperVentsII");
            toUpperVents3 = new Portal(2700, 640, "UpperVentsII");
            toUpperVents5 = new Portal(2900, -350, "UpperVentsII", "Bronze Key");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toUpperVents1, UpperVents1.ToUpperVents2);
            portals.Add(toUpperVents3, UpperVentsIII.ToUpperVents2);
            portals.Add(toUpperVents5, UpperVentsV.ToUpperVents2);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
    }
}
