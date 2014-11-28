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

    class BreakableNest : BreakableObject
    {
        public BreakableNest(Game1 g, int x, int y, Texture2D s, Boolean pass, int hlth, StoryItem story, float mon, bool fore)
            : base(g, x, y, s, pass, hlth, story, mon, fore)
        {
            rec = new Rectangle(x, y, s.Width, s.Height);
            vitalRec = new Rectangle(rec.X, rec.Y, s.Width, s.Height);
        }

        public override Rectangle GetSourceRec()
        {
            return new Rectangle(0, 0, rec.Width, rec.Height);
        }

        public override void Update()
        {
            if (!finished)
            {

                if (health <= 0 && finished == false)
                {
                    Chapter.effectsManager.AddSmokePoof(rec, 2);
                    finished = true;

                    DropHealth();
                    DropMoney();

                    //if (drop != null)
                    //{
                    //    EnemyDrop tempDrop = new EnemyDrop(drop as Equipment, new Rectangle(rec.Center.X, rec.Center.Y, 50, 50));
                    //    game.CurrentChapter.CurrentMap.Drops.Add(tempDrop);
                    //}
                    //else if (enemyDrop != "" && enemyDrop != null)
                    //{
                    //    EnemyDrop tempDrop = new EnemyDrop(enemyDrop, new Rectangle(rec.Center.X, rec.Center.Y, 50, 50));
                    //    game.CurrentChapter.CurrentMap.Drops.Add(tempDrop);
                    //}
                    //else if (storyItem != null)
                    //{
                    //    EnemyDrop tempDrop = new EnemyDrop(storyItem, new Rectangle(rec.Center.X, rec.Center.Y, 50, 50));
                    //    game.CurrentChapter.CurrentMap.Drops.Add(tempDrop);
                    //}
                }
            }
            base.Update();
        }
    }

    class SpookyEyes
    {
        int moveFrame = 1; //1, 2, or 3
        int frameTimer;
        static Random ranTime;
        static Random ranNum;
        Texture2D texture;
        Rectangle rec;
        public SpookyEyes(Texture2D tex, int x, int y)
        {
            texture = tex;
            rec = new Rectangle(x, y, 27, 14);
            ranTime = new Random();
            ranNum = new Random();
        }

        public void Update()
        {
            frameTimer--;

            if (frameTimer <= 0)
            {
                moveFrame = ranNum.Next(1, 5);

                if (moveFrame == 2)
                    frameTimer = 5;
                else
                    frameTimer = ranTime.Next(20, 120);
            }
        }

        public void Draw(SpriteBatch s)
        {
            s.Draw(texture, rec, GetSourceRec(),Color.White);
        }

        public Rectangle GetSourceRec()
        {
            return new Rectangle(27 * (moveFrame - 1), 0, 27, 14);
        }
    }

    class AnotherSpookyField : MapClass
    {
        static Portal toWorkersField;
        static Portal toSpookyField;

        public static Portal ToWorkersField { get { return toWorkersField; } }
        public static Portal ToSpookyField { get { return toSpookyField; } }


        Boolean clearedScarecrows = false;

        Texture2D foreground;

        Texture2D barn, sky, clouds, clouds2, fastClouds, moonHappy, moonAngry, hayBale, backField, eyes;

        float cloudPos = 98;
        float cloud2Pos = 1719;
        float fastCloudPos = 530;

        float moonFaceAlpha = 0f;
        int moonFaceTimer;
        bool moonIsAngry = false;
        Random randomMoonTime;
        int timeUntilNextMoonFace;

        BreakableNest nest;
        ChallengeRoomKey nestKey;
        Sparkles sparkles;

        SpookyEyes eyes1, eyes2, eyes3;

        public AnotherSpookyField(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 2500;
            mapWidth = 2600;
            mapName = "Another Spooky Field";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;
            zoomLevel = .8f;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyAmount = 11;

            //--Map Quest

            mapWithMapQuest = true;

            MapQuestSign sign = new MapQuestSign(1000, platforms[0].Rec.Y - Game1.mapSign.Height, "Clear the area of enemies!", enemiesToKill,
enemiesKilledForQuest, enemyNames, player, new List<Object>() { new Experience(850), new Money(7.50f), new HandSaw() });
            mapQuestSigns.Add(sign);
            
            
            Scarecrow scare = new Scarecrow(game, 670, 630, Game1.interactiveObjects["Scarecrow"], true, 5, 200, .03f, false);
            interactiveObjects.Add(scare);

            Scarecrow scare1 = new Scarecrow(game, 0, 94, Game1.interactiveObjects["Scarecrow"], true, 5, 130, .16f, true);
            interactiveObjects.Add(scare1);

            Scarecrow scare2 = new Scarecrow(game, 2000, -495, Game1.interactiveObjects["Scarecrow"], true, 5, 175, .38f, true);
            interactiveObjects.Add(scare2);

            enemyNamesAndNumberInMap.Add("Crow", 0);
            enemyNamesAndNumberInMap.Add("Scarecrow", 0);
            enemyNamesAndNumberInMap.Add("Field Goblin", 0);


            TreasureChest chester = new TreasureChest(Game1.treasureChestSheet, 2170, -525, player, 0, new Textbook(), this);
            treasureChests.Add(chester);

            randomMoonTime = new Random();

            timeUntilNextMoonFace = randomMoonTime.Next(1000, 6000);


            nestKey = new ChallengeRoomKey(1135, 1049 - 1440);
            nest = new BreakableNest(game, 1135, 1049 - 1440, Game1.interactiveObjects["Nest"], true, 3, nestKey, 0, false);
            interactiveObjects.Add(nest);

            backgroundMusicName = "Spooky Field";

            sparkles = new Sparkles(1135, 1049 - 1440);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\AnotherSpookyField"));
            foreground = content.Load<Texture2D>(@"Maps\Chelseas\AnotherSpookyFieldFore");
            sky = content.Load<Texture2D>(@"Maps\Chelseas\AnotherSpookyFieldSky");
            moonHappy = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldMoonHappy");
            moonAngry = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldMoonAngry");
            barn = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldBackBack");
            clouds = content.Load<Texture2D>(@"Maps\Chelseas\AnotherSpookyFieldClouds");
            clouds2 = content.Load<Texture2D>(@"Maps\Chelseas\AnotherSpookyFieldClouds2");
            fastClouds = content.Load<Texture2D>(@"Maps\Chelseas\AnotherSpookyFieldFastClouds");

            backField = content.Load<Texture2D>(@"Maps\Chelseas\AnotherSpookyFieldBack");
            hayBale = content.Load<Texture2D>(@"Maps\Chelseas\AnotherSpookyFieldHay");
            eyes = content.Load<Texture2D>(@"Maps\Chelseas\SpookyEyesSheet");

            eyes1 = new SpookyEyes(eyes, 2054, 2024 - 1440);
            eyes2 = new SpookyEyes(eyes, 536, 1127 - 1440);
            eyes3 = new SpookyEyes(eyes, 342, 1773 - 1440);

            ////If the last map does not have the same music
            //if (Chapter.lastMap != "Spooky Field")
            //{
            //    SoundEffect bg = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Black Vortex");
            //    SoundEffectInstance backgroundMusic = bg.CreateInstance();
            //    backgroundMusic.IsLooped = true;
            //    Sound.music.Add("Spooky Field", backgroundMusic);
            //    Sound.backgroundVolume = 1f;
            //}
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            //if (Chapter.theNextMap != "SpookyField")
            //{
            //    Sound.UnloadBackgroundMusic();
            //}
        }

        public override void PlayBackgroundMusic()
        {
            base.PlayBackgroundMusic();

            //Sound.PlayBackGroundMusic("Spooky Field");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.Scarecrow(content);
            EnemyContentLoader.Crow(content);
            game.EnemySpriteSheets.Add("Field Goblin", content.Load<Texture2D>(@"EnemySprites\FieldGoblinSheet"));
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            switch (game.chapterState)
            {

                case Game1.ChapterState.chapterTwo:
                    if (enemyNamesAndNumberInMap["Scarecrow"] < 8 && !clearedScarecrows)
                    {

                        //Determines if the scarecrow spawns sneaky or not
                        int activeOrNot = rand.Next(2);
                        Boolean active = false;
                        if (activeOrNot == 0)
                            active = true;

                        ScarecrowEnemy ben = new ScarecrowEnemy(pos, "Scarecrow", game, ref player, this, active, new Rectangle(180, -800, 2200, 1300));
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

                    else if (enemyNamesAndNumberInMap["Field Goblin"] < 6 && clearedScarecrows && enemiesToKill[0] - enemiesKilledForQuest[0] > enemyNamesAndNumberInMap["Field Goblin"])
                    {
                        Goblin ben = new Goblin(pos, "Field Goblin", game, ref player, this);
                        monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;
                        ben.Position = new Vector2(monsterX, monsterY);
                        ben.TimeBeforeSpawn = 120;

                        Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);
                        if (benRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            enemiesInMap.Add(ben);
                            enemyNamesAndNumberInMap["Field Goblin"]++;
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
                    if (enemyNamesAndNumberInMap["Crow"] < 3)
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

            enemiesInMap.Clear();
            clearedScarecrows = false;
            spawnEnemies = true;
            enemyNamesAndNumberInMap["Scarecrow"] = 0;
            enemyNamesAndNumberInMap["Crow"] = 0;
            enemyNamesAndNumberInMap["Field Goblin"] = 0;

            if (nestKey.PickedUp == false)
            {
                nest.Finished = false;
                nest.Health = nest.MaxHealth;
            }

           // if (!completedMapQuest)
                //enemiesKilledForQuest[0] = 0;
        }

        public override void Update()
        {
            base.Update();

            PlayBackgroundMusic();

            //CLOUD STUFF
            cloudPos -= .4f;
            cloud2Pos -= .4f;
            fastCloudPos -= .8f;

            if (cloudPos + clouds.Width < 0)
                cloudPos = 1700;

            if (cloud2Pos + clouds2.Width < 0)
                cloud2Pos = 1700;


            if (fastCloudPos + fastClouds.Width < 0)
                fastCloudPos = 1700;


            //Spooky eyes
            eyes1.Update(); eyes2.Update(); eyes3.Update();

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

            if (!nest.Finished)
            {
                sparkles.Update();
            }

            //KEEP CAMERA LOCKED WHEN PLAYER IS AT BOTTOM OF MAP
            if (game.Camera.center.Y > 330)
            {
                game.Camera.center.Y = 331;
            }

            if (enemiesInMap.Count < enemyAmount && spawnEnemies)
            {
                RespawnGroundEnemies();
                RespawnFlyingEnemies(new Rectangle(180, -800, 2200, 1300));
            }

            if (enemiesInMap.Count == 0 && !spawnEnemies)
            {
                completedMapQuest = true;
                mapQuestSigns[0].CompletedQuest = true;
            }

            if (enemiesInMap.Count == enemyAmount)
            {
                spawnEnemies = false;
            }

            if (!clearedScarecrows && enemyNamesAndNumberInMap["Scarecrow"] == 0 && game.ChapterTwo.ChapterTwoBooleans["goblinGateDestroyed"] == true)
            {
                Chapter.effectsManager.AddInGameDialogue("Who's smashing our scarecrows?!", "Mr. Robatto", "Normal", 120);
                clearedScarecrows = true;
            }

            if (clearedScarecrows)
            {
                RespawnGroundEnemies();
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toSpookyField = new Portal(100, platforms[0], "AnotherSpookyField");
            toWorkersField = new Portal(2400, 145, "AnotherSpookyField");
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            //Spooky eyes
            eyes1.Draw(s); eyes2.Draw(s); eyes3.Draw(s);

            if (!nest.Finished)
            {
                sparkles.Draw(s);
            }
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
            //s.Draw(hayBale, new Rectangle(2234, 1491-1440, hayBale.Width, hayBale.Height), Color.White);

            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
    null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(foreground, new Rectangle(0, 1051 - 1440, foreground.Width, foreground.Height), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.05f, this, game));

            s.Draw(sky, new Rectangle(0, mapRec.Y - 100, sky.Width, sky.Height), Color.White);

            if (moonIsAngry)
                s.Draw(moonAngry, new Rectangle(405, mapRec.Y + 590, moonAngry.Width, moonAngry.Height), Color.White * moonFaceAlpha);
            else
                s.Draw(moonHappy, new Rectangle(405, mapRec.Y + 590, moonHappy.Width, moonHappy.Height), Color.White * moonFaceAlpha);


            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransformWithVertParallax(.1f, .95f, this, game));
            s.Draw(clouds, new Rectangle((int)cloudPos, -725, clouds.Width, clouds.Height), Color.White);
            s.Draw(clouds2, new Rectangle((int)cloud2Pos, -725, clouds2.Width, clouds2.Height), Color.White);
            s.Draw(fastClouds, new Rectangle((int)fastCloudPos, -725, fastClouds.Width, fastClouds.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransformWithVertParallax(.1f, .85f, this, game));
            s.Draw(barn, new Rectangle(-100, -140, barn.Width, barn.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransformWithVertParallax(.6f, .92f, this, game));
            s.Draw(backField, new Rectangle(0, 100, backField.Width, backField.Height), Color.White);
            s.End();
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toSpookyField, SpookyField.ToAnotherSpookyField);
            portals.Add(toWorkersField, InBetweenField.ToAnotherSpookyField);
        }
    }
}