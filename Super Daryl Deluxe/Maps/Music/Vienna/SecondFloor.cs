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
    class SecondFloor : MapClass
    {
        static Portal toStage;
        static Portal toEntranceHall;
        static Portal toTenantFloor;

        public static Portal ToStage { get { return toStage; } }
        public static Portal ToEntranceHall { get { return toEntranceHall; } }
        public static Portal ToTenantFloor { get { return toTenantFloor; } }

        Texture2D foreground, foreground2;

        public SecondFloor(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1800;
            mapWidth = 6940;
            mapName = "Second Floor";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyAmount = 8;

            enemyNamesAndNumberInMap.Add("Tuba Ghost", 0);

        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Second Floor\background"));
            background.Add(content.Load<Texture2D>(@"Maps\Music\Second Floor\background2"));
            foreground = content.Load<Texture2D>(@"Maps\Music\Second Floor\foreground");
            foreground2 = content.Load<Texture2D>(@"Maps\Music\Second Floor\foreground2");
            game.NPCSprites["Andy Warhol"] = content.Load<Texture2D>(@"NPC\Music\Andy Warhol");
            Game1.npcFaces["Andy Warhol"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Music\Andy Warhol Normal");
        }
        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Andy Warhol"] = Game1.whiteFilter;
            Game1.npcFaces["Andy Warhol"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.TubaGhostEnemy(content);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Tuba Ghost"] < 8)
            {
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
                    enemyNamesAndNumberInMap["Tuba Ghost"]++;
                    AddEnemyToEnemyList(en);
                }
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

            toTenantFloor.PortalRecX = 1180;
            toTenantFloor.FButtonYOffset = -30;
            toTenantFloor.PortalNameYOffset = -30;

            if (enemiesInMap.Count < enemyAmount)
            {
                RespawnGroundEnemies();
            }

            //KEEP CAMERA LOCKED WHEN PLAYER IS AT BOTTOM OF MAP
            if (game.Camera.center.Y > 50)
            {
                game.Camera.center.Y = 51;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toStage = new Portal(6640, 420, "Second Floor");
            toEntranceHall = new Portal(800, 280, "Second Floor");
            toTenantFloor = new Portal(1900, 280, "Second Floor");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toEntranceHall, EntranceHall.ToSecondFloor);
            portals.Add(toStage, TheStage.ToSecondFloor);
            portals.Add(toTenantFloor, TenantHallwayWest.ToSecondFloor);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White);
            s.End();
        }
    }
}
