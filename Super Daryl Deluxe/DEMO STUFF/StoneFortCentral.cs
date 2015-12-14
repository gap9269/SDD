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
    public class StoneFortCentralDemo : MapClass
    {
        static Portal toOutsideCamp;
        static Portal toFortWest;
        static Portal toFortEast;

        TrojanHorse horse;

        public static Portal ToFortEast { get { return toFortEast; } }
        public static Portal ToFortWest { get { return toFortWest; } }
        public static Portal ToOutsideCamp { get { return toOutsideCamp; } }

        Goblin commander;

        GoblinHut hut, hut1;
        AnubisHut hut2;
        int goblinAmount;
        int timeBeforeSpawn = 160;

        Texture2D foreground, foreground2, sky, doorParallax, wallParallax, leftForeParallax, rightForeParallax, hutSprite, shrineSprite, destroyedForeground;

        public static Dictionary<String, Texture2D> fireTextures;
        public static Dictionary<String, Texture2D> soldierAnimations;

        struct TorchFire
        {
            public int frame;
            public int frameDelay;
        }

        List<TorchFire> torches;


        public StoneFortCentralDemo(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapWidth = 5000;
            mapHeight = 1160;
            mapName = "Stone Fort - Central";

            //backgroundMusicName = "Noir Halls";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 8;

            yScroll = true;

            interactiveObjects.Add(new MapAnimation(game, 3613, 270, true, MapAnimation.AnimationType.FrenchCorpse, false));

            hut = new GoblinHut(game, 483, 700, Game1.whiteFilter, 10, 5, true);
            interactiveObjects.Add(hut);

            hut1 = new GoblinHut(game, 3130, 660, Game1.whiteFilter, 10, 5, true);
            interactiveObjects.Add(hut1);

            hut2 = new AnubisHut(game, 3700, 765, Game1.whiteFilter, 10, 5, false);
            interactiveObjects.Add(hut2);

            interactiveObjects.Add(new MapAnimation(game, 1024, 436, true, MapAnimation.AnimationType.fencing, false));
            interactiveObjects.Add(new MapAnimation(game, 2396, 350, false, MapAnimation.AnimationType.shoot, false));
            interactiveObjects.Add(new MapAnimation(game, 3417, 427, false, MapAnimation.AnimationType.RomanCorpse, true));
            interactiveObjects.Add(new MapAnimation(game, 2268, 330, true, MapAnimation.AnimationType.RomanCorpseGoblin, false));
            interactiveObjects.Add(new MapAnimation(game, 1592, 415, false, MapAnimation.AnimationType.FrenchCorpse, true));

            interactiveObjects.Add(new MapAnimation(game, 4518, 340, false, MapAnimation.AnimationType.strangle, false));
            interactiveObjects.Add(new MapAnimation(game, 2957, 276, true, MapAnimation.AnimationType.EgyptianCorpse, false));
            interactiveObjects.Add(new MapAnimation(game, 3940, 480, true, MapAnimation.AnimationType.boxing, true));

            #region Torches
            torches = new List<TorchFire>();
            TorchFire torch1 = new TorchFire();
            TorchFire torch2 = new TorchFire();
            TorchFire torch3 = new TorchFire();
            TorchFire torch4 = new TorchFire();

            torch1.frameDelay = 3;
            torch2.frameDelay = 5;
            torch3.frameDelay = 2;
            torch4.frameDelay = 4;


            torch2.frame = 1;
            torch3.frame = 2;

            torches.Add(torch1);
            torches.Add(torch2);
            torches.Add(torch3);
            torches.Add(torch4);
            #endregion

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Bomblin", 0);
            enemyNamesAndNumberInMap.Add("Goblin", 0);
            enemyNamesAndNumberInMap.Add("Nurse Goblin", 0);
            enemyNamesAndNumberInMap.Add("Anubis Warrior", 0);

            currentBackgroundMusic = Sound.MusicNames.FortBattle;

        }

        public override void LoadContent()
        {
            Sound.LoadFortZoneSounds();
            if (game.ChapterTwoDemo.ChapterTwoBooleans["centralCommanderSpawned"])
            {
                background.Add(content.Load<Texture2D>(@"Maps\History\CentralFort\destroyedBackground"));
                destroyedForeground = content.Load<Texture2D>(@"Maps\History\CentralFort\foregroundDestroyed");
            }
            else
                background.Add(content.Load<Texture2D>(@"Maps\History\CentralFort\background"));
            background.Add(content.Load<Texture2D>(@"Maps\History\CentralFort\background2"));
            foreground = content.Load<Texture2D>(@"Maps\History\CentralFort\foreground");
            foreground2 = content.Load<Texture2D>(@"Maps\History\CentralFort\foreground2");

            sky = content.Load<Texture2D>(@"Maps\History\CentralFort\sky");
            doorParallax = content.Load<Texture2D>(@"Maps\History\CentralFort\backgroundDoorParallax");
            wallParallax = content.Load<Texture2D>(@"Maps\History\CentralFort\backgroundWallParallax");
            leftForeParallax = content.Load<Texture2D>(@"Maps\History\CentralFort\leftForegroundParallax");
            rightForeParallax = content.Load<Texture2D>(@"Maps\History\CentralFort\rightForegroundParallax");

            if (game.ChapterTwoDemo.ChapterTwoBooleans["horseInCentral"])
            {
                horse = new TrojanHorse(2000, -70, player, this);
                horse.hasLocker = true;
                horse.faceTexture = content.Load<Texture2D>(@"Bosses\TrojanHorseFace");
                horse.LoadContent(content, true, false);
            }

            fireTextures = ContentLoader.LoadContent(content, "Maps\\History\\CentralFort\\Fire");
            StoneFortCentral.soldierAnimations = ContentLoader.LoadContent(content, "Maps\\History\\SoldierAnimations");

            hutSprite = content.Load<Texture2D>(@"Maps\History\OutsideFort\GoblinHut");
            shrineSprite = content.Load<Texture2D>(@"Maps\History\OutsideFort\AnubisHut");
            hut.Sprite = hutSprite;
            hut1.Sprite = hutSprite;
            hut2.Sprite = shrineSprite;

            Game1.npcFaces["Napoleon"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\NapoleonNormal");

            //if (Chapter.lastMap != "East Hall")
            //{
            //    SoundEffect bg = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Music\Noir Halls");
            //    SoundEffectInstance backgroundMusic = bg.CreateInstance();
            //    backgroundMusic.IsLooped = true;
            //    Sound.music.Add("North Hall", backgroundMusic);
            //}

            if (Chapter.lastMap != "Stone Fort - East" && Chapter.lastMap != "Stone Fort - West")
            {
                SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_fort_battle");
                SoundEffectInstance amb = am.CreateInstance();
                amb.IsLooped = true;
                Sound.ambience.Add("ambience_fort_battle", amb);
            }
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            Game1.npcFaces["Napoleon"].faces["Normal"] = Game1.whiteFilter;

            if (Chapter.theNextMap != "Stone Fort - East" && Chapter.theNextMap != "Stone Fort - West")
            {
                Sound.UnloadAmbience();
            }
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.BomblinEnemy(content);
            EnemyContentLoader.GoblinEnemy(content);
            EnemyContentLoader.SoldierGoblinEnemy(content);
            EnemyContentLoader.SharedGoblinSounds(content);
            EnemyContentLoader.AnubisWarriorEnemy(content);
            EnemyContentLoader.SharedAnubisSounds(content);
            EnemyContentLoader.NurseGoblinEnemy(content);

        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            switch (game.chapterState)
            {

                case Game1.ChapterState.demo:

                    int randomEnemy = rand.Next(3);

                    if (randomEnemy == 0)
                    {
                        if (Game1.randomNumberGen.Next(3) > 1)
                        {
                            GoblinDemo ben = new GoblinDemo(pos, "Goblin Soldier", game, ref player, this);
                            ben.Hostile = true;

                            monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;
                            ben.Position = new Vector2(monsterX, monsterY);

                            Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                            ben.TimeBeforeSpawn = timeBeforeSpawn;

                            if (benRec.Intersects(player.Rec))
                            {
                            }
                            else
                            {
                                AddEnemyToEnemyList(ben);
                                enemyNamesAndNumberInMap["Goblin"]++;
                            }
                        }
                        else
                        {
                            NurseGoblin ben = new NurseGoblin(pos, "Nurse Goblin", game, ref player, this);
                            ben.Hostile = true;

                            monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;
                            ben.Position = new Vector2(monsterX, monsterY);

                            Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                            ben.TimeBeforeSpawn = timeBeforeSpawn;

                            if (benRec.Intersects(player.Rec))
                            {
                            }
                            else
                            {
                                AddEnemyToEnemyList(ben);
                                enemyNamesAndNumberInMap["Nurse Goblin"]++;
                            }
                        }
                    }
                    else if (randomEnemy == 1)
                    {
                        BomblinDemo ben = new BomblinDemo(pos, "Bomblin", game, ref player, this);
                        ben.Hostile = true;

                        monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;
                        ben.Position = new Vector2(monsterX, monsterY);

                        Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                        ben.TimeBeforeSpawn = timeBeforeSpawn;

                        if (benRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            AddEnemyToEnemyList(ben);
                            enemyNamesAndNumberInMap["Bomblin"]++;
                        }
                    }
                    else if (randomEnemy == 2)
                    {

                        AnubisWarriorDemo ben = new AnubisWarriorDemo(pos, "Anubis Warrior", game, ref player, this, new Rectangle(200, 300, mapWidth - 400, 500));
                        ben.Hostile = true;

                        monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;
                        ben.Position = new Vector2(monsterX, monsterY);

                        Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                        ben.TimeBeforeSpawn = timeBeforeSpawn;

                        if (benRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            AddEnemyToEnemyList(ben);
                            enemyNamesAndNumberInMap["Anubis Warrior"]++;
                        }
                    }

                    break;
            }
        }

        public void RespawnInitialGroundEnemies()
        {
            base.RespawnGroundEnemies();

            switch (game.chapterState)
            {

                case Game1.ChapterState.demo:
                    if (enemyNamesAndNumberInMap["Goblin"] < 2)
                    {
                        GoblinDemo ben = new GoblinDemo(pos, "Goblin", game, ref player, this);
                        ben.Hostile = false;

                        monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;
                        ben.Position = new Vector2(monsterX, monsterY);

                        Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                        ben.TimeBeforeSpawn = 0;
                        ben.SpawnWithPoof = false;

                        if (benRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            AddEnemyToEnemyList(ben);
                            enemyNamesAndNumberInMap["Goblin"]++;
                        }
                    }

                    break;
            }
        }

        public override void PlayBackgroundMusic()
        {
            Sound.PlayBackGroundMusic(currentBackgroundMusic.ToString());
        }

        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_fort_battle");
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            Chapter.effectsManager.ClearDialogue();
        }

        public override void Update()
        {
            base.Update();

            PlayAmbience();

            if (!game.ChapterTwoDemo.ChapterTwoBooleans["justEnteredFort"] && !hut1.Finished && !hut.Finished && !hut2.Finished)
            {
                game.ChapterTwoDemo.ChapterTwoBooleans["justEnteredFort"] = true;
                game.ChapterTwoDemo.fortRaid.SpecialConditions.Clear();
                game.ChapterTwoDemo.fortRaid.SpecialConditions.Add("Clear the map of enemies and buildings", false);

                //game.ChapterTwoDemo.NPCs["Napoleon"].QuestDialogue[game.ChapterTwoDemo.NPCs["Napoleon"].DialogueState] = "The enemy does not know we're here yet. Quickly, clear this area of troops to secure our position!";
                Chapter.effectsManager.NotificationQueue.Enqueue(new QuestUpdatedNotification(true));
                //game.ChapterTwoDemo.NPCs["Napoleon"].Talking = true;
               // game.CurrentChapter.TalkingToNPC = true;
                //player.Sprinting = false;
            }

            #region Torches
            for (int i = 0; i < torches.Count; i++)
            {

                TorchFire temp = torches[i];

                temp.frameDelay--;

                if (temp.frameDelay <= 0)
                {
                    temp.frameDelay = 5;
                    temp.frame++;

                    if (temp.frame >= 3)
                        temp.frame = 0;
                }

                torches[i] = temp;
            }
            #endregion

            if(game.ChapterTwoDemo.ChapterTwoBooleans["westTrollKilled"] && !game.ChapterTwoDemo.ChapterTwoBooleans["bombExploded"])
            {
                game.CurrentChapter.state = Chapter.GameState.Cutscene;
                game.ChapterTwoDemo.ChapterTwoBooleans["bombExploded"] = true;
                game.ChapterTwoDemo.fortRaid.SpecialConditions.Clear();
                game.ChapterTwoDemo.fortRaid.SpecialConditions.Add("Investigate the aftermath", false);

            }

            if (game.ChapterTwoDemo.ChapterTwoBooleans["bombExploded"])
            {
                toFortWest.IsUseable = true;
            }
            //Spawn initial enemies
            if(spawnEnemies)
            {
                RespawnInitialGroundEnemies();

                if (enemiesInMap.Count == 2)
                    spawnEnemies = false;
            }

            if (game.ChapterTwoDemo.ChapterTwoBooleans["enemyReinforcementsSpawning"] && enemiesInMap.Count < enemyAmount && !game.ChapterTwoDemo.ChapterTwoBooleans["centralCommanderSpawned"])
            {
                RespawnGroundEnemies();
            }

            if (game.ChapterTwoDemo.ChapterTwoBooleans["centralCommanderSpawned"])
            {
                toOutsideCamp.IsUseable = false;

                if (!game.ChapterTwoDemo.ChapterTwoBooleans["centralCommanderKilled"] && commander == null)
                {
                    enemiesInMap.Clear();

                    commander = new Goblin(new Vector2(2200, 630 - 154), "Goblin Soldier", game, ref player, this);
                    commander.Hostile = true;
                    commander.SpawnWithPoof = false;
                    AddEnemyToEnemyList(commander);

                    AnubisWarrior ben = new AnubisWarrior(new Vector2(1780, 630 - 396 * 1.1f), "Anubis Warrior", game, ref player, this, new Rectangle(500, 300, mapRec.Width - 600, 500));
                    ben.Hostile = true;
                    ben.FacingRight = false;
                    ben.SpawnWithPoof = false;
                    AddEnemyToEnemyList(ben);

                    AnubisWarrior ben5 = new AnubisWarrior(new Vector2(2300, 630 - 396 * 1.1f), "Anubis Warrior", game, ref player, this, new Rectangle(500, 300, mapRec.Width - 600, 500));
                    ben5.Hostile = true;
                    ben5.FacingRight = false;
                    ben5.SpawnWithPoof = false;
                    AddEnemyToEnemyList(ben5);

                    Bomblin ben1 = new Bomblin(new Vector2(1800, 630 - 371 * 1.1f), "Bomblin", game, ref player, this);
                    ben1.Hostile = true;
                    ben1.SpawnWithPoof = false;
                    AddEnemyToEnemyList(ben1);

                    Goblin ben2 = new Goblin(new Vector2(1900, 630 - 154), "Goblin Soldier", game, ref player, this);
                    ben2.Hostile = true;
                    ben2.SpawnWithPoof = false;
                    AddEnemyToEnemyList(ben2);

                    Goblin ben3 = new Goblin(new Vector2(2100, 630 - 154), "Goblin Soldier", game, ref player, this);
                    ben3.Hostile = true;
                    ben3.SpawnWithPoof = false;
                    AddEnemyToEnemyList(ben3);

                    Bomblin ben4 = new Bomblin(new Vector2(2200, 630 - 371 * 1.1f), "Bomblin", game, ref player, this);
                    ben4.Hostile = true;
                    ben4.SpawnWithPoof = false;
                    AddEnemyToEnemyList(ben4);
                }

                if (commander != null && enemiesInMap.Count == 0 && !game.ChapterTwoDemo.ChapterTwoBooleans["centralCommanderKilled"])
                {
                    game.ChapterTwoDemo.ChapterTwoBooleans["centralCommanderKilled"] = true;
                    game.ChapterTwoDemo.BossFight = true;
                    game.ChapterTwoDemo.CurrentBoss = horse;

                    Chapter.effectsManager.NotificationQueue.Enqueue(new QuestUpdatedNotification(true));

                    Chapter.effectsManager.AddInGameDialogue("Protect ze horse!", "Napoleon", "Normal", 400);

                    game.ChapterTwoDemo.fortRaid.SpecialConditions.Clear();
                    game.ChapterTwoDemo.fortRaid.SpecialConditions.Add("Protect ze horse", false);

                }
            }

            if (game.ChapterTwoDemo.ChapterTwoBooleans["centralCommanderKilled"] && game.ChapterTwoDemo.ChapterTwoBooleans["horseInCentral"])
            {
                game.ChapterTwoDemo.BossFight = true;
                game.ChapterTwoDemo.CurrentBoss = horse;
                horse.Move(3f);

                if (toFortEast.IsUseable)
                {
                    toFortEast.IsUseable = false;
                    toFortWest.IsUseable = false;
                    toOutsideCamp.IsUseable = false;
                }

                if (horse.RecX > mapWidth)
                {
                    horse.StopSound();
                    game.ChapterTwoDemo.ChapterTwoBooleans["horseInCentral"] = false;
                    game.ChapterTwoDemo.ChapterTwoBooleans["horseInEast"] = true;

                    StoneFortEastDemo.horse.health = horse.health;

                    toFortEast.IsUseable = true;
                    horse = null;
                    game.ChapterTwoDemo.BossFight = false;
                    game.ChapterTwoDemo.CurrentBoss = null;
                }
            }

            else if (horse != null && game.ChapterTwoDemo.ChapterTwoBooleans["horseInCentral"])
                horse.Update(mapWidth);
                

            if (hut.Finished && hut1.Finished && hut2.Finished && enemiesInMap.Count == 0 && game.ChapterTwoDemo.ChapterTwoBooleans["clearedCentralFortFirstTime"] == false)
            {
                game.ChapterTwoDemo.ChapterTwoBooleans["clearedCentralFortFirstTime"] = true;
                Chapter.effectsManager.NotificationQueue.Enqueue(new QuestUpdatedNotification(true));
                //Chapter.effectsManager.AddInGameDialogue("Well done. Now work your way through the rest of the camp and destroy the rest of their huts and pyramids.", "Napoleon", "Normal", 400);
                game.ChapterTwoDemo.fortRaid.SpecialConditions.Clear();
                game.ChapterTwoDemo.fortRaid.SpecialConditions.Add("Clear the entire camp of huts and \npyramids", false);
            }

            if (toFortEast.IsUseable == false && game.ChapterTwoDemo.ChapterTwoBooleans["clearedCentralFortFirstTime"] && !game.ChapterTwoDemo.ChapterTwoBooleans["centralCommanderKilled"])
            {
                ToFortEast.IsUseable = true;
                ToFortWest.IsUseable = true;
            }
             PlayBackgroundMusic();
            // PlayAmbience();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toOutsideCamp = new Portal(1600, platforms[0], "Stone Fort - Central");
            toOutsideCamp.FButtonYOffset = -60;
            toOutsideCamp.PortalNameYOffset = -60;

            toFortWest = new Portal(50, platforms[0], "Stone Fort - Central");
            toFortWest.FButtonYOffset = -60;
            toFortWest.PortalNameYOffset = -60;

            toFortEast = new Portal(4800, platforms[0], "Stone Fort - Central");
            toFortEast.FButtonYOffset = -60;
            toFortEast.PortalNameYOffset = -60;

            toFortEast.IsUseable = false;
            toFortWest.IsUseable = false;

            //toBathroom = new Portal(500, platforms[0], "MainLobby");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toOutsideCamp, OutsideStoneFortDemo.ToCampMiddle);
            portals.Add(toFortWest, StoneFortWestDemo.ToCentral);
            portals.Add(toFortEast, StoneFortEastDemo.ToCentral);

        }

        public override void AddNPCs()
        {
            base.AddNPCs();
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            s.Draw(fireTextures.ElementAt(torches[0].frame).Value, new Vector2(143, mapRec.Y + 425), Color.White);
            s.Draw(fireTextures.ElementAt(torches[1].frame).Value, new Vector2(1471, mapRec.Y + 444), Color.White);
            s.Draw(fireTextures.ElementAt(torches[2].frame).Value, new Vector2(2084, mapRec.Y + 410), Color.White);
            s.Draw(fireTextures.ElementAt(torches[3].frame).Value, new Vector2(4311, mapRec.Y + 367), Color.White);

            if (game.ChapterTwoDemo.ChapterTwoBooleans["horseInCentral"] && horse != null)
                horse.DrawHorse(s);
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

            if (destroyedForeground != null)
            {
                s.Draw(destroyedForeground, new Vector2(0, mapRec.Y + 20), Color.White);
            }
            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1.23f, this, game));
            s.Draw(leftForeParallax, new Vector2(0, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1.23f, this, game));
            s.Draw(rightForeParallax, new Vector2(mapWidth + 150, mapRec.Y), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Rectangle(0, mapRec.Y, mapWidth, 800), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.9f, this, game));
            s.Draw(doorParallax, new Vector2(-200, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.85f, this, game));
            s.Draw(wallParallax, new Vector2(mapWidth - wallParallax.Width - 300, mapRec.Y), Color.White);
            s.End();
        }
    }
}
