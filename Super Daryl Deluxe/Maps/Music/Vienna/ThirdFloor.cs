using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class ThirdFloor : MapClass
    {
        static Portal toTenantHallway;
        static Portal toEntranceHall;
        static Portal toSecondFloor;

        public static Portal ToTenantHallway { get { return toTenantHallway; } }
        public static Portal ToEntranceHall { get { return toEntranceHall; } }
        public static Portal ToSecondFloor { get { return toSecondFloor; } }

        public ThirdFloor(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 6000;
            mapName = "Third Floor";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 9;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();


            TubaGhost en = new TubaGhost(pos, "Tuba Ghost", game, ref player, this);
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



        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            spawnEnemies = true;
        }

        public override void Update()
        {
            base.Update();

            if (enemiesInMap.Count < enemyAmount && spawnEnemies)
            {
                RespawnGroundEnemies();
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            //toStage = new Portal(4640, 420, "SecondFloor");
            toTenantHallway = new Portal(5800, 630, "ThirdFloor");
            toSecondFloor = new Portal(1900, 630, "ThirdFloor");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toSecondFloor, SecondFloor.ToThirdFloor);
            portals.Add(toTenantHallway, TenantHallway.ToThirdFloor);
            //portals.Add(toSecondFloor
            //portals.Add(toVienna, Science101.ToIntroRoom);
        }
    }
}
