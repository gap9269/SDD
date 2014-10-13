using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class SecondFloor : MapClass
    {
        static Portal toStage;
        static Portal toEntranceHall;
        static Portal toThirdFloor;

        public static Portal ToStage { get { return toStage; } }
        public static Portal ToEntranceHall { get { return toEntranceHall; } }
        public static Portal ToThirdFloor { get { return toThirdFloor; } }

        public SecondFloor(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1800;
            mapWidth = 6940;
            mapName = "Second Floor";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyAmount = 6;
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

            toStage = new Portal(6640, 420, "SecondFloor");
            toEntranceHall = new Portal(1600, 280, "SecondFloor");
            toThirdFloor = new Portal(1900, 280, "SecondFloor");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toEntranceHall, EntranceHall.ToSecondFloor);
            portals.Add(toStage, TheStage.ToSecondFloor);
            portals.Add(toThirdFloor, ThirdFloor.ToSecondFloor);
            //portals.Add(toVienna, Science101.ToIntroRoom);
        }
    }
}
