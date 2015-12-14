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
    class TheMoaningHallway : MapClass
    {
        static Portal toDarkenedChamber;
        public static Portal ToDarkenedChamber { get { return toDarkenedChamber; } }

        static Portal toTheHallofMummies;
        public static Portal ToTheHallofMummies { get { return toTheHallofMummies; } }

        Texture2D foreground;

        public TheMoaningHallway(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 4000;
            mapName = "The Moaning Hallway";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Mummy", 0);

            interactiveObjects.Add(new Barrel(game, 3304, 455 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .26f, false, Barrel.BarrelType.pyramidBirdJar));

            interactiveObjects.Add(new Barrel(game, 804, 455 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .36f, false, Barrel.BarrelType.pyramidUrn));

            interactiveObjects.Add(new Barrel(game, 402, 455 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .16f, false, Barrel.BarrelType.pyramidPitcher));

            interactiveObjects.Add(new Barrel(game, 2291, 455 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .16f, false, Barrel.BarrelType.pyramidPitcher));

            interactiveObjects.Add(new Barrel(game, 2168, 455 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .16f, false, Barrel.BarrelType.pyramidPitcher));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\MoaningHallway\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\MoaningHallway\foreground");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.MummyEnemy(content);
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

            if (enemyNamesAndNumberInMap["Mummy"] < enemyAmount)
            {
                Mummy en = new Mummy(pos, "Mummy", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.SpawnWithPoof = false;

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }

                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Mummy"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }


        public override void Update()
        {
            base.Update();

            if (enemiesInMap.Count < enemyAmount && spawnEnemies)
            {
                RespawnGroundEnemies();

                if (enemiesInMap.Count == enemyAmount)
                    spawnEnemies = false;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toDarkenedChamber = new Portal(50, platforms[0], "The Moaning Hallway");
            toTheHallofMummies = new Portal(3750, platforms[0], "The Moaning Hallway");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toDarkenedChamber, DarkenedChamber.ToTheMoaningHallway);
            portals.Add(toTheHallofMummies, HalloftheUndead.ToHallofMoaning);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, 0), Color.White * .989f);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
