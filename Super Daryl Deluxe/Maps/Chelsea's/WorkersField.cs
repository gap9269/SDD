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
    class WorkersField : MapClass
    {
        static Portal toIrrigationCanal;
        static Portal toAnotherSpookyField;
        static Portal toHut;

        public static Portal ToHut { get { return toHut; } }
        public static Portal ToIrrigationCanal { get { return toIrrigationCanal; } }
        public static Portal ToAnotherSpookyField { get { return toAnotherSpookyField; } }
        FieldTroll troll;

        int enemiesSpawned; //use this to cap the length of the gauntlet. Only takes into account crows and scarecrows spawned by RespawnEnemies()

        int crowAmount, goblinAmount, scareAmount;
        GoblinGate gate;

        float cloudPos = 98;
        float cloud2Pos = 1719;
        float fastCloudPos = 530;

        float moonFaceAlpha = 0f;
        int moonFaceTimer;
        bool moonIsAngry = false;
        Random randomMoonTime;
        int timeUntilNextMoonFace;

        public static Texture2D backField, backCave, holeCover, sky, foreground, overlay, caveParallax, gate1, gate2, gate3, gate4, lightRay, trollDoor, clouds, clouds2, fastClouds, moonHappy, moonAngry;
        public static Texture2D rock1, rock2, explosionSheet;
        int timeBetweenSpawn = 160;

        Platform normalGround, groundAfterFall, fallenGate;

        TreasureChest trollChest;

        Platform gateBarrier;

        public static float lightRayAlpha = 1f;

        Boolean playTrollMusic = false;

        public static Texture2D demoEndTexture;

        public WorkersField(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 3300; //200 height increase
            mapWidth = 4000;
            mapName = "Worker's Field";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;
            zoomLevel = .8f;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyAmount = 30;

            enemyNamesAndNumberInMap.Add("Crow", 0);
            enemyNamesAndNumberInMap.Add("Scarecrow", 0);
            enemyNamesAndNumberInMap.Add("Goblin", 0);

            crowAmount = 1;
            scareAmount = 2;

            goblinAmount = 7;

            gate = new GoblinGate(new Vector2(2600, -1160 - 600), "Goblin Gate", game, ref player, this);
            gateBarrier = new Platform(Game1.whiteFilter, new Rectangle(gate.RecX - 30, gate.RecY, 100, 700), false, false, false);
            platforms.Add(gateBarrier);

            normalGround = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(-6, -1140, 4100, 55), false, true, false);
            platforms.Add(normalGround);

            groundAfterFall = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(-6, -1173, 4100, 55), false, true, false);

            trollChest = new TreasureChest(Game1.treasureChestSheet, -2000, 0, player, 0, new GoldKey(), this);
            treasureChests.Add(trollChest);

            fallenGate = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2040, -1180, 700, 50), true, false, false);

            Barrel bar1 = new Barrel(game, 500, 710, Game1.interactiveObjects["Barrel"], true, 3, 5, .3f, true, Barrel.BarrelType.WoodenLeft);
            interactiveObjects.Add(bar1);

            Barrel bar2 = new Barrel(game, 1300, 650, Game1.interactiveObjects["Barrel"], true, 3, 7, 0, false, Barrel.BarrelType.Radioactive);
            interactiveObjects.Add(bar2);

            Barrel bar3 = new Barrel(game, 1700, 710, Game1.interactiveObjects["Barrel"], true, 3, 6, .7f, true, Barrel.BarrelType.MetalLabel);
            interactiveObjects.Add(bar3);

            Barrel bar4 = new Barrel(game, 2600, 650, Game1.interactiveObjects["Barrel"], true, 3, 3, .15f, false, Barrel.BarrelType.MetalBlank);
            interactiveObjects.Add(bar4);

            randomMoonTime = new Random();
        }

        public override void LoadContent()
        {

            //Demo
            demoEndTexture = content.Load<Texture2D>(@"Tutorial\DemoEnd");

            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\WorkersField"));
            backCave = content.Load<Texture2D>(@"Maps\Chelseas\WorkersFieldBackground");
            moonHappy = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldMoonHappy");
            moonAngry = content.Load<Texture2D>(@"Maps\Chelseas\SpookyFieldMoonAngry");
            foreground = content.Load<Texture2D>(@"Maps\Chelseas\WorkersFieldFore");
            overlay = content.Load<Texture2D>(@"Maps\Chelseas\WorkersFieldOverlay");
            backField = content.Load<Texture2D>(@"Maps\Chelseas\WorkersFieldBackField");
            caveParallax = content.Load<Texture2D>(@"Maps\Chelseas\WorkersFieldCaveParallax");
            lightRay = content.Load<Texture2D>(@"Maps\Chelseas\WorkersFieldLight");
            trollDoor = content.Load<Texture2D>(@"Maps\Chelseas\TrollDoor");

            sky = content.Load<Texture2D>(@"Maps\Chelseas\WorkersFieldSky");
            clouds =content.Load<Texture2D>(@"Maps\Chelseas\WorkersFieldClouds");
            clouds2= content.Load<Texture2D>(@"Maps\Chelseas\WorkersFieldClouds2");
            fastClouds = content.Load<Texture2D>(@"Maps\Chelseas\WorkersFieldCloudsFast");

            if (game.ChapterTwo.ChapterTwoBooleans["goblinGateDestroyed"] == false)
            {
                holeCover = content.Load<Texture2D>(@"Maps\Chelseas\WorkersFieldHoleCover");
                gate1 = content.Load<Texture2D>(@"Maps\Chelseas\GoblinGate");
                gate2 = content.Load<Texture2D>(@"Maps\Chelseas\GoblinGate2");
                gate3 = content.Load<Texture2D>(@"Maps\Chelseas\GoblinGate3");
                gate.faceTexture = content.Load<Texture2D>(@"Bosses\GateHudFace");

                rock1 = content.Load<Texture2D>(@"Maps\Chelseas\GroundChunks1");
                rock2 = content.Load<Texture2D>(@"Maps\Chelseas\GroundChunks2");

                explosionSheet = content.Load<Texture2D>(@"Maps\Chelseas\ExplosionSheet");
            }

            gate4 = content.Load<Texture2D>(@"Maps\Chelseas\GoblinGate4");

            /*
                SoundEffect bg = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Hitman");
                SoundEffectInstance backgroundMusic = bg.CreateInstance();
                backgroundMusic.IsLooped = true;
                Sound.music.Add("Before Wall", backgroundMusic);

                SoundEffect bg1 = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Movement Proposition");
                SoundEffectInstance backgroundMusic1 = bg1.CreateInstance();
                backgroundMusic1.IsLooped = true;
                Sound.music.Add("Wall", backgroundMusic1);

                SoundEffect bg2 = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Mistake the Getaway");
                SoundEffectInstance backgroundMusic2 = bg2.CreateInstance();
                backgroundMusic2.IsLooped = true;
                Sound.music.Add("Troll", backgroundMusic2);*/

            Sound.backgroundVolume = 1f;
        }

        public override void PlayBackgroundMusic()
        {
            base.PlayBackgroundMusic();

            /*
            if (game.ChapterTwo.ChapterTwoBooleans["goblinGateDestroyed"] == false && gate.Health == gate.MaxHealth)
            {
                Sound.PlayBackGoundMusic("Before Wall");
            }
            else if (!playTrollMusic)
            {
                Sound.music["Before Wall"].Stop();
                Sound.PlayBackGoundMusic("Wall");
            }
            else
            {
                Sound.music["Wall"].Stop();
                Sound.PlayBackGoundMusic("Troll");
            }*/
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            Sound.UnloadBackgroundMusic();
            gate.faceTexture = Game1.whiteFilter;
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            game.EnemySpriteSheets.Add("Crow", content.Load<Texture2D>(@"EnemySprites\CrowSheet"));
            game.EnemySpriteSheets.Add("Scarecrow", content.Load<Texture2D>(@"EnemySprites\ScarecrowSheet"));
            game.EnemySpriteSheets.Add("Goblin", content.Load<Texture2D>(@"EnemySprites\GoblinSheet"));

            if (game.ChapterTwo.ChapterTwoBooleans["goblinGateDestroyed"] == false)
            {
                game.EnemySpriteSheets.Add("Field Troll", content.Load<Texture2D>(@"EnemySprites\TrollSprite"));
                game.EnemySpriteSheets.Add("TrollFall", content.Load<Texture2D>(@"EnemySprites\TrollFallSprite"));
                game.EnemySpriteSheets.Add("TrollAttack", content.Load<Texture2D>(@"EnemySprites\TrollAttackSprite"));
                game.EnemySpriteSheets.Add("TrollClubGone", content.Load<Texture2D>(@"EnemySprites\TrollClubDisappearSprite"));
                Chapter.LoadTrollTexturesByDrawing = true;
            }
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            switch (game.chapterState)
            {

                case Game1.ChapterState.chapterTwo:
                    if (enemyNamesAndNumberInMap["Scarecrow"] < scareAmount)
                    {
                        ScarecrowEnemy ben = new ScarecrowEnemy(pos, "Scarecrow", game, ref player, this, true, new Rectangle(100, -1800, 2500, 500));
                            ben.Hostile = true;

                        monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;
                        ben.Position = new Vector2(monsterX, monsterY);

                        if (ben.PositionX > 1900)
                        {
                            ben.PositionX = 1900;
                        }

                        if (ben.PositionX < 800)
                        {
                            ben.PositionX = 800;
                        }

                        Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                        ben.TimeBeforeSpawn = timeBetweenSpawn;
                        if (benRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            enemiesInMap.Add(ben);
                            enemyNamesAndNumberInMap["Scarecrow"]++;
                            enemiesSpawned++;
                        }
                    }

                    if (enemyNamesAndNumberInMap["Goblin"] < goblinAmount)
                    {
                        Goblin ben = new Goblin(pos, "Goblin", game, ref player, this);
                        //ben.Hostile = true;

                        monsterY = 200;
                        ben.Position = new Vector2(monsterX, 200);

                        Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                        ben.TimeBeforeSpawn = timeBetweenSpawn;
                        if (benRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            enemiesInMap.Add(ben);
                            enemyNamesAndNumberInMap["Goblin"]++;
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
                    if (enemyNamesAndNumberInMap["Crow"] < crowAmount)
                    {
                        Crow en = new Crow(pos, "Crow", game, ref player, this, mapRec);
                        en.Hostile = true;

                        en.TimeBeforeSpawn = timeBetweenSpawn;

                        Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                        if (testRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            enemiesInMap.Add(en);
                            enemyNamesAndNumberInMap["Crow"]++;
                            enemiesSpawned++;
                        }
                    }
                    break;
            }

        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            enemiesInMap.Clear();
            spawnEnemies = true;
            enemyNamesAndNumberInMap["Scarecrow"] = 0;
            enemyNamesAndNumberInMap["Crow"] = 0;
            enemyNamesAndNumberInMap["Goblin"] = 0;
            enemiesSpawned = 0;

            if(troll != null)
                troll.ClearGroundHoles();
        }

        public override void Update()
        {
            base.Update();

            PlayBackgroundMusic();

            if (troll != null && troll.HoriztonalDistanceToPlayer < 500 && !playTrollMusic && game.ChapterTwo.ChapterTwoBooleans["trollAdded"])
                playTrollMusic = true;

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

            //Locks the camera at the top of the map if you're on the upper level
            if (player.VitalRecY < -1250 && game.Camera.center.Y > -1449)
            {
                game.Camera.center.Y = -1450;
            }

            //KEEP CAMERA LOCKED WHEN PLAYER IS AT BOTTOM OF MAP
            if (game.Camera.center.Y > 365)
            {
                game.Camera.center.Y = 366;
            }

            //If the gate health is below half, move the barrier back a bit because the gate tilts toward the player
            if (gate.Health < gate.MaxHealth / 2 && !game.MapBooleans.chapterTwoMapBooleans["gateHitHalfHealth"])
            {
                //Moves the player back because the gate tilts forward
                if (player.VitalRecX + player.VitalRecWidth > 2510)
                {
                    player.PositionX -= 60;
                }

                //Sets the gate position
                gateBarrier.RecX = 2510;
                gate.RecX = 2540;
                gate.PositionX = 2540;
                game.MapBooleans.chapterTwoMapBooleans["gateHitHalfHealth"] = true;
                game.Camera.ShakeCamera(10, 15);
            }

            //Allows the player to jump again after the fall
            if (player.VitalRecY > 0 && player.CanJump == false && player.CurrentPlat != null)
                player.CanJump = true;

            //If the gate hasn't been destroyed start the boss fight and make the troll, but don't add him to the map yet
            if (game.ChapterTwo.ChapterTwoBooleans["goblinGateDestroyed"] == false && game.CurrentChapter.CurrentBoss == null)
            {
                game.CurrentChapter.CurrentBoss = gate;
                game.CurrentChapter.BossFight = true;
                troll = new FieldTroll(new Vector2(3060, 50), "Field Troll", game, ref player, this, trollChest);
                troll.Hostile = true;
            }

            //If the gate has been hit but hasn't been destroyed, endlessly spawn enemies
            if (gate.Health < gate.MaxHealth && game.ChapterTwo.ChapterTwoBooleans["goblinGateDestroyed"] == false && gate.HasBeenHit)
            {
                //Make the moon look angry for the entire gauntlet
                moonIsAngry = true;
                moonFaceTimer =  10;

                if (enemiesSpawned < 150)
                {
                    RespawnFlyingEnemies(new Rectangle(100, -1800, 2500, 500));
                    RespawnGroundEnemies();
                }

                //Increases the enemy amount based on how much health the gate has missing
                if (gate.Health < (gate.MaxHealth * .20f))
                {
                    crowAmount = 4;
                    scareAmount = 4;
                    timeBetweenSpawn = 300;
                }
                else if (gate.Health < (gate.MaxHealth * .40f))
                {
                    crowAmount = 3;
                    scareAmount = 4;
                    timeBetweenSpawn = 250;
                }
                else if (gate.Health < (gate.MaxHealth * .75f))
                {
                    crowAmount = 2;
                    scareAmount = 3;
                    timeBetweenSpawn = 200;
                }
            }

            //If you've opened the troll's chest replace the platforms on the upper level
            if (trollChest.Empty && !platforms.Contains(fallenGate))
            {
                platforms.Add(fallenGate);
                platforms.Add(groundAfterFall);
            }

            //If the troll hasn't been added to the map but the gate has been destroyed, add the troll to the map. Only fires once the 
            //player is on the left side of the map
            if (!enemiesInMap.Contains(troll) && game.ChapterTwo.ChapterTwoBooleans["goblinGateDestroyed"] && player.VitalRecX < 1200 && !game.ChapterTwo.ChapterTwoBooleans["trollAdded"])
            {
                game.ChapterTwo.ChapterTwoBooleans["trollAdded"] = true;
                enemiesInMap.Add(troll);
            }

            //For loaded files, if the gate is destroyed, remove and add necessary platforms
            if (game.ChapterTwo.ChapterTwoBooleans["goblinGateDestroyed"] == true && platforms.Contains(normalGround) && game.CurrentChapter.BossFight == false)
            {
                platforms.Remove(normalGround);
                platforms.Remove(gateBarrier);
                platforms.Add(fallenGate);
                platforms.Add(groundAfterFall);
            }

            //If the gate has been destroyed, start the cutscene and remove the ground
            if (gate.Health <= 0 && player.CurrentPlat != null && player.VitalRecX > 2200 && game.ChapterTwo.ChapterTwoBooleans["GateCollapseScenePlayed"] == false)
            {
                platforms.Remove(normalGround);
                game.ChapterTwo.ChapterTwoBooleans["GateCollapseScenePlayed"] = true;
                game.ChapterTwo.ChapterTwoBooleans["goblinGateDestroyed"] = true;
                lightRayAlpha = 0f;
                game.ChapterTwo.state = Chapter.GameState.Cutscene;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toAnotherSpookyField = new Portal(100, -1660, "Worker'sField");
            toIrrigationCanal = new Portal(3000, -1160, "Worker'sField");
            toHut = new Portal(3465, platforms[0], "Worker'sField", "Gold Key");
        }

        public override void DrawMapOverlay(SpriteBatch s)
        {
            base.DrawMapOverlay(s);

            s.Draw(overlay, new Rectangle(0, -860, overlay.Width, overlay.Height), Color.White);
            s.Draw(lightRay, new Rectangle(0, -860, overlay.Width, overlay.Height), Color.White * lightRayAlpha);
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

            s.Draw(foreground, new Rectangle(0, mapRec.Y, foreground.Width, foreground.Height), Color.White);
            s.End();

        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if(toHut.ItemNameToUnlock != "" && toHut.ItemNameToUnlock != null)
                s.Draw(trollDoor, new Rectangle(3435, -mapHeight + 720 + 360 + 2407 + 200, trollDoor.Width, trollDoor.Height), Color.White);

            if (toHut.ItemNameToUnlock != null && toHut.ItemNameToUnlock != "")
                toHut.DrawLock(s);

            if (game.CurrentChapter.BossFight)
            {
                if (gate.Health >= gate.MaxHealth / 2)
                {
                    s.Draw(holeCover, new Rectangle(1729, 686 - 2500, holeCover.Width, holeCover.Height), Color.White);
                    s.Draw(gate1, new Rectangle(1663, -mapHeight + 720 + 360 + 72 + 200, gate1.Width, gate1.Height), Color.White);
                }

                else if (gate.Health < gate.MaxHealth / 2 && gate.Health > 0)
                {
                    s.Draw(holeCover, new Rectangle(1729, 686 - 2500, holeCover.Width, holeCover.Height), Color.White);
                    s.Draw(gate2, new Rectangle(1663, -mapHeight + 720 + 360 + 72 + 200, gate1.Width, gate1.Height), Color.White);
                }

                else if (gate.Health == 0)
                {
                    if (game.MapBooleans.chapterTwoMapBooleans["explosionHitFrame10"])
                        s.Draw(gate3, new Rectangle(1663, -mapHeight + 720 + 360 + 72 + 200, gate1.Width, gate1.Height), Color.White);
                    else
                    {
                        s.Draw(holeCover, new Rectangle(1729, 686 - 2500, holeCover.Width, holeCover.Height), Color.White);
                        s.Draw(gate2, new Rectangle(1663, -mapHeight + 720 + 360 + 72 + 200, gate1.Width, gate1.Height), Color.White);
                    }
                }
            }
            else
            {
                s.Draw(gate4, new Rectangle(1663, -mapHeight + 720 + 360 + 72 + 200, gate1.Width, gate1.Height), Color.White);
            }
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.05f, this, game));

            s.Draw(sky, new Rectangle(0, mapRec.Y, sky.Width, sky.Height), Color.White);

            if (moonIsAngry)
                s.Draw(moonAngry, new Rectangle(-5, mapRec.Y + 245, moonAngry.Width, moonAngry.Height), Color.White * moonFaceAlpha);
            else
                s.Draw(moonHappy, new Rectangle(-5, mapRec.Y + 245, moonHappy.Width, moonHappy.Height), Color.White * moonFaceAlpha);


            s.End();


            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(backCave, mapRec, Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.6f, this, game));
            s.Draw(backField, new Rectangle(0, -1680, backField.Width, backField.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.95f, this, game));
            s.Draw(caveParallax, new Rectangle(900, 2096 - 2010, caveParallax.Width, caveParallax.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransformWithVertParallax(.1f, .95f, this, game));
            s.Draw(clouds, new Rectangle((int)cloudPos, mapRec.Y + 245, clouds.Width, clouds.Height), Color.White);
            s.Draw(clouds2, new Rectangle((int)cloud2Pos, mapRec.Y + 245, clouds2.Width, clouds2.Height), Color.White);
            s.Draw(fastClouds, new Rectangle((int)fastCloudPos, mapRec.Y + 245, fastClouds.Width, fastClouds.Height), Color.White);
            s.End();


        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toAnotherSpookyField, InBetweenField.ToWorkers);
            portals.Add(toIrrigationCanal, IrrigationCanal.ToWorkersField);
            portals.Add(toHut, TrollsHut.ToWorkersField);
        }
    }
}