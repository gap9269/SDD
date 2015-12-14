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
    public class StoneFortWest : MapClass
    {
        static Portal toCentral;
        static Portal toEast;
        static Portal toWasteland;

        public static Portal ToWasteland { get { return toWasteland; } }
        public static Portal ToCentral { get { return toCentral; } }
        public static Portal ToEast { get { return toEast; } }

        public FieldTroll troll;

        GoblinHut hut2, hut1, hut4;
        AnubisHut hut, hut3;

        int goblinAmount;

        int timeBeforeSpawn = 160;

        AnubisCommander commander;

        Texture2D foreground, foreground2, foreground3, background2Hole, sky, backgroundParallax, fence, barracks, trollHole, trollRocks, sandbags, rightForeParallax, hutSprite, shrineSprite, destroyedParallax;

        public static Dictionary<String, Texture2D> fireTextures;
        public static TrojanHorse horse;

        struct TorchFire
        {
            public int frame;
            public int frameDelay;
        }
        List<TorchFire> torches;

        public StoneFortWest(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapWidth = 9000;
            mapHeight = 1160;
            mapName = "Stone Fort - West";

            //backgroundMusicName = "Noir Halls";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 10;

            yScroll = false;
            horse = new TrojanHorse(0, -105, player, this);
            horse.hasLocker = true;

            interactiveObjects.Add(new MapAnimation(game, 1825, 283, true, MapAnimation.AnimationType.BurntBomblin, false));

            hut = new AnubisHut(game, 544, 790, Game1.whiteFilter, 10, 5, false);
            interactiveObjects.Add(hut);

            hut1 = new GoblinHut(game, 999, 650, Game1.whiteFilter, 10, 5, false);
            interactiveObjects.Add(hut1);

            interactiveObjects.Add(new MapAnimation(game, 1530, 370, true, MapAnimation.AnimationType.shoot, false));

            hut2 = new GoblinHut(game, 1584, 640, Game1.whiteFilter, 10, 5, true);
            interactiveObjects.Add(hut2);

            hut3 = new AnubisHut(game, 2108, 740, Game1.whiteFilter, 10, 5, true);
            interactiveObjects.Add(hut3);

            hut4 = new GoblinHut(game, 2521, 650, Game1.whiteFilter, 10, 5, false);
            interactiveObjects.Add(hut4);

            interactiveObjects.Add(new MapAnimation(game, 738, 492, true, MapAnimation.AnimationType.fencing, true));
            interactiveObjects.Add(new MapAnimation(game, 1107, 412, true, MapAnimation.AnimationType.RomanCorpse, true));
            interactiveObjects.Add(new MapAnimation(game, 1978, 423, false, MapAnimation.AnimationType.EgyptianCorpse, true));
            interactiveObjects.Add(new MapAnimation(game, 3020, 323, true, MapAnimation.AnimationType.FrenchCorpse, false));
            interactiveObjects.Add(new MapAnimation(game, 3460, 380, true, MapAnimation.AnimationType.strangle, false));
            interactiveObjects.Add(new MapAnimation(game, 4257, 292, true, MapAnimation.AnimationType.EgyptianCorpse, false));
            interactiveObjects.Add(new MapAnimation(game, 5432, 479, false, MapAnimation.AnimationType.RomanCorpse, true));

            interactiveObjects.Add(new MapAnimation(game, 5600, 365, true, MapAnimation.AnimationType.boxing, false));
            interactiveObjects.Add(new MapAnimation(game, 6038, 320, false, MapAnimation.AnimationType.EgyptianCorpseMongol, false));
            interactiveObjects.Add(new MapAnimation(game, 7270, 365, true, MapAnimation.AnimationType.shoot, false));
            interactiveObjects.Add(new MapAnimation(game, 7672, 330, false, MapAnimation.AnimationType.RomanCorpseGoblin, false));


            hut1.RecY = 10;
            hut2.RecY = 40;
            hut4.RecY = 20;

            #region Torches
            torches = new List<TorchFire>();
            TorchFire torch1 = new TorchFire();
            TorchFire torch2 = new TorchFire();
            TorchFire torch3 = new TorchFire();

            torch1.frameDelay = 3;
            torch2.frameDelay = 5;
            torch3.frameDelay = 2;


            torch2.frame = 1;
            torch3.frame = 2;

            torches.Add(torch1);
            torches.Add(torch2);
            torches.Add(torch3);
            #endregion

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Bomblin", 0);
            enemyNamesAndNumberInMap.Add("Goblin", 0);
            enemyNamesAndNumberInMap.Add("Anubis Warrior", 0);
            enemyNamesAndNumberInMap.Add("Goblin Soldier", 0);

            currentBackgroundMusic = Sound.MusicNames.FortBattle;

        }

        public override void LoadContent()
        {
            Sound.LoadFortZoneSounds();

            if (!game.ChapterTwo.ChapterTwoBooleans["bombExploded"])
            {
                background.Add(content.Load<Texture2D>(@"Maps\History\WestFort\background"));
                background.Add(content.Load<Texture2D>(@"Maps\History\WestFort\background2"));
                background.Add(content.Load<Texture2D>(@"Maps\History\WestFort\background3"));
                foreground = content.Load<Texture2D>(@"Maps\History\WestFort\fog");
                foreground2 = content.Load<Texture2D>(@"Maps\History\WestFort\fog2");
                foreground3 = content.Load<Texture2D>(@"Maps\History\WestFort\fog3");
                rightForeParallax = content.Load<Texture2D>(@"Maps\History\WestFort\foregroundParallax");
            }
            else
            {
                toWasteland.IsUseable = true;
                background.Add(content.Load<Texture2D>(@"Maps\History\WestFort\destroyedBackground"));
                background.Add(content.Load<Texture2D>(@"Maps\History\WestFort\destroyedBackground2"));
                background.Add(content.Load<Texture2D>(@"Maps\History\WestFort\destroyedBackground3"));
                foreground = content.Load<Texture2D>(@"Maps\History\WestFort\destroyedForeground");
                foreground2 = content.Load<Texture2D>(@"Maps\History\WestFort\destroyedForeground2");
                foreground3 = content.Load<Texture2D>(@"Maps\History\WestFort\destroyedForeground3");
                rightForeParallax = content.Load<Texture2D>(@"Maps\History\WestFort\destroyedForegroundParallax");
                destroyedParallax = content.Load<Texture2D>(@"Maps\History\WestFort\destroyedParallax");
            }

            background2Hole = content.Load<Texture2D>(@"Maps\History\WestFort\background2Hole");

            sky = content.Load<Texture2D>(@"Maps\History\CentralFort\sky");
            barracks = content.Load<Texture2D>(@"Maps\History\WestFort\barracks");
            fence = content.Load<Texture2D>(@"Maps\History\WestFort\fence");
            sandbags = content.Load<Texture2D>(@"Maps\History\WestFort\foregroundSandbags");
            backgroundParallax = content.Load<Texture2D>(@"Maps\History\WestFort\parallax");
            trollHole = content.Load<Texture2D>(@"Maps\History\WestFort\trollHole");
            trollRocks = content.Load<Texture2D>(@"Maps\History\WestFort\rocks");

            fireTextures = ContentLoader.LoadContent(content, "Maps\\History\\CentralFort\\Fire");
            StoneFortCentral.soldierAnimations = ContentLoader.LoadContent(content, "Maps\\History\\SoldierAnimations");

            if (game.ChapterTwo.ChapterTwoBooleans["horseInWest"])
            {
                horse.LoadContent(content, true, false);
                horse.faceTexture = content.Load<Texture2D>(@"Bosses\TrojanHorseFace");

            }

            Game1.npcFaces["Napoleon"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\NapoleonNormal");

            hutSprite = content.Load<Texture2D>(@"Maps\History\OutsideFort\GoblinHut");
            shrineSprite = content.Load<Texture2D>(@"Maps\History\OutsideFort\AnubisHut");
            hut.Sprite = shrineSprite;
            hut1.Sprite = hutSprite;
            hut2.Sprite = hutSprite;
            hut3.Sprite = shrineSprite;
            hut4.Sprite = hutSprite;

            //if (Chapter.lastMap != "East Hall")
            //{
            //    SoundEffect bg = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Music\Noir Halls");
            //    SoundEffectInstance backgroundMusic = bg.CreateInstance();
            //    backgroundMusic.IsLooped = true;
            //    Sound.music.Add("North Hall", backgroundMusic);
            //}

            if (Chapter.lastMap != "Stone Fort - East" && Chapter.lastMap != "Stone Fort - Central" && Chapter.lastMap != "Stone Fort Wasteland")
            {
                SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_fort_battle");
                SoundEffectInstance amb = am.CreateInstance();
                amb.IsLooped = true;
                Sound.ambience.Add("ambience_fort_battle", amb);
            }

            if (game.ChapterTwo.ChapterTwoBooleans["bombExploded"] && Chapter.lastMap != "Stone Fort Wasteland")
            {
                SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_wasteland");
                SoundEffectInstance amb = am.CreateInstance();
                amb.IsLooped = true;
                Sound.ambience.Add("ambience_wasteland", amb);
            }
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            Game1.npcFaces["Napoleon"].faces["Normal"] = Game1.whiteFilter;

            if (Chapter.theNextMap != "Stone Fort - East" && Chapter.theNextMap != "Stone Fort - Central" && Chapter.theNextMap != "Stone Fort Wasteland")
            {
                Sound.UnloadAmbience();
                Sound.UnloadBackgroundMusic();
            }
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.AnubisCommanderEnemy(content);
            EnemyContentLoader.BomblinEnemy(content);
            EnemyContentLoader.GoblinEnemy(content);
            EnemyContentLoader.SoldierGoblinEnemy(content);
            EnemyContentLoader.AnubisWarriorEnemy(content);
            EnemyContentLoader.LocustEnemy(content);
            EnemyContentLoader.SharedGoblinSounds(content);
            EnemyContentLoader.SharedAnubisSounds(content);

            if (game.ChapterTwo.ChapterTwoBooleans["horseInWest"])
            {
                EnemyContentLoader.Troll(content);
                Chapter.LoadTrollTexturesByDrawing = true;
            }
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            switch (game.chapterState)
            {

                case Game1.ChapterState.chapterTwo:

                    int randomEnemy = rand.Next(3);
                    int monsterPosX = rand.Next(2);

                    if (randomEnemy == 0)
                    {
                        Goblin ben = new Goblin(pos, "Goblin Soldier", game, ref player, this);
                        ben.Hostile = true;

                        monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;

                        if (game.ChapterTwo.ChapterTwoBooleans["horseInWest"])
                        {
                            if(horse != null)
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
                            enemyNamesAndNumberInMap["Goblin"]++;
                        }
                    }
                    else if (randomEnemy == 1)
                    {
                        Bomblin ben = new Bomblin(pos, "Bomblin", game, ref player, this);
                        ben.Hostile = true;

                        monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;

                        if (game.ChapterTwo.ChapterTwoBooleans["horseInWest"])
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

                        AnubisWarrior ben = new AnubisWarrior(pos, "Anubis Warrior", game, ref player, this, new Rectangle(200, 300, mapWidth - 400, 500));
                        ben.Hostile = true;

                        monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;

                        if (game.ChapterTwo.ChapterTwoBooleans["horseInWest"])
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

                case Game1.ChapterState.chapterTwo:
                    if (enemyNamesAndNumberInMap["Goblin"] < 4)
                    {
                        Goblin ben = new Goblin(pos, "Goblin", game, ref player, this);
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

        public void RespawnTrollEnemies()
        {
            base.RespawnGroundEnemies();

            switch (game.chapterState)
            {

                case Game1.ChapterState.chapterTwo:
                    int monsterPosX = rand.Next(2);
                    
                    if (enemyNamesAndNumberInMap["Bomblin"] < 2)
                    {
                        Bomblin ben = new Bomblin(pos, "Bomblin", game, ref player, this);
                        ben.Hostile = true;

                        monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;
                        
                        if (monsterPosX == 0)
                            monsterX = player.VitalRec.Center.X - 600;
                        else
                            monsterX = player.VitalRec.Center.X + 600;

                        if (monsterX < 0)
                            monsterX = 10;
                        else if (monsterX > mapWidth)
                            monsterX = mapWidth - ben.Rec.Width - 10;

                        ben.Position = new Vector2(monsterX, monsterY);

                        Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                        ben.TimeBeforeSpawn = 120;
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

        public void RespawnCommanderSoldiers()
        {
            base.RespawnGroundEnemies();

            switch (game.chapterState)
            {
                case Game1.ChapterState.chapterTwo:

                    int monsterPosX = Game1.randomNumberGen.Next(2);

                    if (enemyNamesAndNumberInMap["Anubis Warrior"] < 4)
                    {
                        AnubisWarrior ben = new AnubisWarrior(pos, "Anubis Warrior", game, ref player, this, new Rectangle(200, 300, mapWidth - 400, 500));
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

        public void SpawnEnemiesInFrontOfHorse(int numberOfEnemies)
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                int randomEnemy = rand.Next(3);

                if (randomEnemy == 0)
                {
                    Goblin ben = new Goblin(pos, "Goblin Soldier", game, ref player, this);
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
                else if (randomEnemy == 1)
                {
                    Bomblin ben = new Bomblin(pos, "Bomblin", game, ref player, this);
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

                    AnubisWarrior ben = new AnubisWarrior(pos, "Anubis Warrior", game, ref player, this, new Rectangle(200, 300, mapWidth - 400, 500));
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
            if (!game.ChapterTwo.ChapterTwoBooleans["bombExploded"])
            {
                Sound.PlayAmbience("ambience_fort_battle");
            }
            else
            {
                Sound.ambience["ambience_fort_battle"].Stop();
                Sound.PlayAmbience("ambience_wasteland");
            }
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

            if (game.ChapterTwo.ChapterTwoBooleans["horseInWest"])
            {

                if (game.ChapterTwo.BossFight == false)
                    horse.Update();

                if (horse.RecX < 6000)
                {
                    horse.Move(1.5f);

                    game.ChapterTwo.BossFight = true;
                    game.ChapterTwo.CurrentBoss = horse;

                    if (toCentral.IsUseable)
                    {
                        toCentral.IsUseable = false;
                        toEast.IsUseable = false;
                    }

                    if (horse.RecX > 1000 && horse.RecX < 1002)
                        SpawnEnemiesInFrontOfHorse(3);
                    else if (horse.RecX > 2000 && horse.RecX < 2002)
                        SpawnEnemiesInFrontOfHorse(3);
                    else if (horse.RecX > 3000 && horse.RecX < 3002)
                        SpawnEnemiesInFrontOfHorse(4);
                    else if (horse.RecX > 4000 && horse.RecX < 4002)
                        SpawnEnemiesInFrontOfHorse(4);
                }
                else if(!game.ChapterTwo.ChapterTwoBooleans["trollSpawnedInWest"])
                {
                    horse.StopSound();
                    game.CurrentChapter.state = Chapter.GameState.Cutscene;
                    game.ChapterTwo.fortRaid.SpecialConditions.Clear();
                    game.ChapterTwo.fortRaid.SpecialConditions.Add("Kill the Troll", false);
                    game.ChapterTwo.BossFight = false;
                    game.ChapterTwo.CurrentBoss = null;
                }
            }

            if (game.ChapterTwo.ChapterTwoBooleans["bombExploded"])
            {
                interactiveObjects[0].IsHidden = true;

                for (int i = 5; i < interactiveObjects.Count; i++)
                {
                    if(i != 7 && i != 8)
                        interactiveObjects[i].IsHidden = true;   
                }
            }

            if (game.ChapterTwo.ChapterTwoBooleans["bombExploded"] && enemiesInMap.Count > 0)
            {
                enemiesInMap.Clear();
                ResetEnemyNamesAndNumberInMap();
            }

            if (game.ChapterTwo.ChapterTwoBooleans["enemyReinforcementsSpawning"] && enemiesInMap.Count < enemyAmount && (!game.ChapterTwo.ChapterTwoBooleans["westCommanderSpawned"] || game.ChapterTwo.ChapterTwoBooleans["horseInWest"]) && !game.ChapterTwo.ChapterTwoBooleans["trollSpawnedInWest"])
            {
                RespawnGroundEnemies();
            }
            else if (game.ChapterTwo.ChapterTwoBooleans["trollSpawnedInWest"] && !game.ChapterTwo.ChapterTwoBooleans["westTrollKilled"])
            {
                RespawnTrollEnemies();

                if (!enemiesInMap.Contains(troll))
                {
                    game.ChapterTwo.ChapterTwoBooleans["westTrollKilled"] = true;

                    Chapter.effectsManager.AddInGameDialogue("Get out of zhere before ze bomb explodes!", "Napoleon", "Normal", 400);
                    game.ChapterTwo.fortRaid.SpecialConditions.Clear();
                    game.ChapterTwo.fortRaid.SpecialConditions.Add("Leave the area", false);

                    toCentral.IsUseable = true;
                    toEast.IsUseable = true;
                }
            }

            else if (game.ChapterTwo.ChapterTwoBooleans["westCommanderSpawned"] && !game.ChapterTwo.ChapterTwoBooleans["westCommanderKilled"] && commander != null)
                RespawnCommanderSoldiers();

            if (game.ChapterTwo.ChapterTwoBooleans["westCommanderSpawned"])
            {
                if (!game.ChapterTwo.ChapterTwoBooleans["westCommanderKilled"] && commander == null)
                {
                    enemiesInMap.Clear();
                    ResetEnemyNamesAndNumberInMap();

                    if(player.VitalRecX > 1700 && player.VitalRecX < 5300)
                    {
                        commander = new AnubisCommander(new Vector2(3500, platforms[0].RecY - 600), "Commander Anubis", game, ref player, this, new Rectangle(200, 300, mapWidth - 400, 500));
                        commander.SpawnWithPoof = false;

                        AddEnemyToEnemyList(commander);
                    }
                }

                if (commander != null && commander.Health <= 0 && !game.ChapterTwo.ChapterTwoBooleans["westCommanderKilled"])
                {
                    game.ChapterTwo.ChapterTwoBooleans["westCommanderKilled"] = true;

                    if (game.ChapterTwo.ChapterTwoBooleans["eastCommanderKilled"] && !game.ChapterTwo.ChapterTwoBooleans["centralCommanderSpawned"])
                    {
                        Chapter.effectsManager.AddInGameDialogue("The enemy iz attacking ze horse! Get back zhere and protect it!", "Napoleon", "Normal", 400);
                        game.ChapterTwo.fortRaid.SpecialConditions.Clear();
                        game.ChapterTwo.fortRaid.SpecialConditions.Add("Protect the horse in the Central \narea", false);
                        game.ChapterTwo.ChapterTwoBooleans["centralCommanderSpawned"] = true;
                    }
                    else
                    {
                        //Chapter.effectsManager.AddInGameDialogue("Another commander has showned up in the eastern part of camp. Take him out before he can gather too many enemy troops.", "Napoleon", "Normal", 400);
                        Chapter.effectsManager.NotificationQueue.Enqueue(new QuestUpdatedNotification(true));
                        game.ChapterTwo.ChapterTwoBooleans["eastCommanderSpawned"] = true;
                        game.ChapterTwo.fortRaid.SpecialConditions.Clear();
                        game.ChapterTwo.fortRaid.SpecialConditions.Add("Kill the enemy commander and his \ntroops in the Eastern area", false);

                    }
                }
            }
            else
            {
                if (game.ChapterTwo.ChapterTwoBooleans["enemyReinforcementsSpawning"] == false || game.ChapterTwo.ChapterTwoBooleans["clearedWestHuts"] == false)
                {
                    int hutsDestroyed = 0;

                    for (int i = 0; i < interactiveObjects.Count; i++)
                    {
                        if (interactiveObjects[i].Finished)
                            hutsDestroyed++;

                    }

                    if (hutsDestroyed >= 4 && game.ChapterTwo.ChapterTwoBooleans["enemyReinforcementsSpawning"] == false)
                    {
                        game.ChapterTwo.ChapterTwoBooleans["enemyReinforcementsSpawning"] = true;
                        Sound.PlaySoundInstance(Sound.mapZoneSoundEffects["popup_fort_alarm"], "popup_fort_alarm");
                    }

                    if (hutsDestroyed == 5 && game.ChapterTwo.ChapterTwoBooleans["clearedWestHuts"] == false)
                    {
                        if (game.ChapterTwo.ChapterTwoBooleans["clearedEastHuts"])
                        {

                            game.ChapterTwo.NPCs["Napoleon"].QuestDialogue[game.ChapterTwo.NPCs["Napoleon"].DialogueState] = "Zhere is a big-shot gathering troops in ze East. Go stop him!";

                            game.ChapterTwo.NPCs["Napoleon"].Talking = true;
                            game.CurrentChapter.TalkingToNPC = true;
                            player.Sprinting = false;

                            game.ChapterTwo.ChapterTwoBooleans["eastCommanderSpawned"] = true;

                            game.ChapterTwo.fortRaid.SpecialConditions.Clear();
                            game.ChapterTwo.fortRaid.SpecialConditions.Add("Kill the enemy commander and his \ntroops in the Eastern area", false);

                        }
                        else
                            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestUpdatedNotification(true));

                        //    Chapter.effectsManager.AddInGameDialogue("That area is clear! Move on to the eastern side!", "Napoleon", "Normal", 300);

                        game.ChapterTwo.ChapterTwoBooleans["clearedWestHuts"] = true;
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

            toCentral = new Portal(8800, platforms[0], "Stone Fort - West");
            toCentral.FButtonYOffset = -60;
            toCentral.PortalNameYOffset = -60;

            toEast = new Portal(50, platforms[0], "Stone Fort - West");
            toEast.FButtonYOffset = -60;
            toEast.PortalNameYOffset = -60;

            toWasteland = new Portal(6730, platforms[0], "Stone Fort - West");
            toWasteland.FButtonYOffset = -60;
            toWasteland.PortalNameYOffset = -60;
            toWasteland.IsUseable = false;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCentral, StoneFortCentral.ToFortWest);
            portals.Add(ToEast, StoneFortEast.ToWest);
            portals.Add(ToWasteland, StoneFortWasteland.ToWest);
        }

        public override void AddNPCs()
        {
            base.AddNPCs();
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            s.Draw(fireTextures.ElementAt(torches[0].frame).Value, new Vector2(305, mapRec.Y + 440), Color.White);

            if (!game.ChapterTwo.ChapterTwoBooleans["bombExploded"])
            {
                if (hut3.Finished)
                    s.Draw(fireTextures.ElementAt(torches[1].frame).Value, new Vector2(2406, mapRec.Y + 344), Color.White);

                s.Draw(fireTextures.ElementAt(torches[2].frame).Value, new Vector2(8443, mapRec.Y + 410), Color.White);
            }

            if (game.ChapterTwo.ChapterTwoBooleans["trollSpawnedInWest"] && !game.ChapterTwo.ChapterTwoBooleans["bombExploded"])
            {
                if(background[1] != background2Hole)
                    background[1] = background2Hole;
            }
            if (game.ChapterTwo.ChapterTwoBooleans["horseInWest"] && horse != null)
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
            if (!game.ChapterTwo.ChapterTwoBooleans["bombExploded"])
                s.Draw(fence, new Vector2(4137, mapRec.Y), Color.White);

            if (!game.ChapterTwo.ChapterTwoBooleans["bombExploded"])
            {
                s.Draw(sandbags, new Vector2(5696, mapRec.Y), Color.White);

                if (game.ChapterTwo.ChapterTwoBooleans["trollSpawnedInWest"])
                    s.Draw(trollRocks, new Vector2(6528, mapRec.Y), Color.White);
            }
            //6000 is the position of the barracks (origin of the fade, basically), and 225 is how close you can be to that origin before it begins to fade out
            float barracksAlpha = (((6000f) - (float)(player.VitalRecX))) / 225f;

            if (barracksAlpha < .2f)
                barracksAlpha = .2f;
            if (!game.ChapterTwo.ChapterTwoBooleans["bombExploded"])
                s.Draw(barracks, new Vector2(4137, mapRec.Y), Color.White * barracksAlpha);
            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White);
            s.Draw(foreground3, new Vector2(foreground.Width + foreground2.Width, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1.23f, this, game));
            s.Draw(rightForeParallax, new Vector2(mapWidth + 1150, mapRec.Y), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Rectangle(0, mapRec.Y, mapWidth, 800), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.85f, this, game));

            if (!game.ChapterTwo.ChapterTwoBooleans["bombExploded"])
                s.Draw(backgroundParallax, new Vector2(0, mapRec.Y), Color.White);
            else
                s.Draw(destroyedParallax, new Vector2(4851, mapRec.Y), Color.White);
            s.End();
        }
    }
}
