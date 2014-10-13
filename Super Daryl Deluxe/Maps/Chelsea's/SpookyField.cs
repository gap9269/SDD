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
    class SpookyField : MapClass
    {
        static Portal toChelseasField;
        static Portal toAnotherSpookyField;

        public static Portal ToChelseasField { get { return toChelseasField; } }
        public static Portal ToAnotherSpookyField { get { return toAnotherSpookyField; } }

        Texture2D foreground1, foreground2, backField, barn, sky, clouds, clouds2, fastClouds, moonHappy, moonAngry;

        float cloud1Pos = 0;
        float cloud2Pos = 1621;
        float fastCloudPos = 432;

        float moonFaceAlpha = 0f;
        int moonFaceTimer;
        bool moonIsAngry = false;
        Random randomMoonTime;
        int timeUntilNextMoonFace;

        public SpookyField(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1150;
            mapWidth = 4350;
            mapName = "Spooky Field";

            randomMoonTime = new Random();

            timeUntilNextMoonFace = randomMoonTime.Next(1000, 6000);

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;
            zoomLevel = .9f;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Crow", 0);
            enemyNamesAndNumberInMap.Add("Scarecrow", 0);

            enemyAmount = 5;

            //Breakable scarecrows
            Scarecrow scare = new Scarecrow(game, 1332, 308 + 310 + 72, Game1.interactiveObjects["Scarecrow"], true, 5, 5, .20f, false);
            interactiveObjects.Add(scare);

            Scarecrow scare1 = new Scarecrow(game, 2674, 308 + 310 + 72, Game1.interactiveObjects["Scarecrow"], true, 5, 5, .13f, false);
            interactiveObjects.Add(scare1);

            backgroundMusicName = "Spooky Field";
            
        }

        public override void PlayBackgroundMusic()
        {
            base.PlayBackgroundMusic();

            Sound.PlayBackGroundMusic("Spooky Field");
        }

        public override void LoadContent()
        {
            base.LoadContent();

            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\SpookyField"));
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\SpookyField2"));

            foreground1 = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldFore");
            foreground2 = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldFore2");

            backField = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldBack");
            barn = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldBackBack");

            sky = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldSky");

            clouds = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldClouds");
            clouds2 = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldClouds2");
            fastClouds = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldFastClouds");

            moonHappy = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldMoonHappy");

            moonAngry = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldMoonAngry");

            //If the last map does not have the same music
            if (Chapter.lastMap != "Another Spooky Field")
            {
                SoundEffect bg = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Black Vortex");
                SoundEffectInstance backgroundMusic = bg.CreateInstance();
                backgroundMusic.IsLooped = true;
                Sound.music.Add("Spooky Field", backgroundMusic);
                Sound.backgroundVolume = 1f;
            }
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            if (Chapter.theNextMap != "AnotherSpookyField")
            {
                Sound.UnloadBackgroundMusic();
            }
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            game.EnemySpriteSheets.Add("Crow", content.Load<Texture2D>(@"EnemySprites\CrowSheet"));
            game.EnemySpriteSheets.Add("Scarecrow", content.Load<Texture2D>(@"EnemySprites\ScarecrowSheet"));
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            switch (game.chapterState)
            {

                case Game1.ChapterState.chapterTwo:
                    if (enemyNamesAndNumberInMap["Scarecrow"] < 3)
                    {
                        ScarecrowEnemy ben = new ScarecrowEnemy(pos, "Scarecrow", game, ref player, this, false, new Rectangle(100, 100, mapWidth - 500, 300));
                        monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;
                        ben.Position = new Vector2(monsterX, monsterY);

                        Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);
                        if (benRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            enemiesInMap.Add(ben);
                            enemyNamesAndNumberInMap["Scarecrow"]++;
                        }
                    }
                    break;
            }
        }

        public override void RespawnFlyingEnemies(Rectangle mapRec)
        {
            base.RespawnFlyingEnemies(mapRec);

            switch (game.chapterState)
            {

                case Game1.ChapterState.chapterTwo:
                    if (enemyNamesAndNumberInMap["Crow"] < 2)
                    {
                        Crow en = new Crow(pos, "Crow", game, ref player, this, mapRec);

                        Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                        if (testRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            enemiesInMap.Add(en);
                            enemyNamesAndNumberInMap["Crow"]++;
                        }
                    }
                    break;
            }

        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            if (Game1.mapBooleans.chapterTwoMapBooleans["ClearedSpookyField"] == true)
                spawnEnemies = true;
        }

        public override void Update()
        {
            base.Update();

            PlayBackgroundMusic();

            //CLOUD STUFF
            //1750 is just off the map with the parallax value clouds have
            cloud1Pos -= .4f;
            cloud2Pos -= .4f;
            fastCloudPos -= .8f;

            if (cloud1Pos + clouds.Width < 0)
                cloud1Pos = 1750;

            if (cloud2Pos + clouds2.Width < 0)
                cloud2Pos = 1750;

            if (fastCloudPos + fastClouds.Width < 0)
                fastCloudPos = 1750;


            #region MOON FACES
            if (timeUntilNextMoonFace > 0 && moonFaceTimer <= 0)
                timeUntilNextMoonFace--;

            if (timeUntilNextMoonFace == 0 && moonFaceTimer <= 0)
            {
                moonFaceTimer = 300;

                int angry = randomMoonTime.Next(2);

                if (angry == 1)
                    moonIsAngry = true;
                else
                    moonIsAngry = false;
            }

            if (moonFaceTimer > 0)
            {
                moonFaceTimer--;

                if (moonFaceTimer == 0)
                {
                    timeUntilNextMoonFace = randomMoonTime.Next(1000, 6000);
                }
            }

            if (moonFaceTimer > 0 && moonFaceAlpha != 1)
            {
                if (moonIsAngry)
                    moonFaceAlpha += .01f;
                else
                    moonFaceAlpha += .01f;

                if (moonFaceAlpha > 1)
                    moonFaceAlpha = 1f;
            }
            else if (moonFaceTimer <= 0 && moonFaceAlpha != 0)
            {
                moonFaceAlpha -= .01f;

                if (moonFaceAlpha < 0)
                    moonFaceAlpha = 0f;
            }
            #endregion

            #region SPAWN INITIAL ENEMIES
            if (Game1.mapBooleans.chapterTwoMapBooleans["SpawnedScarecrows"] == false)
            {
                spawnEnemies = false;
                Game1.mapBooleans.chapterTwoMapBooleans["SpawnedScarecrows"] = true;
                ScarecrowEnemy s1 = new ScarecrowEnemy(new Vector2(506, platforms[0].Rec.Y - 311), "Scarecrow", game, ref player, this, false, new Rectangle(100, 100, mapWidth - 500, 300));
                s1.CanBeHit = false;

                ScarecrowEnemy s2 = new ScarecrowEnemy(new Vector2(1916, 390), "Scarecrow", game, ref player, this, false, new Rectangle(100, 100, mapWidth - 500, 300));
                s2.CanBeHit = false;
                s2.canBeActivated = false;


                ScarecrowEnemy s3 = new ScarecrowEnemy(new Vector2(2058, platforms[0].Rec.Y - 311), "Scarecrow", game, ref player, this, false, new Rectangle(100, 100, mapWidth - 500, 300));
                s3.CanBeHit = false;
                s3.FacingRight = false;
                s3.canBeActivated = false;

                ScarecrowEnemy s4 = new ScarecrowEnemy(new Vector2(3776, platforms[0].Rec.Y - 311), "Scarecrow", game, ref player, this, false, new Rectangle(100, 100, mapWidth - 500, 300));
                s4.CanBeHit = false;
                s4.canBeActivated = false;

                ScarecrowEnemy s5 = new ScarecrowEnemy(new Vector2(4054, platforms[0].Rec.Y - 311), "Scarecrow", game, ref player, this, false, new Rectangle(100, 100, mapWidth - 500, 300));
                s5.CanBeHit = false;
                s5.FacingRight = false;
                s5.canBeActivated = false;

                ScarecrowEnemy s6 = new ScarecrowEnemy(new Vector2(3906, 390), "Scarecrow", game, ref player, this, false, new Rectangle(100, 100, mapWidth - 500, 300));
                s6.CanBeHit = false;
                s6.canBeActivated = false;


                ScarecrowEnemy s7 = new ScarecrowEnemy(new Vector2(2392, 390), "Scarecrow", game, ref player, this, false, new Rectangle(100, 100, mapWidth - 500, 300));
                s7.CanBeHit = false;
                s7.canBeActivated = false;


                s1.SpawnWithPoof = false;
                s2.SpawnWithPoof = false;
                s3.SpawnWithPoof = false;

                enemiesInMap.Add(s1);
                enemiesInMap.Add(s2);
                enemiesInMap.Add(s3);
                enemiesInMap.Add(s6);
                enemiesInMap.Add(s4);
                enemiesInMap.Add(s5);
                enemiesInMap.Add(s7);


                enemyNamesAndNumberInMap["Scarecrow"] = 7;
                enemyNamesAndNumberInMap["Crow"] = 0;
            }
            #endregion

            //Update the progress of clearing the map
            if (Game1.mapBooleans.chapterTwoMapBooleans["ClearedSpookyField"] == false && Game1.mapBooleans.chapterTwoMapBooleans["SpawnedScarecrows"])
            {
                if (enemyNamesAndNumberInMap["Scarecrow"] == 6 && enemyNamesAndNumberInMap["Crow"] == 0 && Game1.mapBooleans.chapterTwoMapBooleans["KilledFirstScare"] == false)
                {
                    Game1.mapBooleans.chapterTwoMapBooleans["KilledFirstScare"] = true;
                    (enemiesInMap[0] as ScarecrowEnemy).canBeActivated = true;
                    (enemiesInMap[1] as ScarecrowEnemy).canBeActivated = true;

                    (enemiesInMap[0] as ScarecrowEnemy).CanBeHit = true;
                    (enemiesInMap[1] as ScarecrowEnemy).CanBeHit = true;

                }
                if (enemyNamesAndNumberInMap["Scarecrow"] == 4 && enemyNamesAndNumberInMap["Crow"] == 0 && Game1.mapBooleans.chapterTwoMapBooleans["KilledSecondScare"] == false)
                {

                    moonIsAngry = true;
                    moonFaceTimer = 300;

                    Game1.mapBooleans.chapterTwoMapBooleans["KilledSecondScare"] = true;
                    (enemiesInMap[0] as ScarecrowEnemy).Activated = true;
                    (enemiesInMap[1] as ScarecrowEnemy).Activated = true;
                    (enemiesInMap[2] as ScarecrowEnemy).Activated = true;
                    (enemiesInMap[3] as ScarecrowEnemy).Activated = true;

                    (enemiesInMap[0] as ScarecrowEnemy).Hostile = true;
                    (enemiesInMap[1] as ScarecrowEnemy).Hostile = true;
                    (enemiesInMap[2] as ScarecrowEnemy).Hostile = true;
                    (enemiesInMap[3] as ScarecrowEnemy).Hostile = true;

                    (enemiesInMap[0] as ScarecrowEnemy).CanBeHit = true;
                    (enemiesInMap[1] as ScarecrowEnemy).CanBeHit = true;
                    (enemiesInMap[2] as ScarecrowEnemy).CanBeHit = true;
                    (enemiesInMap[3] as ScarecrowEnemy).CanBeHit = true;
                }

                if (enemiesInMap.Count == 0)
                {
                    moonFaceTimer = 300;
                    moonIsAngry = false;
                    Game1.mapBooleans.chapterTwoMapBooleans["ClearedSpookyField"] = true;
                }
            }
            else if (toAnotherSpookyField.IsUseable == false)
            {
                spawnEnemies = false;
                toAnotherSpookyField.IsUseable = true;
            }

            //When you've cleared the map and re-entered, spawn some crows and scarecrows
            if (enemiesInMap.Count < enemyAmount && spawnEnemies && Game1.mapBooleans.chapterTwoMapBooleans["ClearedSpookyField"] == true)
            {
                RespawnGroundEnemies();
                RespawnFlyingEnemies(new Rectangle(100, 100, mapWidth - 500, 300));
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toChelseasField = new Portal(100, platforms[0], "SpookyField");
            toAnotherSpookyField = new Portal(4200, platforms[0], "SpookyField");
            toAnotherSpookyField.IsUseable = false;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toChelseasField, ChelseasField.ToSpookyField);
            portals.Add(toAnotherSpookyField, AnotherSpookyField.ToSpookyField);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }
            }

            s.Draw(foreground1, new Rectangle(0, mapRec.Y, foreground1.Width, foreground1.Height), Color.White);
            s.Draw(foreground2, new Rectangle(foreground1.Width, mapRec.Y, foreground2.Width, foreground2.Height), Color.White);
            s.End();
        }


        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.05f, this, game));
            
            s.Draw(sky, new Rectangle(0, mapRec.Y, sky.Width, sky.Height), Color.White);

            if (moonIsAngry)
                s.Draw(moonAngry, new Rectangle(0, mapRec.Y + 150, moonAngry.Width, moonAngry.Height), Color.White * moonFaceAlpha);
            else
                s.Draw(moonHappy, new Rectangle(0, mapRec.Y + 150, moonHappy.Width, moonHappy.Height), Color.White * moonFaceAlpha);


            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.1f, this, game));
            s.Draw(clouds, new Rectangle((int)cloud1Pos, 0, clouds.Width, clouds.Height), Color.White);
            s.Draw(clouds2, new Rectangle((int)cloud2Pos, 0, clouds2.Width, clouds2.Height), Color.White);
            s.Draw(fastClouds, new Rectangle((int)fastCloudPos, 0, fastClouds.Width, fastClouds.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.1f, this, game));
            s.Draw(barn, new Rectangle(0, mapRec.Y, barn.Width, barn.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.6f, this, game));
            s.Draw(backField, new Rectangle(0, mapRec.Y, backField.Width, backField.Height), Color.White);
            s.End();

        }
    }
}
