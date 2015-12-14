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
    class HallofTrials : MapClass
    {
        static Portal toSummoningCrypt;
        public static Portal ToSummoningCrypt { get { return toSummoningCrypt; } }

        static Portal toBurialChamber;
        public static Portal ToBurialChamber { get { return toBurialChamber; } }

        Texture2D foreground, singleBroken, singleHealthy;

        DisappearingPlatform collapseOne, collapseTwo, collapseThree;

        List<DisappearingPlatform> dispPlats;
        public HallofTrials(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1800;
            mapWidth = 4000;
            mapName = "Hall of Trials";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 8;
            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            interactiveObjects.Add(new Barrel(game, 848, mapRec.Y + 929 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .16f, false, Barrel.BarrelType.pyramidPitcher));

            interactiveObjects.Add(new Barrel(game, 1655, mapRec.Y + 925 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .26f, false, Barrel.BarrelType.pyramidBirdJar));

            interactiveObjects.Add(new Barrel(game, 2115, mapRec.Y + 921 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .36f, false, Barrel.BarrelType.pyramidUrn));

            interactiveObjects.Add(new Barrel(game, 2229, mapRec.Y + 921 + 155, Game1.interactiveObjects["Barrel"], true, 3, 4, .36f, false, Barrel.BarrelType.pyramidUrn));

            interactiveObjects.Add(new Barrel(game, 2342, mapRec.Y + 921 + 155, Game1.interactiveObjects["Barrel"], true, 3, 4, .36f, false, Barrel.BarrelType.pyramidUrn));

            SpikeTrap spikeTrap1 = new SpikeTrap(70, 3169, mapRec.Y + 361, game, 80);
            SpikeTrap spikeTrap2 = new SpikeTrap(70, 2619, mapRec.Y + 193, game, 80);
            SpikeTrap spikeTrap3 = new SpikeTrap(70, 2083, mapRec.Y + 274, game, 80);
            SpikeTrap spikeTrap4 = new SpikeTrap(70, 1510, mapRec.Y + 205, game, 80);
            SpikeTrap spikeTrap5 = new SpikeTrap(70, 1510 - 308, mapRec.Y + 205, game, 80);

            spikeTrap2.Timer = 75;
            spikeTrap3.Timer = 35;
            spikeTrap4.Timer = 65;
            spikeTrap5.Timer = 55;
            mapHazards.Add(spikeTrap1);
            mapHazards.Add(spikeTrap2);
            mapHazards.Add(spikeTrap3);
            mapHazards.Add(spikeTrap4);
            mapHazards.Add(spikeTrap5);

            collapseOne = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(851, -259, 50, 50), false, false, false, 20, 120);
            collapseTwo = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(512, -369, 50, 50), false, false, false, 20, 120);
            collapseThree = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2908, 59, 50, 50), true, false, false, 20, 120);
            platforms.Add(collapseOne);
            platforms.Add(collapseTwo);
            platforms.Add(collapseThree);

            dispPlats = new List<DisappearingPlatform>();
            dispPlats.Add(collapseOne);
            dispPlats.Add(collapseTwo);
            dispPlats.Add(collapseThree);

            //--Map Quest
            mapWithMapQuest = true;

            enemiesToKill.Add(20);
            enemiesKilledForQuest.Add(0);
            enemyNames.Add("Mummy");

            MapQuestSign sign = new MapQuestSign(200, -510, "Kill 20 Mummies", enemiesToKill,
enemiesKilledForQuest, enemyNames, player, new List<Object>() { new Experience(250), new Textbook(), new Karma(2) });
            mapQuestSigns.Add(sign);

            interactiveObjects.Add(new ExplodingFlower(game, 1975, 200, false, 300));

            enemyNamesAndNumberInMap.Add("Locust", 0);
            enemyNamesAndNumberInMap.Add("Mummy", 0);
            enemyNamesAndNumberInMap.Add("Vile Mummy", 0);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\HallOfTrials\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\HallOfTrials\foreground");
            singleBroken = content.Load<Texture2D>(@"Maps\History\Pyramid\HallOfTrials\singleBroken");
            singleHealthy = content.Load<Texture2D>(@"Maps\History\Pyramid\HallOfTrials\singleHealthy");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.LocustEnemy(content);
            EnemyContentLoader.MummyEnemy(content);
            EnemyContentLoader.VileMummyEnemy(content);
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            Enemy en;

            if (Game1.randomNumberGen.Next(0, 3) == 1 && enemyNamesAndNumberInMap["Vile Mummy"] < 2)
            {
                en = new VileMummy(pos, "Vile Mummy", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.SpawnWithPoof = true;
                en.TimeBeforeSpawn = 60;

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
            else if(enemyNamesAndNumberInMap["Mummy"] < 3)
            {
                en = new Mummy(pos, "Mummy", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);


                en.SpawnWithPoof = true;
                en.TimeBeforeSpawn = 60;

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
        }

        public override void RespawnFlyingEnemies(Rectangle mapRec)
        {
            base.RespawnFlyingEnemies(mapRec);
            if (enemyNamesAndNumberInMap["Locust"] < 3)
            {
                Locust en = new Locust(pos, "Locust", game, ref player, this, mapRec);


                en.SpawnWithPoof = true;
                en.TimeBeforeSpawn = 600;

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    enemyNamesAndNumberInMap["Locust"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void Update()
        {
            base.Update();

            if (enemiesInMap.Count < enemyAmount)
            {
                RespawnGroundEnemies();
                RespawnFlyingEnemies(new Rectangle(100, mapRec.Y + 100, mapWidth - 200, 500));
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toSummoningCrypt = new Portal(3800, platforms[0], "Hall of Trials");
            toBurialChamber = new Portal(50, platforms[0], "Hall of Trials");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            foreach (DisappearingPlatform plat in dispPlats)
            {
                if(plat.health < plat.maxHealth / 2 && plat.health > 0)
                    s.Draw(singleBroken, new Vector2(plat.RecX - 50, plat.RecY), Color.White);
                else if(plat.health > 0)
                    s.Draw(singleHealthy, new Vector2(plat.RecX - 50, plat.RecY - 15), Color.White);
            }

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toSummoningCrypt, TheSummoningCrypt.ToHallOfTrialss);
            portals.Add(toBurialChamber, BurialChamber.ToHallOfTrials);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
