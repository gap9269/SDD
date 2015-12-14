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
    class RestrictedHallway : MapClass
    {
        static Portal toBackstage;
        static Portal toAxisOfMusic;
        static Portal toBathroom;
        static Portal toBathroom2;

        public static Portal ToBathroom2 { get { return toBathroom2; } }
        public static Portal ToBathroom { get { return toBathroom; } }
        public static Portal ToBackstage { get { return toBackstage; } }
        public static Portal ToAxisOfMusic { get { return toAxisOfMusic; } }

        Texture2D outhouseTexture, lasers;

        LivingLocker locker;

        Platform laser1, laser2, laser3, laser4, laser5;

        int enemyWaveState;
        int cymbalAmount, maracaAmount, saxAmount;
        int maxSpawnPosX;
        Boolean clearedWave = false;
        int enemiesSpawnedOnFinalWave;

        public RestrictedHallway(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 7000;
            mapName = "Restricted Hallway";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 1;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            locker = new LivingLocker(game, new Rectangle(2000, 35, 3000, 400));
            interactiveObjects.Add(locker);

            laser1 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(725, -50, 50, 800), false, false, false, Platform.PlatformType.rock);
            laser2 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2025, -50, 50, 800), false, false, false, Platform.PlatformType.rock);
            laser3 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3325, -50, 50, 800), false, false, false, Platform.PlatformType.rock);
            laser4 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4625, -50, 50, 800), false, false, false, Platform.PlatformType.rock);
            laser5 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(5825, -50, 50, 800), false, false, false, Platform.PlatformType.rock);

            enemyNamesAndNumberInMap.Add("Sergeant Cymbal", 0);
            enemyNamesAndNumberInMap.Add("Maracas Hermanos", 0);
            enemyNamesAndNumberInMap.Add("Captain Sax", 0);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Restricted Hallway\background"));
            background.Add(content.Load<Texture2D>(@"Maps\Music\Restricted Hallway\background2"));
            outhouseTexture = content.Load<Texture2D>(@"Maps\Outhouse");
            lasers = content.Load<Texture2D>(@"Maps\Music\Restricted Hallway\lasers");

        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.CaptainSaxEnemy(content);
            EnemyContentLoader.MaracasHermanosEnemy(content);
            EnemyContentLoader.SergeantCymbalEnemy(content);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            int ranNum = Game1.randomNumberGen.Next(0, 3);

            if (ranNum == 0)
            {
                SergeantCymbal en = new SergeantCymbal(pos, "Sergeant Cymbal", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.TimeBeforeSpawn = 5;
                en.Hostile = true;
                en.UpdateRectangles();

                Rectangle testRec = new Rectangle(en.VitalRecX, monsterY, en.VitalRec.Width, en.VitalRec.Height);
                if (testRec.Intersects(player.VitalRec))
                {
                }
                else
                {
                    en.UpdateRectangles();
                    AddEnemyToEnemyList(en);
                    enemyNamesAndNumberInMap["Sergeant Cymbal"]++;
                }
            }

            else if (ranNum == 1)
            {
                MaracasHermanos en = new MaracasHermanos(pos, "Maracas Hermanos", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.TimeBeforeSpawn = 5;
                en.UpdateRectangles();
                en.Hostile = true;
                Rectangle testRec = new Rectangle(en.VitalRecX, monsterY, en.VitalRec.Width, en.VitalRec.Height);

                if (testRec.Intersects(player.VitalRec))
                {
                }
                else
                {
                    en.UpdateRectangles();
                    AddEnemyToEnemyList(en);
                    enemyNamesAndNumberInMap["Maracas Hermanos"]++;

                }
            }

            else if (ranNum == 2)
            {
                CaptainSax en = new CaptainSax(pos, "Captain Sax", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.TimeBeforeSpawn = 5;
                en.Hostile = true;
                en.UpdateRectangles();

                Rectangle testRec = new Rectangle(en.VitalRecX, monsterY, en.VitalRec.Width, en.VitalRec.Height);

                if (testRec.Intersects(player.VitalRec))
                {
                }
                else
                {
                    en.UpdateRectangles();
                    AddEnemyToEnemyList(en);
                    enemyNamesAndNumberInMap["Captain Sax"]++;

                }
            }

        }

        public void RespawnGroundEnemiesForWaves()
        {
            //Base update code is changed a bit
            while (platforms[platformNum = rand.Next(0, platforms.Count)].SpawnOnTop == false)
            {
                platformNum = rand.Next(0, platforms.Count);
            }

            monsterX = Game1.randomNumberGen.Next(800, maxSpawnPosX);
            monsterY = 0;
            pos = new Vector2(monsterX, monsterY);

            #region All but the final wave
            if (enemyWaveState != 7)
            {
                if (enemyNamesAndNumberInMap["Sergeant Cymbal"] < cymbalAmount)
                {
                    SergeantCymbal en = new SergeantCymbal(pos, "Sergeant Cymbal", game, ref player, this);
                    monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                    en.Position = new Vector2(monsterX, monsterY);

                    en.TimeBeforeSpawn = 5;
                    en.Hostile = true;
                    en.UpdateRectangles();

                    Rectangle testRec = new Rectangle(en.VitalRecX, monsterY, en.VitalRec.Width, en.VitalRec.Height);
                    if (testRec.Intersects(player.VitalRec))
                    {
                    }
                    else
                    {
                        en.UpdateRectangles();
                        AddEnemyToEnemyList(en);
                        enemyNamesAndNumberInMap["Sergeant Cymbal"]++;
                    }
                }

                else if (enemyNamesAndNumberInMap["Maracas Hermanos"] < maracaAmount)
                {
                    MaracasHermanos en = new MaracasHermanos(pos, "Maracas Hermanos", game, ref player, this);
                    monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                    en.Position = new Vector2(monsterX, monsterY);

                    en.TimeBeforeSpawn = 5;
                    en.UpdateRectangles();
                    en.Hostile = true;
                    Rectangle testRec = new Rectangle(en.VitalRecX, monsterY, en.VitalRec.Width, en.VitalRec.Height);

                    if (testRec.Intersects(player.VitalRec))
                    {
                    }
                    else
                    {
                        en.UpdateRectangles();
                        AddEnemyToEnemyList(en);
                        enemyNamesAndNumberInMap["Maracas Hermanos"]++;
                    }
                }

                else if (enemyNamesAndNumberInMap["Captain Sax"] < saxAmount)
                {
                    CaptainSax en = new CaptainSax(pos, "Captain Sax", game, ref player, this);
                    monsterX = Game1.randomNumberGen.Next(800, maxSpawnPosX);
                    monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                    en.Position = new Vector2(monsterX, monsterY);

                    en.TimeBeforeSpawn = 5;
                    en.Hostile = true;
                    en.UpdateRectangles();

                    Rectangle testRec = new Rectangle(en.VitalRecX, monsterY, en.VitalRec.Width, en.VitalRec.Height);

                    if (testRec.Intersects(player.VitalRec))
                    {
                    }
                    else
                    {
                        en.UpdateRectangles();
                        AddEnemyToEnemyList(en);
                        enemyNamesAndNumberInMap["Captain Sax"]++;
                    }
                }
            }
            #endregion

            else if(enemiesSpawnedOnFinalWave < 25)
            {
                #region Final wave
                int ranNum = Game1.randomNumberGen.Next(0, 3);


                if (ranNum == 0)
                {
                    SergeantCymbal en = new SergeantCymbal(pos, "Sergeant Cymbal", game, ref player, this);
                    monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                    en.Position = new Vector2(monsterX, monsterY);

                    en.TimeBeforeSpawn = 5;
                    en.Hostile = true;
                    en.UpdateRectangles();

                    Rectangle testRec = new Rectangle(en.VitalRecX, monsterY, en.VitalRec.Width, en.VitalRec.Height);
                    if (testRec.Intersects(player.VitalRec))
                    {
                    }
                    else
                    {
                        en.UpdateRectangles();
                        AddEnemyToEnemyList(en);
                        enemyNamesAndNumberInMap["Sergeant Cymbal"]++;
                        enemiesSpawnedOnFinalWave++;
                    }
                }

                else if (ranNum == 1)
                {
                    MaracasHermanos en = new MaracasHermanos(pos, "Maracas Hermanos", game, ref player, this);
                    monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                    en.Position = new Vector2(monsterX, monsterY);

                    en.TimeBeforeSpawn = 5;
                    en.UpdateRectangles();
                    en.Hostile = true;
                    Rectangle testRec = new Rectangle(en.VitalRecX, monsterY, en.VitalRec.Width, en.VitalRec.Height);

                    if (testRec.Intersects(player.VitalRec))
                    {
                    }
                    else
                    {
                        en.UpdateRectangles();
                        AddEnemyToEnemyList(en);
                        enemiesSpawnedOnFinalWave++;
                        enemyNamesAndNumberInMap["Maracas Hermanos"]++;
                    }
                }

                else if (ranNum == 2)
                {
                    CaptainSax en = new CaptainSax(pos, "Captain Sax", game, ref player, this);
                    monsterX = Game1.randomNumberGen.Next(800, maxSpawnPosX);
                    monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                    en.Position = new Vector2(monsterX, monsterY);

                    en.TimeBeforeSpawn = 5;
                    en.Hostile = true;
                    en.UpdateRectangles();

                    Rectangle testRec = new Rectangle(en.VitalRecX, monsterY, en.VitalRec.Width, en.VitalRec.Height);

                    if (testRec.Intersects(player.VitalRec))
                    {
                    }
                    else
                    {
                        en.UpdateRectangles();
                        AddEnemyToEnemyList(en);
                        enemiesSpawnedOnFinalWave++;
                        enemyNamesAndNumberInMap["Captain Sax"]++;
                    }
                }
            }
            #endregion

        }

        public override void Update()
        {
            base.Update();

            if (game.ChapterOne.ChapterOneBooleans["chasingTheManager"] && !game.ChapterOne.ChapterOneBooleans["clearedRestrictedHall"])
            {
                if (enemiesInMap.Count < enemyAmount && spawnEnemies && enemyWaveState != 0)
                    RespawnGroundEnemiesForWaves();

                //Initial lasers added
                if (enemyWaveState == 0 && !platforms.Contains(laser5))
                {
                    platforms.Add(laser2);
                    platforms.Add(laser3);
                    platforms.Add(laser4);
                    platforms.Add(laser5);
                }

                //Reset spawn enemies and change enemy amounts
                if (enemyWaveState == 1 && !platforms.Contains(laser1))
                {
                    platforms.Add(laser1);
                    Game1.camera.ShakeCamera(5, 3);
                    spawnEnemies = true;
                    cymbalAmount = 1;
                    maracaAmount = 2;
                    maxSpawnPosX = 1500;
                    clearedWave = false;
                    toBathroom.IsUseable = false;
                    toBathroom2.IsUseable = false;
                    toBackstage.IsUseable = false;
                    toAxisOfMusic.IsUseable = false;
                }
                else if (enemyWaveState == 2 && spawnEnemies == false && player.PositionX > 1900 && maxSpawnPosX != 2700)
                {
                    spawnEnemies = true;
                    cymbalAmount = 2;
                    maracaAmount = 2;
                    saxAmount = 1;
                    maxSpawnPosX = 2700;
                    clearedWave = false;
                }
                else if (enemyWaveState == 3 && spawnEnemies == false && maxSpawnPosX != 2705)
                {
                    spawnEnemies = true;
                    cymbalAmount = 3;
                    saxAmount = 2;
                    maracaAmount = 0;
                    maxSpawnPosX = 2705;
                    clearedWave = false;

                }
                else if (enemyWaveState == 4 && spawnEnemies == false && maxSpawnPosX != 2706)
                {
                    spawnEnemies = true;
                    cymbalAmount = 2;
                    saxAmount = 4;
                    maracaAmount = 0;
                    maxSpawnPosX = 2706;
                    clearedWave = false;

                }
                else if (enemyWaveState == 5 && spawnEnemies == false && maxSpawnPosX != 4100)
                {
                    spawnEnemies = true;
                    maracaAmount = 6;
                    cymbalAmount = 0;
                    saxAmount = 0;
                    maxSpawnPosX = 4100;
                    clearedWave = false;
                }
                else if (enemyWaveState == 6 && spawnEnemies == false && maxSpawnPosX != 4105)
                {
                    spawnEnemies = true;
                    maracaAmount = 4;
                    cymbalAmount = 2;
                    saxAmount = 2;
                    maxSpawnPosX = 4105;
                    clearedWave = false;
                }
                else if (enemyWaveState == 7 && spawnEnemies == false && maxSpawnPosX != 5300)
                {
                    spawnEnemies = true;
                    enemyAmount = 8;
                    maxSpawnPosX = 5300;
                    clearedWave = false;
                }

                if (enemiesInMap.Count == 0 && clearedWave == false && spawnEnemies == false)
                {
                    clearedWave = true;

                    if (enemyWaveState == 1 && platforms.Contains(laser2))
                    {
                        Game1.camera.ShakeCamera(5, 3);
                        platforms.Remove(laser2);
                        enemyWaveState = 2;
                    }
                    else if (enemyWaveState == 2)
                        enemyWaveState = 3;
                    else if (enemyWaveState == 3)
                        enemyWaveState = 4;
                    else if (enemyWaveState == 4)
                    {
                        enemyWaveState = 5;
                        Game1.camera.ShakeCamera(5, 3);
                        platforms.Remove(laser3);
                    }
                    else if (enemyWaveState == 5)
                        enemyWaveState = 6;
                    else if (enemyWaveState == 6)
                    {
                        enemyWaveState = 7;
                        Game1.camera.ShakeCamera(5, 3);
                        platforms.Remove(laser4);
                    }
                }

                if (enemyWaveState == 7 && enemiesInMap.Count == 0 && enemiesSpawnedOnFinalWave >= 25)
                {
                    enemyWaveState = 8;
                    Game1.camera.ShakeCamera(5, 3);
                    platforms.Remove(laser5);
                    platforms.Remove(laser1);
                    game.ChapterOne.ChapterOneBooleans["clearedRestrictedHall"] = true;
                    toBathroom.IsUseable = true;
                    toBathroom2.IsUseable = true;
                    toBackstage.IsUseable = true;
                    toAxisOfMusic.IsUseable = true;
                }

                if (player.PositionX > 1150 && enemyWaveState == 0)
                    enemyWaveState = 1;

                if (enemyWaveState != 7)
                {
                    enemyAmount = cymbalAmount + saxAmount + maracaAmount;
                    if (enemiesInMap.Count >= enemyAmount)
                        spawnEnemies = false;
                }
            }
            else if (game.ChapterOne.ChapterOneBooleans["finishedManagerQuest"])
            {
                enemyAmount = 10;
                if (enemiesInMap.Count < enemyAmount)
                    RespawnGroundEnemies();
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(outhouseTexture, new Vector2(295, platforms[0].RecY - outhouseTexture.Height + 5), Color.White);
            s.Draw(outhouseTexture, new Rectangle(6095, platforms[0].RecY - outhouseTexture.Height + 5, outhouseTexture.Width, outhouseTexture.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
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
            if (!game.ChapterOne.ChapterOneBooleans["clearedRestrictedHall"])
            {
                switch (enemyWaveState)
                {
                    case 0:
                        s.Draw(lasers, new Vector2(2000, 0), Color.White);
                        s.Draw(lasers, new Vector2(3300, 0), Color.White);
                        s.Draw(lasers, new Vector2(4600, 0), Color.White);
                        s.Draw(lasers, new Vector2(5800, 0), Color.White);
                        break;
                    case 1:
                        s.Draw(lasers, new Vector2(700, 0), Color.White);
                        s.Draw(lasers, new Vector2(2000, 0), Color.White);
                        s.Draw(lasers, new Vector2(3300, 0), Color.White);
                        s.Draw(lasers, new Vector2(4600, 0), Color.White);
                        s.Draw(lasers, new Vector2(5800, 0), Color.White);
                        break;
                    case 2:
                    case 3:
                    case 4:
                        s.Draw(lasers, new Vector2(700, 0), Color.White);
                        s.Draw(lasers, new Vector2(3300, 0), Color.White);
                        s.Draw(lasers, new Vector2(4600, 0), Color.White);
                        s.Draw(lasers, new Vector2(5800, 0), Color.White);
                        break;
                    case 5:
                    case 6:
                        s.Draw(lasers, new Vector2(700, 0), Color.White);
                        s.Draw(lasers, new Vector2(4600, 0), Color.White);
                        s.Draw(lasers, new Vector2(5800, 0), Color.White);
                        break;
                    case 7:
                        s.Draw(lasers, new Vector2(700, 0), Color.White);
                        s.Draw(lasers, new Vector2(5800, 0), Color.White);
                        break;
                }
            }

            s.End();


            //s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White);
            //s.Draw(foreground3, new Vector2(foreground.Width + foreground2.Width, mapRec.Y), Color.White);
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toBathroom = new Portal(400, platforms[0], "Restricted Hallway");
            toBathroom2 = new Portal(6200, platforms[0], "Restricted Hallway");
            toBackstage = new Portal(65, platforms[0], "Restricted Hallway");
            toAxisOfMusic = new Portal(6750, platforms[0], "Restricted Hallway");


            toBackstage.FButtonYOffset = -77;
            toBackstage.PortalNameYOffset = -77;

            toBathroom.FButtonYOffset = -27;
            toBathroom.PortalNameYOffset = -27;
            toBathroom2.FButtonYOffset = -27;
            toBathroom2.PortalNameYOffset = -27;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toBathroom, Bathroom.ToLastMap);
            portals.Add(toBathroom2, Bathroom.ToLastMap);
            portals.Add(toBackstage, Backstage.ToRestrictedHallway);
            portals.Add(toAxisOfMusic, AxisOfMusicalReality.ToRestrictedHallway);
        }
    }
}
