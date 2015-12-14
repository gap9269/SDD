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
    class ForgottenChamberI : MapClass
    {
        static Portal toForgottenChamberII;
        static Portal toCollapsingRoom;

        public static Portal ToCollapsingRoom { get { return toCollapsingRoom; } }
        public static Portal ToForgottenChamberII { get { return toForgottenChamberII; } }

        Texture2D foreground, bars;

        public ForgottenChamberI(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2000;
            mapName = "Forgotten Chamber I";

            //Room is barred
            enemiesToKill.Add(10);
            enemiesKilledForQuest.Add(0);
            enemyNames.Add("Scorpadillo");
            mapWithMapQuest = true;

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Scorpadillo", 0);
            interactiveObjects.Add(new Barrel(game, 786, 464 + 155, Game1.interactiveObjects["Barrel"], true, 3, 5, .36f, false, Barrel.BarrelType.pyramidBirdJar));
            interactiveObjects.Add(new Barrel(game, 666, 464 + 155, Game1.interactiveObjects["Barrel"], true, 2, 2, .16f, false, Barrel.BarrelType.pyramidUrn));
            interactiveObjects.Add(new Barrel(game, 1335, 466 + 155, Game1.interactiveObjects["Barrel"], true, 3, 4, .06f, false, Barrel.BarrelType.pyramidPitcher));
            interactiveObjects.Add(new Barrel(game, 1236, 466 + 155, Game1.interactiveObjects["Barrel"], true, 2, 2, .16f, false, Barrel.BarrelType.pyramidUrn));
            interactiveObjects.Add(new Barrel(game, 1161, 472 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .06f, false, Barrel.BarrelType.pyramidPitcher));

        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\ForgottenChamberI\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\ForgottenChamberI\foreground");
            bars = content.Load<Texture2D>(@"Maps\History\Pyramid\ForgottenChamberI\bars");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();
            EnemyContentLoader.ScorpadilloEnemy(content);
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if ((game.ChapterTwo.ChapterTwoBooleans["forgottenChamberISpawn"] && enemiesToKill[0] - enemiesKilledForQuest[0] > enemiesInMap.Count) || spawnEnemies)
            {
                Scorpadillo en = new Scorpadillo(pos, "Scorpadillo", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                if (spawnEnemies)
                    en.TimeBeforeSpawn = 60;
                else
                {
                    en.TimeBeforeSpawn = 25;

                    if(Game1.randomNumberGen.Next(0, 4) < 3)
                        en.Hostile = true;
                }

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }

                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Scorpadillo"]++;
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

            if (enemiesInMap.Count < enemyAmount && !game.ChapterTwo.ChapterTwoBooleans["forgottenChamberILocked"] && !game.ChapterTwo.ChapterTwoBooleans["forgottenChamberICleared"])
            {
                toForgottenChamberII.IsUseable = false;
                toCollapsingRoom.IsUseable = false;
                spawnEnemies = false;
                game.ChapterTwo.ChapterTwoBooleans["forgottenChamberISpawn"] = true;
            }

            if (enemiesInMap.Count < enemyAmount)
                RespawnGroundEnemies();

            if (enemiesInMap.Count == enemyAmount && game.ChapterTwo.ChapterTwoBooleans["forgottenChamberICleared"] == false && !game.ChapterTwo.ChapterTwoBooleans["forgottenChamberILocked"])
            {
                game.ChapterTwo.ChapterTwoBooleans["forgottenChamberILocked"] = true;
                game.Camera.ShakeCamera(5, 15);
            }

            if (enemiesKilledForQuest[0] >= enemiesToKill[0])
            {
                game.ChapterTwo.ChapterTwoBooleans["forgottenChamberISpawn"] = false;
                game.ChapterTwo.ChapterTwoBooleans["forgottenChamberICleared"] = true;
                game.ChapterTwo.ChapterTwoBooleans["forgottenChamberILocked"] = false;
                toForgottenChamberII.IsUseable = true;
                toCollapsingRoom.IsUseable = true;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toForgottenChamberII = new Portal(1800, platforms[0], "Forgotten Chamber I");
            toCollapsingRoom = new Portal(50, platforms[0], "Forgotten Chamber I");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.ChapterTwo.ChapterTwoBooleans["forgottenChamberILocked"])
            {
                s.Draw(bars, new Vector2(0, 0), Color.White);
            }
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toForgottenChamberII, ForgottenChamberII.ToForgottenChamberI);
            portals.Add(toCollapsingRoom, CollapsingRoom.ToForgottenChamberI);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, 0), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
