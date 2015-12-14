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
    class FalseRoom : MapClass
    {
        static Portal toCollapsingRoom;
        static Portal toFlowerSanctuary;
        static Portal toCliffOfIle;

        public static Portal ToCliffOfIle { get { return toCliffOfIle; } }
        public static Portal ToFlowerSanctuary { get { return toFlowerSanctuary; } }
        public static Portal ToCollapsingRoom { get { return toCollapsingRoom; } }

        Texture2D foreground, wall;
        BreakableObject breakableWall;
        ExplodingFlower flower;
        public FalseRoom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2300;
            mapName = "False Room";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 3;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            breakableWall = new BreakableObject(game, 876, 320, Game1.whiteFilter, false, 4, 0, 0, false);
            breakableWall.Rec = new Rectangle(876, 0, 300, 720);
            breakableWall.VitalRec = new Rectangle(876, 320, 300, 300);
            interactiveObjects.Add(breakableWall);

            interactiveObjects.Add(new Barrel(game, 1107, 462 + 155, Game1.interactiveObjects["Barrel"], true, 3, 5, .36f, false, Barrel.BarrelType.pyramidBirdJar));
            interactiveObjects.Add(new Barrel(game, 1552, 466 + 155, Game1.interactiveObjects["Barrel"], true, 2, 2, .16f, false, Barrel.BarrelType.pyramidUrn));
            interactiveObjects.Add(new Barrel(game, 747, 464 + 155, Game1.interactiveObjects["Barrel"], true, 3, 4, .06f, false, Barrel.BarrelType.pyramidPitcher));
            interactiveObjects.Add(new Barrel(game, 674, 466 + 155, Game1.interactiveObjects["Barrel"], true, 3, 4, .06f, false, Barrel.BarrelType.pyramidPitcher));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\FalseRoom\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\FalseRoom\foreground");
            wall = content.Load<Texture2D>(@"Maps\History\Pyramid\FalseRoom\wall");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.AnubisWarriorEnemy(content);
            EnemyContentLoader.SharedAnubisSounds(content);
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void RespawnGroundEnemies()
        {
            while (platforms[platformNum = rand.Next(0, platforms.Count)].SpawnOnTop == false)
            {
                platformNum = rand.Next(0, platforms.Count);
            }
            if(breakableWall.Finished)
                monsterX = rand.Next(platforms[platformNum].Rec.X, platforms[platformNum].Rec.X + platforms[platformNum].Rec.Width);
            else
                monsterX = rand.Next(1110, platforms[platformNum].Rec.X + platforms[platformNum].Rec.Width);

            monsterY = 0;
            pos = new Vector2(monsterX, monsterY);

            AnubisWarrior en = new AnubisWarrior(pos, "Anubis Warrior", game, ref player, this);
            monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
            en.Position = new Vector2(monsterX, monsterY);

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
        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            spawnEnemies = true;
        }
        public override void Update()
        {
            base.Update();

            if (breakableWall.Health <= 0 && breakableWall.Finished == false)
            {
                Chapter.effectsManager.AddSmokePoof(new Rectangle(836, 390, 250, 250), 2);
                breakableWall.Finished = true;
                game.Camera.ShakeCamera(5, 15);
            }

            if (enemyAmount > enemiesInMap.Count && spawnEnemies)
            {
                RespawnGroundEnemies();

                if (enemiesInMap.Count == enemyAmount)
                    spawnEnemies = false;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toCollapsingRoom = new Portal(50, platforms[0], "False Room");
            toFlowerSanctuary = new Portal(1700, platforms[0], "False Room");
            toCliffOfIle = new Portal(2070, platforms[0], "False Room");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (!breakableWall.Finished)
            {
                s.Draw(wall, new Vector2(876, 0), Color.White);
            }
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toFlowerSanctuary, FlowerSanctuary.ToFalseRoom);
            portals.Add(toCliffOfIle, TheCliffOfIle.ToFalseRoom);
            portals.Add(toCollapsingRoom, CollapsingRoom.ToFalseRoom);
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
