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
    class TheGrandCorridor : MapClass
    {
        public static Portal toEbenezersMansion;
        public static Portal toEasternCorridor;
        public static Portal toBallroom;
        public static Portal toUnderTheFoyer;

        Texture2D foreground, platform;
        GhostLight light1, light2, light3, light4, light5, light6;

        public TheGrandCorridor(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1800;
            mapWidth = 4000;
            mapName = "The Grand Corridor";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 9;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            light1 = new GhostLight(game, 268, mapRec.Y + 667, true, false, true, true);
            interactiveObjects.Add(light1);

            light2 = new GhostLight(game, 1714, mapRec.Y + 667, true, false, true, true);
            interactiveObjects.Add(light2);

            light3 = new GhostLight(game, 2692, mapRec.Y + 760);
            interactiveObjects.Add(light3);

            light4 = new GhostLight(game, 2079, mapRec.Y -29);
            interactiveObjects.Add(light4);

            light5 = new GhostLight(game, 883, mapRec.Y -106, true, false, true, true);
            interactiveObjects.Add(light5);

            light6 = new GhostLight(game, 168, mapRec.Y -24);
            interactiveObjects.Add(light6);

            enemyNamesAndNumberInMap.Add("Spooky Present", 0);
            enemyNamesAndNumberInMap.Add("Eerie Elf", 0);
            enemyNamesAndNumberInMap.Add("Haunted Nutcracker", 0);
        }
        
        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\TheGrandCorridor\background"));
            foreground = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\TheGrandCorridor\foreground");
            platform = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\TheGrandCorridor\platform");
            game.NPCSprites["Gary R. Pigeon"] = content.Load<Texture2D>(@"NPC\Literature\Gary R. Pigeon");
            Game1.npcFaces["Gary R. Pigeon"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Literature\Gary R. Pigeon Normal");
        }
        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Gary R. Pigeon"] = Game1.whiteFilter;
            Game1.npcFaces["Gary R. Pigeon"].faces["Normal"] = Game1.whiteFilter;
        }
        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemiesInMap.Count < enemyAmount)
            {
                int enemyType = Game1.randomNumberGen.Next(3);

                Enemy en;

                switch (enemyType)
                {
                    case 0:
                        en = new HauntedNutcracker(pos, "Haunted Nutcracker", game, ref player, this);
                        monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                        en.Position = new Vector2(monsterX, monsterY);

                        en.TimeBeforeSpawn = 60;

                        Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                        if (testRec.Intersects(player.Rec))
                        {
                        }

                        else
                        {
                            en.UpdateRectangles();
                            enemyNamesAndNumberInMap["Haunted Nutcracker"]++;
                            AddEnemyToEnemyList(en);
                        }
                        break;
                    case 1:
                        en = new SpookyPresent(pos, "Spooky Present", game, ref player, this);
                        monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                        en.Position = new Vector2(monsterX, monsterY);

                        en.TimeBeforeSpawn = 60;

                        Rectangle testRec2 = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                        if (testRec2.Intersects(player.Rec))
                        {
                        }

                        else
                        {
                            en.UpdateRectangles();
                            enemyNamesAndNumberInMap["Spooky Present"]++;
                            AddEnemyToEnemyList(en);
                        }
                        break;
                    case 2:
                        en = new EerieElf(pos, "Eerie Elf", game, ref player, this);
                        monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                        en.Position = new Vector2(monsterX, monsterY);

                        en.TimeBeforeSpawn = 60;

                        Rectangle testRec3 = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                        if (testRec3.Intersects(player.Rec))
                        {
                        }

                        else
                        {
                            en.UpdateRectangles();
                            enemyNamesAndNumberInMap["Eerie Elf"]++;
                            AddEnemyToEnemyList(en);
                        }
                        break;
                }
            }
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.SpookyPresentEnemy(content);
            EnemyContentLoader.EerieElfEnemy(content);
            EnemyContentLoader.HauntedNutcrackerEnemy(content);
        }

        public override void Update()
        {
            base.Update();

            RespawnGroundEnemies();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toEbenezersMansion = new Portal(30, platforms[0], "The Grand Corridor");
            toEasternCorridor = new Portal(250, platforms[1], "The Grand Corridor");
            toUnderTheFoyer = new Portal(3250, platforms[0], "The Grand Corridor");
            toBallroom = new Portal(1550, platforms[0], "The Grand Corridor");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(platform, new Vector2(3661, mapRec.Y + 440), Color.White);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toEbenezersMansion, EbenezersMansion.ToTheFoyer);
            portals.Add(toUnderTheFoyer, UnderTheMansion.toTheFoyer);
            portals.Add(toEasternCorridor, EasternCorridor.toTheFoyer);
            portals.Add(toBallroom, TheHauntedBallroom.toTheFoyer);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }

                if (interactiveObjects[i] is GhostLight)
                {
                    (interactiveObjects[i] as GhostLight).DrawGlow(s);
                }
            }

                s.Draw(foreground, new Vector2(144, mapRec.Y + 463), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
