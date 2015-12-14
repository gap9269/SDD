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
    class CentralHallI : MapClass
    {
        static Portal toMainChamber;
        static Portal toCentralHallII;
        public static Portal ToCentralHallII { get { return toCentralHallII; } }
        public static Portal ToMainChamber { get { return toMainChamber; } }

        public static Portal fromSymmetryLeft;
        public static Portal fromSymmetryRight;


        Texture2D foreground, bars;

        public CentralHallI(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1800;
            mapName = "Central Hall I";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 3;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Anubis Warrior", 0);

            interactiveObjects.Add(new ExplodingFlower(game, 825, 490, false, 250));

            Barrel bar2 = new Barrel(game, 658, 496 + 155, Game1.interactiveObjects["Barrel"], true, 3, 2, .46f, false, Barrel.BarrelType.pyramidBirdJar);
            interactiveObjects.Add(bar2);

            Barrel bar1 = new Barrel(game, 1314, 498 + 155, Game1.interactiveObjects["Barrel"], true, 3, 1, .76f, false, Barrel.BarrelType.pyramidUrn);
            interactiveObjects.Add(bar1);

            collectibles.Add(new Textbook(1250, 100, 2));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\CentralHallI\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\CentralHallI\foreground");
            bars = content.Load<Texture2D>(@"Maps\History\Pyramid\CentralHallI\bars");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.AnubisWarriorEnemy(content);
            EnemyContentLoader.SharedAnubisSounds(content);
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            spawnEnemies = true;
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Anubis Warrior"] < 3)
            {
                AnubisWarrior en = new AnubisWarrior(pos, "Anubis Warrior", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.SpawnWithPoof = false;
                en.FacingRight = false;

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }

                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Anubis Warrior"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void Update()
        {
            base.Update();

            if (enemiesInMap.Count < enemyAmount && game.ChapterTwo.ChapterTwoBooleans["centralHallICleared"] == false && !game.ChapterTwo.ChapterTwoBooleans["centralHallILocked"])
            {
                RespawnGroundEnemies();
                spawnEnemies = false;
                toCentralHallII.IsUseable = false;
                toMainChamber.IsUseable = false;
            }
            else if (enemiesInMap.Count == enemyAmount && game.ChapterTwo.ChapterTwoBooleans["centralHallICleared"] == false && !game.ChapterTwo.ChapterTwoBooleans["centralHallILocked"])
            {
                game.ChapterTwo.ChapterTwoBooleans["centralHallILocked"] = true;
                game.Camera.ShakeCamera(5, 15);
            }
            else if (enemiesInMap.Count == 0 && game.ChapterTwo.ChapterTwoBooleans["centralHallILocked"])
            {
                game.ChapterTwo.ChapterTwoBooleans["centralHallILocked"] = false;
                game.ChapterTwo.ChapterTwoBooleans["centralHallICleared"] = true;
                game.Camera.ShakeCamera(5, 15);

                toCentralHallII.IsUseable = true;
                toMainChamber.IsUseable = true;
            }

            if (game.ChapterTwo.ChapterTwoBooleans["centralHallICleared"] && enemiesInMap.Count < enemyAmount && spawnEnemies)
            {
                RespawnGroundEnemies();

                if (enemiesInMap.Count == enemyAmount)
                    spawnEnemies = false;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMainChamber = new Portal(80, platforms[0], "Central Hall I");
            toCentralHallII = new Portal(1545, platforms[0], "Central Hall I");
            fromSymmetryLeft = new Portal(600, -100, mapName);
            fromSymmetryRight = new Portal(1200, -100, mapName);

        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.ChapterTwo.ChapterTwoBooleans["centralHallILocked"])
            {
                s.Draw(bars, new Vector2(0, 0), Color.White);
            }
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMainChamber, MainChamber.ToCentralHallI);
            portals.Add(toCentralHallII, CentralHallII.ToCentralHallI);
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
            }

            s.Draw(foreground, new Vector2(0, 0), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
