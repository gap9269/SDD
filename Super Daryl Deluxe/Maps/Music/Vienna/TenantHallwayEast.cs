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
    class TenantHallwayEast : MapClass
    {
        static Portal toHallwayWest;
        static Portal toWarholsRoom;
        static Portal toRoomFive;
        static Portal toRoomThree;
        static Portal toRoomFour;
        static Portal toStorageRoom;

        public static Portal ToHallwayWest { get { return toHallwayWest; } }
        public static Portal ToWarholsRoom { get { return toWarholsRoom; } }
        public static Portal ToRoomFive { get { return toRoomFive; } }
        public static Portal ToRoomThree { get { return toRoomThree; } }
        public static Portal ToRoomFour { get { return toRoomFour; } }
        public static Portal ToStorageRoom { get { return toStorageRoom; } }

        public TenantHallwayEast(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 3500;
            mapName = "Tenant Hallway East";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            //--Map Quest
            mapWithMapQuest = true;

            enemiesToKill.Add(15);
            enemiesKilledForQuest.Add(0);
            enemyNames.Add("Tuba Ghost");

            MapQuestSign sign = new MapQuestSign(3000, 630 -  Game1.mapSign.Height + 20, "Kill 15 Tuba Ghosts", enemiesToKill,
enemiesKilledForQuest, enemyNames, player, new List<Object>() { new Experience(250), new BandHat(), new Karma(2) });
            mapQuestSigns.Add(sign);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Tenant Hall\Tenant Hallway"));
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void ResetEnemyNamesAndNumberInMap()
        {
            base.ResetEnemyNamesAndNumberInMap();
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.TubaGhostEnemy(content);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();


            TubaGhost en = new TubaGhost(pos, "Tuba Ghost", game, ref player, this);
            monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
            en.Position = new Vector2(monsterX, monsterY);

            en.TimeBeforeSpawn = 120;

            Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
            if (testRec.Intersects(player.Rec))
            {
            }
            else
            {
                en.UpdateRectangles();
                AddEnemyToEnemyList(en);
            }

        }

        public override void Update()
        {
            base.Update();

            if (enemiesKilledForQuest[0] >= enemiesToKill[0])
            {
                completedMapQuest = true;
                mapQuestSigns[0].CompletedQuest = true;
            }

            if (enemiesInMap.Count < enemyAmount && spawnEnemies)
            {
                RespawnGroundEnemies();
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toHallwayWest = new Portal(28, platforms[0], "Tenant Hallway East");
            toRoomThree = new Portal(502, platforms[0], "Tenant Hallway East");
            toWarholsRoom = new Portal(1078, platforms[0], "Tenant Hallway East");
            toRoomFour = new Portal(1655, platforms[0], "Tenant Hallway East");
            toRoomFive = new Portal(2233, platforms[0], "Tenant Hallway East");
            toStorageRoom = new Portal(2810, platforms[0], "Tenant Hallway East");//, "Silver Key");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toHallwayWest, TenantHallwayWest.ToHallEast);

            portals.Add(toRoomThree, RoomThree.ToTenantHallway);
            portals.Add(toWarholsRoom, WarholsRoom.ToTenantHallway);
            portals.Add(toRoomFour, RoomFour.ToTenantHallway);
            portals.Add(toRoomFive, RoomFive.ToTenantHallway);
            portals.Add(toStorageRoom, StorageRoom.ToTenantHallway);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.End();
        }
    }
}
