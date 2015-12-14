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
    public class StoneFortEastDemo : MapClass
    {
        static Portal toCentral;
        static Portal toWest;

        public static Portal ToCentral { get { return toCentral; } }
        public static Portal ToWest { get { return toWest; } }

        GoblinHut hut, hut1, hut4, hut5;
        AnubisHut hut2, hut3;
        int goblinAmount;
        int timeBeforeSpawn = 160;

        CommanderGoblin commander;

        Texture2D foreground, foreground2, sky, farParallax, medParallax1, medParallax2, leftForeParallax, closeParallax, hutSprite, shrineSprite;

        public static Dictionary<String, Texture2D> fireTextures;

        public static TrojanHorse horse;

        struct TorchFire
        {
            public int frame;
            public int frameDelay;
        }

        List<TorchFire> torches;


        public StoneFortEastDemo(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapWidth = 8000;
            mapHeight = 1160;
            mapName = "Stone Fort - East";

            //backgroundMusicName = "Noir Halls";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 10;

            yScroll = false;

            horse = new TrojanHorse(0, -105, player, this);
            horse.hasLocker = true;

            interactiveObjects.Add(new MapAnimation(game, 2570, 360, false, MapAnimation.AnimationType.boxing, false));
            interactiveObjects.Add(new MapAnimation(game, 6443, 294, false, MapAnimation.AnimationType.BurntBomblin, false));

            hut = new GoblinHut(game, -95, 680, Game1.whiteFilter, 10, 5, false);
            interactiveObjects.Add(hut);

            hut1 = new GoblinHut(game, 1121, 660, Game1.whiteFilter, 10, 5, true);
            interactiveObjects.Add(hut1);

            hut2 = new AnubisHut(game, 2746, 785, Game1.whiteFilter, 10, 5, false);
            interactiveObjects.Add(hut2);

            hut3 = new AnubisHut(game, 3450, 755, Game1.whiteFilter, 10, 5, true);
            interactiveObjects.Add(hut3);

            hut4 = new GoblinHut(game, 5195, 630, Game1.whiteFilter, 10, 5, false);
            interactiveObjects.Add(hut4);

            hut5 = new GoblinHut(game, 6302, 630, Game1.whiteFilter, 10, 5, true);
            interactiveObjects.Add(hut5);

            interactiveObjects.Add(new MapAnimation(game, 569, 490, true, MapAnimation.AnimationType.strangle, true));
            interactiveObjects.Add(new MapAnimation(game, 1764, 421, false, MapAnimation.AnimationType.EgyptianCorpse, true));
            interactiveObjects.Add(new MapAnimation(game, 2258, 480, true, MapAnimation.AnimationType.shoot, true));
            interactiveObjects.Add(new MapAnimation(game, 3770, 417, false, MapAnimation.AnimationType.RomanCorpse, true));
            interactiveObjects.Add(new MapAnimation(game, 4743, 407, false, MapAnimation.AnimationType.FrenchCorpse, true));
            interactiveObjects.Add(new MapAnimation(game, 4920, 390, true, MapAnimation.AnimationType.fencing, false));
            interactiveObjects.Add(new MapAnimation(game, 5260, 400, false, MapAnimation.AnimationType.RomanCorpse, true));
            interactiveObjects.Add(new MapAnimation(game, 6187, 410, true, MapAnimation.AnimationType.EgyptianCorpseFish, true));
            interactiveObjects.Add(new MapAnimation(game, 6838, 320, false, MapAnimation.AnimationType.RomanCorpseGoblin, false));
            interactiveObjects.Add(new MapAnimation(game, 7223, 300, true, MapAnimation.AnimationType.MongolCorpse, false));

            hut.RecY = 73;

            #region Torches
            torches = new List<TorchFire>();
            TorchFire torch1 = new TorchFire();
            TorchFire torch2 = new TorchFire();
            TorchFire torch3 = new TorchFire();
            TorchFire torch4 = new TorchFire();
            TorchFire torch5 = new TorchFire();
            TorchFire torch6 = new TorchFire();

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
            torches.Add(torch5);
            torches.Add(torch6);
            #endregion

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Goblin", 0);
            enemyNamesAndNumberInMap.Add("Goblin Soldier", 0);
            enemyNamesAndNumberInMap.Add("Bomblin", 0);
            enemyNamesAndNumberInMap.Add("Anubis Warrior", 0);
            enemyNamesAndNumberInMap.Add("Nurse Goblin", 0);

            currentBackgroundMusic = Sound.MusicNames.FortBattle;

        }

        public override void LoadContent()
        {
            Sound.LoadFortZoneSounds();

            background.Add(content.Load<Texture2D>(@"Maps\History\EastFort\background"));
            background.Add(content.Load<Texture2D>(@"Maps\History\EastFort\background2"));
            foreground = content.Load<Texture2D>(@"Maps\History\EastFort\foreground");
            foreground2 = content.Load<Texture2D>(@"Maps\History\EastFort\foreground2");

            sky = content.Load<Texture2D>(@"Maps\History\EastFort\sky");
            farParallax = content.Load<Texture2D>(@"Maps\History\EastFort\farParallax");
            medParallax1 = content.Load<Texture2D>(@"Maps\History\EastFort\mediumParallax1");
            medParallax2 = content.Load<Texture2D>(@"Maps\History\EastFort\mediumParallax2");
            leftForeParallax = content.Load<Texture2D>(@"Maps\History\EastFort\ForegroundParallax");
            closeParallax = content.Load<Texture2D>(@"Maps\History\EastFort\closeParallax");

            fireTextures = ContentLoader.LoadContent(content, "Maps\\History\\CentralFort\\Fire");
            StoneFortCentral.soldierAnimations = ContentLoader.LoadContent(content, "Maps\\History\\SoldierAnimations");

            if(game.ChapterTwoDemo.ChapterTwoBooleans["horseInEast"])
            {
                horse.LoadContent(content, true, false);
                horse.faceTexture = content.Load<Texture2D>(@"Bosses\TrojanHorseFace");
            }

            Game1.npcFaces["Napoleon"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\NapoleonNormal");

            hutSprite = content.Load<Texture2D>(@"Maps\History\OutsideFort\GoblinHut");
            shrineSprite = content.Load<Texture2D>(@"Maps\History\OutsideFort\AnubisHut");
            hut.Sprite = hutSprite;
            hut1.Sprite = hutSprite;
            hut2.Sprite = shrineSprite;
            hut3.Sprite = shrineSprite;
            hut4.Sprite = hutSprite;
            hut5.Sprite = hutSprite;

            //if (Chapter.lastMap != "East Hall")
            //{
            //    SoundEffect bg = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Music\Noir Halls");
            //    SoundEffectInstance backgroundMusic = bg.CreateInstance();
            //    backgroundMusic.IsLooped = true;
            //    Sound.music.Add("North Hall", backgroundMusic);
            //}

            if (Chapter.lastMap != "Stone Fort - West" && Chapter.lastMap != "Stone Fort - Central")
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

            if (Chapter.theNextMap != "Stone Fort - West" && Chapter.theNextMap != "Stone Fort - Central")
            {
                Sound.UnloadAmbience();
                Sound.UnloadBackgroundMusic();
            }
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.GoblinEnemy(content);
            EnemyContentLoader.CommanderGoblinEnemy(content);
            EnemyContentLoader.BomblinEnemy(content);
            EnemyContentLoader.AnubisWarriorEnemy(content);
            EnemyContentLoader.SoldierGoblinEnemy(content);
            EnemyContentLoader.SharedGoblinSounds(content);
            EnemyContentLoader.SharedAnubisSounds(content);
            EnemyContentLoader.NurseGoblinEnemy(content);

        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            switch (game.chapterState)
            {
                case Game1.ChapterState.demo:

                    timeBeforeSpawn = Game1.randomNumberGen.Next(100, 200);
                    int randomEnemy = rand.Next(3);

                    if (randomEnemy == 0)
                    {

                        if (Game1.randomNumberGen.Next(3) > 1)
                        {
                            GoblinDemo ben = new GoblinDemo(pos, "Goblin Soldier", game, ref player, this);
                            ben.Hostile = true;

                            monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;

                            if (game.ChapterTwoDemo.ChapterTwoBooleans["horseInEast"])
                            {
                                if (horse != null)
                                    ben.objectToAttack = horse;
                            }

                            ben.Position = new Vector2(monsterX, monsterY);

                            Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                            ben.TimeBeforeSpawn = timeBeforeSpawn;

                            if (benRec.Intersects(player.Rec))
                            {
                            }
                            else
                            {
                                AddEnemyToEnemyList(ben);
                                enemyNamesAndNumberInMap["Goblin Soldier"]++;
                            }
                        }
                        else
                        {
                            NurseGoblin ben = new NurseGoblin(pos, "Nurse Goblin", game, ref player, this);
                            ben.Hostile = true;

                            monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;

                            if (game.ChapterTwoDemo.ChapterTwoBooleans["horseInEast"])
                            {
                                if (horse != null)
                                    ben.objectToAttack = horse;
                            }

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

                        if (game.ChapterTwoDemo.ChapterTwoBooleans["horseInEast"])
                        {
                            if (horse != null)
                                ben.objectToAttack = horse;
                        }


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

                        if (game.ChapterTwoDemo.ChapterTwoBooleans["horseInEast"])
                        {
                            if (horse != null)
                                ben.objectToAttack = horse;
                        }

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
                    if (enemyNamesAndNumberInMap["Goblin"] < 4)
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

        public void RespawnCommanderSoldiers()
        {
            base.RespawnGroundEnemies();

            switch (game.chapterState)
            {
                case Game1.ChapterState.demo:

                    int monsterPosX = Game1.randomNumberGen.Next(2);

                    if (enemyNamesAndNumberInMap["Goblin Soldier"] < 3)
                    {
                        GoblinDemo ben = new GoblinDemo(pos, "Goblin Soldier", game, ref player, this);
                        ben.Hostile = true;

                        monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;

                        if (commander != null)
                        {
                            if (monsterPosX == 0)
                                monsterX = commander.VitalRec.Center.X - 600;
                            else
                                monsterX = commander.VitalRec.Center.X + 600;

                            if (monsterX < 0)
                                monsterX = 10;
                            else if (monsterX > mapWidth)
                                monsterX = mapWidth - ben.Rec.Width - 10;
                        }

                        ben.Position = new Vector2(monsterX, monsterY);

                        Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                        ben.TimeBeforeSpawn = Game1.randomNumberGen.Next(60, 120);
                        ben.SpawnWithPoof = true;

                        if (benRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            AddEnemyToEnemyList(ben);
                            enemyNamesAndNumberInMap["Goblin Soldier"]++;
                        }
                    }

                    if (enemyNamesAndNumberInMap["Nurse Goblin"] < 2)
                    {
                        NurseGoblin ben = new NurseGoblin(pos, "Nurse Goblin", game, ref player, this);
                        ben.Hostile = true;

                        monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;

                        if (commander != null)
                        {
                            if (monsterPosX == 0)
                                monsterX = commander.VitalRec.Center.X - 600;
                            else
                                monsterX = commander.VitalRec.Center.X + 600;

                            if (monsterX < 0)
                                monsterX = 10;
                            else if (monsterX > mapWidth)
                                monsterX = mapWidth - ben.Rec.Width - 10;
                        }

                        ben.Position = new Vector2(monsterX, monsterY);

                        Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                        ben.TimeBeforeSpawn = Game1.randomNumberGen.Next(60, 120);
                        ben.SpawnWithPoof = true;

                        if (benRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            AddEnemyToEnemyList(ben);
                            enemyNamesAndNumberInMap["Nurse Goblin"]++;
                        }
                    }

                    if (enemyNamesAndNumberInMap["Bomblin"] < 3)
                    {
                        BomblinDemo ben = new BomblinDemo(pos, "Bomblin", game, ref player, this);
                        ben.Hostile = true;

                        monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;

                        if (commander != null)
                        {
                            if (monsterPosX == 0)
                                monsterX = commander.VitalRec.Center.X - 600;
                            else
                                monsterX = commander.VitalRec.Center.X + 600;
                        }

                        ben.Position = new Vector2(monsterX, monsterY);

                        Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                        ben.TimeBeforeSpawn = Game1.randomNumberGen.Next(60, 120);
                        ben.SpawnWithPoof = true;

                        if (benRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            AddEnemyToEnemyList(ben);
                            enemyNamesAndNumberInMap["Bomblin"]++;
                        }
                    }

                    break;
            }
        }

        public void SpawnEnemiesInFrontOfHorse(int numberOfEnemies)
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                int randomEnemy = rand.Next(3);

                if (randomEnemy == 0)
                {
                    if (Game1.randomNumberGen.Next(3) > 1)
                    {
                        GoblinDemo ben = new GoblinDemo(pos, "Goblin Soldier", game, ref player, this);
                        ben.Hostile = true;

                        monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;

                        monsterX = horse.Rec.Center.X + Game1.randomNumberGen.Next(600, 1000);

                        if (horse != null)
                            ben.objectToAttack = horse;

                        ben.Position = new Vector2(monsterX, monsterY);

                        Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                        ben.TimeBeforeSpawn = timeBeforeSpawn;

                        if (benRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            AddEnemyToEnemyList(ben);
                            enemyNamesAndNumberInMap["Goblin Soldier"]++;
                        }
                    }
                    else
                    {
                        NurseGoblin ben = new NurseGoblin(pos, "Nurse Goblin", game, ref player, this);
                        ben.Hostile = true;

                        monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;

                        monsterX = horse.Rec.Center.X + Game1.randomNumberGen.Next(600, 1000);

                        if (horse != null)
                            ben.objectToAttack = horse;

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

                    monsterX = horse.Rec.Center.X + Game1.randomNumberGen.Next(600, 1000);
                    if (horse != null)
                        ben.objectToAttack = horse;

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

                    monsterX = horse.Rec.Center.X + Game1.randomNumberGen.Next(600, 1000);

                    if (horse != null)
                        ben.objectToAttack = horse;

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

        public override void Update()
        {
            base.Update();

            //Spawn initial enemies
            if (spawnEnemies)
            {
                RespawnInitialGroundEnemies();

                if (enemiesInMap.Count == 4)
                    spawnEnemies = false;
            }
            if (game.ChapterTwoDemo.ChapterTwoBooleans["bombExploded"])
            {
                toCentral.IsUseable = true;
            }

            if (game.ChapterTwoDemo.ChapterTwoBooleans["westTrollKilled"] && !game.ChapterTwoDemo.ChapterTwoBooleans["bombExploded"])
            {
                game.CurrentChapter.state = Chapter.GameState.Cutscene;
                game.ChapterTwoDemo.ChapterTwoBooleans["bombExploded"] = true;
                game.ChapterTwoDemo.fortRaid.SpecialConditions.Clear();
                game.ChapterTwoDemo.fortRaid.SpecialConditions.Add("Investigate the aftermath", false);
            }

            if (game.ChapterTwoDemo.ChapterTwoBooleans["horseInEast"])
            {
                game.ChapterTwoDemo.BossFight = true;
                game.ChapterTwoDemo.CurrentBoss = horse;
                //horse.Update();
                if (toCentral.IsUseable)
                {
                    toCentral.IsUseable = false;
                    toWest.IsUseable = false;
                }

                horse.Move(1.5f);

                if (horse.RecX > 1000 && horse.RecX < 1002)
                    SpawnEnemiesInFrontOfHorse(3);
                else if (horse.RecX > 2000 && horse.RecX < 2002)
                    SpawnEnemiesInFrontOfHorse(3);
                else if (horse.RecX > 3000 && horse.RecX < 3002)
                    SpawnEnemiesInFrontOfHorse(4);
                else if (horse.RecX > 6000 && horse.RecX < 6002)
                    SpawnEnemiesInFrontOfHorse(4);

                if (horse.RecX > mapWidth)
                {
                    horse.StopSound();
                    game.ChapterTwoDemo.ChapterTwoBooleans["horseInEast"] = false;
                    game.ChapterTwoDemo.ChapterTwoBooleans["horseInWest"] = true;
                    toWest.IsUseable = true;
                    game.ChapterTwoDemo.BossFight = false;
                    game.ChapterTwoDemo.CurrentBoss = null;
                    StoneFortWestDemo.horse.health = horse.health;
                    StoneFortWestDemo.horse.HasBeenHit = true;
                    horse = null;

                    for (int i = 0; i < enemiesInMap.Count; i++)
                        enemiesInMap[i].objectToAttack = null;
                }
            }

            if (game.ChapterTwoDemo.ChapterTwoBooleans["enemyReinforcementsSpawning"] && enemiesInMap.Count < enemyAmount && (!game.ChapterTwoDemo.ChapterTwoBooleans["eastCommanderSpawned"] || game.ChapterTwoDemo.ChapterTwoBooleans["horseInEast"]))
            {
                RespawnGroundEnemies();
            }
            else if (game.ChapterTwoDemo.ChapterTwoBooleans["eastCommanderSpawned"] && !game.ChapterTwoDemo.ChapterTwoBooleans["eastCommanderKilled"] && commander != null)
                RespawnCommanderSoldiers();

            if (game.ChapterTwoDemo.ChapterTwoBooleans["eastCommanderSpawned"])
            {
                if (!game.ChapterTwoDemo.ChapterTwoBooleans["eastCommanderKilled"] && commander == null)
                {
                    enemiesInMap.Clear();
                    ResetEnemyNamesAndNumberInMap();

                    if (player.VitalRecX > 1700 && player.VitalRecX < 5300)
                    {
                        commander = new CommanderGoblin(new Vector2(3500, platforms[0].RecY - 300), "Commander Goblin", game, ref player, this);
                        commander.SpawnWithPoof = false;

                        AddEnemyToEnemyList(commander);
                    }
                }
                if (commander != null && commander.Health <= 0 && !game.ChapterTwoDemo.ChapterTwoBooleans["eastCommanderKilled"])
                {
                    game.ChapterTwoDemo.ChapterTwoBooleans["eastCommanderKilled"] = true;

                    if (game.ChapterTwoDemo.ChapterTwoBooleans["westCommanderKilled"] && !game.ChapterTwoDemo.ChapterTwoBooleans["centralCommanderSpawned"])
                    {
                        Chapter.effectsManager.AddInGameDialogue("Ze enemy iz attacking ze horse! Get back zhere and protect it!", "Napoleon", "Normal", 400);
                        game.ChapterTwoDemo.fortRaid.SpecialConditions.Clear();
                        game.ChapterTwoDemo.fortRaid.SpecialConditions.Add("Protect the horse in the Central \narea", false);
                        game.ChapterTwoDemo.ChapterTwoBooleans["centralCommanderSpawned"] = true;

                    }
                    else
                    {
                        //Chapter.effectsManager.AddInGameDialogue("Another commander has showned up in the western part of camp. Take him out before he can gather too many enemy troops.", "Napoleon", "Normal", 400);
                        Chapter.effectsManager.NotificationQueue.Enqueue(new QuestUpdatedNotification(true));
                        game.ChapterTwoDemo.ChapterTwoBooleans["westCommanderSpawned"] = true;
                        game.ChapterTwoDemo.fortRaid.SpecialConditions.Clear();
                        game.ChapterTwoDemo.fortRaid.SpecialConditions.Add("Kill the enemy commander and his \ntroops in the Western area", false);

                    }
                }
            }
            else
            {
                if (game.ChapterTwoDemo.ChapterTwoBooleans["enemyReinforcementsSpawning"] == false || game.ChapterTwoDemo.ChapterTwoBooleans["clearedEastHuts"] == false)
                {
                    int hutsDestroyed = 0;

                    for (int i = 0; i < interactiveObjects.Count; i++)
                    {
                        if (interactiveObjects[i].Finished)
                            hutsDestroyed++;

                    }

                    if (hutsDestroyed >= 4 && game.ChapterTwoDemo.ChapterTwoBooleans["enemyReinforcementsSpawning"] == false)
                    {
                        game.ChapterTwoDemo.ChapterTwoBooleans["enemyReinforcementsSpawning"] = true;

                        Sound.PlaySoundInstance(Sound.mapZoneSoundEffects["popup_fort_alarm"], "popup_fort_alarm");
                    }

                    if (hutsDestroyed == 6 && game.ChapterTwoDemo.ChapterTwoBooleans["clearedEastHuts"] == false)
                    {
                        if (game.ChapterTwoDemo.ChapterTwoBooleans["clearedWestHuts"])
                        {
                            //Chapter.effectsManager.AddInGameDialogue("Excellent job, soldier! Your work isn't done though, an enemy commander is rallying troops in the West. Shut him down!", "Napoleon", "Normal", 400);

                            game.ChapterTwoDemo.NPCs["Napoleon"].QuestDialogue[game.ChapterTwoDemo.NPCs["Napoleon"].DialogueState] = "Zhere is a big-shot gathering troops in ze West. Go stop him!";
                            game.ChapterTwoDemo.NPCs["Napoleon"].Talking = true;
                            game.CurrentChapter.TalkingToNPC = true;
                            player.Sprinting = false;

                            game.ChapterTwoDemo.ChapterTwoBooleans["westCommanderSpawned"] = true;
                            game.ChapterTwoDemo.fortRaid.SpecialConditions.Clear();
                            game.ChapterTwoDemo.fortRaid.SpecialConditions.Add("Kill the enemy commander and his \ntroops in the Western area", false);
                        }
                        else
                            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestUpdatedNotification(true));

                           // Chapter.effectsManager.AddInGameDialogue("That area is clear! Move on to the western side!", "Napoleon", "Normal", 300);

                        game.ChapterTwoDemo.ChapterTwoBooleans["clearedEastHuts"] = true;
                    }
                }
            }

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

            PlayBackgroundMusic();
            PlayAmbience();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toCentral = new Portal(560, platforms[0], "Stone Fort - East");
            toCentral.FButtonYOffset = -60;
            toCentral.PortalNameYOffset = -60;


            toWest = new Portal(7800, platforms[0], "Stone Fort - East");
            toWest.FButtonYOffset = -60;
            toWest.PortalNameYOffset = -60;
            //toBathroom = new Portal(500, platforms[0], "MainLobby");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCentral, StoneFortCentralDemo.ToFortEast);
            portals.Add(toWest, StoneFortWestDemo.ToEast);
        }

        public override void AddNPCs()
        {
            base.AddNPCs();
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            s.Draw(fireTextures.ElementAt(torches[0].frame).Value, new Vector2(510, mapRec.Y + 419), Color.White);
            s.Draw(fireTextures.ElementAt(torches[1].frame).Value, new Vector2(1978, mapRec.Y + 398), Color.White);
            s.Draw(fireTextures.ElementAt(torches[2].frame).Value, new Vector2(2467, mapRec.Y + 386), Color.White);
            s.Draw(fireTextures.ElementAt(torches[3].frame).Value, new Vector2(4835, mapRec.Y + 342), Color.White);
            s.Draw(fireTextures.ElementAt(torches[4].frame).Value, new Vector2(6154, mapRec.Y + 356), Color.White);
            s.Draw(fireTextures.ElementAt(torches[5].frame).Value, new Vector2(7560, mapRec.Y + 418), Color.White);
            if (game.ChapterTwoDemo.ChapterTwoBooleans["horseInEast"] && horse != null)
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

            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1.23f, this, game));
            s.Draw(leftForeParallax, new Vector2(0, mapRec.Y), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Rectangle(0, mapRec.Y, mapWidth, 800), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.6f, this, game));
            s.Draw(farParallax, new Vector2(-700, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.85f, this, game));
            s.Draw(medParallax1, new Vector2(3090, mapRec.Y), Color.White);
            s.Draw(medParallax2, new Vector2(mapWidth - medParallax2.Width - 1200, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.95f, this, game));
            s.Draw(closeParallax, new Vector2(2878, mapRec.Y), Color.White);
            s.End();
        }
    }
}
